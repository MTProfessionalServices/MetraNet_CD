﻿// Usage:  http://localhost/MetraNet/BEList.aspx?Name=Core.OrderManagement.Order&Extension=Account
//         &Association=Account&ParentId=123&ParentName=Core.OrderManagement.Parent 
// Loads list of orders with the OrderTemplate.xml grid layout in the Account extension
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetraTech.ActivityServices.Common;
using MetraTech.BusinessEntity.Core;
using MetraTech.BusinessEntity.Service.ClientProxies;
using MetraTech.UI.Common;
using MetraTech.BusinessEntity.DataAccess.Metadata;
using MetraTech.UI.Controls;

public partial class BEUnrelatedEntityList : MTPage
{

  #region Properties
 

  public string BEName
  {
    get { return ViewState["BEName"] as string; }
    set { ViewState["BEName"] = value; }
  }

  public string MultiSelect
  {
    get { return ViewState["MultiSelect"] as string; }
    set { ViewState["MultiSelect"] = value; }
  }

 public string RefererUrl
  {
    get { return ViewState["RefererURL"] as string; }
    set { ViewState["RefererURL"] = value; }
  }

  public string ReturnUrl
  {
    get { return ViewState["ReturnURL"] as string; }
    set { ViewState["ReturnURL"] = value; }
  }

  public string AccountDefProp
  {
    get { return ViewState["AccountDef"] as string; }
    set { ViewState["AccountDef"] = value; }
  }

  public string SubscriptionDefProp
  {
    get { return ViewState["SubscriptionDef"] as string; }
    set { ViewState["SubscriptionDef"] = value; }
  }

  public string AssociationValue
  {
    get { return ViewState["AssociationValue"] as string; }
    set { ViewState["AssociationValue"] = value; }
  }

  public string ParentId
  {
    get { return ViewState["ParentId"] as string; }
    set { ViewState["ParentId"] = value; }
  }

  public string ParentName
  {
    get { return ViewState["ParentName"] as string; }
    set { ViewState["ParentName"] = value; }
  }

  public string ReadOnly
  {
    get { return ViewState["ReadOnly"] as string; }
    set { ViewState["ReadOnly"] = value; }
  }

  public string Unrelated
  {
    get { return ViewState["Unrelated"] as string; }
    set { ViewState["Unrelated"] = value; }
  }
  
  #endregion

