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
    /// A BillSoftExemption defines the criteria that determines if a transaction 
    /// will be exempt from taxes.
    ///
    /// From EZTaxUsersManualGen.pdf
    ///
    /// Specific Tax Exemptions are used to specify a specific Tax Type at a 
    /// specific Tax Level to be exempted for the current transaction. 
    /// The exemption jurisdiction code specifies the jurisdiction for the 
    /// tax exemption. If the jurisdiction code is not specified (i.e. set to zero), 
    /// then all taxes of the Tax Type and Tax Level specified are considered 
    /// exempt regardless of the jurisdiction they are calculated for. 
    /// Typically the JCode should be specified as specific tax exemptions 
    /// are normally only effective for specific jurisdictions.  Another option 
    /// allows the tax type to be set to zero, to indicate that all taxes of a 
    /// specific tax level are exempt in the specific jurisdiction.
    ///
    /// Example: transactions in Waltham, Ma (PermanentLocationCode/PCode/JCode=xxx)
    ///     is exempt from State level (TaxLevel)
    ///     for UTILITY_USER_TAX (TaxType)
    ///
    /// </summary>
  [DataContract]
  [Serializable]
  public class BillSoftExemption : BaseObject
  {
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isUniqueIdDirty = false;
    private int m_UniqueId;
    /// <summary>
    /// Unique ID generated by the DB when a BillSoftExemption is created.
    /// Clients cannot change this value.
    /// </summary>
    [MTDataMember(Description = "Unique identifier of the BillSoftExemption", Length = 40)]
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
    /// Set this to true if you would like this exemption to apply to the specified id_acc
    /// and all descendents.  Set this to false if you would like this exemption
    /// to only apply to the specified id_acc.
    /// </summary>
    [MTDataMember(Description = "apply exemption to id_acc and descendents", Length = 40)]
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
    private bool isCertificateIdDirty = false;
    private string m_CertificateId;
    /// <summary>
    /// An identification string supplied by the taxing authority
    /// for this exemption.  Used for reporting only.
    /// </summary>
    [MTDataMember(Description = "Identification string supplied by the taxing authority", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string CertificateId
    {
      get { return m_CertificateId; }
      set
      {
          m_CertificateId = value;
          isCertificateIdDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsCertificateIdDirty
    {
      get { return isCertificateIdDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isPermanentLocationCodeDirty = false;
    private int m_PermanentLocationCode;
    /// <summary>
    /// The PermanentLocationCode (or pcode) is converted by BillSoft into a JCode.
    /// The exemption JCode is the JCode associated with the jurisdictional level of the 
    /// taxing authority that defines the tax. It is used to exempt all federal, state, 
    /// county and / or local taxes. If the exemption JCode fields are not 
    /// specified then all taxes are exempt at that level.
    /// To pass an individual tax exemption using the JCode Exemption, 
    /// use the Tax Exempt structure to specify the tax type, tax level 
    /// and the JCode for the jurisdiction. Refer to Section 10.9.
    /// NOTE: JCodes are an internal intermediate Jurisdiction Code that can change monthly. 
    /// The JCode can be obtained from a PCode or an address using like functions listed in Section 7.
    /// </summary>
    [MTDataMember(Description = "Permanent location code associated with this exemption", Length = 40)]
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
    /// Defines what level of government this exemption applies to.
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
    private bool isStartDateDirty = false;
    private DateTime m_StartDate;
    /// <summary>
    /// Date when this exemption starts to take effect.
    /// </summary>
    [MTDataMember(Description = "Date when this exemption starts to take effect.", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DateTime StartDate
    {
      get { return m_StartDate; }
      set
      {
          m_StartDate = value;
          isStartDateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsStartDateDirty
    {
      get { return isStartDateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isEndDateDirty = false;
    private DateTime m_EndDate;
    /// <summary>
    /// Date when this exemption stops taking effect.
    /// </summary>
    [MTDataMember(Description = "Date when this exemption stops taking effect", Length = 40)]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public DateTime EndDate
    {
      get { return m_EndDate; }
      set
      {
          m_EndDate = value;
          isEndDateDirty = true;
      }
    }
    [ScriptIgnore]
    public bool IsEndDateDirty
    {
      get { return isEndDateDirty; }
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    private bool isCreateDateDirty = false;
    private DateTime m_CreateDate;
    /// <summary>
    /// Date when this exemption was created.  This value can be read by clients, but cannot be set.
    /// </summary>
    [MTDataMember(Description = "Date when this exemption was created", Length = 40)]
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
    /// Date when this exemption was last updated.  This value can be read by clients, but cannot be set.
    /// </summary>
    [MTDataMember(Description = "Date when this exemption was last updated", Length = 40)]
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
