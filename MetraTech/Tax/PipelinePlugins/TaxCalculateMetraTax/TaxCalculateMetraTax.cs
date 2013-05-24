#region Generated using ICE (Do not modify this region)
/// Generated using ICE
/// ICE CodeGen Version: 1.0.0
/// Transactional = false
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MetraTech.Pipeline;
using MetraTech.Tax.Framework.DataAccess;
using MetraTech.Tax.Framework.MetraTax;
using MetraTech.DataAccess;
using MetraTech.DomainModel.Enums.Tax.Metratech_com_tax;
#region AutoGenerated Enum Includes (Do not modify this region)
//ENUM_USING
#endregion

namespace MetraTech.Tax.Plugins
{
    public sealed partial class TaxCalculateMetraTax : PlugInBase
    {
		#region ProcessAllSessions (Only modify this in the rare case you need to handle more than one session at once)
        /// <summary>
        /// This method is called each time the plug-in recieves a session set
        /// </summary>
        protected override void ProcessAllSessions(PropertiesCollection propsCol)
        {
            bool partiallyFailed = false;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Make sure we don't write details more than once
            m_sessionSetTransactionDetails.Clear();

            foreach (Properties props in propsCol)
            {
                //set the current session for logging purposes
                SetCurrentSession(props.Session);

                try
                {
                    //process each session
                    ProcessSession(props);
                }
                catch (Exception ex)
                {
                    partiallyFailed = true;
                    Log(LogLevel.Warning,
                        string.Format("Error when processing session: {0}", ex.Message));
                    Log(LogLevel.Warning, "Exception: " + ex.Message + " StackTrace: " +
                        ex.StackTrace);
                    //mark the session as failed
                    props.Session.MarkAsFailed(
                        string.Format("Error when processing session. Source: {0}, Message: {1}",
                        ex.Source, ex.Message), MARK_AS_FAILED_CODE_FAILED);
                }
                finally
                {
                    Log(LogLevel.Debug, "Session object disposed successfully");
                    props.Session.Dispose();
                }
            }

            //if any of the sessions failed, go ahead and throw an exception here
            if (partiallyFailed)
                throw new MetraTech.Pipeline.PlugIns.PartialFailureException();

            // All of the sessions completed successfully. If appropriate, store
            // tax transaction details in the t_tax_details table via bulk insert.
            StoreTransactionDetailsInDb();
            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessAllSessions numSessions={0} ms={1}", propsCol.Count,
                stopWatch.ElapsedMilliseconds));
        }
		#endregion
    
        #region Startup override
        protected override void StartUp(MetraTech.Interop.SysContext.IMTSystemContext systemContext, MetraTech.Interop.MTPipelineLib.IMTConfigPropSet propSet)
        {
            Log(LogLevel.Debug, "Starting Startup TaxCalculateMetraTax");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //TODO: add any code you would like to run at Configure time (when the plug-in is first loaded)

            // Set up the MetraTax manager that will be used to calculate tax.
            m_taxManager = new MetraTaxSyncTaxManagerDBBatch();

            // Create a list to hold info that will later be written to t_tax_details
            m_sessionSetTransactionDetails = new List<TaxManagerPersistenceObject>();
            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.Startup ms={0}", stopWatch.ElapsedMilliseconds));
        }
        #endregion

        #region Shutdown override
        public override void Shutdown()
        {
            //TODO: add any code you would like to run when the plug-in shuts down
        }
        #endregion

        /// <summary>
        /// This method is called for each session in the session set.
        /// </summary>
        protected override void ProcessSession(Properties props)
        {
            #region Instructions
            /* -Pipeline Inputs and Outputs
             *  Pipeline inputs and outputs are accessed via the props variable.
             *  Value types (int, long, bool, enums, etc..) are marked as nullable.
             *  When you look at the type you'll see, for example, int? instead of int.
             *  Nullable value types are used because the Pipeline value could possibly be null.
             *  You can check to see if the value is null by checking the .HasValue property. 
             *  There are two ways you can get the actual value of the variable. First by using 
             *  the .Value property.
             *  Example:
             *      int a = props.Pipeline.Foo.Value; //The type of Foo is int? (a.k.a. Nullable<int>) but the type of Foo.Value is int.
             *  And second by doing a cast:
             *  Example:
             *      int a = (int)props.Pipeline.Foo; //The type of Foo is int? (a.k.a. Nullable<int>)
             *  Reference types (string, classes you write) are already nullable so there's no 
             *  need to treat them special.
             *  Example:
             *      string foo = props.Pipeline.Foo; //The type of Foo is string.
             *
             * -General Configuration Variables
             *  Accessed via the GeneralConfig property of TaxCalculateMetraTax.
             *  Example: 
             *      string filePath = GeneralConfig.FilePath;
             * 
             * -Errors
             *  If there is a critical error and you would like to make the the current session as failed,
             *  throw an exception of type ApplicationException with a custom message. 
             *  The message will be logged. The ProcessAllSessions method will catch the 
             *  exception and mark the current session as failed and move on to the next session to process.
             *  Example: 
             *      throw new ApplicationException("The value Foo was outside the expected range");
             * 
             * -Logging
             *  You can log messages via the Log function.
             *  Example: Log(LogLevel.Debug, "Test message");
             */
            #endregion
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Log(LogLevel.Debug, "Starting ProcessSession TaxCalculateMetraTax");

            // Set the tax manager parameters for this tax calculation.
            // A tax run ID of 0 is associated with a pipeline run.
            m_taxManager.TaxRunId = 0;

            // Generate a unique tax charge ID for this transaction.
            // This ID is unique with respect to tax run ID -1 (pipeline runs).
            long id_tax_charge = m_taxManager.GenerateUniqueTaxChargeIdForPipeline();
            Log(LogLevel.Debug, "The id tax charge associated with this is " + id_tax_charge);
            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessSession determined id_tax_charge ms={0}", stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();

            // Always set TaxDetailsNeeded to false so that the TaxManager will NOT write
            // to t_tax_details table.  The plugin will write to t_tax_details via
            // bulk insert if appropriate after the sessionSet is complete.
            m_taxManager.TaxDetailsNeeded = false;

            // Construct an input tax row.
            TaxableTransaction taxableTransaction = new TaxableTransaction(TaxVendor.MetraTax);
            TaxParameter taxParameter;
            
            // Parameters coming from required pipeline session variables.
            taxParameter = new TaxParameter("id_acc", "Account", Type.GetType("System.Int32"), props.Pipeline.AccountID);
            taxableTransaction.StoreTaxParameter(taxParameter);
            taxParameter = new TaxParameter("id_tax_charge", "ID Tax Charge", Type.GetType("System.Int64"), id_tax_charge);
            taxableTransaction.StoreTaxParameter(taxParameter);
            taxParameter = new TaxParameter("id_usage_interval", "Usage Interval", Type.GetType("System.Int64"), props.Pipeline.IntervalID);
            taxableTransaction.StoreTaxParameter(taxParameter);
            taxParameter = new TaxParameter("amount", "Account", Type.GetType("System.Decimal"), props.Pipeline.Amount);
            taxableTransaction.StoreTaxParameter(taxParameter);
            taxParameter = new TaxParameter("invoice_date", "Invoice Date", Type.GetType("System.DateTime"), props.Pipeline.InvoiceDate);
            taxableTransaction.StoreTaxParameter(taxParameter);

            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessSession retrieved 5 required params ms={0}", stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();

            // Parameters coming from optional pipeline session variables or plug-in configuration values.

            // Parameter: IsImpliedTax
            bool? isImpliedTax;
            if (props.Pipeline.IsImpliedTax.HasValue)
            {
                isImpliedTax = props.Pipeline.IsImpliedTax;
            }
            else
            {
                isImpliedTax = GeneralConfig.DefaultIsImpliedTax;
            }
            taxParameter = new TaxParameter("is_implied_tax", "IsImpliedTax", Type.GetType("System.String"), 
                                            (isImpliedTax.HasValue && isImpliedTax.Value)?"true":"false");
            taxableTransaction.StoreTaxParameter(taxParameter);

            // Parameter: ProductCode
            if (props.Pipeline.ProductCode != null)
            {
                Log(LogLevel.Debug, "Getting product code from session.");
                taxParameter = new TaxParameter("product_code", "Product Code", Type.GetType("System.String"), props.Pipeline.ProductCode);
            }
            else
            {
                Log(LogLevel.Debug, "Getting product code from config.");
                taxParameter = new TaxParameter("product_code", "Product Code", Type.GetType("System.String"), GeneralConfig.DefaultProductCode);
            }
            taxableTransaction.StoreTaxParameter(taxParameter);

            // Parameter: RoundingDigits
            if (props.Pipeline.RoundingDigits.HasValue)
                taxParameter = new TaxParameter("round_digits", "Rounding Digits", Type.GetType("System.Int32"), props.Pipeline.RoundingDigits);
            else
                taxParameter = new TaxParameter("round_digits", "Rounding Digits", Type.GetType("System.Int32"),GeneralConfig.DefaultRoundingDigits);
            taxableTransaction.StoreTaxParameter(taxParameter);

            // Parameter: RoundingAlgorithm
            if (props.Pipeline.RoundingAlgorithm != null)
                taxParameter = new TaxParameter("round_alg", "Rounding Algorithm", Type.GetType("System.String"), props.Pipeline.RoundingAlgorithm);
            else
                taxParameter = new TaxParameter("round_alg", "Rounding Algorithm", Type.GetType("System.String"), GeneralConfig.DefaultRoundingAlgorithm);
            taxableTransaction.StoreTaxParameter(taxParameter);
            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessSession retrieved 4 optional params ms={0}", stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();

            // Calculate the taxes
            List<TransactionIndividualTax> transactionDetails;
            TransactionTaxSummary transactionSummary;
            m_taxManager.CalculateTaxes(taxableTransaction, out transactionSummary, out transactionDetails);

            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessSession calculated taxes ms={0}", stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();
            
            // Store the taxes
            props.Pipeline.FederalTaxAmount = transactionSummary.TaxFedAmount;
            props.Pipeline.FederalTaxAmountRounded = transactionSummary.TaxFedRounded;
            props.Pipeline.FederalTaxName = transactionSummary.TaxFedName;
            props.Pipeline.StateTaxAmount = transactionSummary.TaxStateAmount;
            props.Pipeline.StateTaxAmountRounded = transactionSummary.TaxStateRounded;
            props.Pipeline.StateTaxName = transactionSummary.TaxStateName;
            props.Pipeline.CountyTaxAmount = transactionSummary.TaxCountyAmount;
            props.Pipeline.CountyTaxAmountRounded = transactionSummary.TaxCountyRounded;
            props.Pipeline.CountyTaxName = transactionSummary.TaxCountyName;
            props.Pipeline.LocalTaxAmount = transactionSummary.TaxLocalAmount;
            props.Pipeline.LocalTaxAmountRounded = transactionSummary.TaxLocalRounded;
            props.Pipeline.LocalTaxName = transactionSummary.TaxLocalName;
            props.Pipeline.OtherTaxAmount = transactionSummary.TaxOtherAmount;
            props.Pipeline.OtherTaxAmountRounded = transactionSummary.TaxOtherRounded;
            props.Pipeline.OtherTaxName = transactionSummary.TaxOtherName;

            if (GeneralConfig.ShouldTaxDetailsBeStored.HasValue && (GeneralConfig.ShouldTaxDetailsBeStored.Value == true))
            {
                m_sessionSetTransactionDetails.AddRange(transactionDetails);
            }
            stopWatch.Stop();
            Log(LogLevel.Debug,
                string.Format("TIMING: TaxCalculateMetraTax.ProcessSession finished ms={0}", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Store the contents of m_sessionSetTransactionDetails in t_tax_details table
        /// </summary>
        private void StoreTransactionDetailsInDb()
        {
            try
            {
                if (m_sessionSetTransactionDetails.Count > 0)
                {
                    TaxManagerBatchDbTableWriter tableWriter = new TaxManagerBatchDbTableWriter(
                        m_taxManager.GetTaxDetailTableName(),
                        m_taxManager.GetBulkInsertSize());

                    tableWriter.Add(m_sessionSetTransactionDetails);

                    tableWriter.Commit();
                }
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error,
                    string.Format("StoreTransactionDetailsInDb: Unable to store the details of the tax transactions in the table t_tax_details: {0}", 
                    ex.Message));
                throw;
            }
        }
         
        // Holds the MetraTax manager that is used to calculate tax. This manager holds
        // cache of information for efficiency (example: a cache of MetraTax rate schedules).
        private MetraTaxSyncTaxManagerDBBatch m_taxManager;

        // While processing sessions within the sessionSet, this member holds the
        // transaction details that eventually need to be written to the t_tax_details
        // table.  After all sessions within the sessionSet have been processed
        // successfully, the elements in this list are written to t_tax_details.
        // Note: no rows are written to the t_tax_details if any sessions fail.
        private List<TaxManagerPersistenceObject> m_sessionSetTransactionDetails;
	} 
} 
