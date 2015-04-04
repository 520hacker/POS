using System;
using Creek.Database.Api;

namespace POS.Models
{
    public class Coupon
    {
        public enum CouponType
        {
            Free,
            Discount,
            Special
        }

        public string Code { get; set; }

        public bool IsValid
        {
            get
            {
                return Validate(this);
            }
        }

        public DateTime ExpireDate { get; set; }

        public bool IsExpired
        {
            get
            {
                return DateTime.Today <= ExpireDate;
            }
        }

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

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