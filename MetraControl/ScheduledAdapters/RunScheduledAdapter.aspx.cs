﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using MetraTech.UI.Common;

namespace MetraNet.MetraControl.ScheduledAdapters
{
  public partial class RunScheduledAdapter : MTPage, ICallbackEventHandler
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!UI.CoarseCheckCapability("Manage Scheduled Adapters"))
      {
        Response.End();
      }

      if (!IsPostBack)
      {
        var cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
        var callbackScript = "function CallServer(arg, context)" + "{ " + cbReference + ";}";
        Page.ClientScript.RegisterClientScriptBlock(GetType(), "CallServer", callbackScript, true);
      }
    }

    #region Implementation of ICallbackEventHandler

    private string _callbackResult = string.Empty;

    /// <summary>
    /// Processes a callback event that targets a control.
    /// </summary>
    /// <param name="eventArgument">A string that represents an event argument to pass to the event handler.</param>
    public void RaiseCallbackEvent(string eventArgument)
    {
      object result = null;
      var serializer = new JavaScriptSerializer();
      var value = serializer.Deserialize<Dictionary<string, string>>(eventArgument);
      var action = value["action"];

      try
      {
        var eventIds = new List<int>();
        switch (action)
        {
          case "run":
            {
              var idsString = value["eventIds"];
              var ids = idsString.Split(new[] {','});
              eventIds.AddRange(ids.Select(s => int.Parse(s, CultureInfo.InvariantCulture)));
              RunAdapters(eventIds);
              result = new
                {
                  result = "ok",
                  message = String.Format("Events with IDs {0} were submitted for execution.", idsString)
                };
              break;
            }
        }
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        result = new {result = "error", errorMessage = ex.Message};
      }

      if (result != null)
      {
        _callbackResult = serializer.Serialize(result);
      }
    }

    private void RunAdapters(IEnumerable<int> eventIds)
    {
      var usmClient = new MetraTech.UsageServer.Client {SessionContext = UI.User.SessionContext};

      foreach (var instanceId in eventIds.Select(usmClient.InstantiateScheduledEvent))
      {
        usmClient.SubmitEventForExecution(instanceId, "");
      }
      usmClient.NotifyServiceOfSubmittedEvents();
    }

    /// <summary>
    /// Returns the results of a callback event that targets a control.
    /// </summary>
    /// <returns>
    /// The result of the callback.
    /// </returns>
    public string GetCallbackResult()
    {
      return _callbackResult;
    }

    #endregion
  }
}