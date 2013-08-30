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
using MetraTech;
using MetraTech.UI.Common;
using MetraTech.Core.Services.ClientProxies;
using MetraTech.DomainModel.ProductCatalog;
using MetraTech.ActivityServices.Common;
using System.ServiceModel;
using MetraTech.Debug.Diagnostics;
using System.Web.Script.Serialization;
using System.Threading;


// This Ajax service loads all decisions into a grid or
// it updates the Active status of a particular decision.
public partial class AjaxServices_DecisionSvc : MTListServicePage
{
    private const int MAX_RECORDS_PER_BATCH = 50;

    private Logger logger = new Logger("[AmpWizard]");


    protected bool ExtractDataInternal(AmpServiceClient client, ref MTList<Decision> items, int batchID, int limit)
    {
        try
        {
            items.Items.Clear();
            items.PageSize = limit;
            items.CurrentPage = batchID;

            client.GetDecisions(ref items);

            if (items != null)
            {
                if (items.Items != null)
                {
                    foreach (var decision in items.Items)
                    {
                        if (String.IsNullOrEmpty(decision.PvToAmountChainMappingValue))
                        {
                            decision.PvToAmountChainMappingValue = decision.PvToAmountChainMappingColumnName;
                        }
                        if (String.IsNullOrEmpty(decision.AccountQualificationGroupValue))
                        {
                            decision.AccountQualificationGroupValue = decision.AccountQualificationGroupColumnName;
                        }
                        if (String.IsNullOrEmpty(decision.UsageQualificationGroupValue))
                        {
                            decision.UsageQualificationGroupValue = decision.UsageQualificationGroupColumnName;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
          Response.StatusCode = 500; // right status code?
          logger.LogException("An error occurred while retrieving Decision data.  Please check system logs.", ex);
          return false;
        }

        return true;
    }


    protected bool ExtractData(AmpServiceClient client, ref MTList<Decision> items)
    {
        if (Page.Request["mode"] == "csv")
        {
            Response.BufferOutput = false;
            Response.ContentType = "application/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=export.csv");
        }

        //if there are more records to process than we can process at once, we need to break up into multiple batches
        if ((items.PageSize > MAX_RECORDS_PER_BATCH) && (Page.Request["mode"] == "csv"))
        {
            int advancePage = (items.PageSize % MAX_RECORDS_PER_BATCH != 0) ? 1 : 0;

            int numBatches = advancePage + (items.PageSize / MAX_RECORDS_PER_BATCH);
            for (int batchID = 0; batchID < numBatches; batchID++)
            {
                if (!ExtractDataInternal(client, ref items, batchID + 1, MAX_RECORDS_PER_BATCH))
                {
                  //unable to extract data
                  return false;
                }

                string strCSV = ConvertObjectToCSV(items, (batchID == 0));
                Response.Write(strCSV);
            }
        }
        else
        {
            if (!ExtractDataInternal(client, ref items, items.CurrentPage, items.PageSize))
            {
              //unable to extract data
              return false;
            }

            if (Page.Request["mode"] == "csv")
            {
                string strCSV = ConvertObjectToCSV(items, true);
                Response.Write(strCSV);
            }
        }

        return true;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Extra check that user has permission to configure AMP decisions.
        if (!UI.CoarseCheckCapability("ManageAmpDecisions"))
        {
          Response.End();
          return;
        }

        using (new HighResolutionTimer("DecisionSvcAjax", 5000))
        {
          AmpServiceClient client = null;

          try
          {
            string decisionName = "";
            bool updateActiveStatusFlag = false;
            bool isActiveStatus = false;

            if (Request["decisionName"] != null)
            {
              decisionName = Request["decisionName"];
            }

            //True if need to activate or deactivate decision
            if (Request["setStatus"] != null)
            {
              updateActiveStatusFlag = Convert.ToBoolean(Request["setStatus"]);
            }

            if (Request["status"] != null)
            {
              isActiveStatus = Convert.ToBoolean(Request["status"]);
            }

            // Set up client.
            client = new AmpServiceClient();
            if (client.ClientCredentials != null)
            {
              client.ClientCredentials.UserName.UserName = UI.User.UserName;
              client.ClientCredentials.UserName.Password = UI.User.SessionPassword;
            }

            if (updateActiveStatusFlag)
            {
              //Must activate/deactivate decision
              Decision decisionInstance;
              client.GetDecision(decisionName, out decisionInstance);
              decisionInstance.IsActive = isActiveStatus;
              client.SaveDecision(decisionInstance);
            }
            else
            {
              //Just load the grid             
              MTList<Decision> items = new MTList<Decision>();
              SetPaging(items);
              SetSorting(items);
              SetFilters(items);

              if (ExtractData(client, ref items))
              {
                if (items.Items.Count == 0)
                {
                  Response.Write("{\"TotalRows\":\"0\",\"Items\":[]}");
                  HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else if (Page.Request["mode"] != "csv")
                {
                  JavaScriptSerializer jss = new JavaScriptSerializer();
                  string json = jss.Serialize(items);
                  Response.Write(json);
                }
              }

            } // else load grid

            // Clean up client.
            client.Close();
            client = null;

          }
          catch (Exception ex)
          {
            logger.LogException("An error occurred while processing Decision data.  Please check system logs.", ex);
            throw;
          }
          finally
          {
            Response.End();
            if (client != null)
            {
              client.Abort();
            }
          }
        } // using

    } // Page_Load

}