  #region Events
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {

      RefererUrl = Encrypt(Request.Url.ToString());

      if (!String.IsNullOrEmpty(Request["Association"]))
      {
        AssociationValue = Request["Association"];
        if (AssociationValue.ToLower().Contains("account"))
        {
          AccountDefProp = UI.Subscriber.SelectedAccount._AccountID.ToString();
        }

        if (AssociationValue.ToLower().Contains("subscription"))
        {
          Session["BESubscriptionId"] = String.IsNullOrEmpty(Request["id_sub"]) ? Session["BESubscriptionId"].ToString() : Request["id_sub"];
          SubscriptionDefProp = Session["BESubscriptionId"].ToString();
        }
      }

      ParentName = String.IsNullOrEmpty(Request["ParentName"]) ? null : Request["ParentName"];
      ParentId = String.IsNullOrEmpty(Request["ParentId"]) ? null : Request["ParentId"];

      if (String.IsNullOrEmpty(Request["ReturnURL"]))
      {
        if (Request.UrlReferrer != null)
        {
          if (Request.UrlReferrer.ToString().ToLower().Contains("login.aspx") ||
              Request.UrlReferrer.ToString().ToLower().Contains("default.aspx"))
          {
            ReturnUrl = UI.DictionaryManager["DashboardPage"].ToString();
          }
          else
          {
            ReturnUrl = Request.UrlReferrer.ToString();
          }
        }
        else
        {
          ReturnUrl = UI.DictionaryManager["DashboardPage"].ToString();
        }
      }
      else
      {
        ReturnUrl = Request["ReturnURL"].Replace("'", "").Replace("|", "?").Replace("**", "&");
      }

	  ReadOnly = "false";
      BEName = Server.HtmlEncode(Request.QueryString["Name"]);

      MyGrid1.ExtensionName = Request.QueryString["Extension"];
      MyGrid1.TemplateFileName = BEName + ".xml";
      //LblcurrentEntityName.Text = String.IsNullOrEmpty(Request["currentEntityName"]) ? null : Resources.Resource.TEXT_ADD_RELATED_ENTITIES + Request["currentEntityName"];
      LblcurrentEntityName.Text = String.IsNullOrEmpty(Request["currentEntityName"]) ? null : Resources.Resource.TEXT_RELATED_ENTITIES_1 + BEName.Substring(BEName.LastIndexOf(".") + 1) + Resources.Resource.TEXT_RELATED_ENTITIES_2  + Request["currentEntityName"];
      
    }
  }

  protected override void OnLoadComplete(EventArgs e)
  {

    MultiSelect = String.IsNullOrEmpty(Request["MultiSelect"]) ? "false" : Request["MultiSelect"];
    Unrelated = String.IsNullOrEmpty(Request["Unrelated"]) ? "false" : Request["Unrelated"];
    
    if (String.IsNullOrEmpty(MyGrid1.Title))
    {
      string Text1 = Resources.Resource.TEXT_UNRELATED_1;
      string Text2 = Resources.Resource.TEXT_UNRELATED_2;
      //MyGrid1.Title = Server.HtmlEncode(Text1 + BEName.Substring(BEName.LastIndexOf(".") + 1) + " " + Text2);

      MyGrid1.Title = Text1 + BEName.Substring(BEName.LastIndexOf(".") + 1) + Text2;
    
      // Set additional argument for grid     
      if (!String.IsNullOrEmpty(AccountDefProp))
      {
        MyGrid1.DataSourceURL += "&AccountDef=" + Encrypt(AccountDefProp);
      }

      if (!String.IsNullOrEmpty(SubscriptionDefProp))
      {
        MyGrid1.DataSourceURL += "&SubscriptionDef=" + Encrypt(SubscriptionDefProp);
      }

      if (!String.IsNullOrEmpty(ParentName))
      {
        MyGrid1.DataSourceURL += "&ParentName=" + ParentName;
      }

      if (!String.IsNullOrEmpty(ParentId))
      {
        MyGrid1.DataSourceURL += "&ParentId=" + ParentId;
      }

      MyGrid1.DataSourceURL += "&UnrelatedEntityList=true";
      
      if (!MenuManager.CheckBECapability(MyGrid1.ExtensionName, AccessType.Write, UI.SessionContext.SecurityContext))
      {
        ReadOnly = "true";
        if (MyGrid1.ToolbarButtons.Count > 0)
        {
          // remove the first toolbar button which is add by default on generic page
          MyGrid1.ToolbarButtons.RemoveAt(0);
        }
      }
      else
      {
        ReadOnly = "false";
      }
      

      if (Unrelated == "true")
      {
        if (MultiSelect == "Many")
        {
          MyGrid1.MultiSelect = true;
          MyGrid1.SelectionModel = MTGridSelectionModel.Checkbox;
        }
        else
        {
          MyGrid1.MultiSelect = false;
          MyGrid1.SelectionModel = MTGridSelectionModel.Checkbox;
        }

        MTGridButton gridButton1 = new MTGridButton();
        gridButton1.ButtonID = "Delete";
        gridButton1.ButtonText = "Delete";
        gridButton1.ToolTip = "Delete";
        gridButton1.JSHandlerFunction = "onDelete";
        gridButton1.IconClass = "remove";
        this.MyGrid1.ToolbarButtons.Add(gridButton1);

        MyGrid1.Buttons = MTButtonType.OKCancel;
      }
      base.OnLoadComplete(e);


    }
  }
  #endregion

 
}

