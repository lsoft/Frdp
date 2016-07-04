using System.IO;
using System.IO.Compression;
using System.ServiceModel.Channels;


namespace System.ServiceModel.Extensions.Compression
{
    internal class CompressionMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly CompressionAlgorithm _compressionAlgorithm;

        private readonly long _maxReceivedMessageSize;

        private readonly MessageEncoder _encoder;

        private readonly MessageEncoderFactory _innerFactory;


        public CompressionMessageEncoderFactory(MessageEncoderFactory messageEncoderFactory, CompressionAlgorithm compressionAlgorithm, long maxReceivedMessageSize)
        {
            if (messageEncoderFactory == null)
            {
                throw new ArgumentNullException(
                    "messageEncoderFactory",
                    "A valid message encoder factory must be passed to the CompressionMessageEncoderFactory");
            }

            _compressionAlgorithm = compressionAlgorithm;
            _maxReceivedMessageSize = maxReceivedMessageSize;
            _innerFactory = messageEncoderFactory;

            _encoder = new CompressionMessageEncoder(messageEncoderFactory.Encoder, compressionAlgorithm, _maxReceivedMessageSize);
        }


        public override MessageEncoder Encoder
        {
            get { return _encoder; }
        }


        public override MessageVersion MessageVersion
        {
            get { return _encoder.MessageVersion; }
        }


        public override MessageEncoder CreateSessionEncoder()
        {
            return new CompressionMessageEncoder(_innerFactory.CreateSessionEncoder(), _compressionAlgorithm, _maxReceivedMessageSize);
        }


        private class CompressionMessageEncoder : MessageEncoder
        {
            private const string GZipContentType = "application/x-gzip";

            private const string DeflateContentType = "application/x-deflate";

            private readonly CompressionAlgorithm _compressionAlgorithm;

            private readonly MessageEncoder _innerEncoder;

            private readonly long _maxReceivedMessageSize;


            internal CompressionMessageEncoder(MessageEncoder messageEncoder, CompressionAlgorithm compressionAlgorithm, long maxReceivedMessageSize)
            {
                if (messageEncoder == null)
                {
                    throw new ArgumentNullException(
                        "messageEncoder",
                        "A valid message encoder must be passed to the CompressionMessageEncoder");
                }

                _innerEncoder = messageEncoder;
                _compressionAlgorithm = compressionAlgorithm;
                _maxReceivedMessageSize = maxReceivedMessageSize;
            }


            public override string ContentType
            {
                get { return _compressionAlgorithm == CompressionAlgorithm.GZip ? GZipContentType : DeflateContentType; }
            }


            public override string MediaType
            {
                get { return ContentType; }
            }


            public override MessageVersion MessageVersion
            {
                get { return _innerEncoder.MessageVersion; }
            }


            private static ArraySegment<byte> CompressBuffer(
                ArraySegment<byte> buffer,
                BufferManager bufferManager,
                int messageOffset,
                CompressionAlgorithm compressionAlgorithm)
            {
                var memoryStream = new MemoryStream();

                using (
                    Stream compressedStream = compressionAlgorithm == CompressionAlgorithm.GZip
                        ? new GZipStream(memoryStream, CompressionMode.Compress, true)
                        : (Stream) new DeflateStream(memoryStream, CompressionMode.Compress, true))
                {
                    compressedStream.Write(buffer.Array, buffer.Offset, buffer.Count);
                }

                byte[] compressedBytes = memoryStream.ToArray();
                int totalLength = messageOffset + compressedBytes.Length;
                byte[] bufferedBytes = bufferManager.TakeBuffer(totalLength);

                Array.Copy(compressedBytes, 0, bufferedBytes, messageOffset, compressedBytes.Length);

                bufferManager.ReturnBuffer(buffer.Array);
                var byteArray = new ArraySegment<byte>(bufferedBytes, messageOffset, compressedBytes.Length);

                return byteArray;
            }


