//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.FileLandingService
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Runtime.Serialization;
  using System.Linq;
  using MetraTech.Basic;
  using MetraTech.BusinessEntity.Core;
  using MetraTech.BusinessEntity.Core.Model;
  using MetraTech.BusinessEntity.DataAccess.Metadata;
  using MetraTech.BusinessEntity.DataAccess.Persistence;
  
  
  [System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
  [System.SerializableAttribute()]
  [System.Runtime.Serialization.KnownTypeAttribute("GetKnownTypes")]
  [MetraTech.BusinessEntity.Core.Model.ConfigDrivenAttribute()]
  public partial class ArgumentBE : MetraTech.BusinessEntity.DataAccess.Metadata.DataObject, global::Core.FileLandingService.Interface.IArgumentBE
  {
    
    public const string FullName = "Core.FileLandingService.ArgumentBE";
    
    private int @__Version;
    
    public const string Property__Version = "_Version";
    
    private System.Nullable<System.DateTime> _creationDate;
    
    public const string Property_CreationDate = "CreationDate";
    
    private System.Nullable<System.DateTime> _updateDate;
    
    public const string Property_UpdateDate = "UpdateDate";
    
    private System.Nullable<System.Int32> _uID;
    
    public const string Property_UID = "UID";
    
    public const string Property_Id = "Id";
    
    private global::Core.FileLandingService.ArgumentBEBusinessKey _argumentBEBusinessKey = new ArgumentBEBusinessKey();
    
    private string @__Regex;
    
    public const string Property__Regex = "_Regex";
    
    private MetraTech.DomainModel.Enums.Core.Metratech_com_FileLandingService.EConditionalType @__ConditionalFlag = MetraTech.DomainModel.Enums.Core.Metratech_com_FileLandingService.EConditionalType.ALWAYS;
    
    public const string Property__ConditionalFlag = "_ConditionalFlag";
    
    private string @__Format;
    
    public const string Property__Format = "_Format";
    
    private int @__Order = 1;
    
    public const string Property__Order = "_Order";
    
    private global::Core.FileLandingService.Interface.ITargetBE _targetBE;
    
    private IList<global::Core.FileLandingService.Interface.ITargetBE_ArgumentBE> _targetBE_ArgumentBEList = new List<global::Core.FileLandingService.Interface.ITargetBE_ArgumentBE>();
    
    public const string Relationship_TargetBE_ArgumentBE = "TargetBE_ArgumentBE";
    
    public const string Property_InternalKey = "InternalKey";
    
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
    
    [MetraTech.BusinessEntity.DataAccess.Metadata.BusinessKeyAttribute()]
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual global::Core.FileLandingService.ArgumentBEBusinessKey ArgumentBEBusinessKey
    {
      get
      {
        return this._argumentBEBusinessKey;
      }
      set
      {
        this._argumentBEBusinessKey = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string _Regex
    {
      get
      {
        return this.@__Regex;
      }
      set
      {
        this.@__Regex = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual MetraTech.DomainModel.Enums.Core.Metratech_com_FileLandingService.EConditionalType _ConditionalFlag
    {
      get
      {
        return this.@__ConditionalFlag;
      }
      set
      {
        this.@__ConditionalFlag = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string _Format
    {
      get
      {
        return this.@__Format;
      }
      set
      {
        this.@__Format = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual int _Order
    {
      get
      {
        return this.@__Order;
      }
      set
      {
        this.@__Order = value;
      }
    }
    
    public virtual global::Core.FileLandingService.Interface.ITargetBE TargetBE
    {
      get
      {
        return this._targetBE;
      }
      set
      {
        this._targetBE = value;
      }
    }
    
    public virtual IList<global::Core.FileLandingService.Interface.ITargetBE_ArgumentBE> TargetBE_ArgumentBEList
    {
      get
      {
        return this._targetBE_ArgumentBEList;
      }
      set
      {
        this._targetBE_ArgumentBEList = value;
      }
    }
    
    public override void SetupRelationships()
    {
    }
    
    public virtual object Clone()
    {
      var _argumentBE = new global::Core.FileLandingService.ArgumentBE();
      _argumentBE.ArgumentBEBusinessKey = (global::Core.FileLandingService.ArgumentBEBusinessKey)ArgumentBEBusinessKey.Clone();
      _argumentBE._Regex = _Regex;
      _argumentBE._ConditionalFlag = _ConditionalFlag;
      _argumentBE._Format = _Format;
      _argumentBE._Order = _Order;
      return _argumentBE;
    }
    
    public virtual void Save()
    {
      var item = this;
      global::MetraTech.BusinessEntity.DataAccess.Persistence.StandardRepository.Instance.SaveInstance(ref item);
    }
    
    public override void CopyPropertiesFrom(global::MetraTech.BusinessEntity.DataAccess.Metadata.DataObject dataObject)
    {
      var item = dataObject as global::Core.FileLandingService.ArgumentBE;
      _Regex = item._Regex;
      _ConditionalFlag = item._ConditionalFlag;
      _Format = item._Format;
      _Order = item._Order;
      ArgumentBEBusinessKey = item.ArgumentBEBusinessKey;
    }
    
    public static new System.Type[] GetKnownTypes()
    {
      var knownTypes = new List<System.Type>();
      knownTypes.Add(typeof(MetraTech.DomainModel.Enums.Core.Metratech_com_FileLandingService.EConditionalType));
      knownTypes.Add(typeof(global::Core.FileLandingService.ArgumentBEBusinessKey));
      return knownTypes.ToArray();
    }
  }
  
  [System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
  [System.SerializableAttribute()]
  public partial class ArgumentBEBusinessKey : MetraTech.BusinessEntity.DataAccess.Metadata.BusinessKey, global::Core.FileLandingService.Interface.IArgumentBEBusinessKey
  {
    
    private string _entityFullName = "Core.FileLandingService.ArgumentBE";
    
    private System.Guid _internalKey;
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public override string EntityFullName
    {
      get
      {
        return this._entityFullName;
      }
      set
      {
        this._entityFullName = "Core.FileLandingService.ArgumentBE";
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual System.Guid InternalKey
    {
      get
      {
        return this._internalKey;
      }
      set
      {
        this._internalKey = value;
      }
    }
    
    public override object Clone()
    {
      var _businessKey = new ArgumentBEBusinessKey();
      _businessKey.InternalKey = InternalKey;
      return _businessKey;
    }
  }
}
