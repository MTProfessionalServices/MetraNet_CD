#region Generated using ICE (Do not modify this region)
/// Generated using ICE
/// ICE CodeGen Version: 1.0.0
/// Transactional = false
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MetraTech.DataAccess;
using MetraTech.Interop.MTPipelineLib;
using MetraTech.Interop.RCD;
using MetraTech.Pipeline;
#region AutoGenerated Enum Includes (Do not modify this region)
//ENUM_USING
#endregion

namespace MetraTech.Custom.Plugins.Subscription
{
    /// <summary>
    /// Pipeline Plugin returns extended subscription properties for Subscription or GroupSubscription by Id
    /// </summary>
    public sealed partial class GetSubscriptionProperties : PlugInBase
    {
        private MetraTech.Interop.SysContext.IMTNameID mNameID;
        private const string IdSubcriptionSqlParam = "%%ID_SUBSCRIPTION%%";

        #region Output parameters

        // output parameters name
        private const string SpecCharValIdColumnName = "SpecCharValId";
        private const string EntityIdColumnName = "EntityId";
        private const string ValueColumnName = "Value";
        private const string StartDateColumnName = "StartDate";
        private const string EndDateColumnName = "EndDate";
        private const string SpecNameColumnName = "SpecName";
        private const string SpecTypeColumnName = "SpecType";

        #endregion Output parameters

        #region ProcessAllSessions (Only modify this in the rare case you need to handle more than one session at once)

        /// <summary>
        /// This method is called each time the plug-in recieves a session set
        /// </summary>
        protected override void ProcessAllSessions(PropertiesCollection propsCol)
        {
            bool partiallyFailed = false;
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
        }

        #endregion

        #region Startup override

        private string _fullPathQueryFolder;
        protected override void StartUp(MetraTech.Interop.SysContext.IMTSystemContext systemContext,
                                        MetraTech.Interop.MTPipelineLib.IMTConfigPropSet propSet)
        {
            Log(LogLevel.Debug, String.Format("Starting Up the plugin {0} ... Inside Configure method.", GetPluginName()));
            try
            {
                // Get the NameID class
                mNameID = systemContext.GetNameID();
                _fullPathQueryFolder = GetQueryPath(GeneralConfig.QueryPath);

                if (!Directory.Exists(_fullPathQueryFolder))
                {
                    throw new DirectoryNotFoundException(
                        String.Format("{0} folder does not exists. Can not initialize {1} plug-in.",
                                      _fullPathQueryFolder, GetPluginName()));
                }
            }
            catch (Exception)
            {
                //TODO: I think this info will be written to log file when exception caught
                //Log(LogLevel.Fatal, string.Format("Unexpected error ..: {0}", ex.ToString()));
                throw;
            }
        }

        #endregion

        #region Shutdown override

