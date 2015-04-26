using System;
using LiteDB;
using POS.Internals;

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

        [BsonId]
        public VoucherID Code { get; set; }

        public bool IsValid
        {
            get
            {
                return Validate(this);
            }
        }

        public DateTime ExpireDate { get; set; }

        [BsonIgnore]
        public bool IsExpired
        {
            get
            {
                return DateTime.Today <= this.ExpireDate;
            }
        }

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

        public static Coupon NewCoupon()
        {
            return new Coupon { Code = VoucherID.NewID() };
        }

        public static bool Validate(Coupon c)
        {
            VoucherID result;

            return VoucherID.TryParse(c.Code.ToString(), out result);
        }
    }
}