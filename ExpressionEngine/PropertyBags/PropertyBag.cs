﻿using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.Serialization;
using MetraTech.ExpressionEngine.Entities;
using MetraTech.ExpressionEngine.MTProperties;
using MetraTech.ExpressionEngine.MTProperties.Enumerations;
using MetraTech.ExpressionEngine.TypeSystem;
using MetraTech.ExpressionEngine.TypeSystem.Enumerations;
using System.IO;
using Type = MetraTech.ExpressionEngine.TypeSystem.Type;

namespace MetraTech.ExpressionEngine.PropertyBags
{
    /// <summary>
    /// Implements a ComplexType, esentially something that PropertyCollection which may include properties and
    /// other complex types. Note that DataTypeInfo.IsEntity determines if it's deemed an PropertyBag (an important destinction for Metanga)
    /// </summary>
    [DataContract(Namespace = "MetraTech")]
    [KnownType(typeof(AccountViewEntity))]
    [KnownType(typeof(ProductViewEntity))]
    [KnownType(typeof(ServiceDefinitionEntity))]
    public class PropertyBag : Property, IExpressionEngineTreeNode
    {
        #region Properties

        [DataMember]
        public PropertyCollection Properties { get; private set; }

        public override string CompatibleKey
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}|{1}", Name, Type.CompatibleKey); }
        }

        public virtual string XqgPrefix { get { return null; } }

        /// <summary>
        /// The actual database table name. Used in MetraNet which has a prefix on all table names.
        /// Not sure what to do for Metanga here.
        /// </summary>
        public virtual string DBTableName
        {
            get { return Name; }
        }

        #endregion

        #region GUI Helper Properties (move in future)
        public override string ToolTip
        {
            get
            {
                var tip = ((PropertyBagType)Type).Name;
                if (!string.IsNullOrEmpty(Description))
                    tip += Environment.NewLine + Description;
                if (UserSettings.ShowActualMappings)
                    tip += string.Format(CultureInfo.InvariantCulture, "\r\n[TableName={0}]", DBTableName);
                return tip;
            }
        }

        public override string Image {get { return ((PropertyBagType) Type).PropertyBagMode.ToString() + ".png"; }}

        public override string ImageDirection
        {
            get
            {
                switch (Direction)
                {
                    case Direction.InOut:
                        return "EntityInOut.png";
                    case Direction.Input:
                        return "EntityInput.png";
                    case Direction.Output:
                        return "EntityOutput.png";
                    default:
                        return null;
                }
            }
        }
        #endregion

        #region Constructor

        public PropertyBag(string name, string propertyBagTypeName, PropertyBagMode propertyBagMode, string description)
            : base(name, TypeFactory.CreatePropertyBag(propertyBagTypeName, propertyBagMode), true, description)
        {
            Name = name;
            Type = TypeFactory.CreatePropertyBag(propertyBagTypeName, propertyBagMode);
            Description = description;
            Properties = new PropertyCollection(this);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Determines if the Properties collection exactly matches the nameFilter or the typeFilter. Useful
        /// for filtering in the GUI.
        /// TODO: Add recursive option to look sub entities
        /// </summary>
        public bool HasPropertyMatch(Regex nameFilter, Type typeFilter)
        {
            foreach (var property in Properties)
            {
                if (nameFilter != null && !nameFilter.IsMatch(property.Name))
                    continue;
                if (typeFilter != null && !property.Type.IsBaseTypeFilterMatch(typeFilter))
                    continue;
                return true;
            }
            return false;
        }

        public override string ToExpressionSnippet
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", XqgPrefix, Name);
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
            //var newEntity = new ComplexType(Name, Type.ComplexType, Description);
            //newEntity.Properties = Properties.Clone();
            //return newEntity;
        }

        public override ValidationMessageCollection Validate(bool prefixMsg)
        {
            return Validate(prefixMsg, null);
        }

        public override ValidationMessageCollection Validate(bool prefixMsg, ValidationMessageCollection messages)
        {
            if (messages == null)
                messages = new ValidationMessageCollection();

            var prefix = string.Format(CultureInfo.InvariantCulture, Localization.PropertyMessagePrefix, Name);

            //if (NameRegex.IsMatch(Name))
            //    messages.Error(prefix + Localization.InvalidName);

            Type.Validate(prefix, messages);

            foreach (var property in Properties)
            {
                property.Validate(prefixMsg, messages);
            }

            return messages;
        }
        #endregion

        #region IO Methods

        public void Save(string file)
        {
            IOHelper.Save(file, this);
        }

        public static PropertyBag CreateFromFile(string file)
        {
            var xmlContent = File.ReadAllText(file);
            return CreateFromString(xmlContent);
        }

        public static PropertyBag CreateFromString(string xmlContent)
        {
            return IOHelper.CreateFromString<PropertyBag>(xmlContent);
        }

        #endregion
    }
}
