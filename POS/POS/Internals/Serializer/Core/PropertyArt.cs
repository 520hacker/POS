namespace Polenter.Serialization.Core
{
    ///<summary>
    /// Of what art is the property
    ///</summary>
    public enum PropertyArt
    {
        ///<summary>
        ///</summary>
        Unknown = 0,
        ///<summary>
        ///</summary>
        Simple,
        ///<summary>
        ///</summary>
        Complex,
        ///<summary>
        ///</summary>
        Collection,
        ///<summary>
        ///</summary>
        Dictionary,
        ///<summary>
        ///</summary>
        SingleDimensionalArray,
        ///<summary>
        ///</summary>
        MultiDimensionalArray,
        ///<summary>
        ///</summary>
        Null,
        ///<summary>
        ///</summary>
        Reference
    }
}