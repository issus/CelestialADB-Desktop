using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public class ColumnDefinition
    {
        public string ColumnName { get; set; }
        public string ViewName { get; set; }
    }

    public class DatabaseViewDefinition
    {
        public string ViewName { get; set; }
        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
