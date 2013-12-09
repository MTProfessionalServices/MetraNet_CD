using System;
using System.Collections.Generic;
using System.Globalization;
using MetraTech.Core.Services.ClientProxies;
using MetraTech.Core.Services.Test.Quoting.Domain;
using MetraTech.DataAccess;
using MetraTech.Domain.Quoting;
using MetraTech.DomainModel.Enums.Core.Metratech_com_billingcycle;
using MetraTech.Interop.MTProductCatalog;
using MetraTech.TestCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetraTech.Core.Services.Test.Quoting
{
  [TestClass]
  public class QuotingBackendFunctionalTests
  {

      private static TestContext _testContext;
      #region Setup/Teardown

      [ClassInitialize]
      public static void InitTests(TestContext testContext)
      {
          _testContext = testContext;
          SharedTestCode.MakeSureServiceIsStarted("ActivityServices");
          SharedTestCode.MakeSureServiceIsStarted("Pipeline");
      }

    #endregion

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T01QuotingWithIcbBasicScenario_PositiveTest()
      {
          #region Prepare

          const string testShortName = "Q_Basic";
          //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
          var testRunUniqueIdentifier = MetraTime.Now.ToString(CultureInfo.InvariantCulture);
          //Identifier to make this run unique

          var quoteImpl = new QuoteImplementationData();
          var expected = new QuoteVerifyData();

          // Create account
          var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

          // Create/Verify Product Offering Exists
          var pofConfiguration = new ProductOfferingFactoryConfiguration(_testContext.TestName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

          //IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
          var productOffering = ProductOfferingFactory.Create(pofConfiguration);
          var idProductOfferingToQuoteFor = productOffering.ID;

          using (var client = new PriceListServiceClient())
          {
              if (client.ClientCredentials != null)
              {
                  client.ClientCredentials.UserName.UserName = "su";
                  client.ClientCredentials.UserName.Password = "su123";
              }

              IMTCollection instances = productOffering.GetPriceableItems();

              var productOfferingFactory = new ProductOfferingFactory();
              productOfferingFactory.Initialize(_testContext.TestName, testRunUniqueIdentifier);

              var parameterTableFlatRc =
                productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComFlatrecurringcharge);
              var parameterTableNonRc =
                productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComNonrecurringcharge);
              var parameterTableUdrcTapered =
                productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComUdrctapered);
              var parameterTableUdrcTiered =
                productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComUdrctiered);

              #region Set Allow ICB for PIs

              foreach (IMTPriceableItem possibleRC in instances)
              {
                  switch (possibleRC.Kind)
                  {
                      case MTPCEntityType.PCENTITY_TYPE_RECURRING_UNIT_DEPENDENT:
                          {
                              var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID,
                                                                                      parameterTableUdrcTapered.ID, SharedTestCode.MetratechComUdrctapered);
                              pofConfiguration.PriceableItemsAndParameterTableForUdrc.Add(piAndPTParameters);

                              piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableUdrcTiered.ID,
                                                                                  SharedTestCode.MetratechComUdrctiered);
                              pofConfiguration.PriceableItemsAndParameterTableForUdrc.Add(piAndPTParameters);

                          }
                          break;
                      case MTPCEntityType.PCENTITY_TYPE_RECURRING:
                          {
                              var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableFlatRc.ID,
                                                                                      SharedTestCode.MetratechComFlatrecurringcharge);
                              pofConfiguration.PriceableItemsAndParameterTableForRc.Add(piAndPTParameters);
                          }
                          break;
                      case MTPCEntityType.PCENTITY_TYPE_NON_RECURRING:
                          {
                              var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableNonRc.ID,
                                                                                      SharedTestCode.MetratechComNonrecurringcharge);
                              pofConfiguration.PriceableItemsAndParameterTableForNonRc.Add(piAndPTParameters);
                          }
                          break;
                  }
              }

              #endregion
          }

          //Values to use for verification

          #endregion

          #region Test

          //Prepare request
          quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
          quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
          quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
          quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + _testContext.TestName;
          quoteImpl.Request.ReportParameters = new ReportParams()
          {
              PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault
          };
          quoteImpl.Request.EffectiveDate = MetraTime.Now;
          quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
          quoteImpl.Request.Localization = "en-US";
          quoteImpl.Request.SubscriptionParameters.UDRCValues =
            SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

          expected.CountAccounts = quoteImpl.Request.Accounts.Count;
          expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
          expected.CountFlatRCs = pofConfiguration.CountPairRCs +
                                          (pofConfiguration.CountPairRCs * expected.CountAccounts);
          expected.CountUDRCs = pofConfiguration.CountPairUDRCs +
                                        (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

          pofConfiguration.RCAmount = 66.66m;
          pofConfiguration.NRCAmount = 77.77m;
          expected.TotalForUDRCs = 15 * 16.6m + 5 * 13m;

          expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                       (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                       (expected.CountNRCs * pofConfiguration.NRCAmount);

          #region Initialize ICB prices

          quoteImpl.Request.IcbPrices = new List<IndividualPrice>();

          if (pofConfiguration.PriceableItemsAndParameterTableForRc != null &&
              pofConfiguration.PriceableItemsAndParameterTableForRc.Count > 0)
          {
              var chargeRate = new ChargesRate { Price = 66.66m };
              var qip = new IndividualPrice
              {
                  CurrentChargeType = ChargeType.RecurringCharge,
                  ProductOfferingId = idProductOfferingToQuoteFor,
                  ChargesRates = new List<ChargesRate> { chargeRate },
                  PriceableItemId = null
              };

              quoteImpl.Request.IcbPrices.Add(qip);
          }

          if (pofConfiguration.PriceableItemsAndParameterTableForUdrc != null &&
              pofConfiguration.PriceableItemsAndParameterTableForUdrc.Count > 0)
          {
              var qip = new IndividualPrice
              {
                  CurrentChargeType = ChargeType.UDRCTapered,
                  ProductOfferingId = idProductOfferingToQuoteFor,
                  PriceableItemId = null
              };
              var chargeRates = new List<ChargesRate>
                    {
                        new ChargesRate {UnitValue = 15, UnitAmount = 16.6m},
                        new ChargesRate {UnitValue = 40, UnitAmount = 13m}
                    };
              qip.ChargesRates.AddRange(chargeRates);
              quoteImpl.Request.IcbPrices.Add(qip);

              var chargeRate = new ChargesRate { UnitValue = 20, UnitAmount = 16.6m, BaseAmount = 10m };
              qip = new IndividualPrice
              {
                  CurrentChargeType = ChargeType.UDRCTiered,
                  ProductOfferingId = idProductOfferingToQuoteFor,
                  ChargesRates = new List<ChargesRate> { chargeRate },
                  PriceableItemId = null
              };

              quoteImpl.Request.IcbPrices.Add(qip);
          }

          if (pofConfiguration.PriceableItemsAndParameterTableForRc != null &&
              pofConfiguration.PriceableItemsAndParameterTableForRc.Count > 0)
          {
              var chargeRate = new ChargesRate { Price = 77.77m };
              var qip = new IndividualPrice
              {
                  CurrentChargeType = ChargeType.NonRecurringCharge,
                  ProductOfferingId = idProductOfferingToQuoteFor,
                  ChargesRates = new List<ChargesRate> { chargeRate },
                  PriceableItemId = null
              };

              quoteImpl.Request.IcbPrices.Add(qip);
          }

          #endregion

          //Give request to testing scenario along with expected results for verification; get back response for further verification
          quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);


          #endregion
      }
    
    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T02QuotingWithPOWithoutAnyRCsOrNRCs_PositiveTest()
    {
        #region Prepare

        string testName = "QuotingProductOfferingWithoutAnyRCsOrNRCs";
        string testShortName = "Q_PONoRCNoNRC"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        //string testDescription = @"Given an account and a quote request for a Product Offering that has no RCs or NRCs, When quote is run Then it there is no error and total is zero";
        string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

        QuoteImplementationData quoteImpl = new QuoteImplementationData();
        QuoteVerifyData expected = new QuoteVerifyData();

        // Create account for test run
        var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier);

        pofConfiguration.CountNRCs = 0;
        pofConfiguration.CountPairRCs = 0; //????

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        int idProductOfferingToQuoteFor = productOffering.ID;

        //Values to use for verification
        expected.Total = 0M;
        expected.Currency = ""; //Might be nice in future if currency picked up from PO but now if there is no usage, we don't see currency

        expected.CountNRCs = 0;
        expected.CountFlatRCs = 0;
        #endregion


        #region Test and Verify

        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
        quoteImpl.Request.Localization = "en-US";

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T03QuotingWithAccountsWithDifferentPayersInOneSet_PositiveTest()
    {
        #region Prepare

        string testName = "QuotingWithAccountsWithDifferentPayers";
        string testShortName = "Q_DifP"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        string testRunUniqueIdentifier = MetraTime.NowWithMilliSec; //Identifier to make this run unique

        QuoteImplementationData quoteImpl = new QuoteImplementationData();

        // Create account #1 Corporate payer
        var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create account #2 Department payee
        testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
        var idAccountToQuoteFor2 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor, idAccountToQuoteFor);

        // Create account #3 Department payee
        testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
        var idAccountToQuoteFor3 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor, idAccountToQuoteFor);

        // Create account #4 Corporate self-payed non-payer
        testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
        var idAccountToQuoteFor4 = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier)
        {
            CountNRCs = 2,
            CountPairRCs = 1
        };

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        var idProductOfferingToQuoteFor = productOffering.ID;

        var expected = new QuoteVerifyData { CountAccounts = 4 };

        //Values to use for verification
        expected.CountNRCs = expected.CountAccounts * pofConfiguration.CountNRCs;
        expected.CountFlatRCs = expected.CountAccounts * pofConfiguration.CountPairRCs * 2;

        expected.Total = expected.CountAccounts *
                            (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 +
                            pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
        expected.Currency = "USD";

        #endregion

        #region Test and Verify

        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor2);
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor3);
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor4);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
        quoteImpl.Request.Localization = "en-US";

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T04QuotingWithMultipleAccountsAndMultiplePOs_PositiveTest()
    {
        #region Prepare
        var testName = "QuotingWithMultipleAccountsAndMultiplePOs";
        var testShortName = "Q_MAMPO"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        //string testDescription = @"Given a quote request for multiple accounts and multiple Product Offerings, When quote is run Then it includes all the usage";
        var testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

        var quoteImpl = new QuoteImplementationData();
        var expected = new QuoteVerifyData
        {
            CountAccounts = 2,
            CountProducts = 2
        };

        // Create accounts
        var corpAccountHolders = new List<int>();

        for (var i = 1; i < expected.CountAccounts + 1; i++)
        {
            corpAccountHolders.Add(SharedTestCode.GetCorporateAccountToQuoteFor(testShortName + "_" + i, testRunUniqueIdentifier));
        }

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier)
        {
            CountNRCs = 2,
            CountPairRCs = 1
        };

        //Now generate the Product Offerings we need
        var posToAdd = new List<int>();

        for (var i = 1; i < expected.CountProducts + 1; i++)
        {
            pofConfiguration.Name = testShortName + "_" + i;
            pofConfiguration.UniqueIdentifier = testRunUniqueIdentifier + "_" + i;
            IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
            Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
            posToAdd.Add(productOffering.ID);
        }

        //Values to use for verification
        expected.CountNRCs = expected.CountAccounts * expected.CountProducts * pofConfiguration.CountNRCs;
        expected.CountFlatRCs = expected.CountAccounts * expected.CountProducts * pofConfiguration.CountPairRCs * 2; //???

        expected.Total = expected.CountAccounts * expected.CountProducts * (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 + pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
        expected.Currency = "USD";

        #endregion

        #region Test and Verify
        //Prepare request
        quoteImpl.Request.Accounts.AddRange(corpAccountHolders);
        quoteImpl.Request.ProductOfferings.AddRange(posToAdd);

        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
        quoteImpl.Request.Localization = "en-US";

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T05QuotingWithGroupSubscription_PositiveTest()
    {
      #region Prepare
      string testName = "QuotingWithGroupSubscription";
      string testShortName = "Q_GSub"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
      string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

      QuoteImplementationData quoteImpl = new QuoteImplementationData();
      var expected = new QuoteVerifyData();

      // Create account #1 Corporate
      // Create account #1 Corporate payer
      var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

      // Create account #2 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor2 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);

      // Create account #3 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor3 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);
      
      // Create/Verify Product Offering Exists
      var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

      var productOffering = ProductOfferingFactory.Create(pofConfiguration);
      var idProductOfferingToQuoteFor = productOffering.ID;

      //Values to use for verification
      
      #endregion

      #region Test

      // Ask backend to start quote

      //Prepare request
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor2);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor3);
      quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
      quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
      quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
      quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
      quoteImpl.Request.EffectiveDate = MetraTime.Now;
      quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddDays(1);
      quoteImpl.Request.Localization = "en-US";
      quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);
      quoteImpl.Request.SubscriptionParameters.CorporateAccountId = idAccountToQuoteFor;
      quoteImpl.Request.SubscriptionParameters.IsGroupSubscription = true;

      expected.CountAccounts = quoteImpl.Request.Accounts.Count;
      expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
      expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
      expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

      expected.TotalForUDRCs = 30;//introduce formula based on PT

      expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                      (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                      (expected.CountNRCs * pofConfiguration.NRCAmount);

      //Give request to testing scenario along with expected results for verification; get back response for further verification

      quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);


      #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T06QuotingWithGroupSubscriptionTwoTimes_PositiveTest()
    {
      #region Prepare
      string testName = "QuotingWithGroupSubscription";
      string testShortName = "Q_GSub"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
      string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

      QuoteImplementationData quoteImpl = new QuoteImplementationData();
      QuoteVerifyData expected = new QuoteVerifyData();

      // Create account #1 Corporate
      // Create account #1 Corporate payer
      var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

      // Create account #2 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor2 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);

      // Create account #3 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor3 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);

      // Create/Verify Product Offering Exists
      var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
      Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
      int idProductOfferingToQuoteFor = productOffering.ID;

      //Values to use for verification
      //decimal expectedQuoteTotal = (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2) + (pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
      expected.Currency = "USD";

      #endregion

      #region Test

      // Ask backend to start quote

      //Prepare request
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor2);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor3);
      quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
      quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
      quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
      quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
      quoteImpl.Request.EffectiveDate = MetraTime.Now;
      quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddHours(1);
      quoteImpl.Request.Localization = "en-US";
      quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);
      quoteImpl.Request.SubscriptionParameters.CorporateAccountId = idAccountToQuoteFor;
      quoteImpl.Request.SubscriptionParameters.IsGroupSubscription = true;

      expected.CountAccounts = quoteImpl.Request.Accounts.Count;
      expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
      expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
      expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

      expected.TotalForUDRCs = 30;//introduce formula based on PT

      expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                   (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                   (expected.CountNRCs * pofConfiguration.NRCAmount);

      //Give request to testing scenario along with expected results for verification; get back response for further verification
      QuoteResponse response1 = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

      QuoteResponse response2 = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

      Assert.AreEqual(response1.TotalAmount, response2.TotalAmount, "Total amount was different on the second run");


      #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T07SubscribeAfterQuoting_PositiveTest()
    {
      #region Prepare
      string testName = "SubscribeAfterQuoting";
      string testShortName = "Q_SubA"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
      string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

      QuoteImplementationData quoteImpl = new QuoteImplementationData();
      var expected = new QuoteVerifyData();

      // Create account      
      var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

      // Create/Verify Product Offering Exists
      var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
      Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
      var idProductOfferingToQuoteFor = productOffering.ID;

      //Values to use for verification
      expected.Currency = "USD";

      #endregion

      #region Test

      //Prepare request
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
      quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
      quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
      quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
      quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
      quoteImpl.Request.EffectiveDate = MetraTime.Now;
      quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
      quoteImpl.Request.Localization = "en-US";
      quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

      expected.CountAccounts = quoteImpl.Request.Accounts.Count;
      expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
      expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
      expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

      expected.TotalForUDRCs = 30;

      expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                   (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                   (expected.CountNRCs * pofConfiguration.NRCAmount);

      //Give request to testing scenario along with expected results for verification; get back response for further verification
      quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

      #region subscribe after quote
          MTSubscription subscription = null;
          MTPCAccount account = null;

          try
          {
            account = SharedTestCode.CurrentProductCatalog.GetAccount(idAccountToQuoteFor);

            var effDate = new MTPCTimeSpanClass
              {
                StartDate = quoteImpl.Request.EffectiveDate,
                StartDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE
              };

            object modifiedDate = MetraTime.Now;
            subscription = account.Subscribe(idProductOfferingToQuoteFor, effDate, out modifiedDate);
          }
          catch (Exception ex)
          {
            Assert.Fail("Creating subscription after quotion failed with exception: " + ex);
          }
          finally
          {
            if (subscription != null)
            {
              account.RemoveSubscription(subscription.ID);
            }
          }
      #endregion

      #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T08SubscribeAfterQuotingWithGroupSubscription_PositiveTest()
    {
      #region Prepare

      string testName = "SubscribeAfterQuotingWithGroupSubscription";
      string testShortName = "Q_GSubA";
        //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
      string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

      var quoteImpl = new QuoteImplementationData();
      var expected = new QuoteVerifyData();

      // Create account #1 Corporate
      // Create account #1 Corporate payer
      var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

      // Create account #2 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor2 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);

      // Create account #3 Department child
      testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
      var idAccountToQuoteFor3 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);
      
      // Create/Verify Product Offering Exists
      var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
      Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
      int idProductOfferingToQuoteFor = productOffering.ID;

      //Values to use for verification
      //decimal expectedQuoteTotal = (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2) + (pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
      expected.Currency = "USD";

      #endregion

      #region Test

      // Ask backend to start quote

      //Prepare request
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor2);
      quoteImpl.Request.Accounts.Add(idAccountToQuoteFor3);
      quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
      quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
      quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
      quoteImpl.Request.ReportParameters = new ReportParams
        {
          PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault
        };
      quoteImpl.Request.EffectiveDate = MetraTime.Now;
      quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddHours(1);
      quoteImpl.Request.Localization = "en-US";
      quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);
      quoteImpl.Request.SubscriptionParameters.CorporateAccountId = idAccountToQuoteFor;
      quoteImpl.Request.SubscriptionParameters.IsGroupSubscription = true;

      expected.CountAccounts = quoteImpl.Request.Accounts.Count;
      expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
      expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
      expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

      expected.TotalForUDRCs = 30; //introduce formula based on PT

      expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                   (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                   (expected.CountNRCs * pofConfiguration.NRCAmount);

      //Give request to testing scenario along with expected results for verification; get back response for further verification
      quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

      #region create group subscription
      var createdGroupSubsciptions = new List<IMTGroupSubscription>();
      try
      {        
        var effectiveDate = new MTPCTimeSpanClass
          {
            StartDate = quoteImpl.Request.EffectiveDate,
            StartDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE,
            EndDate = quoteImpl.Request.EffectiveEndDate,
            EndDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE
          };

        //TODO: Figure out correct cycle for group sub or if it should be passed
        var groupSubscriptionCycle = new MTPCCycle
          {
            CycleTypeID = 1,
            EndDayOfMonth = 31
          };

        IMTGroupSubscription mtGroupSubscription = SharedTestCode.CurrentProductCatalog.CreateGroupSubscription();
        mtGroupSubscription.EffectiveDate = effectiveDate;
        mtGroupSubscription.ProductOfferingID = idProductOfferingToQuoteFor;
        mtGroupSubscription.ProportionalDistribution = true;
        mtGroupSubscription.Name = string.Format("TempQuoteGSForPO_{0}", idProductOfferingToQuoteFor);
        mtGroupSubscription.Description = "Group subscription for Quoting. ProductOffering: " +
                                          idProductOfferingToQuoteFor;
        mtGroupSubscription.SupportGroupOps = true;
        mtGroupSubscription.CorporateAccount = quoteImpl.Request.SubscriptionParameters.CorporateAccountId;
        mtGroupSubscription.Cycle = groupSubscriptionCycle;

        foreach (MTPriceableItem pi in SharedTestCode.CurrentProductCatalog.GetProductOffering(idProductOfferingToQuoteFor).GetPriceableItems())
        {
          switch (pi.Kind)
          {
            case MTPCEntityType.PCENTITY_TYPE_RECURRING:
                  mtGroupSubscription.SetChargeAccount(pi.ID, quoteImpl.Request.SubscriptionParameters.CorporateAccountId,
                                                   quoteImpl.Request.EffectiveDate, quoteImpl.Request.EffectiveEndDate);
              break;
            case MTPCEntityType.PCENTITY_TYPE_RECURRING_UNIT_DEPENDENT:
              {
                  mtGroupSubscription.SetChargeAccount(pi.ID, quoteImpl.Request.SubscriptionParameters.CorporateAccountId,
                                                     quoteImpl.Request.EffectiveDate, quoteImpl.Request.EffectiveEndDate);


                  if (quoteImpl.Request.SubscriptionParameters.UDRCValues.ContainsKey(idProductOfferingToQuoteFor.ToString()))
                {
                    foreach (var udrcInstanceValue in quoteImpl.Request.SubscriptionParameters.UDRCValues[idProductOfferingToQuoteFor.ToString()])
                  {
                    mtGroupSubscription.SetRecurringChargeUnitValue(udrcInstanceValue.UDRC_Id,
                                                                    udrcInstanceValue.Value,
                                                                    udrcInstanceValue.StartDate,
                                                                    udrcInstanceValue.EndDate);
                  }
                }
                break;
              }
          }
        }        

        mtGroupSubscription.Save();
        //createdGroupSubsciptions.Add(mtGroupSubscription);

        foreach (int idAccount in quoteImpl.Request.Accounts)
        {
          var mtGsubMember = new MTGSubMember
            {
              AccountID = idAccount,
              StartDate = quoteImpl.Request.EffectiveDate,
              EndDate = quoteImpl.Request.EffectiveEndDate
            };

          mtGroupSubscription.AddAccount(mtGsubMember);
        }

        mtGroupSubscription.Save();
        createdGroupSubsciptions.Add(mtGroupSubscription);
      }
      catch (Exception ex)
      {
        Assert.Fail("Creating group subscription after quoting failed with exception: " + ex);
      }
      finally
      {
        // Remove group subscriptions
        foreach (var subscription in createdGroupSubsciptions)
        {
          // Unsubscribe members
          foreach (var idAccount in quoteImpl.Request.Accounts)
          {
            IMTGSubMember gsmember = new MTGSubMemberClass();
            gsmember.AccountID = idAccount;

            if (subscription.FindMember(idAccount, quoteImpl.Request.EffectiveDate) != null)
            {
              subscription.UnsubscribeMember((MTGSubMember) gsmember);
            }
          }

          using (IMTNonServicedConnection conn = ConnectionManager.CreateNonServicedConnection())
          {
            using (IMTCallableStatement stmt = conn.CreateCallableStatement("REMOVEGSUBS_QUOTING"))
            {
              stmt.AddParam("p_id_sub", MTParameterType.Integer, subscription.ID);
              stmt.AddParam("p_systemdate", MTParameterType.DateTime, quoteImpl.Request.EffectiveDate);
              stmt.AddParam("p_status", MTParameterType.Integer, 0);
              stmt.ExecuteNonQuery();
            }

            using (var stmt = conn.CreateAdapterStatement("Queries\\Quoting", "__REMOVE_RC_METRIC_VALUES__"))
            {
              stmt.AddParam("%%ID_SUB%%", subscription.ID);
              stmt.ExecuteNonQuery();
            }
          }
        }
      }
      #endregion
      #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T09QuotingWithExistingSubscriptionToDifferentPO_PositiveTest()
    {
        #region Prepare
        var testName = "QuotingWithExistingSubscriptionToDifferentPO";
        var testShortName = "Q_OtherPO"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        //string testDescription = @"Given an account with existing subscription to a Product Offering and a quote request to a second Product Offering, When quote is run Then it includes only the usage for the second PO";
        string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

        var quoteImpl = new QuoteImplementationData();
        var expected = new QuoteVerifyData();

        // Create account
        var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
            {
                CountPairRCs = 1,
                CountNRCs = 1,
                Currency = "USD"
            };

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        var idExisitingProductOffering = productOffering.ID;

        // Subscribe account to PO
        var effDate = new MTPCTimeSpanClass
        {
            StartDate = MetraTime.Now,
            StartDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE
        };
        object modifiedDate = MetraTime.Now;

        IMTProductCatalog productCatalog = new MTProductCatalogClass();
        var sessionContext = (IMTSessionContext)SharedTestCode.LoginAsSU();

        productCatalog.SetSessionContext(sessionContext);

        var acc = productCatalog.GetAccount(idAccountToQuoteFor);

        var subscription = acc.Subscribe(idExisitingProductOffering, effDate, out modifiedDate);

        //Values to use for verification
        expected.CountAccounts = 1;
        expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);



        expected.Total = pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 +
                                     pofConfiguration.CountNRCs * pofConfiguration.NRCAmount;

        #endregion

        #region Test and Verify

        try
        {
            // Create another PO
            pofConfiguration.UniqueIdentifier = MetraTime.Now.ToString();
            productOffering = ProductOfferingFactory.Create(pofConfiguration);

            Assert.IsNotNull(productOffering.ID, "Unable to create PO the second time for test run");
            int idProductOfferingToQuoteFor = productOffering.ID;

            //Prepare quote request
            quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
            quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
            quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
            quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
            quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
            quoteImpl.Request.EffectiveDate = MetraTime.Now;
            quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
            quoteImpl.Request.Localization = "en-US";

            quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);
            // Make sure account is still subscribed to initial PO
            Assert.IsNotNull(acc.GetSubscriptionByProductOffering(idExisitingProductOffering));
        }
        finally
        {
            // Remove created subscription
            acc.RemoveSubscription(subscription.ID);
        }

        #endregion
    }
 
    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T10QuotingWithIcbForMultipleAccounts_PositiveTest()
    {
        string billcycle = "Annually";

        #region Prepare

        string testName = "QuotingWithIcbForMultipleAccounts_BillingCycles_" + billcycle;
        string testShortName = "Q_GSub";//Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier

        string testRunUniqueIdentifier = MetraTime.NowWithMilliSec; //Identifier to make this run unique

        QuoteImplementationData quoteImpl = new QuoteImplementationData();
        QuoteVerifyData expected = new QuoteVerifyData();

        #region create accounts

        List<MetraTech.DomainModel.BaseTypes.Account> Hierarchy = SharedTestCode.CreateHierarchyofAccounts(billcycle, testShortName,
                                                                              testRunUniqueIdentifier);
        Hierarchy.AddRange(SharedTestCode.CreateHierarchyofAccounts(billcycle, testShortName + "_2", testRunUniqueIdentifier + "_2"));

        var independent = new IndependentAccountFactory(testShortName, testRunUniqueIdentifier);
        independent.CycleType = UsageCycleType.Annually;
        independent.Instantiate();

        var independent2 = new IndependentAccountFactory(testShortName + "_2", testRunUniqueIdentifier + "_2");
        independent2.CycleType = UsageCycleType.Annually;
        independent2.Instantiate();

        #endregion

        #region Create/Verify Product Offering Exists

        IMTProductOffering productOffering;
        var pofConfiguration = SharedTestCode.CreateProductOfferingConfiguration(testName, testRunUniqueIdentifier, out productOffering);//set count of PIs inside this method

        #endregion


        int idProductOfferingToQuoteFor = productOffering.ID;

        using (var client = new PriceListServiceClient())
        {
            if (client.ClientCredentials != null)
            {
                client.ClientCredentials.UserName.UserName = "su";
                client.ClientCredentials.UserName.Password = "su123";
            }

            IMTCollection instances = productOffering.GetPriceableItems();

            var productOfferingFactory = new ProductOfferingFactory();
            productOfferingFactory.Initialize(testName, testRunUniqueIdentifier);

            var parameterTableFlatRc = productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComFlatrecurringcharge);

            var parameterTableNonRc = productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComNonrecurringcharge);
            var parameterTableUdrcTapered = productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComUdrctapered);

            var parameterTableUdrcTiered = productOfferingFactory.ProductCatalog.GetParamTableDefinitionByName(SharedTestCode.MetratechComUdrctiered);

            #region Set Allow ICB for PIs

            foreach (IMTPriceableItem possibleRC in instances)
            {
                if (possibleRC.Kind == MTPCEntityType.PCENTITY_TYPE_RECURRING_UNIT_DEPENDENT)
                {
                    var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableUdrcTapered.ID, SharedTestCode.MetratechComUdrctapered);
                    pofConfiguration.PriceableItemsAndParameterTableForUdrc.Add(piAndPTParameters);

                    piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableUdrcTiered.ID, SharedTestCode.MetratechComUdrctiered);
                    pofConfiguration.PriceableItemsAndParameterTableForUdrc.Add(piAndPTParameters);

                }
                else if (possibleRC.Kind == MTPCEntityType.PCENTITY_TYPE_RECURRING)
                {
                    var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableFlatRc.ID, SharedTestCode.MetratechComFlatrecurringcharge);
                    pofConfiguration.PriceableItemsAndParameterTableForRc.Add(piAndPTParameters);
                }
                else if (possibleRC.Kind == MTPCEntityType.PCENTITY_TYPE_NON_RECURRING)
                {
                    var piAndPTParameters = SharedTestCode.SetAllowICBForPI(possibleRC, client, productOffering.ID, parameterTableNonRc.ID, SharedTestCode.MetratechComNonrecurringcharge);
                    pofConfiguration.PriceableItemsAndParameterTableForNonRc.Add(piAndPTParameters);
                }
            }

            #endregion
        }

        //Values to use for verification
        expected.Currency = "USD";

        #endregion

        #region Test

        // Ask backend to start quote

        //Prepare request

        foreach (var hierarhy in Hierarchy)
        {
            quoteImpl.Request.Accounts.Add(hierarhy._AccountID.Value);
        }

        quoteImpl.Request.Accounts.Add((int)independent.Item._AccountID);
        quoteImpl.Request.Accounts.Add((int)independent2.Item._AccountID);

        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams()
        {
            PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault
        };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
        quoteImpl.Request.Localization = "en-US";
        quoteImpl.Request.SubscriptionParameters.UDRCValues =
            SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

        expected.CountAccounts = quoteImpl.Request.Accounts.Count;
        expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
        expected.CountFlatRCs = (pofConfiguration.CountPairRCs * expected.CountAccounts) * 2;
        expected.CountUDRCs = (pofConfiguration.CountPairUDRCs * expected.CountAccounts) * 2;

        pofConfiguration.RCAmount = 66.66m;
        pofConfiguration.NRCAmount = 77.77m;
        expected.TotalForUDRCs = 15 * 16.6m + 5 * 13m;

        expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                     (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                     (expected.CountNRCs * pofConfiguration.NRCAmount);

        #region Initialize ICB prices

        quoteImpl.Request.IcbPrices = new List<IndividualPrice>();

        if (pofConfiguration.PriceableItemsAndParameterTableForRc != null &&
            pofConfiguration.PriceableItemsAndParameterTableForRc.Count > 0)
        {
            var chargeRate = new ChargesRate { Price = 66.66m };
            var qip = new IndividualPrice
            {
                CurrentChargeType = ChargeType.RecurringCharge,
                ProductOfferingId = idProductOfferingToQuoteFor,
                ChargesRates = new List<ChargesRate> { chargeRate },
                PriceableItemId = null
            };

            quoteImpl.Request.IcbPrices.Add(qip);
        }

        if (pofConfiguration.PriceableItemsAndParameterTableForUdrc != null &&
            pofConfiguration.PriceableItemsAndParameterTableForUdrc.Count > 0)
        {
            var qip = new IndividualPrice
            {
                CurrentChargeType = ChargeType.UDRCTapered,
                ProductOfferingId = idProductOfferingToQuoteFor,
                PriceableItemId = null
            };
            var chargeRates = new List<ChargesRate>
                    {
                        new ChargesRate {UnitValue = 15, UnitAmount = 16.6m},
                        new ChargesRate {UnitValue = 40, UnitAmount = 13m}
                    };
            qip.ChargesRates.AddRange(chargeRates);
            quoteImpl.Request.IcbPrices.Add(qip);

            var chargeRate = new ChargesRate { UnitValue = 20, UnitAmount = 16.6m, BaseAmount = 10m };
            qip = new IndividualPrice
            {
                CurrentChargeType = ChargeType.UDRCTiered,
                ProductOfferingId = idProductOfferingToQuoteFor,
                ChargesRates = new List<ChargesRate> { chargeRate },
                PriceableItemId = null
            };

            quoteImpl.Request.IcbPrices.Add(qip);
        }

        if (pofConfiguration.PriceableItemsAndParameterTableForRc != null &&
            pofConfiguration.PriceableItemsAndParameterTableForRc.Count > 0)
        {


            var chargeRate = new ChargesRate { Price = 77.77m };
            var qip = new IndividualPrice
            {
                CurrentChargeType = ChargeType.NonRecurringCharge,
                ProductOfferingId = idProductOfferingToQuoteFor,
                ChargesRates = new List<ChargesRate> { chargeRate },
                PriceableItemId = null
            };

            quoteImpl.Request.IcbPrices.Add(qip);
        }

        #endregion

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);


        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T11QuotingWithMonthlyInAdvanceAndArrearsRC_PositiveTest()
    {
        #region Prepare
        string testName = "QuotingWithMonthlyInAdvanceAndArrearsRC";
        string testShortName = "Q_MonAdv"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        //string testDescription = @"";
        string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

        QuoteImplementationData quoteImpl = new QuoteImplementationData();
        QuoteVerifyData expected = new QuoteVerifyData();

        var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier);

        // Create account
        CorporateAccountFactory corpAccountHolder = new CorporateAccountFactory(testShortName, testRunUniqueIdentifier);
        corpAccountHolder.Instantiate();

        Assert.IsNotNull(corpAccountHolder.Item._AccountID, "Unable to create account for test run");
        int idAccountToQuoteFor = (int)corpAccountHolder.Item._AccountID;

        // Create/Verify Product Offering Exists

        pofConfiguration.CountNRCs = 1;
        pofConfiguration.CountPairRCs = 1;

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        int idProductOfferingToQuoteFor = productOffering.ID;

        IMTProductCatalog productCatalog = new MTProductCatalogClass();
        IMTSessionContext sessionContext = (IMTSessionContext)SharedTestCode.LoginAsSU();

        productCatalog.SetSessionContext(sessionContext);

        #region Add monthly in advance RC

        productOffering.AvailabilityDate.SetStartDateNull();

        IMTPriceableItemType priceableItemTypeFRRC = productCatalog.GetPriceableItemTypeByName("Flat Rate Recurring Charge");

        if (priceableItemTypeFRRC == null)
        {
            throw new ApplicationException("'Flat Rate Recurring Charge' Priceable Item Type not found");
        }

        var pl = productOffering.GetNonSharedPriceList();

        string fullName = string.Concat("FRRC_Monthly_In_Advance_", MetraTime.Now);

        var piTemplate_FRRC = (IMTRecurringCharge)priceableItemTypeFRRC.CreateTemplate(false);
        piTemplate_FRRC.Name = fullName;
        piTemplate_FRRC.DisplayName = fullName;
        piTemplate_FRRC.Description = fullName;
        piTemplate_FRRC.ChargeInAdvance = true;
        piTemplate_FRRC.ProrateOnActivation = false;
        piTemplate_FRRC.ProrateOnDeactivation = false;
        piTemplate_FRRC.ProrateOnRateChange = false;
        piTemplate_FRRC.FixedProrationLength = false;
        piTemplate_FRRC.ChargePerParticipant = true;
        IMTPCCycle pcCycle = piTemplate_FRRC.Cycle;
        pcCycle.CycleTypeID = 1;
        pcCycle.EndDayOfMonth = 31;
        piTemplate_FRRC.Save();

        IMTParamTableDefinition pt = productCatalog.GetParamTableDefinitionByName("metratech.com/flatrecurringcharge");
        IMTRateSchedule sched = pt.CreateRateSchedule(pl.ID, piTemplate_FRRC.ID);
        sched.ParameterTableID = pt.ID;
        sched.Description = "Unit Test Rates";
        sched.EffectiveDate.StartDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE;
        sched.EffectiveDate.StartDate = DateTime.Parse("1/1/2000");
        sched.EffectiveDate.EndDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE;
        sched.EffectiveDate.EndDate = DateTime.Parse("1/1/2038");

        sched.Save();

        sched.RuleSet.Read(string.Format("{0}\\Development\\Core\\MTProductCatalog\\{1}",
                           Environment.GetEnvironmentVariable("METRATECHTESTDATABASE"),
                           "flatrcrules1.xml"));

        sched.SaveWithRules();

        productOffering.AddPriceableItem((MTPriceableItem)piTemplate_FRRC);

        productOffering.AvailabilityDate.StartDate = MetraTime.Now;

        #endregion

        //Values to use for verification
        expected.Total = (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2) + //First the in arrears monthly 'pairs' of per participant and per subscription
                                       pofConfiguration.RCAmount + //Then our one monthly in advance charge (same amount)
                                       pofConfiguration.CountNRCs * pofConfiguration.NRCAmount; //Then our 1 setup charge for the subscription (NRC)
        expected.Currency = "USD";

        expected.CountNRCs = 1;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs * 2 + 1;

        #endregion


        #region Test and Verify
        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddMonths(2);
        quoteImpl.Request.Localization = "en-US";

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T12QuotingWithYearlyInAdvanceAndArrearsRC_PositiveTest()
    {
        #region Prepare
        string testName = "QuotingWithYearlyInAdvanceAndArrearsRC";
        string testShortName = "Q_YearAdv"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        //string testDescription = @"";
        string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

        QuoteImplementationData quoteImpl = new QuoteImplementationData();
        QuoteVerifyData expected = new QuoteVerifyData();

        // Create account
        CorporateAccountFactory corpAccountHolder = new CorporateAccountFactory(testShortName, testRunUniqueIdentifier);
        corpAccountHolder.CycleType = UsageCycleType.Annually;
        corpAccountHolder.Instantiate();

        Assert.IsNotNull(corpAccountHolder.Item._AccountID, "Unable to create account for test run");
        int idAccountToQuoteFor = (int)corpAccountHolder.Item._AccountID;

        int countOfMonthsToCharge = (12 - MetraTime.Now.Month + 1);

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier);

        pofConfiguration.CountNRCs = 1;
        pofConfiguration.CountPairRCs = 1;

        IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        int idProductOfferingToQuoteFor = productOffering.ID;

        IMTProductCatalog productCatalog = new MTProductCatalogClass();
        IMTSessionContext sessionContext = (IMTSessionContext)SharedTestCode.LoginAsSU();

        productCatalog.SetSessionContext(sessionContext);

        #region Add monthly in advance RC

        productOffering.AvailabilityDate.SetStartDateNull();

        IMTPriceableItemType priceableItemTypeFRRC = productCatalog.GetPriceableItemTypeByName("Flat Rate Recurring Charge");

        if (priceableItemTypeFRRC == null)
        {
            throw new ApplicationException("'Flat Rate Recurring Charge' Priceable Item Type not found");
        }

        var pl = productOffering.GetNonSharedPriceList();

        string fullName = string.Concat("FRRC_Monthly_In_Advance_", MetraTime.Now);

        var piTemplate_FRRC = (IMTRecurringCharge)priceableItemTypeFRRC.CreateTemplate(false);
        piTemplate_FRRC.Name = fullName;
        piTemplate_FRRC.DisplayName = fullName;
        piTemplate_FRRC.Description = fullName;
        piTemplate_FRRC.ChargeInAdvance = true;
        piTemplate_FRRC.ProrateOnActivation = false;
        piTemplate_FRRC.ProrateOnDeactivation = false;
        piTemplate_FRRC.ProrateOnRateChange = false;
        piTemplate_FRRC.FixedProrationLength = false;
        piTemplate_FRRC.ChargePerParticipant = true;
        IMTPCCycle pcCycle = piTemplate_FRRC.Cycle;
        pcCycle.Relative = true;

        piTemplate_FRRC.Save();

        IMTParamTableDefinition pt = productCatalog.GetParamTableDefinitionByName("metratech.com/flatrecurringcharge");
        IMTRateSchedule sched = pt.CreateRateSchedule(pl.ID, piTemplate_FRRC.ID);
        sched.ParameterTableID = pt.ID;
        sched.Description = "Unit Test Rates";
        sched.EffectiveDate.StartDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE;
        sched.EffectiveDate.StartDate = DateTime.Parse("1/1/2000");
        sched.EffectiveDate.EndDateType = MTPCDateType.PCDATE_TYPE_ABSOLUTE;
        sched.EffectiveDate.EndDate = DateTime.Parse("1/1/2038");

        sched.Save();

        sched.RuleSet.Read(string.Format("{0}\\Development\\Core\\MTProductCatalog\\{1}",
                           Environment.GetEnvironmentVariable("METRATECHTESTDATABASE"),
                           "flatrcrules1.xml"));

        sched.SaveWithRules();

        productOffering.AddPriceableItem((MTPriceableItem)piTemplate_FRRC);

        productOffering.AvailabilityDate.StartDate = MetraTime.Now;

        #endregion

        //Values to use for verification
        expected.Total = (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 * countOfMonthsToCharge) + //First the in arrears monthly 'pairs' of per participant and per subscription
                                       pofConfiguration.RCAmount + //Then our one yearly in advance charge (same amount)
                                       pofConfiguration.CountNRCs * pofConfiguration.NRCAmount; //Then our 1 setup charge for the subscription (NRC)
        expected.Currency = "USD";

        expected.CountNRCs = 1;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs * 2 * countOfMonthsToCharge + 1;
        #endregion


        #region Test and Verify
        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddYears(1);
        quoteImpl.Request.Localization = "en-US";

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T13QuotingWithPOWithProrationRCs_PositiveTest()
    {
        #region Prepare
        const string testName = "Quote_Basic";
        const string testShortName = "Q_Basic"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        var testRunUniqueIdentifier = MetraTime.Now.ToString(CultureInfo.CurrentCulture); //Identifier to make this run unique

        var quoteImpl = new QuoteImplementationData();
        var expected = new QuoteVerifyData();

        var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
            {
                CountNRCs = 1,
                CountPairRCs = 1,
                CountPairUDRCs = 1,
                rcParameters = new RCParameters() {ProrateOnActivation = true, ProrateOnDeactivation = true, ProrateOnRateChange = true}
            };

        var productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        var idProductOfferingToQuoteFor = productOffering.ID;

        #endregion

        #region Test

        // Ask backend to start quote

        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
        quoteImpl.Request.Localization = "en-US";
        quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

        expected.CountAccounts = quoteImpl.Request.Accounts.Count;
        expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
        expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

        expected.TotalForUDRCs = 30;

        var prorationDivisor = DateTime.DaysInMonth(MetraTime.Now.Year, MetraTime.Now.Month);

        expected.Total = expected.CountFlatRCs * (pofConfiguration.RCAmount / prorationDivisor) +
                                     expected.CountUDRCs.Value * (expected.TotalForUDRCs / prorationDivisor) +
                                     expected.CountNRCs * pofConfiguration.NRCAmount;

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    }

    [TestMethod, MTFunctionalTest(TestAreas.Quoting)]
    public void T14QuotingSubThenGSubWithSameAccToSamePO_PositiveTest()
    {
        #region Prepare
        const string testName = "Quote_Basic";
        const string testShortName = "Q_Basic"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
        var testRunUniqueIdentifier = MetraTime.Now.ToString(CultureInfo.CurrentCulture); //Identifier to make this run unique

        var quoteImpl = new QuoteImplementationData();
        var expected = new QuoteVerifyData();

        var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

        // Create/Verify Product Offering Exists
        var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
        {
            CountNRCs = 1,
            CountPairRCs = 1,
            CountPairUDRCs = 1
        };

        var productOffering = ProductOfferingFactory.Create(pofConfiguration);
        Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
        var idProductOfferingToQuoteFor = productOffering.ID;

        //Values to use for verification

        #endregion

        #region Test

        // Ask backend to start quote

        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddDays(1);
        quoteImpl.Request.Localization = "en-US";
        quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

        expected.CountAccounts = quoteImpl.Request.Accounts.Count;
        expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
        expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

        expected.TotalForUDRCs = 30;

        expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                     (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                     (expected.CountNRCs * pofConfiguration.NRCAmount);

        //Give request to testing scenario along with expected results for verification; get back response for further verification
        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

        #endregion
    
        #region Prepare
        quoteImpl = new QuoteImplementationData();
        // Create account #2 Department child
        testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
        var idAccountToQuoteFor2 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);

        // Create account #3 Department child
        testRunUniqueIdentifier = MetraTime.NowWithMilliSec;
        var idAccountToQuoteFor3 = SharedTestCode.GetDepartmentAccountToQuoteFor(testShortName, testRunUniqueIdentifier, idAccountToQuoteFor);
        //Values to use for verification

        #endregion

        #region Test

        // Ask backend to start quote

        //Prepare request
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor2);
        quoteImpl.Request.Accounts.Add(idAccountToQuoteFor3);
        quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
        quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
        quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
        quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
        quoteImpl.Request.EffectiveDate = MetraTime.Now;
        quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddDays(1);
        quoteImpl.Request.Localization = "en-US";
        quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);
        quoteImpl.Request.SubscriptionParameters.CorporateAccountId = idAccountToQuoteFor;
        quoteImpl.Request.SubscriptionParameters.IsGroupSubscription = true;

        expected.CountAccounts = quoteImpl.Request.Accounts.Count;
        expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
        expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
        expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

        expected.TotalForUDRCs = 30;//introduce formula based on PT

        expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                        (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                        (expected.CountNRCs * pofConfiguration.NRCAmount);

        //Give request to testing scenario along with expected results for verification; get back response for further verification

        quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);


        #endregion
    }

     

      #region duplicated tests
      //[TestMethod, MTFunctionalTest(TestAreas.Quoting)]
      //T01QuotingBasicEndToEnd_PositiveTest() duplicates T12QuotingWithIcbBasicScenario_PositiveTest()

      public void T01QuotingBasicEndToEnd_PositiveTest()
      {
          #region Prepare
          const string testName = "Quote_Basic";
          const string testShortName = "Q_Basic"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
          var testRunUniqueIdentifier = MetraTime.Now.ToString(CultureInfo.CurrentCulture); //Identifier to make this run unique

          var quoteImpl = new QuoteImplementationData();
          var expected = new QuoteVerifyData();

          var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

          // Create/Verify Product Offering Exists
          var pofConfiguration = new ProductOfferingFactoryConfiguration(testName, testRunUniqueIdentifier)
          {
              CountNRCs = 1,
              CountPairRCs = 1,
              CountPairUDRCs = 1
          };

          var productOffering = ProductOfferingFactory.Create(pofConfiguration);
          Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
          var idProductOfferingToQuoteFor = productOffering.ID;

          //Values to use for verification

          #endregion

          #region Test

          // Ask backend to start quote

          //Prepare request
          quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
          quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
          quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
          quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
          quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
          quoteImpl.Request.EffectiveDate = MetraTime.Now;
          quoteImpl.Request.EffectiveEndDate = MetraTime.Now.AddDays(1);
          quoteImpl.Request.Localization = "en-US";
          quoteImpl.Request.SubscriptionParameters.UDRCValues = SharedTestCode.GetUDRCInstanceValuesSetToMiddleValues(productOffering);

          expected.CountAccounts = quoteImpl.Request.Accounts.Count;
          expected.CountNRCs = pofConfiguration.CountNRCs * expected.CountAccounts;
          expected.CountFlatRCs = pofConfiguration.CountPairRCs + (pofConfiguration.CountPairRCs * expected.CountAccounts);
          expected.CountUDRCs = pofConfiguration.CountPairUDRCs + (pofConfiguration.CountPairUDRCs * expected.CountAccounts);

          expected.TotalForUDRCs = 30;

          expected.Total = (expected.CountFlatRCs * pofConfiguration.RCAmount) +
                                       (expected.CountUDRCs.Value * expected.TotalForUDRCs) +
                                       (expected.CountNRCs * pofConfiguration.NRCAmount);

          //Give request to testing scenario along with expected results for verification; get back response for further verification
          quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

          #endregion
      }
      //[TestMethod, MTFunctionalTest(TestAreas.Quoting)]
      public void T03QuotingWithMultipleProductOfferings()
      {
          #region Prepare

          string testName = "QuotingWithMultipleProductOfferings";
          string testShortName = "Q_MultiPO"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
          //string testDescription = @"Given an account and a quote request for more than one Product Offering, When quote is run Then it includes all the usage";
          string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

          QuoteImplementationData quoteImpl = new QuoteImplementationData();
          var expected = new QuoteVerifyData { CountProducts = 4 };

          // Create account for test run
          var idAccountToQuoteFor = SharedTestCode.GetCorporateAccountToQuoteFor(testShortName, testRunUniqueIdentifier);

          // Create/Verify Product Offering Exists
          var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier)
          {
              CountNRCs = 2,
              CountPairRCs = 1
          };

          //Now generate the Product Offerings we need
          var posToAdd = new List<int>();

          for (var i = 1; i < expected.CountProducts + 1; i++)
          {
              pofConfiguration.Name = testShortName + "_" + i;
              pofConfiguration.UniqueIdentifier = testRunUniqueIdentifier + "_" + i;
              IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
              Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
              posToAdd.Add(productOffering.ID);
          }

          //Values to use for verification
          expected.CountNRCs = expected.CountProducts * pofConfiguration.CountNRCs;
          expected.CountFlatRCs = expected.CountProducts * pofConfiguration.CountPairRCs * 2; //???

          expected.Total = expected.CountProducts * (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 + pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
          expected.Currency = "USD";

          #endregion

          #region Test and Verify

          //Prepare request
          quoteImpl.Request.Accounts.Add(idAccountToQuoteFor);
          quoteImpl.Request.ProductOfferings.AddRange(posToAdd);
          quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
          quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
          quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
          quoteImpl.Request.EffectiveDate = MetraTime.Now;
          quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
          quoteImpl.Request.Localization = "en-US";

          //Give request to testing scenario along with expected results for verification; get back response for further verification
          quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

          #endregion
      }

      //[TestMethod, MTFunctionalTest(TestAreas.Quoting)]
      public void T04QuotingWithMultipleAccounts()
      {
          #region Prepare

          string testName = "QuotingWithMultipleAccounts";
          string testShortName = "Q_MAcc"; //Account name and perhaps others need a 'short' (less than 40 when combined with testRunUniqueIdentifier
          //string testDescription = @"Given a quote request for multiple accounts and a Product Offering, When quote is run Then it includes all the usage";
          string testRunUniqueIdentifier = MetraTime.Now.ToString(); //Identifier to make this run unique

          QuoteImplementationData quoteImpl = new QuoteImplementationData();
          QuoteVerifyData expected = new QuoteVerifyData();

          expected.CountAccounts = 4;

          // Create accounts
          var corpAccountHolders = new List<int>();

          for (var i = 1; i < expected.CountAccounts + 1; i++)
          {
              corpAccountHolders.Add(SharedTestCode.GetCorporateAccountToQuoteFor(testShortName + "_" + i, testRunUniqueIdentifier));
          }

          // Create/Verify Product Offering Exists
          // Create/Verify Product Offering Exists
          var pofConfiguration = new ProductOfferingFactoryConfiguration(testShortName, testRunUniqueIdentifier)
          {
              CountNRCs = 2,
              CountPairRCs = 1
          };

          IMTProductOffering productOffering = ProductOfferingFactory.Create(pofConfiguration);
          Assert.IsNotNull(productOffering.ID, "Unable to create PO for test run");
          int idProductOfferingToQuoteFor = productOffering.ID;

          //Values to use for verification
          expected.CountNRCs = expected.CountAccounts * pofConfiguration.CountNRCs;
          expected.CountFlatRCs = expected.CountAccounts * pofConfiguration.CountPairRCs * 2; //???

          expected.Total = expected.CountAccounts * (pofConfiguration.CountPairRCs * pofConfiguration.RCAmount * 2 + pofConfiguration.CountNRCs * pofConfiguration.NRCAmount);
          expected.Currency = "USD";

          #endregion

          #region Test and Verify

          //Prepare request
          quoteImpl.Request.Accounts.AddRange(corpAccountHolders);
          quoteImpl.Request.ProductOfferings.Add(idProductOfferingToQuoteFor);
          quoteImpl.Request.QuoteIdentifier = "MyQuoteId-" + testShortName + "-1234";
          quoteImpl.Request.QuoteDescription = "Quote generated by Automated Test: " + testName;
          quoteImpl.Request.ReportParameters = new ReportParams() { PDFReport = QuotingTestScenarios.RunPDFGenerationForAllTestsByDefault };
          quoteImpl.Request.EffectiveDate = MetraTime.Now;
          quoteImpl.Request.EffectiveEndDate = MetraTime.Now;
          quoteImpl.Request.Localization = "en-US";

          //Give request to testing scenario along with expected results for verification; get back response for further verification
          quoteImpl.Response = QuotingTestScenarios.CreateQuoteAndVerifyResults(quoteImpl, expected);

          #endregion
      }
      #endregion
  }
}
