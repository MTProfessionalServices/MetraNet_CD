using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MetraTech.DomainModel.ProductCatalog;
using MetraTech.UI.Common;
using MetraTech.PageNav.ClientProxies;
using MetraTech.DomainModel;
using MetraTech.UI.Controls;
using System.Globalization;
using System.Reflection;
using System.Resources;
using MetraTech.Interop.IMTAccountType;
using MetraTech.Accounts.Type;
using YAAC = MetraTech.Interop.MTYAAC;
using MetraTech.Interop.MTProductCatalog;


public partial class DataExportReportManagement_ShowAllReportParameters : MTPage
{
  public string strincomingReportId { get; set; } //so we can read it any time in the session 
  public int intincomingReportID { get; set; }

  protected void Page_Load(object sender, EventArgs e)
  {

    strincomingReportId = Request.QueryString["reportid"];
    intincomingReportID = System.Convert.ToInt32(strincomingReportId);
    Session["intSessionReportID"] = intincomingReportID;
  }

/* Buttons not in use
  protected void btnOK_Click(object sender, EventArgs e)
  
  { 
  
  }

  protected void btnCancel_Click(object sender, EventArgs e)
  {
    
  }

*/
}