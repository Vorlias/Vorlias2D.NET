﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorliasEngine2D.Serialization
{
    public enum SerializedPropertyType
    {
        String,
        Int32,
        Int64,
        UInt32,
        Float,
        Double,
        Vector2,
        UICoordinates,
        Enum,
        Polygon,
        Bool,
    }

    /// <summary>
    /// A property that can be serialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PersistentPropertyAttribute : Attribute
    {
        public SerializedPropertyType PropertyType
        {
            get;
            set;
        }

        public PersistentPropertyAttribute(string propertyName)
        {

        }
    }
}
