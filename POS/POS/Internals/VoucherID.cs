using System;

namespace POS.Internals
{
    public struct VoucherID
    {
        private string _id;

        public static bool IsEmpty(VoucherID id)
        {
            return id._id.Length == 0;
        }

        public static VoucherID Empty
        {
            get
            {
                return new VoucherID { _id = "0000000" };
            }
        }

        public static VoucherID NewID()
        {
            var rndm = new Random();
            var ret = new VoucherID();

            var count = rndm.Next(5, 8);
            for (int i = 1; i <= count; i++)
            {
                var num = rndm.Next(0, 9);
                ret._id += num.ToString();
            }

            return ret;
        }

        public static VoucherID Parse(string src)
        {
            VoucherID value;
            if (TryParse(src, out value))
            {
                return value;
            }
            else
            {
                throw new FormatException("String is not a valid VoucherID");
            }
        }

        public static bool TryParse(string src, out VoucherID result)
        {
            var ret = new VoucherID();

            if (src.Length >= 5 && src.Length <= 8)
            {
                ret._id = src;
                result = ret;

                return true;
            }

            result = Empty;

            return false;
        }

        public static explicit operator String(VoucherID id)
        {
            return id.ToString();
        }

        public static explicit operator VoucherID(string id)
        {
            return VoucherID.Parse(id);
        }

        public override string ToString()
        {
            return _id;
        }
    }
}