        public override void Shutdown()
        {
            Log(LogLevel.Debug, String.Format("Shutting down the plugin: {0}", GetPluginName()));
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
             *  Accessed via the GeneralConfig property of GetSubscriptionProperties.
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

            Log(LogLevel.Debug, "Start");
            
            try
            {
                if (props.Pipeline.IdSubscription == null || props.Pipeline.IdSubscription.Value <= 0)
                    throw new COMException(CreateErrMsgParmNotSet(GetVariableName(() => props.Pipeline.IdSubscription)));


                if (props.Pipeline.IsGroupSubscription == null)
                    throw new COMException(CreateErrMsgParmNotSet(GetVariableName(() => props.Pipeline.IsGroupSubscription)));

                using (IMTServicedConnection conn = ConnectionManager.CreateConnection())
                    {
                        IMTAdapterStatement stmt = conn.CreateAdapterStatement(_fullPathQueryFolder,
                                                                               props.Pipeline.IsGroupSubscription.Value ? GeneralConfig.QueryTagForGroupSubscription
                                                                                   : GeneralConfig.QueryTagForSubscription);

                        stmt.AddParam(IdSubcriptionSqlParam, props.Pipeline.IdSubscription);

                        int countRecords = 0;
                        using (IMTDataReader dataReader = stmt.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                props.Session.SetIntegerProperty(CreatePropertyName(SpecCharValIdColumnName, countRecords),
                                                           dataReader.GetInt32(SpecCharValIdColumnName));

                                props.Session.SetIntegerProperty(CreatePropertyName(EntityIdColumnName, countRecords),
                                                           dataReader.GetInt32(EntityIdColumnName));

                                props.Session.SetStringProperty(CreatePropertyName(ValueColumnName, countRecords),
                                                          dataReader.GetString(ValueColumnName));

                                props.Session.SetDateTimeProperty(CreatePropertyName(StartDateColumnName, countRecords),
                                                            dataReader.GetDateTime(StartDateColumnName));

                                props.Session.SetDateTimeProperty(CreatePropertyName(EndDateColumnName, countRecords),
                                                            dataReader.GetDateTime(EndDateColumnName));

                                props.Session.SetStringProperty(CreatePropertyName(SpecNameColumnName, countRecords),
                                                          dataReader.GetString(SpecNameColumnName));

                                props.Session.SetIntegerProperty(CreatePropertyName(SpecTypeColumnName, countRecords),
                                                           dataReader.GetInt32(SpecTypeColumnName));
                                countRecords++;
                            }
                        }

                        props.Pipeline.CountRecords = countRecords;

                        Log(LogLevel.Info,
                                       String.Format("Exists '{0}' subscription properties for {1} with ID = '{2}'.",
                                                     countRecords, props.Pipeline.IsGroupSubscription.Value ? "IdGroupSubscription" : "IdSubscriptionId", props.Pipeline.IdSubscription));
                    }
            }
            catch (Exception)
            {
                //TODO: It seems that this info will be written to log file when exception caught
                //Log(LogLevel.Fatal, ex.Message);
                throw;
            }
            Log(LogLevel.Debug, "Stop");
        }

        private static string GetVariableName<T>(Expression<Func<T>> expression)
        {
            var body = expression.Body as MemberExpression;
            return body.Member.Name;
        }

        private IMTRcd mRcd = null;
        private string GetQueryPath(string pathChunk)
        {
            if (mRcd == null)
            {
                mRcd = new MTRcd();
            }

            return Path.Combine(mRcd.ConfigDir, pathChunk);
        }

        /// <summary>
        /// Creates the property name (prefix + number) 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="index"></param>
        /// <returns>ID of new property</returns>
        private int CreatePropertyName(string propertyName, int index)
        {
            return mNameID.GetNameID(String.Format("{0}{1}", propertyName, index));
        }

        //private T ExistsValue<T>(ISession session, string paramName, Converter<string, T> converter) where T : IConvertible, IComparable
        //{
        //    try
        //    {
        //        string strValue = propSet.NextStringWithName(paramName);
        //        if (typeof(string) == typeof(T))
        //        {
        //            if (String.IsNullOrEmpty(strValue))
        //            {
        //                //TODO: I think this info will be written to log file when exception caught
        //                //mLog.LogString(MetraTech.Interop.SysContext.PlugInLogLevel.PLUGIN_LOG_ERROR, CreateErrMsgNonExistsParamNameInConfigFile(QueryPathConfigParam));
        //                throw new COMException(CreateErrMsgNonExistsParamNameInConfigFile(paramName));
        //            }

        //        }
        //        return converter(strValue);
        //    }
        //    catch (Exception)
        //    {
        //        throw new COMException(CreateErrMsgErrorWhileParsingConfigValue<T>(paramName,
        //                                                                             propSet.NextStringWithName(
        //                                                                                 paramName)));
        //        throw;
        //    }
        //}

        private string CreateErrMsgParmNotSet(string paramName)
        {
            return String.Format("The value of {0} parameter cannot be null or emty.", paramName);
        }
    }
} 
