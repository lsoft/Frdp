using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Resource = SharpDX.DXGI.Resource;
using ResultCode = SharpDX.DXGI.ResultCode;

namespace Frdp.Client.ScreenshotContainer.Factory.Duplicate
{
    /// <summary>
    ///     Provides access to frame-by-frame updates of a particular desktop (i.e. one monitor), with image and cursor
    ///     information.
    /// </summary>
    public class DesktopDuplicator : IDisposable
    {
        private readonly OutputDuplication _deskDupl;
        private readonly Device _device;
        private readonly Texture2DDescription _textureDesc;
        private readonly int _whichOutputDevice = -1;
        private Texture2D _desktopImageTexture;

        private Bitmap _finalImage1, _finalImage2;
        private OutputDuplicateFrameInformation _frameInfo;
        private bool _isFinalImage1;
        private OutputDescription _outputDesc;

        private bool _disposed = false;

        private Bitmap _finalImage
        {
            get
            {
                return _isFinalImage1 ? _finalImage1 : _finalImage2;
            }
            set
            {
                if (_isFinalImage1)
                {
                    _finalImage2 = value;
                    if (_finalImage1 != null)
                    {
                        _finalImage1.Dispose();
                    }
                }
                else
                {
                    _finalImage1 = value;
                    if (_finalImage2 != null)
                    {
                        _finalImage2.Dispose();
                    }
                }

                _isFinalImage1 = !_isFinalImage1;
            }
        }

        /// <summary>
        ///     Duplicates the output of the specified monitor.
        /// </summary>
        /// <param name="whichMonitor">
        ///     The output device to duplicate (i.e. monitor). Begins with zero, which seems to correspond
        ///     to the primary monitor.
        /// </param>
        public DesktopDuplicator(int whichMonitor)
            : this(0, whichMonitor)
        {
        }

        /// <summary>
        ///     Duplicates the output of the specified monitor on the specified graphics adapter.
        /// </summary>
        /// <param name="whichGraphicsCardAdapter">The adapter which contains the desired outputs.</param>
        /// <param name="whichOutputDevice">
        ///     The output device to duplicate (i.e. monitor). Begins with zero, which seems to
        ///     correspond to the primary monitor.
        /// </param>
        public DesktopDuplicator(int whichGraphicsCardAdapter, int whichOutputDevice)
        {
            _whichOutputDevice = whichOutputDevice;
            Adapter1 adapter = null;
            try
            {
                adapter = new Factory1().GetAdapter1(whichGraphicsCardAdapter);
            }
            catch (SharpDXException)
            {
                throw new DesktopDuplicationException("Could not find the specified graphics card adapter.");
            }
            _device = new Device(adapter);
            Output output = null;
            try
            {
                output = adapter.GetOutput(whichOutputDevice);
            }
            catch (SharpDXException)
            {
                throw new DesktopDuplicationException("Could not find the specified output device.");
            }

            var output1 = output.QueryInterface<Output1>();
            _outputDesc = output.Description;
            _textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = _outputDesc.DesktopBounds.Width,
                Height = _outputDesc.DesktopBounds.Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = {Count = 1, Quality = 0},
                Usage = ResourceUsage.Staging
            };

            try
            {
                _deskDupl = output1.DuplicateOutput(_device);
            }
            catch (SharpDXException excp)
            {
                if (excp.ResultCode.Code == ResultCode.NotCurrentlyAvailable.Result.Code)
                {
                    throw new DesktopDuplicationException("There is already the maximum number of applications using the Desktop Duplication API running, please close one of the applications and try again.", excp);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     Retrieves the latest desktop image and associated metadata.
        /// </summary>
        public DesktopFrame GetLatestFrame()
        {
           
            // Try to get the latest frame; this may timeout
            bool retrievalTimedOut = RetrieveFrame();
            if (retrievalTimedOut)
            {
                return null;
            }

            var frame = new DesktopFrame();

            try
            {
                RetrieveFrameMetadata(frame);
                RetrieveCursorMetadata(frame);
                ProcessFrame(frame);
            }
            finally
            {
                try
                {
                    ReleaseFrame();
                }
                catch
                {
                    //nothing can be done here
                }
            }

            return
                frame;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_deskDupl != null)
                {
                    _deskDupl.Dispose();
                }

                if (_desktopImageTexture != null)
                {
                    _desktopImageTexture.Dispose();
                }

                if (_device != null)
                {
                    _device.Dispose();
                }

                if (_finalImage1 != null)
                {
                    _finalImage1.Dispose();
                }

                if (_finalImage2 != null)
                {
                    _finalImage2.Dispose();
                }
            }
        }

