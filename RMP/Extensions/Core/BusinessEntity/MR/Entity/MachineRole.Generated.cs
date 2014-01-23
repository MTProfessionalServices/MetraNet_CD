//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.MR
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
  public partial class MachineRole : MetraTech.BusinessEntity.DataAccess.Metadata.DataObject, global::Core.MR.Interface.IMachineRole
  {
    
    public const string FullName = "Core.MR.MachineRole";
    
    private int @__Version;
    
    public const string Property__Version = "_Version";
    
    private System.Nullable<System.DateTime> _creationDate;
    
    public const string Property_CreationDate = "CreationDate";
    
    private System.Nullable<System.DateTime> _updateDate;
    
    public const string Property_UpdateDate = "UpdateDate";
    
    private System.Nullable<System.Int32> _uID;
    
    public const string Property_UID = "UID";
    
    public const string Property_Id = "Id";
    
    private global::Core.MR.MachineRoleBusinessKey _machineRoleBusinessKey = new MachineRoleBusinessKey();
    
    private string _description;
    
    public const string Property_Description = "Description";
    
    private IList<global::Core.MR.Interface.IMachine> _machines = new List<global::Core.MR.Interface.IMachine>();
    
    private IList<global::Core.MR.Interface.IMachine_MachineRole_Machines_MachineRoles> _machine_MachineRole_Machines_MachineRolesList = new List<global::Core.MR.Interface.IMachine_MachineRole_Machines_MachineRoles>();
    
    public const string Relationship_Machines_MachineRoles = "Machines_MachineRoles";
    
    public const string Property_Name = "Name";
    
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
    public virtual global::Core.MR.MachineRoleBusinessKey MachineRoleBusinessKey
    {
      get
      {
        return this._machineRoleBusinessKey;
      }
      set
      {
        this._machineRoleBusinessKey = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string Description
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
    
    public virtual List<global::Core.MR.Interface.IMachine> Machines
    {
      get
      {
        return _machines as List<global::Core.MR.Interface.IMachine>;
      }
      set
      {
        this._machines = value;
      }
    }
    
    public virtual IList<global::Core.MR.Interface.IMachine_MachineRole_Machines_MachineRoles> Machine_MachineRole_Machines_MachineRolesList
    {
      get
      {
        return this._machine_MachineRole_Machines_MachineRolesList;
      }
      set
      {
        this._machine_MachineRole_Machines_MachineRolesList = value;
      }
    }
    
    public override void SetupRelationships()
    {
    }
    
    public virtual object Clone()
    {
      var _machineRole = new global::Core.MR.MachineRole();
      _machineRole.MachineRoleBusinessKey = (global::Core.MR.MachineRoleBusinessKey)MachineRoleBusinessKey.Clone();
      _machineRole.Description = Description;
      return _machineRole;
    }
    
    public virtual void Save()
    {
      var item = this;
      global::MetraTech.BusinessEntity.DataAccess.Persistence.StandardRepository.Instance.SaveInstance(ref item);
    }
    
    public override void CopyPropertiesFrom(global::MetraTech.BusinessEntity.DataAccess.Metadata.DataObject dataObject)
    {
      var item = dataObject as global::Core.MR.MachineRole;
      Description = item.Description;
      MachineRoleBusinessKey = item.MachineRoleBusinessKey;
    }
    
    public static new System.Type[] GetKnownTypes()
    {
      var knownTypes = new List<System.Type>();
      knownTypes.Add(typeof(global::Core.MR.MachineRoleBusinessKey));
      return knownTypes.ToArray();
    }
  }
  
  [System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
  [System.SerializableAttribute()]
  public partial class MachineRoleBusinessKey : MetraTech.BusinessEntity.DataAccess.Metadata.BusinessKey, global::Core.MR.Interface.IMachineRoleBusinessKey
  {
    
    private string _entityFullName = "Core.MR.MachineRole";
    
    private string _name;
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public override string EntityFullName
    {
      get
      {
        return this._entityFullName;
      }
      set
      {
        this._entityFullName = "Core.MR.MachineRole";
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }
    
    public override object Clone()
    {
      var _businessKey = new MachineRoleBusinessKey();
      _businessKey.Name = Name;
      return _businessKey;
    }
  }
}