            private static ArraySegment<byte> DecompressBuffer(
                ArraySegment<byte> buffer,
                BufferManager bufferManager,
                CompressionAlgorithm compressionAlgorithm)
            {
                var memoryStream = new MemoryStream(buffer.Array, buffer.Offset, buffer.Count);
                var decompressedStream = new MemoryStream();
                const int blockSize = 1024;
                byte[] tempBuffer = bufferManager.TakeBuffer(blockSize);

                using (
                    Stream compressedStream = compressionAlgorithm == CompressionAlgorithm.GZip
                        ? new GZipStream(memoryStream, CompressionMode.Decompress)
                        : (Stream) new DeflateStream(memoryStream, CompressionMode.Decompress))
                {
                    while (true)
                    {
                        int bytesRead = compressedStream.Read(tempBuffer, 0, blockSize);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        decompressedStream.Write(tempBuffer, 0, bytesRead);
                    }
                }

                bufferManager.ReturnBuffer(tempBuffer);

                byte[] decompressedBytes = decompressedStream.ToArray();
                byte[] bufferManagerBuffer = bufferManager.TakeBuffer(decompressedBytes.Length + buffer.Offset);
                Array.Copy(buffer.Array, 0, bufferManagerBuffer, 0, buffer.Offset);
                Array.Copy(decompressedBytes, 0, bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);

                var byteArray = new ArraySegment<byte>(bufferManagerBuffer, buffer.Offset, decompressedBytes.Length);
                bufferManager.ReturnBuffer(buffer.Array);

                return byteArray;
            }


            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                ArraySegment<byte> decompressedBuffer = DecompressBuffer(buffer, bufferManager, _compressionAlgorithm);

                Message returnMessage = _innerEncoder.ReadMessage(decompressedBuffer, bufferManager);
                returnMessage.Properties.Encoder = this;

                return returnMessage;
            }


            public override ArraySegment<byte> WriteMessage(
                Message message,
                int maxMessageSize,
                BufferManager bufferManager,
                int messageOffset)
            {
                ArraySegment<byte> buffer = _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, 0);

