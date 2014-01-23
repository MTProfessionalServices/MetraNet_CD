//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.UI
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
  public partial class Dashboard_Column : MetraTech.BusinessEntity.DataAccess.Metadata.DataObject, global::Core.UI.Interface.IDashboard_Column
  {
    
    public const string FullName = "Core.UI.Dashboard_Column";
    
    private int @__Version;
    
    public const string Property__Version = "_Version";
    
    private System.Nullable<System.DateTime> _creationDate;
    
    public const string Property_CreationDate = "CreationDate";
    
    private System.Nullable<System.DateTime> _updateDate;
    
    public const string Property_UpdateDate = "UpdateDate";
    
    private System.Nullable<System.Int32> _uID;
    
    public const string Property_UID = "UID";
    
    public const string Property_Id = "Id";
    
    private global::Core.UI.Dashboard_ColumnBusinessKey _dashboard_ColumnBusinessKey = new Dashboard_ColumnBusinessKey();
    
    private global::Core.UI.Interface.IDashboard _dashboard;
    
    private global::Core.UI.Interface.IColumn _column;
    
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
    public virtual global::Core.UI.Dashboard_ColumnBusinessKey Dashboard_ColumnBusinessKey
    {
      get
      {
        return this._dashboard_ColumnBusinessKey;
      }
      set
      {
        this._dashboard_ColumnBusinessKey = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual global::Core.UI.Interface.IDashboard Dashboard
    {
      get
      {
        return this._dashboard;
      }
      set
      {
        this._dashboard = value;
      }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute()]
    public virtual global::Core.UI.Interface.IColumn Column
    {
      get
      {
        return this._column;
      }
      set
      {
        this._column = value;
      }
    }
    
    public virtual void SetRelationshipItem(object item)
    {
      if (item is global::Core.UI.Interface.IDashboard)
      {
        Dashboard = item as global::Core.UI.Interface.IDashboard;
      }
      else
      {
        if (item is global::Core.UI.Interface.IColumn)
        {
          Column = item as global::Core.UI.Interface.IColumn;
        }
        else
        {
          throw new ApplicationException("Invalid argument");
        }
      }
    }
    
    public override void SetupRelationships()
    {
    }
    
    public virtual object Clone()
    {
      var _dashboard_Column = new global::Core.UI.Dashboard_Column();
      _dashboard_Column.Dashboard_ColumnBusinessKey = (global::Core.UI.Dashboard_ColumnBusinessKey)Dashboard_ColumnBusinessKey.Clone();
      return _dashboard_Column;
    }
    
    public virtual void Save()
    {
      var item = this;
      global::MetraTech.BusinessEntity.DataAccess.Persistence.StandardRepository.Instance.SaveInstance(ref item);
    }
    
    public override void CopyPropertiesFrom(global::MetraTech.BusinessEntity.DataAccess.Metadata.DataObject dataObject)
    {
      var item = dataObject as global::Core.UI.Dashboard_Column;
      Dashboard_ColumnBusinessKey = item.Dashboard_ColumnBusinessKey;
    }
    
    public static new System.Type[] GetKnownTypes()
    {
      var knownTypes = new List<System.Type>();
      knownTypes.Add(typeof(global::Core.UI.Dashboard_ColumnBusinessKey));
      return knownTypes.ToArray();
    }
  }
  
  [System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
  [System.SerializableAttribute()]
  public partial class Dashboard_ColumnBusinessKey : MetraTech.BusinessEntity.DataAccess.Metadata.BusinessKey, global::Core.UI.Interface.IDashboard_ColumnBusinessKey
  {
    
    private string _entityFullName = "Core.UI.Dashboard_Column";
    
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
        this._entityFullName = "Core.UI.Dashboard_Column";
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
      var _businessKey = new Dashboard_ColumnBusinessKey();
      _businessKey.InternalKey = InternalKey;
      return _businessKey;
    }
  }
}
