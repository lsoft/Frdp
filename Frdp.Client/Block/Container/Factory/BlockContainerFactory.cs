using System;
using Frdp.Client.Block.Cutter;
using Frdp.Client.Block.Cutter.Settings;
using Frdp.Client.Crc;
using Frdp.Client.ScreenshotContainer.Factory;

namespace Frdp.Client.Block.Container.Factory
{
    public class BlockContainerFactory : IBlockContainerFactory
    {
        private readonly IBlockSettingsFactory _blockSettingsFactory;
        private readonly IScreenshotContainerFactory _screenshotContainerFactory;
        private readonly ICutterFactory _cutterFactory;
        private readonly ICrcCalculator _crcCalculator;

        public BlockContainerFactory(
            IBlockSettingsFactory blockSettingsFactory,
            IScreenshotContainerFactory screenshotContainerFactory,
            ICutterFactory cutterFactory,
            ICrcCalculator crcCalculator
            )
        {
            if (blockSettingsFactory == null)
            {
                throw new ArgumentNullException("blockSettingsFactory");
            }
            if (screenshotContainerFactory == null)
            {
                throw new ArgumentNullException("screenshotContainerFactory");
            }
            if (cutterFactory == null)
            {
                throw new ArgumentNullException("cutterFactory");
            }
            if (crcCalculator == null)
            {
                throw new ArgumentNullException("crcCalculator");
            }

            _blockSettingsFactory = blockSettingsFactory;
            _screenshotContainerFactory = screenshotContainerFactory;
            _cutterFactory = cutterFactory;
            _crcCalculator = crcCalculator;
        }

        public IBlockContainer CreateBlockContainer()
        {
            var blockSettings = _blockSettingsFactory.CreateBlockSettings();

            using (var screenshot = _screenshotContainerFactory.TakeScreenShot())
            {
                using (var mat = screenshot.ConvertToMinimizedMat())
                {
                    int blockCountHorizontal;
                    int blockCountVertical;
                    int totalBlockCount;
                    blockSettings.CalculateBlockCount(
                        mat.Width,
                        mat.Height,
                        out blockCountHorizontal,
                        out blockCountVertical,
                        out totalBlockCount
                        );

                    var blockContainer = new BlockContainer(
                        blockSettings,
                        mat.Width,
                        mat.Height,
                        blockCountHorizontal,
                        blockCountVertical
                        );

                    var cutter = _cutterFactory.CreateCutter(
                        blockContainer
                        );

                    //downgrade to less colors (8 or 16 or something else)

                    //var before = DateTime.Now;
                    //for (var cddc = 0; cddc < 500; cddc++)
                    {
                        cutter.DoCut(
                            mat
                            );
                    }
                    //var after = DateTime.Now;
                    //MessageBox.Show((after - before).ToString());

                    #region save images

                    /*

                    unsafe
                    {
                        var m00 = new Mat((int)_blockSettings.BlockHeight, (int)_blockSettings.BlockWidth, MatType.CV_8UC1);
                        for (var cc = 0; cc < _blockSettings.BlockHeight * _blockSettings.BlockWidth; cc++)
                        {
                            m00.DataPointer[cc] = blockContainer.BlockData[cc];
                        }

                        m00.SaveImage("_b0-0.bmp");

                        var m11 = new Mat((int)_blockSettings.BlockHeight, (int)_blockSettings.BlockWidth, MatType.CV_8UC1);
                        for (var cc = 0; cc < _blockSettings.BlockHeight * _blockSettings.BlockWidth; cc++)
                        {
                            m11.DataPointer[cc] = blockContainer.BlockData[(blockCountHorizontal + 1) * _blockSettings.SingleBlockSize + cc];
                        }

                        m11.SaveImage("_b1-1.bmp");

                        var mlast = new Mat((int)_blockSettings.BlockHeight, (int)_blockSettings.BlockWidth, MatType.CV_8UC1);
                        for (var cc = 0; cc < _blockSettings.BlockHeight * _blockSettings.BlockWidth; cc++)
                        {
                            mlast.DataPointer[cc] = blockContainer.BlockData[(totalBlockCount - 1) * _blockSettings.SingleBlockSize + cc];
                        }

                        mlast.SaveImage("_blast.bmp");
                    }

                    unsafe
                    {
                        var mmm = new Mat(
                            (int)_blockSettings.BlockHeight * blockCountVertical,
                            (int)_blockSettings.BlockWidth * blockCountHorizontal,
                            MatType.CV_8UC1
                            );

                        for (var h = 0; h < blockCountVertical; h++)
                        {
                            for (var w = 0; w < blockCountHorizontal; w++)
                            {
                                var blockindex = h*blockCountHorizontal + w;

                                for (var hh = 0; hh < _blockSettings.BlockHeight; hh++)
                                {
                                    for (var ww = 0; ww < _blockSettings.BlockWidth; ww++)
                                    {
                                        var toy = h * _blockSettings.BlockHeight + hh;
                                        var tox = w * _blockSettings.BlockWidth + ww;

                                        var fromy = hh;
                                        var fromx = ww;

                                        mmm.DataPointer[
                                            toy*mmm.Width + tox
                                            ] = blockContainer.BlockData[blockindex * _blockSettings.SingleBlockSize + (fromy * _blockSettings.BlockWidth + fromx)];
                                    }
                                }
                            }
                        }

                        mmm.SaveImage(
                            "_reconstructed.bmp"
                            );
                    }

                    screenshot.Save(
                        "_normal.bmp"
                        );

                    mat.SaveImage(
                        "_mini.bmp"
                        );

                    //*/

                    #endregion

                    //считаем контрольную сумму всех блоков
                    blockContainer.CalculateCrc(
                        _crcCalculator
                        );

                    return
                        blockContainer;
                }
            }
        }

    }
}