                return CompressBuffer(buffer, bufferManager, messageOffset, _compressionAlgorithm);
            }


            public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
            {
                stream =
                    new MaxMessageSizeStream(
                        _compressionAlgorithm == CompressionAlgorithm.GZip
                            ? new GZipStream(stream, CompressionMode.Decompress, false)
                            : (Stream) new DeflateStream(stream, CompressionMode.Decompress, false),
                        _maxReceivedMessageSize);

                return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders);
            }


            public override void WriteMessage(Message message, Stream stream)
            {
                stream = _compressionAlgorithm == CompressionAlgorithm.GZip
                    ? new GZipStream(stream, CompressionMode.Compress, true)
                    : (Stream) new DeflateStream(stream, CompressionMode.Compress, true);

                _innerEncoder.WriteMessage(message, stream);

                stream.Flush();
                stream.Close();
            }


            internal abstract class DelegatingStream : Stream
            {
                private readonly Stream _stream;


                protected DelegatingStream(Stream stream)
                {
                    if (stream == null)
                    {
                        throw new ArgumentNullException("stream");
                    }
                    _stream = stream;
                }


                protected Stream BaseStream
                {
                    get { return _stream; }
                }


                public override bool CanRead
                {
                    get { return _stream.CanRead; }
                }


                public override bool CanSeek
                {
                    get { return _stream.CanSeek; }
                }


                public override bool CanTimeout
                {
                    get { return _stream.CanTimeout; }
                }


                public override bool CanWrite
                {
                    get { return _stream.CanWrite; }
                }


                public override long Length
                {
                    get { return _stream.Length; }
                }


                public override long Position
                {
                    get { return _stream.Position; }
                    set { _stream.Position = value; }
                }


                public override int ReadTimeout
                {
                    get { return _stream.ReadTimeout; }
                    set { _stream.ReadTimeout = value; }
                }


                public override int WriteTimeout
                {
                    get { return _stream.WriteTimeout; }
                    set { _stream.WriteTimeout = value; }
                }


                public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
                {
                    return _stream.BeginRead(buffer, offset, count, callback, state);
                }


                public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
                {
                    return _stream.BeginWrite(buffer, offset, count, callback, state);
                }


                public override void Close()
                {
                    _stream.Close();
                }


                public override int EndRead(IAsyncResult result)
                {
                    return _stream.EndRead(result);
                }


                public override void EndWrite(IAsyncResult result)
                {
                    _stream.EndWrite(result);
                }


                public override void Flush()
                {
                    _stream.Flush();
                }


                public override int Read(byte[] buffer, int offset, int count)
                {
                    return _stream.Read(buffer, offset, count);
                }


                public override int ReadByte()
                {
                    return _stream.ReadByte();
                }


                public override long Seek(long offset, SeekOrigin origin)
                {
                    return _stream.Seek(offset, origin);
                }


                public override void SetLength(long value)
                {
                    _stream.SetLength(value);
                }


                public override void Write(byte[] buffer, int offset, int count)
                {
                    _stream.Write(buffer, offset, count);
                }


                public override void WriteByte(byte value)
                {
                    _stream.WriteByte(value);
                }
            }


            internal class MaxMessageSizeStream : DelegatingStream
            {
                private readonly long _maxMessageSize;

                private long _bytesWritten;

                private long _totalBytesRead;


                public MaxMessageSizeStream(Stream stream, long maxMessageSize)
                    : base(stream)
                {
                    _maxMessageSize = maxMessageSize;
                }


                public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
                {
                    count = PrepareRead(count);
                    return base.BeginRead(buffer, offset, count, callback, state);
                }


                public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
                {
                    PrepareWrite(count);
                    return base.BeginWrite(buffer, offset, count, callback, state);
                }


                public override int EndRead(IAsyncResult result)
                {
                    return FinishRead(base.EndRead(result));
                }


                public override int Read(byte[] buffer, int offset, int count)
                {
                    count = PrepareRead(count);
                    return FinishRead(base.Read(buffer, offset, count));
                }


                public override int ReadByte()
                {
                    PrepareRead(1);
                    int i = base.ReadByte();
                    if (i != -1)
                    {
                        FinishRead(1);
                    }
                    return i;
                }


                public override void Write(byte[] buffer, int offset, int count)
                {
                    PrepareWrite(count);
                    base.Write(buffer, offset, count);
                }


                public override void WriteByte(byte value)
                {
                    PrepareWrite(1);
                    base.WriteByte(value);
                }


                private static Exception CreateMaxSentMessageSizeExceededException(long maxMessageSize)
                {
                    string message = string.Format("MaxSentMessageSizeExceeded:{0}", maxMessageSize);
                    Exception inner = new QuotaExceededException(message);

                    return new CommunicationException(message, inner);
                }


                private int PrepareRead(int bytesToRead)
                {
                    if (_totalBytesRead >= _maxMessageSize)
                    {
                        throw CreateMaxReceivedMessageSizeExceededException(_maxMessageSize);
                    }

                    long bytesRemaining = _maxMessageSize - _totalBytesRead;

                    if (bytesRemaining > int.MaxValue)
                    {
                        return bytesToRead;
                    }
                    return Math.Min(bytesToRead, (int) (_maxMessageSize - _totalBytesRead));
                }


                private int FinishRead(int bytesRead)
                {
                    _totalBytesRead += bytesRead;
                    return bytesRead;
                }


                private void PrepareWrite(int bytesToWrite)
                {
                    if (_bytesWritten + bytesToWrite > _maxMessageSize)
                    {
                        throw (CreateMaxSentMessageSizeExceededException(_maxMessageSize));
                    }

                    _bytesWritten += bytesToWrite;
                }


                private static Exception CreateMaxReceivedMessageSizeExceededException(long maxMessageSize)
                {
                    string @string = string.Format("Max received message size exceeded: {0}}", maxMessageSize);
                    Exception innerException = new QuotaExceededException(@string);
                    return new CommunicationException(@string, innerException);
                }
            }
        }
    }
}