        private bool RetrieveFrame()
        {
            if (_desktopImageTexture == null)
            {
                _desktopImageTexture = new Texture2D(_device, _textureDesc);
            }

            Resource desktopResource = null;
            try
            {
                _frameInfo = new OutputDuplicateFrameInformation();
                try
                {
                    _deskDupl.AcquireNextFrame(250, out _frameInfo, out desktopResource);
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode.Code == ResultCode.WaitTimeout.Result.Code)
                    {
                        return true;
                    }
                    if (ex.ResultCode.Failure)
                    {
                        throw new DesktopDuplicationException("Failed to acquire next frame.");
                    }
                }

                using (var tempTexture = desktopResource.QueryInterface<Texture2D>())
                {
                    _device.ImmediateContext.CopyResource(tempTexture, _desktopImageTexture);
                }
            }
            finally
            {
                if (desktopResource != null)
                {
                    desktopResource.Dispose();
                }
            }
            
            return false;
        }

        private void RetrieveFrameMetadata(DesktopFrame frame)
        {
            if (_frameInfo.TotalMetadataBufferSize > 0)
            {
                // Get moved regions
                int movedRegionsLength = 0;
                var movedRectangles = new OutputDuplicateMoveRectangle[_frameInfo.TotalMetadataBufferSize];
                _deskDupl.GetFrameMoveRects(movedRectangles.Length, movedRectangles, out movedRegionsLength);
                frame.MovedRegions = new MovedRegion[movedRegionsLength/Marshal.SizeOf(typeof (OutputDuplicateMoveRectangle))];
                for (int i = 0; i < frame.MovedRegions.Length; i++)
                {
                    frame.MovedRegions[i] = new MovedRegion
                    {
                        Source = new Point(movedRectangles[i].SourcePoint.X, movedRectangles[i].SourcePoint.Y),
                        Destination = new Rectangle(movedRectangles[i].DestinationRect.X, movedRectangles[i].DestinationRect.Y, movedRectangles[i].DestinationRect.Width, movedRectangles[i].DestinationRect.Height)
                    };
                }

                // Get dirty regions
                int dirtyRegionsLength = 0;
                var dirtyRectangles = new SharpDX.Rectangle[_frameInfo.TotalMetadataBufferSize];
                _deskDupl.GetFrameDirtyRects(dirtyRectangles.Length, dirtyRectangles, out dirtyRegionsLength);
                frame.UpdatedRegions = new Rectangle[dirtyRegionsLength/Marshal.SizeOf(typeof (SharpDX.Rectangle))];
                for (int i = 0; i < frame.UpdatedRegions.Length; i++)
                {
                    frame.UpdatedRegions[i] = new Rectangle(dirtyRectangles[i].X, dirtyRectangles[i].Y, dirtyRectangles[i].Width, dirtyRectangles[i].Height);
                }
            }
            else
            {
                frame.MovedRegions = new MovedRegion[0];
                frame.UpdatedRegions = new Rectangle[0];
            }
        }

