using System;

namespace Polenter.Serialization.Core
{
    ///<summary>
    /// All properties derived from this property can be a target of a reference
    ///</summary>
    public abstract class ReferenceTargetProperty : Property
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        protected ReferenceTargetProperty(string name, Type type) : base(name, type)
        {
        }

        ///<summary>
        /// Information about the References for this property
        ///</summary>
        public ReferenceInfo Reference { get; set; }

        ///<summary>
        /// Makes flat copy (only references) of vital properties
        ///</summary>
        ///<param name="source"></param>
        public virtual void MakeFlatCopyFrom(ReferenceTargetProperty source)
        {
            this.Reference = source.Reference;
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        ///<exception cref="NotImplementedException"></exception>
        public override string ToString()
        {
            var text = base.ToString();
            string reference = this.Reference != null ? this.Reference.ToString() : "null";
            return string.Format("{0}, Reference={1}", text, reference);
        }
    }
}