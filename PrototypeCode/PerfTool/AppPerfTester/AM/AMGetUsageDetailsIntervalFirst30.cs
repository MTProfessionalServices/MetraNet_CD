﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using log4net;
using MetraTech.ActivityServices.Common;
using MetraTech.Core.Services.ClientProxies;
using MetraTech.DomainModel.BaseTypes;
using MetraTech.DomainModel.ProductCatalog;
using MetraTech.DomainModel.Billing;
using MetraTech.DomainModel.Enums.Core.Global;

namespace BaselineGUI
{
    public class AMGetUsageDetailsIntervalFirst30 : AppMethodBase, AppMethodI
    {
        Statistic statistic;
        UsageHistoryServiceClient client = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMGetUsageDetailsIntervalFirst30));

        public AMGetUsageDetailsIntervalFirst30()
        {
            group = "usage";
            name = "getUsageDetailsIntervalFirst30";
            fullName = "Get Usage Details IntervalFirst30";
            statistic = useStatistic(name);

            commands.Add("go", cmdGo);
        }

        public void setup()
        {
            acquireClient(out client);
        }

        public void teardown()
        {
            releaseClient(client);
            client = null;
        }

        public void cmdGo()
        {
            Stopwatch watch = new Stopwatch();

            while (true)
            {

                // determine the account where we will search for usage
                int id_acc = Framework.netMeter.pickModifiableAccountID();

                // find an open interval
                List<MetraTech.DomainModel.Billing.Interval> acctIntervals =
                    new List<MetraTech.DomainModel.Billing.Interval>();
                MetraTech.DomainModel.Billing.Interval openInterval = null;
                client.GetAccountIntervals(new AccountIdentifier(id_acc), out acctIntervals);
                foreach (var intervalCandidate in acctIntervals)
                {
                    if (intervalCandidate.Status == IntervalStatusCode.Open)
                    {
                        openInterval = intervalCandidate;
                        break;
                    }
                }


                UsageIntervalSlice usageIntervalSlice = new UsageIntervalSlice();
                usageIntervalSlice.UsageInterval = openInterval.ID;

                ReportParameters repParams = new ReportParameters();
                repParams.ReportView = ReportViewType.OnlineBill;
                repParams.InlineAdjustments = false;
                repParams.InlineVATTaxes = false;
                repParams.Language = LanguageCode.US;
                repParams.DateRange = usageIntervalSlice;

                ProductViewSlice pvSlice = new ProductViewSlice();
                pvSlice.ViewID = new PCIdentifier("metratech.com/ldperfSimplePV");
                SingleProductSlice productSlice = pvSlice;

                PayerAccountSlice paslice = new PayerAccountSlice();
                paslice.PayerID = new AccountIdentifier(id_acc);
                AccountSlice accountSlice = paslice;

                MTList<BaseProductView> list = new MTList<BaseProductView>();
                list.PageSize = 30;
                list.CurrentPage = 1;

                watch.Restart();
                client.GetUsageDetails(repParams, productSlice, accountSlice, ref list);
                watch.Stop();
                
                if (list.Items.Count < 5)
                {
                    log.ErrorFormat("expected 5 or more usage records, but got {0}",
                                    list.Items.Count);
                }
                else
                {
                    statistic.addSample(watch.ElapsedMilliseconds,
                                    String.Format("list.TotalRows={0}, list.Items.Count={1}",
                                                  list.TotalRows, list.Items.Count));
                    break;
                }
            }
        }

        public void dispose()
        {
            disposeClient(client);
            client = null;
        }
    }
}
