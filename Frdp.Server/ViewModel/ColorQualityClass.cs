using System;

namespace Frdp.Server.ViewModel
{
    public class ColorQualityClass
    {
        private readonly string _description;

        public byte Mask
        {
            get;
            private set;
        }

        public ColorQualityClass(
            byte mask,
            string description
            )
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            Mask = mask;
            _description = description;
        }

        public override string ToString()
        {
            return
                _description;
        }
    }
}