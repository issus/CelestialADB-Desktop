using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public class GenericViewDefinition
    {
        public GenericViewDefinition()
        {
            Columns = new List<ViewColumn>();
        }

        public string ViewName { get; set; }

        public List<ViewColumn> Columns { get; set; }

        public string ComponentType { get; set; }
        public string ComponentSubType { get; set; }
    }

    public class ViewColumn
    {
        public ViewColumn()
        {

        }

        public ViewColumn(string col)
        {
            Column = col;
        }

        public ViewColumn(string col, string friendly)
        {
            Column = col;
            FriendlyName = friendly;
        }

        public string Column { get; set; }
        public string FriendlyName { get; set; }
    }
}
