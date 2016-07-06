using System.Runtime.Serialization;

namespace Frdp.Wcf.Contract.MainChannel
{
    [DataContract]
    public class WcfClientSettings
    {
        [DataMember]
        public uint BlockWidth
        {
            get;
            private set;
        }

        [DataMember]
        public uint BlockHeight
        {
            get;
            private set;
        }

        [DataMember]
        public int ImageWidth
        {
            get;
            private set;
        }

        [DataMember]
        public int ImageHeight
        {
            get;
            private set;
        }

        [DataMember]
        public int BlockCountHorizontal
        {
            get;
            private set;
        }

        [DataMember]
        public int BlockCountVertical
        {
            get;
            private set;
        }

        [DataMember]
        public int TotalBlockCount
        {
            get;
            private set;
        }

        public WcfClientSettings()
        {
        }

        public WcfClientSettings(uint blockWidth, uint blockHeight, int imageWidth, int imageHeight, int blockCountHorizontal, int blockCountVertical, int totalBlockCount)
        {
            BlockWidth = blockWidth;
            BlockHeight = blockHeight;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            BlockCountHorizontal = blockCountHorizontal;
            BlockCountVertical = blockCountVertical;
            TotalBlockCount = totalBlockCount;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "blockWidth {0}, blockHeight {1}, imageWidth {2}, imageHeight {3}, blockCountHorizontal {4}, blockCountVertical {5}, totalBlockCount {6}", 
                    BlockWidth, 
                    BlockHeight, 
                    ImageWidth, 
                    ImageHeight, 
                    BlockCountHorizontal, 
                    BlockCountVertical, 
                    TotalBlockCount
                );
        }
    }
}