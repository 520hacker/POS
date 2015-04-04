using System;

namespace POS.Models
{
    public class Coupon
    {
        public string Code { get; set; }
        public bool IsValid { get { return Validate(this); } }
        public DateTime ExpireDate { get; set; }
        public bool IsExpired { get { return DateTime.Today <= ExpireDate; } }

        public static Coupon NewCoupon()
        {
            return new Coupon { Code = Guid.NewGuid().ToString("N") };
        }

        public static bool Validate(Coupon c)
        {
            Guid result;

            return Guid.TryParseExact(c.Code, "N", out result);
        }
    }
}