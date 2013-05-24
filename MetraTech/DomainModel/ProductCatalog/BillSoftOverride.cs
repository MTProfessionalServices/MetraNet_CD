using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MetraTech.DomainModel.Common;
using MetraTech.DomainModel.BaseTypes;
using System.Web.Script.Serialization;

namespace MetraTech.DomainModel.ProductCatalog
{
    /// <summary>
    /// A BillSoftOverride defines the criteria that determines if the taxes for a transaction
    /// should be overridden, and the tax rate if the criteria is met.
    /// </summary>
  [DataContract]
  [Serializable]
  public class BillSoftOverride : BaseObject
  {
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isUniqueIdDirty = false;
    private int m_UniqueId;
    /// <summary>
    /// Unique ID generated by the DB when a BillSoftOverride is created.
    /// Clients cannot change this value.
    /// </summary>
    [MTDataMember(Description = "Unique identifier of the BillSoftOverride", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int UniqueId
    {
      get { return m_UniqueId; }
      set
      {
          m_UniqueId = value;
          isUniqueIdDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsUniqueIdDirty
    {
      get { return isUniqueIdDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isApplyToAccountAndDescendentsDirty = false;
    private bool m_ApplyToAccountAndDescendents;
    /// <summary>
    /// Set this to true if you would like this override to apply to the specified id_acc
    /// and all descendents.  Set this to false if you would like this override
    /// to only apply to the specified id_acc.
    /// </summary>
    [MTDataMember(Description = "apply override to id_acc and descendents", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool ApplyToAccountAndDescendents
    {
      get { return m_ApplyToAccountAndDescendents; }
      set
      {
          m_ApplyToAccountAndDescendents = value;
          isApplyToAccountAndDescendentsDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsApplyToAccountAndDescendentsDirty
    {
      get { return isApplyToAccountAndDescendentsDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isAccountIdDirty = false;
    private int m_AccountId;
    /// <summary>
    /// Either the root account id within a hierarchy or a specific account id
    /// depending on the value of ApplyToAccountAndDescendents.
    /// </summary>
    [MTDataMember(Description = "root account within hierarchy or specific account", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int AccountId
    {
      get { return m_AccountId; }
      set
      {
          m_AccountId = value;
          isAccountIdDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsAccountIdDirty
    {
      get { return isAccountIdDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isPermanentLocationCodeDirty = false;
    private int m_PermanentLocationCode;
    /// <summary>
    /// The PermanentLocationCode (or pcode) is converted by BillSoft into a JCode.
    /// The override JCode is the JCode associated with the jurisdictional level of the 
    /// taxing authority that defines the tax. It is used to exempt all federal, state, 
    /// county and / or local taxes. If the override JCode fields are not 
    /// specified then all taxes are exempt at that level.
    /// To pass an individual tax override using the JCode Override, 
    /// use the Tax Exempt structure to specify the tax type, tax level 
    /// and the JCode for the jurisdiction. Refer to Section 10.9.
    /// NOTE: JCodes are an internal intermediate Jurisdiction Code that can change monthly. 
    /// The JCode can be obtained from a PCode or an address using like functions listed in Section 7.
    /// </summary>
    [MTDataMember(Description = "Permanent location code associated with this override", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int PermanentLocationCode
    {
      get { return m_PermanentLocationCode; }
      set
      {
          m_PermanentLocationCode = value;
          isPermanentLocationCodeDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsPermanentLocationCodeDirty
    {
      get { return isPermanentLocationCodeDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isScopeDirty = false;
    private DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel m_Scope;
    /// <summary>
    /// The Scope defines the magnitude of the override.  For example, if the taxLevel=Federal
    /// and the scope=State, this indicates that there is a federal tax override within a specified state.  /// </summary>
    [MTDataMember(Description = "Scope associated with this override", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel Scope
    {
      get { return m_Scope; }
      set
      {
          m_Scope = value;
          isScopeDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsScopeDirty
    {
      get { return isScopeDirty; }
    }

    public string ScopeValueDisplayName
    {
      get
      {
        return GetDisplayName(this.Scope);
      }
      set
      {
        this.Scope = ((DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel)(GetEnumInstanceByDisplayName(typeof(DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel), value)));
      }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isTaxTypeDirty = false;
    private int m_TaxType;
    /// <summary>
    /// Integer value assigned by BillSoft for different tax types.  For example,
    /// SALES_TAX, BUSINESS_OCCUPATION_TAX, CARRIER_GROSS_RECEIPTS, DISTRICT_TAX, EXCISE_TAX.
    /// For a full list of the tax types, see section 4.3.5 of EZTaxUsersManualGen.pdf.
    /// </summary>
    [MTDataMember(Description = "Type of tax that is exempt", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int TaxType
    {
      get { return m_TaxType; }
      set
      {
          m_TaxType = value;
          isTaxTypeDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsTaxTypeDirty
    {
      get { return isTaxTypeDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isTaxLevelDirty = false;
    private DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel m_TaxLevel;
    /// <summary>
    /// Defines what level of government this override applies to.
    /// e.g. exempt from STATE taxes
    /// </summary>
    [MTDataMember(Description = "Type of tax that is exempt", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel TaxLevel
    {
      get { return m_TaxLevel; }
      set
      {
          m_TaxLevel = value;
          isTaxLevelDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsTaxLevelDirty
    {
      get { return isTaxLevelDirty; }
    }

    public string TaxLevelValueDisplayName
    {
      get
      {
        return GetDisplayName(this.TaxLevel);
      }
      set
      {
        this.TaxLevel = ((DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel)(GetEnumInstanceByDisplayName(typeof(DomainModel.Enums.Tax.Metratech_com_tax.BillSoftTaxLevel), value)));
      }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isEffectiveDateDirty = false;
    private DateTime m_EffectiveDate;
    /// <summary>
    /// The Date field is used to define the effective date that the tax rate 
    /// is active. The transaction date is compared to the effective date to 
    /// determine if the current tax rate or the previous tax rate is to be applied.
    /// </summary>
    [MTDataMember(Description = "effective date", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DateTime EffectiveDate
    {
      get { return m_EffectiveDate; }
      set
      {
          m_EffectiveDate = value;
          isEffectiveDateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsEffectiveDateDirty
    {
      get { return isEffectiveDateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isExemptLevelDirty = false;
    private bool m_ExemptLevel;
    /// <summary>
    /// The Exempt Level indicator is used to define whether a tax can be 
    /// exempted by an exemption for all taxes at the same level as this tax. 
    /// TRUE indicates that the tax can be exempted while FALSE indicates 
    /// that it cannot be exempted. Note that the tax can still be exempted 
    /// by a specific tax exemption.  For instance, a state level 
    /// universal service fund will be exempt if this field is set TRUE 
    /// and a state level tax exemption flag is passed to EZTax. If the 
    /// flag is set TRUE an exemption at the level of the tax will exempt 
    /// the tax. If the flag is set FALSE an exemption at the level of the 
    /// tax will have no effect on that specific tax.
    /// </summary>
    [MTDataMember(Description = "exempt level indicator.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool ExemptLevel
    {
      get { return m_ExemptLevel; }
      set
      {
          m_ExemptLevel = value;
          isExemptLevelDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsExemptLevelDirty
    {
      get { return isExemptLevelDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isMaximumBaseDirty = false;
    private decimal m_MaximumBase;
    /// <summary>
    /// The maximum base defines the maximum amount that the rate is applied to, for example if the following was set:
    /// amount = $20
    /// rate = 0.50 (50% tax rate)
    /// maximum =  $10
    /// If the regular rate is 10%
    /// Then for a charge (amount) of $20, the first $10 will be taxed at a rate of 50% or $5 tax plus 10% of the final $10 or a tax of $1; thus the total tax in this scenario would be $6.
    /// It is possible to set up to two bands of the same override with different maximum values. Use -1 here to represent infinity as a maximum.
    /// </summary>
    [MTDataMember(Description = "exempt level indicator.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public decimal MaximumBase
    {
      get { return m_MaximumBase; }
      set
      {
          m_MaximumBase = value;
          isMaximumBaseDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsMaximumBaseDirty
    {
      get { return isMaximumBaseDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isReplaceTaxLevelDirty = false;
    private bool m_ReplaceTaxLevel;
    /// <summary>
	/// If set to 'TRUE' the TaxLevel will be completely replaced by the tax_rate. For example 
    /// this option is made available for the rare occasion when a Locality, County, or 
    /// State sales tax replaces the sales tax completely.
    /// </summary>
    [MTDataMember(Description = "replace tax level.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool ReplaceTaxLevel
    {
      get { return m_ReplaceTaxLevel; }
      set
      {
          m_ReplaceTaxLevel = value;
          isReplaceTaxLevelDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsReplaceTaxLevelDirty
    {
      get { return isReplaceTaxLevelDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isExcessTaxRateDirty = false;
    private decimal m_ExcessTaxRate;
    /// <summary>
    /// Tax rate for amount above the Maximum Base
    /// </summary>
    [MTDataMember(Description = "tax rate for the amount above the maximum base.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public decimal ExcessTaxRate
    {
      get { return m_ExcessTaxRate; }
      set
      {
          m_ExcessTaxRate = value;
          isExcessTaxRateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsExcessTaxRateDirty
    {
      get { return isExcessTaxRateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isTaxRateDirty = false;
    private decimal m_TaxRate;
    /// <summary>
    /// Tax rate.  Example .15 = 15%.
    /// </summary>
    [MTDataMember(Description = "tax rate", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public decimal TaxRate
    {
      get { return m_TaxRate; }
      set
      {
          m_TaxRate = value;
          isTaxRateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsTaxRateDirty
    {
      get { return isTaxRateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isLimitIndicatorDirty = false;
    private decimal m_LimitIndicator;
    /// <summary>
    /// The limit indicator is only used for taxes applied per line or location. 
    /// When this field is set to zero it indicates no limits are in effect 
    /// for the specified tax. When the limit is not zero, EZTax will 
    /// apply the tax based upon the number of lines or locations up to, 
    /// but never exceeding, the limit amount. This has no effect on sales taxes.
    /// </summary>
    [MTDataMember(Description = "limit indicator.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public decimal LimitIndicator
    {
      get { return m_LimitIndicator; }
      set
      {
          m_LimitIndicator = value;
          isLimitIndicatorDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsLimitIndicatorDirty
    {
      get { return isLimitIndicatorDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isCreateDateDirty = false;
    private DateTime m_CreateDate;
    /// <summary>
    /// Date when this override was created.  This value can be read by clients, but cannot be set.
    /// </summary>
    [MTDataMember(Description = "Date when this override was created", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DateTime CreateDate
    {
      get { return m_CreateDate; }
      set
      {
          m_CreateDate = value;
          isCreateDateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsCreateDateDirty
    {
      get { return isCreateDateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isUpdateDateDirty = false;
    private DateTime? m_UpdateDate;
    /// <summary>
    /// Date when this override was last updated.  This value can be read by clients, but cannot be set.
    /// </summary>
    [MTDataMember(Description = "Date when this override was last updated", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DateTime? UpdateDate
    {
      get { return m_UpdateDate; }
      set
      {
          m_UpdateDate = value;
          isUpdateDateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsUpdateDateDirty
    {
      get { return isUpdateDateDirty; }
    }
  }
}
