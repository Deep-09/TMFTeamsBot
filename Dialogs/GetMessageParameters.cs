using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Dialogs
{
    public class GetMessageParameters
    {
        public string columnName { get; set; }
        public string displayName { get; set; }
        public string columnType { get; set; }
        public bool visibility { get; set; }
        public string comparator { get; set; }
        public string[] values { get; set; }
        
    }

}