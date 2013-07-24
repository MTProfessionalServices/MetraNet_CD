﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using MetraTech.ActivityServices.Common;
using MetraTech.BusinessEntity.DataAccess.Metadata;
using MetraTech.BusinessEntity.DataAccess.Persistence;
using Core.Quoting;
using MetraTech.Debug.Diagnostics;
using MetraTech.Domain.Quoting;
using IMTSessionContext = MetraTech.Interop.MTAuth.IMTSessionContext;

namespace MetraTech.Quoting
{
  public interface IQuotingRepository
  {
    int CreateQuote(QuoteRequest quoteRequest, IMTSessionContext sessionContext);
    void UpdateQuoteWithResponse(QuoteResponse quoteResponse);
    void UpdateQuoteWithErrorResponse(int idQuote, QuoteResponse quoteResponse, string errorMessage);
    QuoteHeader GetQuoteHeader(int quoteID, bool loadAllRelatedEntities = true);
    int GetQuoteHeaderCount();
    int GetQuoteContentCount();
    int GetAccountForQuoteCount();
    int GetPOForQuoteCount();
    void SaveQuoteLog(List<QuoteLogRecord> logRecords);
    int GetQuoteLogRecordsCount();
  }

  public class QuotingRepository : IQuotingRepository
  {
    private static Logger mLogger = new Logger("[QuotingRepository]");

    /// <summary>
    /// Creates quoting BMEs and save them into DB
    /// </summary>
    /// <param name="quoteRequest"></param>
    /// <param name="sessionContext">SessionContext used for knowing which user is performing the action</param>/// 
    /// <returns>ID of created quote</returns>
    public int CreateQuote(QuoteRequest quoteRequest, IMTSessionContext sessionContext)
    {
      var quoteHeader = SetQuoteHeader(quoteRequest, sessionContext);
      return quoteHeader.QuoteID ?? -1;

    }
    /// <summary>
    /// Updates quoting BMEs in DB
    /// </summary>     
    /// <param name="quoteResponse"></param>
    public void UpdateQuoteWithResponse(QuoteResponse quoteResponse)
    {
      SetQuoteContent(quoteResponse);
    }

    public void UpdateQuoteWithErrorResponse(int quoteId, QuoteResponse quoteResponse, string errorMessage)
    {
      quoteResponse.FailedMessage = errorMessage;
      SetQuoteContent(quoteResponse);

      //TODO: Determine how state/error information should be saved (different method?) if we failed in generating quote
    }
    /// <summary>
    /// Get QuoteHeader with related QuoteContent, AccountForQuote and POforQuote
    /// </summary>
    /// <param name="quoteID">Id of quote to get data about</param>
    /// <param name="loadAllRelatedEntities">If false - loads only QuoteHeader </param>
    /// <returns>Null if there is no QuoteHeader for quoteID in DB</returns>
    public QuoteHeader GetQuoteHeader(int quoteID, bool loadAllRelatedEntities = true)
    {
      RepositoryAccess.Instance.Initialize();
      try
      {
        //get quoteContent BME
        IStandardRepository standardRepository = RepositoryAccess.Instance.GetRepository();

        var qouteHeaderList = new MTList<QuoteHeader>();
        qouteHeaderList.Filters.Add(
            new MTFilterElement("QuoteID",
                MTFilterElement.OperationType.Equal,
                quoteID));

        standardRepository.LoadInstances(ref qouteHeaderList);

        if (qouteHeaderList.TotalRows < 1)
          return null;

        var quoteHeder = qouteHeaderList.Items.First();

        //get related entities
        if (loadAllRelatedEntities)
        {
          quoteHeder.QuoteContent = quoteHeder.LoadQuoteContent();

          var mtList = new MTList<DataObject>();
          StandardRepository.Instance.LoadInstancesFor(typeof(AccountForQuote).FullName,
                                                       typeof(QuoteHeader).FullName,
                                                       quoteHeder.Id,
                                                       mtList);
          foreach (var accountforQuoteDataObject in mtList.Items)
          {
            var accountforQuote = new AccountForQuote();
            accountforQuote.CopyPropertiesFrom(accountforQuoteDataObject);
            quoteHeder.AccountForQuotes.Add(accountforQuote);
          }

          mtList = new MTList<DataObject>();
          StandardRepository.Instance.LoadInstancesFor(typeof(POforQuote).FullName,
                                                       typeof(QuoteHeader).FullName,
                                                       quoteHeder.Id,
                                                       mtList);
          foreach (var POforQuoteDataObject in mtList.Items)
          {
            var pOforQuote = new POforQuote();
            pOforQuote.CopyPropertiesFrom(POforQuoteDataObject);
            

            mtList = new MTList<DataObject>();
            StandardRepository.Instance.LoadInstancesFor(typeof(UDRCForQuoting).FullName,
                                                         typeof(POforQuote).FullName,
                                                         POforQuoteDataObject.Id,
                                                         mtList);

            foreach (var UDRCQuoteDataObject in mtList.Items)
            {
              var udrcForQuote = new UDRCForQuoting();
              udrcForQuote.CopyPropertiesFrom(UDRCQuoteDataObject);
              pOforQuote.UDRCForQuotings.Add(udrcForQuote);
            }

            quoteHeder.POforQuotes.Add(pOforQuote);
          }
        }

        return quoteHeder;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error get QuoteHeader", ex);
        throw;
      }
    }

