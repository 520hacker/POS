using System;
using System.Collections.Generic;
using Polenter.Serialization.Advanced;
using Polenter.Serialization.Advanced.Serializing;

namespace Polenter.Serialization.Core
{
    ///<summary>
    ///</summary>
    public class AdvancedSharpSerializerSettings
    {
        private PropertiesToIgnore _propertiesToIgnore;
        private IList<Type> _attributesToIgnore;

        ///<summary>
        ///</summary>
        public AdvancedSharpSerializerSettings()
        {
            this.AttributesToIgnore.Add(typeof(ExcludeFromSerializationAttribute));
            this.RootName = "Root";
        }

        /// <summary>
        ///   Which properties should be ignored during the serialization.
        /// </summary>
        /// <remarks>
        ///   In your business objects you can mark these properties with ExcludeFromSerializationAttribute
        ///   In built in .NET Framework classes you can not do this. Therefore you define these properties here.
        ///   I.e. System.Collections.Generic.List has property Capacity which is irrelevant for
        ///   the whole Serialization and should be ignored.
        /// </remarks>
        public PropertiesToIgnore PropertiesToIgnore
        {
            get
            {
                if (this._propertiesToIgnore == null)
                {
                    this._propertiesToIgnore = new PropertiesToIgnore();
                }
                return this._propertiesToIgnore;
            }
            set
            {
                this._propertiesToIgnore = value;
            }
        }

        /// <summary>
        /// All Properties marked with one of the contained attribute-types will be ignored on save.
        /// As default, this list contains only ExcludeFromSerializationAttribute.
        /// For performance reasons it would be better to clear this list if this attribute 
        /// is not used in serialized classes.
        /// </summary>
        public IList<Type> AttributesToIgnore
        {
            get
            {
                if (this._attributesToIgnore == null)
                {
                    this._attributesToIgnore = new List<Type>();
                }
                return this._attributesToIgnore;
            }
            set
            {
                this._attributesToIgnore = value;
            }
        }

        /// <summary>
        ///   What name has the root item of your serialization. Default is "Root".
        /// </summary>
        public string RootName { get; set; }

        /// <summary>
        ///   Converts Type to string and vice versa. Default is an instance of TypeNameConverter which serializes Types as "type name, assembly name"
        ///   If you want to serialize your objects as fully qualified assembly name, you should set this setting with an instance of TypeNameConverter
        ///   with overloaded constructor.
        /// </summary>
        public ITypeNameConverter TypeNameConverter { get; set; }
    }
}