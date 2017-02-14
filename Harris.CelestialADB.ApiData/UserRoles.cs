using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public static class UserRoles
    {
        public static string Administrator { get { return "Administrator"; } }
        public static string SuperUser { get { return "SuperUser"; } }
        public static string ViewContributor { get { return "ViewContributor"; } }
        public static string ColumnContributor { get { return "ColumnContributor"; } }
        public static string ComponentContributor { get { return "ComponentContributor"; } }
        public static string Registered { get { return "Registered"; } }
        public static string Active { get { return "Active"; } }
    }
}
