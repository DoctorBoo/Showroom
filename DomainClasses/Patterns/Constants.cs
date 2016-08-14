using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Helpers
{
    public class Constants
    {
        public const string PathArchive = "PathArchive";
        public const string UsersFromAppSettings = "users.csv";
        public const string DropCreateDatabaseIfModelChanges = "DropCreateDatabaseIfModelChanges";
        /// <summary>
        /// Like : \"  , \' 
        /// </summary>
        public static List<string> Punctuations = new List<String>() { @"""", @"'" };
        /// <summary>
        /// ? \ / , ' "
        /// </summary>
        public static List<string> IllegalChars = new List<String>() { @"""", @"'", @"\", "/", ",", "?"
                                                    , "?", "!"};
    }
}
