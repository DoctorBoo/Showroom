using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using log4net;
using log4net.Config;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Resources;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;



namespace DomainClasses.Helpers
{
    using ParserFunc = Func<Func<string, string, string, IDictionary<string, object>>, string, string, IDictionary<string, object>>;
    using TokenizerFunc = Func<string, string, string, IDictionary<string, object>>;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;
    using DomainClasses.Helpers;


    public static class CommonExtensions
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        /// <summary>
        /// Period folder MUST be in filesystem under app_data.
        /// </summary>
        public static string PeriodFolder
        {
            get
            {
                return string.Format("{0}\\app_data\\Period", CommonExtensions.AssemblyDirectory);
            }
        }
        /// <summary>
        /// Storage of Google.Apis.Auth.OAuth2.Responses.TokenResponse-user
        /// </summary>
        public static string FileDataStore
        {
            get
            {
                return string.Format("{0}\\app_data", CommonExtensions.AssemblyDirectory);
            }
        }
        public static string GetAppDataFile(this string any)
        {
            return FileDataStore;
        }
        public static string GetAppDataFile(this string any, string fileInAppData)
        {
            string appData;

            //overwrite
            appData = string.Format("{0}\\app_data", CommonExtensions.AssemblyDirectory);
            return string.Format("{0}\\{1}", appData, fileInAppData);
        }
        public static string GetAppDataFile(this string any, HttpServerUtility server, string fileInAppData)
        {
            string appData;
            if (server != null)
            {
                //only if action is root
                //SUT
                appData = server.MapPath("App_data");
            }

            //overwrite
            appData = string.Format("{0}\\app_data", CommonExtensions.AssemblyDirectory);
            return string.Format("{0}\\{1}", appData, fileInAppData);
        }
        public static string GetAppDataFolder(this HttpServerUtilityBase server)
        {
            return server.MapPath("~/App_data");
        }
        public static string GetAppDataFolder(this HttpServerUtility server)
        {
            return server.MapPath("~/App_data");
        }
        public static string GetAppDataTempFolder(this HttpServerUtility server)
        {
            return server.MapPath("~/App_data/Temp");
        }
        /// <summary>
        /// Gets all files in subdirectories recursively.
        /// </summary>
        /// <param name="any"></param>
        /// <param name="directories"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(this string[] directories, string searchPattern = "")
        {
            List<string> totalFiles = new List<string>();
            directories.ToList().ForEach(d =>
            {
                var localFiles = String.IsNullOrWhiteSpace(searchPattern) ? Directory.GetFiles(d) : Directory.GetFiles(d,searchPattern);
                totalFiles.AddRange(localFiles);

                totalFiles.AddRange(d.GetFilesRecursive(searchPattern));
            });            
            return totalFiles.ToArray();
        }
        /// <summary>
        /// Gets all files in subdirectories recursively.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(this string path, string searchPattern = "")
        {
            var dirs = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path).ToList();
            files.AddRange(dirs.GetFilesRecursive(searchPattern));
            var distincted = files.Distinct();
            return distincted.ToArray();
        }
        public static IList<T> ToGMailList<T>(this ConcurrentDictionary<string, T> any)
             where T : class
        {
            IList<T> list = new List<T>();

            foreach (var item in any)
            {
                list.Add(item.Value);
            }

            return list;
        }
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                // do nothing
            }
        }
        public static Boolean ValidateFilePdf(this string file)
        {
            bool found = false;
            string outputText = String.Empty;
            StringBuilder sb = new StringBuilder();
            string search = "naam cliënt";

            using (PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.ReadOnly))
            {
                //Console.WriteLine("***********************************************************");
                foreach (PdfPage page in inputDocument.Pages)
                {
                    for (int index = 0; index < page.Contents.Elements.Count; index++)
                    {
                        PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(index).Stream;
                        outputText = new PDFTextExtractor().ExtractTextFromPDFBytes(stream.Value);

                        outputText = !String.IsNullOrWhiteSpace(outputText) ? outputText.Replace("\n\r", "") : String.Empty;
                        
                        var match = Regex.Match(outputText, search, RegexOptions.IgnoreCase);
                        if (match.Length > 0)
                        {
                            found = true;
                            //Console.WriteLine("FOUND: {0}, in {1}", search, f);
                            break;
                        }
                        //Console.WriteLine(outputText);
                    }
                    if (found)
                        break;
                }
                //Console.WriteLine("***********************************************************");
            }

            return found;
        }
        public static string ReadFile(this string file)
        {
            return file.ToText();
        }
        public static string ReadPdf(this string file)
        {
            string outputText = String.Empty;
            using (PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.ReadOnly))
            {
                //Console.WriteLine("***********************************************************");
                foreach (PdfPage page in inputDocument.Pages)
                {
                    for (int index = 0; index < page.Contents.Elements.Count; index++)
                    {
                        PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(index).Stream;
                        bool unfiltered = stream.TryUnfilter();

                        outputText = new PDFTextExtractor().ExtractTextFromPDFBytes(stream.UnfilteredValue);

                        outputText = !String.IsNullOrWhiteSpace(outputText) ? outputText.Replace("\n\r", "") : String.Empty;

                        string converted = PDFTextExtractor.ConvertToRawText(stream.UnfilteredValue);

                       
                        using(FileStream fs = new FileStream(file + ".txt", FileMode.Create))
                        {
                            byte[] bytes = stream.UnfilteredValue;
                            fs.Write(bytes, 0, bytes.Length);
                        }
                        Console.WriteLine(outputText);
                        var dict = page.Contents.Elements.GetDictionary(index).Stream;
                        var value = dict.Value;
                    }
                }
                //Console.WriteLine("***********************************************************");
            }
            return outputText;
        }
        public static DateTime ConvertFromGoogle(this DateTime any, long? timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddMilliseconds(timestamp.HasValue ? timestamp.Value : 0);
        }
        public static DateTime ConvertFromGoogle(this long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddMilliseconds(timestamp);
        }
        public static DateTime ConvertFromGoogle(this long? timestamp)
        {
            long timeStampValue = timestamp.HasValue ? timestamp.Value : 0;
            return timeStampValue.ConvertFromGoogle();
        }
        public static long ConvertToGoogle(this DateTime any)
        {
            var result = any.ToUniversalTime() - new DateTime(1970, 1, 1);
            return Convert.ToInt64(result.TotalMilliseconds);
        }

        public static string GetMonthYear(this DateTime value)
        {
            List<string> months = new List<string>{"januari", "februari", "maart", "april", "mei", "juni",
			"juli", "augustus", "september", "oktober", "november", "december"};
            return string.Format("{0} {1}", months[value.Month - 1], value.Year);
        }

        public static IList<string> GetMonths(this string value)
        {
            List<string> months = new List<string>{
			"januari", "februari" , "maart",  "april", "augustus", "september", "oktober", "november","december",
             "feb", "mrt", "apr", "mei", "juni","juli", "aug", "sep", "okt", "nov", "dec"
			};
            return months;
        }

        public static int GetMonth(this string value)
        {
            string months = String.Join("|", value.GetMonths());
            var match = Regex.Match(value, months, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                var month = match.Captures[0].Value;
                if (month == "januari")
                {
                    return 1;
                }
                if (month == "februari" || month == "feb")
                {
                    return 2;
                }
                if (month == "maart" || month == "mrt")
                {
                    return 3;
                }
                if (month == "april" || month == "apr")
                {
                    return 4;
                }
                if (month == "mei")
                {
                    return 5;
                }
                if (month == "juni")
                {
                    return 6;
                }
                if (month == "juli")
                {
                    return 7;
                }
                if (month == "augustus" || month == "aug")
                {
                    return 8;
                }
                if (month == "september" || month == "sep")
                {
                    return 9;
                }
                if (month == "oktober" || month == "okt")
                {
                    return 10;
                }
                if (month == "november" || month == "nov")
                {
                    return 11;
                }
                if (month == "december" || month == "dec")
                {
                    return 12;
                }
                return 0;
            }
            else
                return 0;
        }
        /// <summary>
        /// Retrieves the year in a filename: 'bladibla juni 2013.pdf' or 'bladibla juni 2014 3.pdf'
        /// returns 1 of year value cannot be retrieved
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetYear(this string value)
        {
            int year = 1;
            string months = String.Join("|", value.GetMonths());
            var match = Regex.Match(value, months, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var yearString = value.LRPostfixify(match.Captures[0].Value, false)
                .LRPrefixify(@"\.", false).Trim()
                .LRPrefixify(@"\s+", false);

                yearString = yearString.LRPrefixify("-", false);
                if (!int.TryParse(yearString, out year))
                {
                    year = 1;
                };
            }


            return year;
        }
        /// <summary>
        /// Retrieves a date from a filename.
        /// returns 1900-01-01 if not exists.
        /// </summary>
        /// <param name="value"></param>    
        /// <returns></returns>
        public static DateTime GetDate(this string value)
        {
            int month = value.GetMonth();
            int year = month > 0 ? value.GetYear() : 1900;
            month = month > 0 ? month : 1;

            var date = new DateTime(year, month, 1);
            date = date < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : date;
            return date;
        }

        public static int GetPreviousMonth(this DateTime current)
        {
            int month = current.Month > 1 ? current.Month -1 : 12;
            return month;
        }
        public static DateTime GetPreviousMonth(this DateTime current, int day = 1)
        {
            int month = current.Month > 1 ? current.Month - 1 : 12;
            int year = current.Month > 1 ? current.Year : current.Year - 1;
            return new DateTime(year,month, day);
        }

        public static DateTime GetNextDay(this DateTime current)
        {           
            DateTime newDate = current;
            try
            {
                newDate = new DateTime(current.Year, current.Month, current.Day + 1);
            }
            catch (ArgumentException)
            {
                int month = current.Month < 12 ? current.Month + 1 : 12;
                int year = current.Month < 12 ? current.Year : current.Year + 1;
                newDate = new DateTime(year, month, 1);
            }
            return newDate;
        }
        public static double Mean(this List<double> valueList)
        {
            double length = Convert.ToDouble(valueList.Count());
            double total = 0;
            double[] values = valueList.ToArray();

            values.Aggregate((c, n) =>
            {
                total += c;
                return n;
            });
            return total / length;
        }

        public static double Variance(this List<double> valueList)
        {
            double length = Convert.ToDouble(valueList.Count());
            double mean = valueList.Mean();
            double sum = 0;
            double[] values = valueList.ToArray();

            double[] stdVal = new double[values.Length];
            for (int i = 0; i < length; i++)
            {
                stdVal[i] = Math.Pow(values[i] - mean, 2);
                sum += stdVal[i];
            }
            return Math.Sqrt(sum / values.Length);
        }
        public static double StandardDeviation(this List<double> valueList)
        {
            return Math.Sqrt(valueList.Variance());
        }

        public static MemoryStream ToMemoryStream(this byte[] byteArray)
        {
            return new MemoryStream(byteArray);
        }
        public static Stream ToStream(this Stream target, MemoryStream source)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
            return target;
        } 
        public static MemoryStream ToMemoryStream(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                input.Position = 0;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms;
            }
        }
        public static byte[] ToByteArray(this Stream input)
        {
            return ToMemoryStream(input).ToArray();
        }
        public static string[] CreateHeader(string[] headers)
        {
            string[] list = new string[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                list[i] = (@"""Column_" + i + @"""");
            }
            return list;
        }

        public static string ToByteString(this byte[] input)
        {
            StringBuilder sb = new StringBuilder();
            if (input == null || input.Length == 0) return "";

            for (int i = 0; i < input.Length; i++)
            {
                char c = (char)input[i];
                sb.Append(c);
            }

            string value = sb.ToString();
            return value;
        }

        public static string[] ToArray(this MatchCollection matches)
        {
            int length = matches.Count;
            string[] target = new string[length];

            for (int i = 0; i < length; i++)
            {
                target[i] = matches[i].Captures[0].Value;
            }

            return target;
        }
        public static IDictionary<string, Match> ToDictionary(this MatchCollection matches)
        {
            int length = matches.Count;
            Dictionary<string, Match> target = new Dictionary<string, Match>();

            for (int i = 0; i < length; i++)
            {
                string key = matches[i].Captures[0].Value;
                if(!target.ContainsKey(key))
                    target.Add(key, matches[i]);
            }

            return target;
        }
        /// <summary>
        /// Returns the default if index does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetValue<T>(this T[] values, int index)
        {
            try
            {
                return values[index];
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public static T GetValue<T>(this T[] values, int index, out IList<Exception> exceptionList)
        {
            exceptionList = new List<Exception>();
            try
            {
                return values[index];
            }
            catch (Exception ex)
            {
                string strMessage = String.Format("Error:{0} does not exist. Message: {1}", index, ex.Message);
                exceptionList.Add(new ApplicationException(strMessage));
                return default(T);
            }
        }
        /// <summary>
        /// Spaces are trimmed.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasOnlyNullOrEmptyValues (this IList<object> values, bool trimSpaces = true)
        {
            var result = values.Aggregate(new List<bool>(), (c, n) =>
            {
                if ((n == null 
                        ||
                        (trimSpaces && n.ToString().Trim() == String.Empty))
                        ||
                        (!trimSpaces && n.ToString() == String.Empty))
                    c.Add(true);
                else
                {
                    c.Add(false);
                }
                return c;
            });

            return result.All(r => r == true);
        }

        /// <summary>
        /// Gets values from a comma-seperated list in lowercase.
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] GetValues(this IDictionary<string, string> appSettings, string key)
        {
            string valuesToBeParsed = appSettings[key].Trim().ToLower();
            string[] values = valuesToBeParsed.GetValues();
            return values;
        }
        /// <summary>
        /// Gets values from a comma-seperated list in lowercase.
        /// </summary>
        /// <param name="appSettingValue"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] GetValues(this string appSettingValue)
        {
            string[] values = appSettingValue
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim()).ToArray();
            return values;
        }
    }
    public static class StringExtension
    {
        /// <summary>
        /// nl-NL style
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BeautifyZipCode(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return String.Empty;

            string patternZipcodeMinified = @"^(\d){4}(\w){2}$";
            string patternPrefix = @"^(\d){4}";
            string patternPostFix = @"(\w){2}$";

            var match = Regex.Match(value, patternZipcodeMinified, RegexOptions.IgnoreCase);
            //match.Dump();
            if (match.Length > 0)
            {
                string found = match.Captures[0].Value;
                string prefix = Regex.Match(found, patternPrefix, RegexOptions.IgnoreCase).Captures[0].Value;
                string postfix = Regex.Match(found, patternPostFix, RegexOptions.IgnoreCase).Captures[0].Value.ToUpper();
                string beautified = string.Format("{0} {1}", prefix, postfix);

                return beautified;
            }

            return value;
        }
        public static string CsvToJson(this string value, char seperator)
        {
            // Get lines.
            if (value == null) return null;
            string[] lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) throw new InvalidDataException("Must have header line.");

            // Get headers.
            string[] headers = lines.First().SplitQuotedLine(seperator, true);

            for (int i = 0; i < headers.Length; i++)
            {
                headers[i] = @"""Column_" + i + @"""";
            }

            // Build JSON array.
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    //fields[0][0] == 26 
                    string line = lines[i].Replace("\r\n", string.Empty);
                    if (!string.IsNullOrEmpty(line) && line[0] != 26)
                    {
                        string[] fields = line.SplitQuotedLine(seperator, true);
                        if (fields.Length != headers.Length) throw new InvalidDataException("Field count must match header count.");
                        var jsonElements = headers.Zip(fields, (header, field) => string.Format("{0}: {1}", header, field)).ToArray();
                        string jsonObject = "{" + string.Format("{0}", string.Join(",", jsonElements)) + "}";
                        //if (i < lines.Length - 1)
                        //    jsonObject += seperator;
                        sb.AppendLine(jsonObject);
                        var dynamicObject = System.Web.Helpers.Json.Decode(jsonObject);
                    }
                }
                catch (Exception ex)
                {
                    Log<String>.Write.Error(ex.Message, ex);
                }
            }
            var result = sb.Append("]").ToString().Replace("\r\n", ",");
            return sb.ToString();
        }
        /// <summary>
        /// Currentculture
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal result = 0;

            string decimalString1 = value.Replace(" ", String.Empty) ;
            //if (!string.IsNullOrEmpty(decimalString1)) decimalString1 = decimalString1.Replace(',', '.');// en-US style

            decimal.TryParse(decimalString1, System.Globalization.NumberStyles.Currency, CultureInfo.CurrentCulture, out result);

            return result;
        }
        /// <summary>
        /// Try parse en-US and then nl-NL.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal2(this string value)
        {
            decimal result = 0;
            value = value.Replace(" ", String.Empty);
            bool containsSymbol = value.Contains("€") || value.ToString().Contains("$");
            if (containsSymbol)
            {
                bool succeeded = decimal.TryParse(value.LRPostfix("€",false).LRPostfix("$",false),
                           System.Globalization.NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out result);
                if (!succeeded)//try nl-NL
                    decimal.TryParse(value.LRPostfix("€", false).LRPostfix("$", false),
                         System.Globalization.NumberStyles.Currency, CultureInfo.GetCultureInfo("nl-NL"), out result);
            }
            else
            {
                bool succeeded = decimal.TryParse(value, System.Globalization.NumberStyles.Currency,
                    CultureInfo.GetCultureInfo("nl-NL"), out result);
                if (!succeeded)//try nl-NL
                    decimal.TryParse(value, System.Globalization.NumberStyles.Currency,
                        CultureInfo.GetCultureInfo("en-US"), out result);
            }
            return result;
        }
        public static IList<dynamic> CsvToDynamicDictionaries(this string value, char seperator)
        {
            // Get lines.
            if (value == null) return null;
            string[] lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] headers = new string[] { };
            if (lines.Length < 2)
            {
                //NOTE: throw new InvalidDataException("Must have header line.");
                //only for avero newline = \n
                lines = value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                var firstLine = lines[0].SplitQuotedLine(seperator, true);
                headers = firstLine.Aggregate(new string[firstLine.Count()], (c, n) =>
                {
                    c[i] = "Column_" + i;
                    i++;
                    return c;
                });
            };

            // Get headers.
            headers = headers.Count() ==0 ? 
                CommonExtensions.CreateHeader(lines.First().SplitQuotedLine(seperator, true)) :
                headers;
            List<dynamic> list = new List<dynamic>();

            // Build JSON array.
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    //fields[0][0] == 26 
                    string line = lines[i].Replace("\r\n", string.Empty);
                    if (!string.IsNullOrEmpty(line) && line[0] != 26)
                    {
                        string[] fields = line.SplitQuotedLine(seperator, true);
                        if (fields.Length != headers.Length) throw new InvalidDataException("Field count must match header count.");
                        var jsonElements = headers.Zip(fields, (header, field) => string.Format("{0}: {1}", header, field)).ToArray();
                        string jsonObject = "{" + string.Format("{0}", string.Join(",", jsonElements)) + "}";

                        var keyValues = headers.Zip(fields, (header, field) => new KeyValuePair<string, object>(header, field));
                        dynamic dynamicObject = new ExpandoObject();
                        dynamicObject.Dictionary = keyValues.ToDictionary(k => k.Key, f => f.Value);
                        dynamicObject.OriginalValue = System.Web.Helpers.Json.Decode(jsonObject);
                        foreach (var pair in keyValues)
                        {
                            //dynamicObject[pair.Key.Replace(Constants.Punctuations, string.Empty)] = pair.Value;
                        }
                        list.Add(dynamicObject);
                    }
                }
                catch (Exception ex)
                {
                    Log<String>.Write.Error(ex.Message, ex);
                }
            }

            return list;
        }

        public static string[] SplitQuotedLine(this string value, char separator, bool quotes = true)
        {
            // Use the "quotes" bool if you need to keep/strip the quotes or something...
            var s = new StringBuilder();
            string format = string.Format("(?<=^|{0})(\"(?:[^\"]|\"\")*\"|[^{0}]*)", separator.ToString());
            //string format = string.Format("(?<=^|{0})|([^{0}]*)", separator.ToString());
            var regex = new Regex(format);
            var matches = regex.Matches(value);
            string[] splitted = new string[] { };
            foreach (Match m in matches)
            {
                if (!string.IsNullOrEmpty(m.Value))
                {
                    string newString = string.Format(@"'{0}'{1}", m.Value.Replace("'", string.Empty), '\r');
                    newString = quotes ?
                        newString : string.Format("{0}\r", newString.Replace(Constants.Punctuations, string.Empty));
                    s.Append(newString);
                }
                else
                    s.Append("''\r");
            }
            var result = s.ToString().Split('\r').Take(matches.Count);

            return result.ToArray();
        }
        private static IList<XDocument> _xmlTokens = new List<XDocument>();

        /// <summary>
        /// Result of the tokenization.
        /// </summary>
        public static IList<XDocument> XmlTokens
        {
            get { return _xmlTokens; }
            set { _xmlTokens = value; }
        }

        /// <summary>
        /// Extension of string replace, replaces targetlist with replacement.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetList"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(this string value, IList<string> targetList, string replacement)
        {
            return targetList.Aggregate(value, (current, target) => current.Replace(target, replacement).Trim());
        }
        public static string ReplacePattern(this string value, string pattern, string replacement)
        {            
            return Regex.Replace(value, pattern, replacement,RegexOptions.IgnoreCase);
        }
        public static string FirstCharToUpper(this String input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        //public static IDictionary<string, object> LRParserCurried(this string stringValue, TokenizerFunc Tokenizer, string startProduction, string endProduction)
        //{
        //    ParserFunc lrParser = stringValue.LRParser;
        //    var curried = lrParser.Curry();
        //    var f2 = curried(Tokenizer);
        //    var f3 = f2(startProduction);

        //    return curried(Tokenizer)(startProduction)(endProduction);
        //}
        public static TResult LRParserCurried<TResult>(this string stringValue,
                Func<string, string, string, TResult> Tokenizer,
                string startProduction, string endProduction)
            where TResult : IDictionary<string, object>, new()
        {
            Func<Func<string, string, string, TResult>, string, string, TResult> lrParser = stringValue.LRParser;
            var f1 = lrParser.Curry();
            var f2 = f1(Tokenizer);
            var f3 = f2(startProduction);

            return f1(Tokenizer)(startProduction)(endProduction);
        }
        public static Dictionary<string, object> Tokenizer(string prefix, string body, string postfix)
        {
            Dictionary<string, object> parsedTokens = new Dictionary<string, object>();
            //body = body.LRPostfixify(Constants.Header, false);
            parsedTokens.Add(Guid.NewGuid().ToString(), String.Format("{0}{1}{2}", prefix, body, postfix));//+ @";" +  	
            parsedTokens.Add("prefix", prefix);//+ @";" +  Guid.NewGuid().ToString()
            parsedTokens.Add("body", body);//+ @";" +  Guid.NewGuid().ToString()
            parsedTokens.Add("postfix", postfix);//+ @";" +  Guid.NewGuid().ToString()
            return parsedTokens;
        }
        //public static TokenizerFunc _tokenizer;
        //public static TokenizerFunc TokenizerFunctor
        //{
        //    get { return default(TokenizerFunc); }
        //    set { _tokenizer = value; }
        //}
        /// <summary>
        /// Parses left to right and collects tokens with tokenizer.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tokenizer">Collects tokens as a sublanguage</param>
        /// <param name="startProductionRule"></param>
        /// <param name="endProductionRule"></param>
        /// <returns></returns>
        public static TResult LRParser<TResult>(this String text, Func<string, string, string, TResult> tokenizer,
                string beginToken, string endToken)
            where TResult : IDictionary<string, object>, new()
        {
            TResult parsedTokens = new TResult();
            string matchedBeginToken = String.Empty;
            string matchedEndToken = String.Empty;
            string lookahead = text.LRPostfixify(beginToken, out matchedBeginToken);
            string body = lookahead.LRPrefixify(endToken, out matchedEndToken);
            string nextPass = text.LRPostfixify(endToken);

            if (!string.IsNullOrEmpty(body))
            {
                tokenizer(matchedBeginToken, body, matchedEndToken).Aggregate(parsedTokens, (c, n) =>
                {
                    c.Add(n.Key, n.Value);
                    return c;
                });
            }

            if (!string.IsNullOrEmpty(nextPass))
            {
                nextPass.LRParser(tokenizer, beginToken, endToken).Aggregate(parsedTokens, (c, n) =>
                {
                    if (!c.ContainsKey(n.Key))
                        c.Add(n.Key, n.Value);
                    return c;
                });
            }

            return parsedTokens;
        }

        public static TResult LRParser<TResult>(this String text, Func<string, string, string, TResult> tokenizer,
            string beginToken, string endToken, bool includeTokens)
            where TResult : IDictionary<string, object>, new()
        {
            TResult parsedTokens = new TResult();
            string matchedBeginToken = String.Empty;
            string matchedEndToken = String.Empty;
            string lookahead = text.LRPostfixify(beginToken, out matchedBeginToken);
            string body = lookahead.LRPrefixify(endToken, out matchedEndToken);
            string nextPass = text.LRPostfixify(endToken);

            if (includeTokens)
            {
                tokenizer(matchedBeginToken, body, matchedEndToken).Aggregate(parsedTokens, (c, n) =>
                {
                    c.Add(n.Key, n.Value);
                    return c;
                });
            }

            if (!string.IsNullOrEmpty(nextPass))
            {
                nextPass.LRParser(tokenizer, beginToken, endToken).Aggregate(parsedTokens, (c, n) =>
                {
                    if (!c.ContainsKey(n.Key))
                        c.Add(n.Key, n.Value);
                    return c;
                });
            }

            return parsedTokens;
        }        
        
        /// <summary>
        /// Parses this string value from left to right and collects tokens (tokenization).
        /// </summary>
        /// <param name="text">this string value</param>
        /// <param name="tokenizer">parses lookahead into tokens until next pass marker</param>
        /// <param name="beginToken">marks the begin of a token. Token can be a regex.</param>
        /// <param name="endToken">marks the end of a token. Token can be a regex.</param>
        /// <param name="format">formats the parsed token as input for the tokenizer</param>
        /// <returns></returns>
        public static void LeftToRightParse(this String text, Action<string> tokenizer,
            string beginToken, string endToken, string format)
        {
            string lookahead = text.LRPostfixify(beginToken);
            string body = lookahead.LRPrefixify(endToken);
            string nextPass = text.LRPostfixify(endToken);

            if (!string.IsNullOrEmpty(body))
                tokenizer(string.Format(format, beginToken, body, endToken));

            if (!string.IsNullOrEmpty(nextPass))
                nextPass.LeftToRightParse(tokenizer, beginToken, endToken, format);
        }
        /// <summary>
        /// Parses a text for a tokenizer.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tokenizer"></param>
        /// <param name="beginToken"></param>
        /// <param name="endToken"></param>
        public static void LeftToRightParse(this String text, Action<string, string, string> tokenizer,
            string beginToken, string endToken)
        {
            string matchedBeginToken = String.Empty;
            string matchedEndToken = String.Empty;
            string lookahead = text.LRPostfixify(beginToken, out matchedBeginToken);
            string body = lookahead.LRPrefixify(endToken, out matchedEndToken);
            string nextPass = text.LRPostfixify(endToken);

            if (!string.IsNullOrEmpty(body))
                tokenizer(matchedBeginToken, body, matchedEndToken);

            if (!string.IsNullOrEmpty(nextPass))
                nextPass.LeftToRightParse(tokenizer, beginToken, endToken);
        }

        /// <summary>
        /// Returns a non-null or nullified postfix value marked by marker which is excluded.
        /// left to right search direction is used.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public static string LRPostfix(this String word, string marker, bool nullify = true)
        {
            if (!string.IsNullOrEmpty(marker) && !string.IsNullOrEmpty(word))
            {
                int res = word.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase);
                if (res > -1)
                    return word.Substring(word.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase) + marker.Length);
            }
            return nullify ? null : word;
        }

        /// <summary>
        /// returns a non-null or nullified postfix value after marker. Marker is a regex .
        /// </summary>
        /// <param name="word"></param>
        /// <param name="patternMarker"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public static string LRPostfixify(this String word, string patternMarker, bool nullify = true)
        {
            string matched = String.Empty;
            if (String.IsNullOrWhiteSpace(word))
                return matched;

            return word.LRPostfixify(patternMarker, out matched, nullify);
        }
        public static string LRPostfixify(this String word, string patternMarker, out string matchedMarker, bool nullify = true)
        {
            matchedMarker = String.Empty;

            if (String.IsNullOrWhiteSpace(word) || String.IsNullOrEmpty(patternMarker))
                return String.Empty;

            var match = Regex.Match(word, patternMarker, RegexOptions.IgnoreCase);
            //match.Dump();
            if (match.Length > 0)
            {
                matchedMarker = match.Captures[0].Value;
                string postfixified = word.LRPostfix(matchedMarker);

                return postfixified;
            }

            return nullify ? null : word;
        }
        /// <summary>
        /// Returns the postfix in word marked by marker which is excluded.
        /// Right to left search direction is used.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public static string RLPostfix(this String word, string marker)
        {
            if (!string.IsNullOrEmpty(marker) && !string.IsNullOrEmpty(word))
            {
                int res = word.LastIndexOf(marker, 0, StringComparison.OrdinalIgnoreCase);
                if (res > -1)
                    return word.Substring(word.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase) + marker.Length);
            }
            return null;
        }
        /// <summary>
        /// Returns a non-null or nullified prefix value marked by marker which is excluded.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public static string LRPrefix(this String word, string marker, bool nullify = true)
        {
            if (!string.IsNullOrEmpty(marker) && !string.IsNullOrEmpty(word))
            {
                int res = word.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase);
                if (res > -1)
                    return word.Substring(0, word.IndexOf(marker, StringComparison.OrdinalIgnoreCase));
            }
            return nullify ? null : word;
        }
        public static string LRPrefix2(this String word, string marker)
        {
            if (!string.IsNullOrEmpty(marker) && !string.IsNullOrEmpty(word))
            {
                int res = word.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase);
                if (res > -1)
                    return word.Substring(0, word.IndexOf(marker, StringComparison.OrdinalIgnoreCase));
            }
            return word;
        }
        /// <summary>
        /// Returns a non-null or nullified prefix value marked by marker which is excluded.
        /// Marker is a regex.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="patternMarker"></param>
        /// <param name="nullify"></param>
        /// <returns></returns>
        public static string LRPrefixify(this String word, string patternMarker, bool nullify = true)
        {
            string matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word))
                return String.Empty;

            return word.LRPrefixify(patternMarker, out matchedMarker, nullify);
        }
        public static string LRPrefixify(this String word, string patternMarker, out string matchedMarker, bool nullify = true)
        {
            matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word) || String.IsNullOrEmpty(patternMarker))
                return matchedMarker;

            var match = Regex.Match(word, patternMarker, RegexOptions.IgnoreCase);
            //match.Dump();
            if (match.Length > 0)
            {
                matchedMarker = match.Captures[0].Value;
                string prefixified = word.LRPrefix(matchedMarker);
                if (String.IsNullOrWhiteSpace(prefixified) || String.IsNullOrEmpty(prefixified))
                    return word;

                return prefixified;
            }

            return nullify ? null : word;
        }
        public static string RLPostfixify(this String word, string patternMarker, bool nullify = true)
        {
            string matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word))
                return String.Empty;

            return word.RLPostfixify(patternMarker, out matchedMarker, nullify);
        }
        public static string RLPostfixify(this String word, string patternMarker, out string matchedMarker, bool nullify = true)
        {
            matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word) || String.IsNullOrEmpty(patternMarker))
                return matchedMarker;

            var match = Regex.Match(word, patternMarker, RegexOptions.RightToLeft | RegexOptions.IgnoreCase);

            if (match.Length > 0)
            {
                matchedMarker = match.Captures[0].Value;
                string postfixified = word.Substring(match.Index + matchedMarker.Length, word.Length - matchedMarker.Length - match.Index);

                return postfixified;
            }

            return nullify ? null : word;
        }
        public static string RLPrefixify(this String word, string patternMarker, bool nullify = true)
        {
            string matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word))
                return String.Empty;

            return word.RLPrefixify(patternMarker, out matchedMarker, nullify);
        }
        public static string RLPrefixify(this String word, string patternMarker, out string matchedMarker, bool nullify = true)
        {
            matchedMarker = String.Empty;
            if (String.IsNullOrWhiteSpace(word) || String.IsNullOrEmpty(patternMarker))
                return matchedMarker;

            var match = Regex.Match(word.ToLower(), patternMarker.ToLower(), RegexOptions.RightToLeft | RegexOptions.IgnoreCase);
            //match.Dump();
            if (match.Length > 0)
            {
                matchedMarker = match.Captures[0].Value;
                string prefixified = word.Substring(0, match.Index);

                return prefixified;
            }

            return nullify ? null : word;
        }
        public static string ToText(this string file)
        {
            using(FileStream fs = new FileStream(file,FileMode.Open))
            {
                return fs.ToByteArray().ToByteString();
            }
        }
        public static Stream ToStream(this string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                return fs;
            }
        }
        public static string ToText(this Stream stream)
        {
            return stream.ToByteArray().ToByteString();
        }
        public static string RemoveSpaces(this string _)
        {
            return _.Replace(" ", String.Empty);
        }
        public static bool IsMatch(this string source, string target)
        {
            return Regex.IsMatch(source,target,RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }
        /// <summary>
        /// If source is string indexed Array.IndexOf (array, value) does not work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this T[] array, T value)
        {
            int index = -1, length = array.Length;

            for (int i = 0; i < length; i++)
            {
                if(array[i].Equals(value))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public static string ConvertToSpaces(this string current)
        {
            StringBuilder spaces = new StringBuilder();
            spaces.Append(' ', current.Length);
            return spaces.ToString();
        }
        /// <summary>
        /// Get string value for a property.
        /// </summary>
        /// <param name="ExpectedStringsToFind"></param>
        /// <param name="searchSpace"></param>
        /// <param name="stringsInSearchSpace"></param>
        /// <param name="propertyIndex"></param>
        /// <param name="errorMargin">Margin of deviation in index value. Values do not have same index as their expected counterpart in the dictionary.</param>
        /// <returns></returns>
        public static string GetStringValue(this IDictionary<string, Match> ExpectedStringsToFind, string propertyName, string searchSpace,
            string[] stringsInSearchSpace, int errorMargin = 10)
        {
            var propertyExists = ExpectedStringsToFind.Where(s => s.Key == propertyName);
            if (!propertyExists.Any())
                return null;

            var index = ExpectedStringsToFind[propertyName].Index;
            int newIndex = 0;
            Dictionary<int, string> stringsInSearchSpaceDictionified = stringsInSearchSpace.Aggregate(new Dictionary<int, string>(), (c, n) =>
            {
                c.Add(newIndex, n);
                newIndex++;
                return c;
            });

            int length = stringsInSearchSpace.Length;
            List<int> indexUsed = new List<int>();

            newIndex = 0;
            //var metaDataQry = stringsInSearchSpace.Select(k => new { Name = k, Index = searchSpace.IndexOf(k) }).ToList();//values can be duplicates
            //query the indexes of metadata for comparing indexes.
            var metaDataQry = stringsInSearchSpace.Aggregate(new Dictionary<int, int>(), (c, n) =>
            {
                n = " " + n + " ";
                int ix = searchSpace.IndexOf(n);
                if (indexUsed.Any(u => u == ix))
                {
                    ix = searchSpace.IndexOf(n, ix + 1) + 1;
                }
                else
                {
                    indexUsed.Add(ix);
                };

                c.Add(newIndex, ix);
                newIndex++;
                return c;
            });

            string[] expectedStringsToFind = new string[ExpectedStringsToFind.Count()];
            newIndex = 0;
            var arrayList = ExpectedStringsToFind.ToArray().Aggregate(new List<string>(), (c, n) =>
            {
                c.Add(n.Key);
                expectedStringsToFind[newIndex] = n.Key;
                newIndex++;
                return c;
            }).ToArray();


            var searchSpaceSplitted = searchSpace.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ///Use only values that are not belonging to previous neighbour
            string neighbourValue = String.Empty;
            searchSpace = " " + searchSpace;
            var candidates = searchSpaceSplitted.Aggregate(new Dictionary<int, string>(), (current, value) =>
            {
                value = " " + value + " ";
                searchSpace = searchSpace + " ";

                int location = searchSpace.IndexOf(value);
                int deltaToIndex = Math.Abs(location - index);
                if ( deltaToIndex <= errorMargin)
                {
                    int indexOfPropertyName = arrayList.IndexOf<string>(propertyName);
                    int refIxNeighbourProp = indexOfPropertyName == 0 ?
                        1 : //exception: neighbour of contract is at righthand side
                        indexOfPropertyName - 1;//rest
                    var neighbourProp = expectedStringsToFind[refIxNeighbourProp];
                    int indexNeighbour = ExpectedStringsToFind[expectedStringsToFind[refIxNeighbourProp]].Index;

                    int stdMargin = Math.Abs(location - indexNeighbour);
                    int relativeMargin = Math.Abs(index - indexNeighbour) - neighbourProp.Length;
                    int locationNeighbourValue = searchSpace.IndexOf(neighbourValue);

                    //Note: if true value can be neighbour! If false it is certainly not neighbour.
                    bool isSafeDistanceFromNeighbour = stdMargin > errorMargin;
                    
                    bool iscandidate = deltaToIndex <= relativeMargin;
                    bool isNeighbourValue = value.Trim().Equals(neighbourValue.Trim()) && indexOfPropertyName != 0;

                     // (locationNeighbourValue - indexNeighbour); 
                    if (isSafeDistanceFromNeighbour || iscandidate)
                    {                        
                        //bool iscandidate = deltaToIndex <= relativeMargin;

                        if (!current.ContainsKey(location))
                            current.Add(location, value);
                    };
                }

                neighbourValue = value;//does not apply to first value.

                //replace value with spaces (otherwise searchspace could have duplicate search-items)
                string prefix = searchSpace.LRPrefix(value);
                string postfix = searchSpace.LRPostfix(value);
                StringBuilder newSearchSpaceSpacified = new StringBuilder();
                newSearchSpaceSpacified.Append(prefix);
                newSearchSpaceSpacified.Append(value.ConvertToSpaces());
                newSearchSpaceSpacified.Append(postfix);
                searchSpace = newSearchSpaceSpacified.ToString();
                return current;
            });

            #region obsolete
            //		var foundCandidateQry = stringsInSearchSpaceDictionified.
            //							Join(metaDataQry, oint => oint.Key, meta => meta.Key, (o, mta) => new { o, mta })
            //									///Values do not have same index as their expected counterpart in the dictionary. **\0/**
            //									///values are indexed fo identification
            //									.Where(w => (Math.Abs(w.mta.Value - index) <= errorMargin));
            #endregion

            if (candidates.Any())
                return candidates.Last().Value;
            else
                return null;
        }

        public static int TryGetBoid(this string broker)
        {
            var last = broker.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();
            int number = 0;
            int.TryParse(last, out number);
            return number;
        }
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        public static decimal QryStringToDecimal(this string queryString, string queryParam)
        {
            queryString = String.Format("{0}&", queryString);
            string stringValue = queryString.LRPostfixify(String.Format("{0}=", queryParam)).LRPrefix("&");
            stringValue = String.IsNullOrWhiteSpace(stringValue) ? String.Empty : stringValue;
            decimal number = 0;
            decimal.TryParse(stringValue, out number);
            return number;
        }
        public static string QryStringToString(this string queryString, string queryParam)
        {
            queryString = String.Format("{0}&", queryString);
            string stringValue = queryString.LRPostfixify(String.Format("{0}=", queryParam)).LRPrefix("&");
            stringValue = String.IsNullOrWhiteSpace(stringValue) ? String.Empty : stringValue;
            return stringValue;
        }
        /// <summary>
        /// Converts and trims a string to an invariant lowercase.
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static string Simplefy(this string _)
        {
            return String.IsNullOrWhiteSpace(_) ? String.Empty : _.Trim().ToLowerInvariant();
        }
    }
    public static class FunctionalExtensions
    {
        public static Func<C, R> Partial<A, B, C, R>(this Func<A, B, C, R> f, A a, B b)
        {
            return c => f(a, b, c);
        }
        public static Func<A, Func<B, Func<C, R>>> Curry<A, B, C, R>(this Func<A, B, C, R> f)
        {
            return a => b => c => f(a, b, c);
        }
        public static Func<TResult> Apply<TResult, TArg>(Func<TArg, TResult> func, TArg arg)
        {
            return () => func(arg);
        }

        public static Func<TResult> Apply<TResult, TArg1, TArg2>(Func<TArg1, TArg2, TResult> func,
                                                                  TArg1 arg1, TArg2 arg2)
        {
            return () => func(arg1, arg2);
        }
    }
}
