using System.Collections.Generic;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Contain info about array (i.e. how many dimensions, lower/upper bounds)
    /// </summary>
    public sealed class ArrayInfo
    {
        private IList<DimensionInfo> _dimensionInfos;

        ///<summary>
        ///</summary>
        public IList<DimensionInfo> DimensionInfos
        {
            get
            {
                if (this._dimensionInfos == null)
                {
                    this._dimensionInfos = new List<DimensionInfo>();
                }
                return this._dimensionInfos;
            }
            set
            {
                this._dimensionInfos = value;
            }
        }
    }
}