    public int GetQuoteHeaderCount()
    {
      RepositoryAccess.Instance.Initialize();
      try
      {
        //get quoteContent BME
        IStandardRepository standardRepository = RepositoryAccess.Instance.GetRepository();

        var qouteHeaderList = new MTList<QuoteHeader>();
        standardRepository.LoadInstances(ref qouteHeaderList);
        return qouteHeaderList.TotalRows;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error get QuoteHeader", ex);
        throw;
      }
    }

    public int GetQuoteContentCount()
    {
      RepositoryAccess.Instance.Initialize();
      try
      {
        //get quoteContent BME
        IStandardRepository standardRepository = RepositoryAccess.Instance.GetRepository();

        var qouteHeaderList = new MTList<QuoteContent>();
        standardRepository.LoadInstances(ref qouteHeaderList);
        return qouteHeaderList.TotalRows;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error get QuoteContent", ex);
        throw;
      }
    }

    public int GetAccountForQuoteCount()
    {
      RepositoryAccess.Instance.Initialize();
      try
      {
        //get quoteContent BME
        IStandardRepository standardRepository = RepositoryAccess.Instance.GetRepository();

        var qouteHeaderList = new MTList<AccountForQuote>();
        standardRepository.LoadInstances(ref qouteHeaderList);
        return qouteHeaderList.TotalRows;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error get AccountForQuote", ex);
        throw;
      }
    }

    public int GetPOForQuoteCount()
    {
      RepositoryAccess.Instance.Initialize();
      try
      {
        //get quoteContent BME
        IStandardRepository standardRepository = RepositoryAccess.Instance.GetRepository();

        var qouteHeaderList = new MTList<POforQuote>();
        standardRepository.LoadInstances(ref qouteHeaderList);
        return qouteHeaderList.TotalRows;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error get POforQuote", ex);
        throw;
      }
    }

