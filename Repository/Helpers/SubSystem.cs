using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System;
using System.Web;
using log4net;
using log4net.Config;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;

namespace Repository.Helpers
{
    /// <summary>
    /// Responsible for logging and configurables.
    /// </summary>
    public class SubSystem
    {
        /// <summary>
        /// Dictionary for app.config or web.config
        /// </summary>
        public static IDictionary<string, string> AppSettings;
        public static ILog GetLogger<TLogger>()
        {
            return LogManager.GetLogger(typeof(TLogger));
        }

        /// <summary>
        /// Get system stuff like appsettings.
        /// </summary>
        public SubSystem()
        {
            if (AppSettings == null)
            {
                AppSettings = new Dictionary<string, string>();
                GetSections("appSettings");
            }
        }

        public static async Task Wait(int seconds, bool logging = false)
        {
            for (int i = 0; i < seconds; i++)
            {
                if (logging) Log<SubSystem>.Write.Info(string.Format("{0} (s) left ... ", seconds - i));
                await Task.Delay(1000, CancellationToken.None);
            }
        }
        public static async Task ReportProgressAsync(CancellationToken cancelToken, IProgress<int> progress, int jobCount)
        {
            for (int i = 0; i < jobCount; i++)
            {
                await Task.Delay(1000, cancelToken);
                progress.Report((i + 1) * jobCount);
            }
        }
        /// <summary>
        /// Continue action after a given period. NON-BLOCKING method.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="waitSeconds"></param>
        public static void WaitAsync(Action action, int waitMilliSeconds)	// Asynchronous NON-BLOCKING method
        {
            new Timer(_ => action()).Change(waitMilliSeconds, -1);
        }
        public static void WaitAsync(Action action, int waitMilliSeconds, int period)	// Asynchronous NON-BLOCKING method
        {
            new Timer(_ => action()).Change(waitMilliSeconds, period);
        }
        public static void GetSections(string section)
        {
            try
            {

                // Get the current configuration file.
                Configuration config;
                if (HttpContext.Current != null)
                {
                    config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                }
                else
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;
                }
                // Get the selected section. 
                switch (section)
                {
                    case "appSettings":
                        try
                        {
                            AppSettingsSection appSettings =
                                config.AppSettings as AppSettingsSection;
                            // Get the KeyValueConfigurationCollection  
                            // from the configuration.
                            KeyValueConfigurationCollection settings =
                              config.AppSettings.Settings;

                            // Display each KeyValueConfigurationElement. 
                            foreach (KeyValueConfigurationElement keyValueElement in settings)
                            {
                                AppSettings.Add(keyValueElement.Key, keyValueElement.Value);
                            }
                        }
                        catch (ConfigurationErrorsException e)
                        {

                        }
                        break;

                    case "connectionStrings":
                        ConnectionStringsSection
                            conStrSection =
                            config.ConnectionStrings as ConnectionStringsSection;

                        try
                        {
                            if (conStrSection.ConnectionStrings.Count != 0)
                            {

                                // Get the collection elements. 
                                foreach (ConnectionStringSettings connection in
                                  conStrSection.ConnectionStrings)
                                {
                                    string name = connection.Name;
                                    string provider = connection.ProviderName;
                                    string connectionString = connection.ConnectionString;
                                }
                            }
                        }
                        catch (ConfigurationErrorsException e)
                        {

                        }
                        break;

                    default:

                        break;
                }

            }
            catch (ConfigurationErrorsException err)
            {

            }

        }
        /// <summary>
        /// Logs to eventviewer. Make sure the app has priviliges on production to read/write to eventviewer
        /// </summary>
        /// <param name="sEvent"></param>
        public static void LogToEventViewer(string sEvent)
        {
            string sSource;
            string sLog;

            sSource = SubSystem.AppSettings["eventsource"];
            sLog = SubSystem.AppSettings["eventlog"];

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent);
            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);
        }

        public static void HandleEntityValidationErrors(DbEntityValidationException ex)
        {
            foreach (var error in ex.EntityValidationErrors)
            {
                string entity = error.Entry.Entity.ToString();
                Log<SubSystem>.Write.Error(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    error.Entry.Entity.GetType().Name, error.Entry.State));

                foreach (var validErr in error.ValidationErrors)
                {
                    List<string> listProperties = GetPropertyNames(error.Entry);
                    string typeValues = String.Join("|", listProperties.ToArray());
                    Log<SubSystem>.Write.Error(string.Format("'{0}' for {1}: {2} ", validErr.ErrorMessage, entity, typeValues), ex);
                    Log<SubSystem>.Write.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                        validErr.PropertyName, validErr.ErrorMessage));
                }
            }
        }

        public static void HandleDbUpdateException(DbUpdateException ex)
        {

            if (ex.InnerException != null)
            {
                var innerExc = ex.InnerException;
                var inner = innerExc.InnerException;
                if (inner == null)
                {
                    HandleEntityErrors(ex, innerExc);
                }
                else
                {
                    HandleEntityErrors(ex, inner);
                }
            }
            else
            {
                Log<SubSystem>.Write.Error(ex.Message, ex);
            }
        }

        private static void HandleEntityErrors(DbUpdateException ex, Exception inner)
        {
            if (ex.Entries.Any())
            {
                ex.Entries.ToList().ForEach(cur =>
                {
                    string entity = cur.Entity.ToString();

                    List<string> listProperties = GetPropertyNames(cur);
                    string typeValues = String.Join("|", listProperties.ToArray());
                    string message = string.Format("'{0}' for {1}: {2} ", inner.Message, entity, typeValues);
                    Log<SubSystem>.Write.Error(message, ex);
                });
            }
        }

        private static List<string> GetPropertyNames(DbEntityEntry cur)
        {
            List<string> listProperties = new List<string>();
            try
            {
                cur.CurrentValues.PropertyNames.ToList().ForEach(p =>
                {
                    listProperties.Add(String.Format("({0},{1})", p, cur.CurrentValues.GetValue<object>(p).ToString()));
                });
            }
            catch (Exception)
            {

            }
            return listProperties;
        }
        /// <summary>
        /// Returns a list of keys. Each key is a errormessage.
        /// </summary>
        /// <param name="modelstate"></param>
        /// <returns></returns>
        public static IDictionary<string, Exception> HandleModelState(ModelStateDictionary modelstate)
        {
            var list = new ConcurrentDictionary<string, Exception>();
            int i = 0;
            foreach (var item in modelstate)
            {
                var errors = item.Value.Errors;
                foreach (var error in errors)
                {
                    Log<SubSystem>.Write.Warn(error.ErrorMessage, error.Exception);
                    //list.TryAdd((i++).ToString(), new ApplicationException(error.ErrorMessage));
                    list.TryAdd(item.Key, new ApplicationException(error.ErrorMessage));
                }
            }

            foreach (var item in list)
            {
                modelstate.AddModelError(item.Key, item.Value);
            }
            return list.ToDictionary(k => k.Key, v => v.Value);
        }
        public static void ExecuteCommand(string cmd, string path)
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + path + "\\" + cmd);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Log<SubSystem>.Write.Error(ex.Message, ex);
            }
        }
    }
}
