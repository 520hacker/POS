#region Copyright © 2010 Pawel Idzikowski [idzikowski@sharpserializer.com]
//  ***********************************************************************
//  Project: sharpSerializer
//  Web: http://www.sharpserializer.com
//  
//  This software is provided 'as-is', without any express or implied warranty.
//  In no event will the author(s) be held liable for any damages arising from
//  the use of this software.
//  
//  Permission is granted to anyone to use this software for any purpose,
//  including commercial applications, and to alter it and redistribute it
//  freely, subject to the following restrictions:
//  
//      1. The origin of this software must not be misrepresented; you must not
//        claim that you wrote the original software. If you use this software
//        in a product, an acknowledgment in the product documentation would be
//        appreciated but is not required.
//  
//      2. Altered source versions must be plainly marked as such, and must not
//        be misrepresented as being the original software.
//  
//      3. This notice may not be removed or altered from any source distribution.
//  
//  ***********************************************************************
#endregion
using System;
using System.Collections.Generic;
using Polenter.Serialization.Advanced;
using Polenter.Serialization.Advanced.Serializing;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Base class for the settings of the SharpSerializer. Is passed to its constructor.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public abstract class SharpSerializerSettings<T> where T : AdvancedSharpSerializerSettings, new()
    {
        private T _advancedSettings;
        
        /// <summary>
        /// IncludeAssemblyVersionInTypeName, IncludeCultureInTypeName and IncludePublicKeyTokenInTypeName are true
        /// </summary>
        protected SharpSerializerSettings()
        {
            this.IncludeAssemblyVersionInTypeName = true;
            this.IncludeCultureInTypeName = true;
            this.IncludePublicKeyTokenInTypeName = true;
        }
        
        /// <summary>
        ///   Contains mostly classes from the namespace Polenter.Serialization.Advanced
        /// </summary>
        public T AdvancedSettings
        {
            get
            {
                if (this._advancedSettings == default(T))
                {
                    this._advancedSettings = new T();
                }
                return this._advancedSettings;
            }
            set
            {
                this._advancedSettings = value;
            }
        }
        
        /// <summary>
        ///   Version=x.x.x.x will be inserted to the type name
        /// </summary>
        public bool IncludeAssemblyVersionInTypeName { get; set; }
        
        /// <summary>
        ///   Culture=.... will be inserted to the type name
        /// </summary>
        public bool IncludeCultureInTypeName { get; set; }
        
        /// <summary>
        ///   PublicKeyToken=.... will be inserted to the type name
        /// </summary>
        public bool IncludePublicKeyTokenInTypeName { get; set; }
    }
}