        private void RetrieveCursorMetadata(DesktopFrame frame)
        {
            var pointerInfo = new PointerInfo();

            // A non-zero mouse update timestamp indicates that there is a mouse position update and optionally a shape change
            if (_frameInfo.LastMouseUpdateTime == 0)
            {
                return;
            }

            bool updatePosition = true;

            // Make sure we don't update pointer position wrongly
            // If pointer is invisible, make sure we did not get an update from another output that the last time that said pointer
            // was visible, if so, don't set it to invisible or update.

            if (!_frameInfo.PointerPosition.Visible && (pointerInfo.WhoUpdatedPositionLast != _whichOutputDevice))
            {
                updatePosition = false;
            }

            // If two outputs both say they have a visible, only update if new update has newer timestamp
            if (_frameInfo.PointerPosition.Visible && pointerInfo.Visible && (pointerInfo.WhoUpdatedPositionLast != _whichOutputDevice) && (pointerInfo.LastTimeStamp > _frameInfo.LastMouseUpdateTime))
            {
                updatePosition = false;
            }

            // Update position
            if (updatePosition)
            {
                pointerInfo.Position = new SharpDX.Point(_frameInfo.PointerPosition.Position.X, _frameInfo.PointerPosition.Position.Y);
                pointerInfo.WhoUpdatedPositionLast = _whichOutputDevice;
                pointerInfo.LastTimeStamp = _frameInfo.LastMouseUpdateTime;
                pointerInfo.Visible = _frameInfo.PointerPosition.Visible;
            }

            // No new shape
            if (_frameInfo.PointerShapeBufferSize == 0)
            {
                return;
            }

            if (_frameInfo.PointerShapeBufferSize > pointerInfo.BufferSize)
            {
                pointerInfo.PtrShapeBuffer = new byte[_frameInfo.PointerShapeBufferSize];
                pointerInfo.BufferSize = _frameInfo.PointerShapeBufferSize;
            }

            try
            {
                unsafe
                {
                    fixed (byte* ptrShapeBufferPtr = pointerInfo.PtrShapeBuffer)
                    {
                        _deskDupl.GetFramePointerShape(_frameInfo.PointerShapeBufferSize, (IntPtr) ptrShapeBufferPtr, out pointerInfo.BufferSize, out pointerInfo.ShapeInfo);
                    }
                }
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to get frame pointer shape.");
                }
            }

            //frame.CursorVisible = pointerInfo.Visible;
            frame.CursorLocation = new Point(pointerInfo.Position.X, pointerInfo.Position.Y);
        }

        private void ProcessFrame(DesktopFrame frame)
        {
            // Get the desktop capture texture
            DataBox mapSource = _device.ImmediateContext.MapSubresource(_desktopImageTexture, 0, MapMode.Read, MapFlags.None);

            _finalImage = new Bitmap(_outputDesc.DesktopBounds.Width, _outputDesc.DesktopBounds.Height, PixelFormat.Format32bppRgb);
            var boundsRect = new Rectangle(0, 0, _outputDesc.DesktopBounds.Width, _outputDesc.DesktopBounds.Height);
            // Copy pixels from screen capture Texture to GDI bitmap
            BitmapData mapDest = _finalImage.LockBits(boundsRect, ImageLockMode.WriteOnly, _finalImage.PixelFormat);
            IntPtr sourcePtr = mapSource.DataPointer;
            IntPtr destPtr = mapDest.Scan0;
            for (int y = 0; y < _outputDesc.DesktopBounds.Height; y++)
            {
                // Copy a single line 
                Utilities.CopyMemory(destPtr, sourcePtr, _outputDesc.DesktopBounds.Width*4);

                // Advance pointers
                sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                destPtr = IntPtr.Add(destPtr, mapDest.Stride);
            }

            // Release source and dest locks
            _finalImage.UnlockBits(mapDest);
            _device.ImmediateContext.UnmapSubresource(_desktopImageTexture, 0);
            frame.DesktopImage = _finalImage;
        }

        private void ReleaseFrame()
        {
            try
            {
                _deskDupl.ReleaseFrame();
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Failure)
                {
                    throw new DesktopDuplicationException("Failed to release frame.");
                }
            }
        }

    }
}