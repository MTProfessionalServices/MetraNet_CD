//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Core
{
  using System;
  using System.Collections.Generic;
  using System.Runtime.Serialization;
  using MetraTech.Basic;
  using MetraTech.BusinessEntity.Core;
  using MetraTech.BusinessEntity.Core.Model;
  using MetraTech.BusinessEntity.DataAccess.Metadata;
  
  
  [System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
  [System.SerializableAttribute()]
  [MetraTech.BusinessEntity.Core.Model.ConfigDrivenAttribute()]
  public partial class DisputeHistory : MetraTech.BusinessEntity.DataAccess.Persistence.BaseHistory
  {
    
    private global::Core.Core.DisputeBusinessKey _disputeHistoryBusinessKey;
    
    public const string Property_Id = "Id";
    
    private System.Guid _disputeId;
    
    public const string Property_DisputeId = "DisputeId";
    
    private System.Nullable<System.Int32> _invoiceId;
    
    public const string Property_invoiceId = "invoiceId";
    
    private string _invoiceNum;
    
    public const string Property_invoiceNum = "invoiceNum";
    
    private MetraTech.DomainModel.Enums.Core.Metratech_com_Dispute.DisputeStatus _status;
    
    public const string Property_status = "status";
    
    private string _title;
    
    public const string Property_title = "title";
    
    private string _description;
    
    public const string Property_description = "description";
    
    private string _newProperty;
    
    public const string Property_NewProperty = "NewProperty";
    
    private int @__Version;
    
    public const string Property__Version = "_Version";
    
    private System.Nullable<System.DateTime> _creationDate;
    
    public const string Property_CreationDate = "CreationDate";
    
    private System.Nullable<System.DateTime> _updateDate;
    
    public const string Property_UpdateDate = "UpdateDate";
    
    private System.Nullable<System.Int32> _uID;
    
    public const string Property_UID = "UID";
    
    [MetraTech.BusinessEntity.DataAccess.Metadata.BusinessKeyAttribute()]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual global::Core.Core.DisputeBusinessKey DisputeBusinessKey
    {
      get
      {
        return this._disputeHistoryBusinessKey;
      }
      set
      {
        this._disputeHistoryBusinessKey = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual System.Guid DisputeId
    {
      get
      {
        return this._disputeId;
      }
      set
      {
        this._disputeId = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual System.Nullable<System.Int32> invoiceId
    {
      get
      {
        return this._invoiceId;
      }
      set
      {
        this._invoiceId = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string invoiceNum
    {
      get
      {
        return this._invoiceNum;
      }
      set
      {
        this._invoiceNum = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual MetraTech.DomainModel.Enums.Core.Metratech_com_Dispute.DisputeStatus status
    {
      get
      {
        return this._status;
      }
      set
      {
        this._status = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string title
    {
      get
      {
        return this._title;
      }
      set
      {
        this._title = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string description
    {
      get
      {
        return this._description;
      }
      set
      {
        this._description = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string NewProperty
    {
      get
      {
        return this._newProperty;
      }
      set
      {
        this._newProperty = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public override int _Version
    {
      get
      {
        return this.@__Version;
      }
      set
      {
        this.@__Version = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public override System.Nullable<System.DateTime> CreationDate
    {
      get
      {
        return this._creationDate;
      }
      set
      {
        this._creationDate = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public override System.Nullable<System.DateTime> UpdateDate
    {
      get
      {
        return this._updateDate;
      }
      set
      {
        this._updateDate = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual System.Nullable<System.Int32> UID
    {
      get
      {
        return this._uID;
      }
      set
      {
        this._uID = value;
      }
    }
    
    public override MetraTech.BusinessEntity.DataAccess.Metadata.DataObject GetDataObject()
    {
      var _dispute = new global::Core.Core.Dispute();
      _dispute.Id = DisputeId;
      _dispute._Version = _Version;
      _dispute.DisputeBusinessKey = DisputeBusinessKey;
      _dispute.invoiceId = invoiceId;
      _dispute.invoiceNum = invoiceNum;
      _dispute.status = status;
      _dispute.title = title;
      _dispute.description = description;
      _dispute.NewProperty = NewProperty;
      _dispute._Version = _Version;
      _dispute.CreationDate = CreationDate;
      _dispute.UpdateDate = UpdateDate;
      _dispute.UID = UID;
      return _dispute;
    }
  }
}