    private QuoteHeader SetQuoteHeader(QuoteRequest q, IMTSessionContext sessionContext)
    {
      using (new MetraTech.Debug.Diagnostics.HighResolutionTimer("SetQuoteHeader"))
      {
        RepositoryAccess.Instance.Initialize();

        var quoteHeader = new QuoteHeader();
        try
        {
          using (var scope = new TransactionScope(TransactionScopeOption.Required))
          {

            var quoteContent = new QuoteContent();

            quoteHeader.CustomDescription = q.QuoteDescription;
            quoteHeader.CustomIdentifier = q.QuoteIdentifier;

            quoteHeader.StartDate = q.EffectiveDate;
            quoteHeader.EndDate = q.EffectiveEndDate;

            if (sessionContext != null)
              quoteHeader.UID = sessionContext.AccountID;

            quoteHeader.Save();

            foreach (var accountForQuoteBME in q.Accounts.Select(account => new AccountForQuote { AccountID = account }))
            {
              accountForQuoteBME.QuoteHeader = quoteHeader;
              accountForQuoteBME.Save();
            }

            int poOrder = 0;
            foreach (var POforQuoteBME in q.ProductOfferings.Select(po => new POforQuote { POID = po }))
            {
              POforQuoteBME.QuoteHeader = quoteHeader;
              POforQuoteBME.Order = poOrder++;
              POforQuoteBME.Save();

              if (q.SubscriptionParameters.UDRCValues.ContainsKey(POforQuoteBME.POID.ToString()))
              {

                foreach (var UDRCValue in q.SubscriptionParameters.UDRCValues[POforQuoteBME.POID.ToString()])
                {
                  var udrcValuesForQuote = new UDRCForQuoting();
                  udrcValuesForQuote.CreationDate = MetraTime.Now;
                  udrcValuesForQuote.StartDate = UDRCValue.StartDate;
                  udrcValuesForQuote.EndDate = UDRCValue.EndDate;
                  udrcValuesForQuote.POforQuote = POforQuoteBME;
                  udrcValuesForQuote.Value = UDRCValue.Value;
                  udrcValuesForQuote.PI_Id = UDRCValue.UDRC_Id;

                  udrcValuesForQuote.Save();
                }
              }
            }

            quoteContent.Status = 1;
            quoteContent.QuoteHeader = quoteHeader;
            quoteContent.Save();

            scope.Complete();
          }

        }
        catch (Exception ex)
        {
          mLogger.LogException("Error save Quote header", ex);
          throw;
        }

        return quoteHeader;
      }
    }

    private QuoteContent GetQuoteContent(int quoteID)
    {
      var quoteHeader = GetQuoteHeader(quoteID, false);

      if (quoteHeader == null)
        return null;

      return quoteHeader.LoadQuoteContent() as QuoteContent;
    }

    private QuoteContent SetQuoteContent(QuoteResponse q)
    {
      using (new MetraTech.Debug.Diagnostics.HighResolutionTimer("GetQuoteContent"))
      {
        RepositoryAccess.Instance.Initialize();

        //get quoteContent BME
        var quoteContent = GetQuoteContent(q.idQuote);

        try
        {
          if (quoteContent == null)
            throw new Exception(String.Format("Can't find quote header with idQuote = {0}", q.idQuote));

          quoteContent.Total = q.TotalAmount;
          quoteContent.Currency = q.Currency;
          quoteContent.ReportLink = q.ReportLink;
          quoteContent.CreationDate = q.CreationDate;
          var failedMessage = q.FailedMessage;
          if (!String.IsNullOrEmpty(failedMessage) && failedMessage.Length > 2000)
          {
            failedMessage = failedMessage.Substring(0, 2000);
          }
          quoteContent.FailedMessage = failedMessage;
          quoteContent.Status = Convert.ToInt32(q.Status);

          // todo fill .ReportContent =

          quoteContent.Save();
        }
        catch (Exception ex)
        {
          mLogger.LogException("Error save Quote content", ex);
          throw;
        }

        return quoteContent;
      }
    }

    public void SaveQuoteLog(List<QuoteLogRecord> logRecords)
    {
      using (new HighResolutionTimer("SaveQuoteLog"))
      {
        RepositoryAccess.Instance.Initialize();

        try
        {
          foreach (var record in logRecords)
          {
            var quoteLog = record.ConvertToQuoteLog();
            quoteLog.Save();
          }
        }
        catch (Exception ex)
        {
          mLogger.LogException("Error saving log record(s)", ex);
          throw;
        }
      }
    }
    
