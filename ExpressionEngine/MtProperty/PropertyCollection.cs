﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MetraTech.ExpressionEngine.TypeSystem;

namespace MetraTech.ExpressionEngine
{
    /// <summary>
    /// TO DO:
    /// *Should property names be case sensitive???
    /// </summary>
    [DataContract (Namespace = "MetraTech")]
    [KnownType(typeof(Property))]
    [KnownType(typeof(Entity))]
    public class PropertyCollection : IEnumerable<IProperty>
    {
        #region Properties

        public object Parent { get { return _parent; } }
        private readonly object _parent;

        /// <summary>
        /// The Entity to which the collection belongs (may be null)
        /// </summary>
        public Entity Entity { get { return Parent == null || !(Parent is Entity) ? null : (Entity)Parent; } }

        /// <summary>
        /// The number of properties
        /// </summary>
        public int Count { get { return Properties.Count; } }

        /// <summary>
        /// Internal list. Kept private to reduce what a developer has access to
        /// </summary>
        [DataMember]
        private List<IProperty> Properties = new List<IProperty>();
        #endregion

        #region Constructors
        public PropertyCollection(object parent)
        {
            _parent = parent;
        }
        #endregion

        #region Methods

        //TODO: Combine with override once I get it all working!
        public IProperty Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            foreach (var property in Properties)
            {
                if (name == property.Name)
                    return property;
            }
            return null;
        }

        public bool Exists(string name)
        {
            return (GetValue(name) != null);
        }

        /// <summary>
        /// Returns the value for the specified property name. If the property isn't found, null is returned
        /// </summary>
        public string GetValue(string name)
        {
            var property = Get(name);
            if (property == null)
                return null;
            return property.Value;
        }


        /// <summary>
        /// Clears all of the properties
        /// </summary>
        public void Clear()
        {
            Properties.Clear();
        }

        public ValidationMessageCollection Validate()
        {
            return Validate(null);
        }
        public ValidationMessageCollection Validate(ValidationMessageCollection messages)
        {
            if (messages == null)
                messages = new ValidationMessageCollection();

            var names = new Dictionary<string, bool>();
            foreach (var property in Properties)
            {
                //Ensure the that property names are unique
                if (names.ContainsKey(property.Name))
                {
                    messages.Error(string.Format(Localization.DuplicatePropertyName, property.Name));
                    continue;
                }
                names.Add(property.Name, false);

                //Validate each property and prefix with property identifier
                property.Validate(true, messages);

            }
            return messages;
        }

        public void Add(IProperty property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (property is Property)
                ((Property)property).PropertyCollection = this;
            Properties.Add(property);
        }
       
        public Property AddString(string name, string description, bool isRequired, string defaultValue=null, int length=0)
        {
            var property = new Property(name, TypeFactory.CreateString(length), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }

        public Property AddEnum(string name, string description, bool isRequired, string enumSpace, string enumType, string defaultValue = null)
        {
            var property = new Property(name, TypeFactory.CreateEnumeration(enumSpace, enumType), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }

        public Property AddInteger32(string name, string description, bool isRequired, string defaultValue = null)
        {
            var property = new Property(name, TypeFactory.CreateInteger32(), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }

        public Property AddDateTime(string name, string description, bool isRequired, string defaultValue = null)
        {
            var property = new Property(name, TypeFactory.CreateDateTime(), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }

        public Property AddDecimal(string name, string description, bool isRequired, string defaultValue = null)
        {
            var property = new Property(name, TypeFactory.CreateDecimal(), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }


        public Property AddCharge(string name, string description, bool isRequired, string defaultValue = null)
        {
            var property = new Property(name, TypeFactory.CreateMoney(), description);
            property.Required = isRequired;
            property.DefaultValue = defaultValue;
            Add(property);
            return property;
        }

        public PropertyCollection Clone()
        {
            var newCollection = new PropertyCollection(null);
            foreach (IProperty property in Properties)
            {
                var newProperty = property.Clone();
                newCollection.Add((IProperty)newProperty);
            }
            return newCollection;
        }
        #endregion

        #region IEnumerable Methods
        IEnumerator<IProperty> IEnumerable<IProperty>.GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
