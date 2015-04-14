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

namespace Polenter.Serialization.Core.Binary
{
    /// <summary>
    ///   These elements are used during the binary serialization. They should be unique from SubElements and Attributes.
    /// </summary>
    public static class Elements
    {
        ///<summary>
        ///</summary>
        public const byte Collection = 1;
        
        ///<summary>
        ///</summary>
        public const byte ComplexObject = 2;
        
        ///<summary>
        ///</summary>
        public const byte Dictionary = 3;
        
        ///<summary>
        ///</summary>
        public const byte MultiArray = 4;
        
        ///<summary>
        ///</summary>
        public const byte Null = 5;
        
        ///<summary>
        ///</summary>
        public const byte SimpleObject = 6;
        
        ///<summary>
        ///</summary>
        public const byte SingleArray = 7;
        
        ///<summary>
        /// For binary compatibility reason extra type-id: same as ComplexObjectWith, but contains 
        ///</summary>
        public const byte ComplexObjectWithId = 8;
        
        ///<summary>
        /// reference to previosly serialized  ComplexObjectWithId
        ///</summary>
        public const byte Reference = 9;
        
        ///<summary>
        ///</summary>
        public const byte CollectionWithId = 10;
        
        ///<summary>
        ///</summary>
        public const byte DictionaryWithId = 11;
        
        ///<summary>
        ///</summary>
        public const byte SingleArrayWithId = 12;
        
        ///<summary>
        ///</summary>
        public const byte MultiArrayWithId = 13;
        
        ///<summary>
        ///</summary>
        ///<param name="elementId"></param>
        ///<returns></returns>
        public static bool IsElementWithId(byte elementId)
        {
            if (elementId == ComplexObjectWithId)
            {
                return true;
            }
            if (elementId == CollectionWithId)
            {
                return true;
            }
            if (elementId == DictionaryWithId)
            {
                return true;
            }
            if (elementId == SingleArrayWithId)
            {
                return true;
            }
            if (elementId == MultiArrayWithId)
            {
                return true;
            }
            return false;
        }
    }
}