    public int GetQuoteLogRecordsCount()
    {
      RepositoryAccess.Instance.Initialize();

      try
      {
        IStandardRepository repository = RepositoryAccess.Instance.GetRepository();

        var logRecords = new MTList<QuoteLog>();
        repository.LoadInstances(ref logRecords);

        return logRecords.TotalRows;
      }
      catch (Exception ex)
      {
        mLogger.LogException("Error loading QuoteLog instances", ex);
        throw;
      }
    }
  }

  public class QuotingRepositoryDummy : IQuotingRepository
  {

    private int idCurrent = 0;
    private Dictionary<int, QuoteRequest> requests = new Dictionary<int, QuoteRequest>();
    private Dictionary<int, QuoteResponse> responses = new Dictionary<int, QuoteResponse>();
    private readonly Dictionary<int, QuoteHeader> headers = new Dictionary<int, QuoteHeader>();
    private readonly Dictionary<int, QuoteContent> contents = new Dictionary<int, QuoteContent>();
    private readonly Dictionary<int, List<AccountForQuote>> accounts = new Dictionary<int, List<AccountForQuote>>();
    private readonly Dictionary<int, List<POforQuote>> pos = new Dictionary<int, List<POforQuote>>();
    private readonly List<QuoteLogRecord> quoteLog = new List<QuoteLogRecord>();

    public int GetQuoteHeaderCount()
    {
      return HeadersCount;
    }


    public int GetQuoteContentCount()
    {
      return ContentsCount;
    }

    public int GetAccountForQuoteCount()
    {
      return AccountsCount;
    }

    public int GetPOForQuoteCount()
    {
      return POsCount;
    }

    public int HeadersCount
    {
      get { return headers.Count; }
    }

    public int ContentsCount
    {
      get { return contents.Count; }
    }

    public int AccountsCount
    {
      get { return accounts.Values.Count == 0 ? 0 : accounts.Values.First().Count; }
    }


    public int POsCount
    {
      get { return pos.Values.Count == 0 ? 0 : pos.Values.First().Count; }
    }

    #region IQuotingRepository Members

    public int CreateQuote(QuoteRequest quoteRequest, IMTSessionContext sessionContext)
    {
      int newId = idCurrent++;
      requests.Add(newId, quoteRequest);

      // Add empty entities for now

      headers.Add(newId, new QuoteHeader());
      contents.Add(newId, new QuoteContent());

      var accountToAdd = new List<AccountForQuote>();
      var posToAdd = new List<POforQuote>();

      for (int i = 0; i < quoteRequest.Accounts.Count; i++)
        accountToAdd.Add(new AccountForQuote());

      accounts.Add(newId, accountToAdd);

      for (int i = 0; i < quoteRequest.ProductOfferings.Count; i++)
        posToAdd.Add(new POforQuote());

      pos.Add(newId, posToAdd);

      return newId;
    }

    public QuoteHeader GetQuoteHeader(int quoteID, bool loadAllRelatedEntities = true)
    {
      var quoteHeder = new QuoteHeader { QuoteID = quoteID };
      return quoteHeder;
    }

    public void UpdateQuoteWithResponse(QuoteResponse quoteResponse)
    {
      if (!requests.ContainsKey(quoteResponse.idQuote))
        throw new ArgumentException(string.Format("Quote with id {0} not found in repository", quoteResponse.idQuote), "idQuote");

      responses.Add(quoteResponse.idQuote, quoteResponse);

    }

    public void UpdateQuoteWithErrorResponse(int idQuote, QuoteResponse quoteResponse, string errorMessage)
    {
      if (!requests.ContainsKey(idQuote))
        throw new ArgumentException(string.Format("Quote with id {0} not found in repository", idQuote), "idQuote");

      //For now don't know what the dummy implementation should do so do nothing
    }

    public void SaveQuoteLog(List<QuoteLogRecord> logRecords)
    {
      quoteLog.AddRange(logRecords);
    }

    public int GetQuoteLogRecordsCount()
    {
      return quoteLog.Count;
    }

    #endregion
  }
}
