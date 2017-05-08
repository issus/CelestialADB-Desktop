using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public class DatabaseTableColumnDefinition
    {
        public string ColumnName { get; set; }
        public DatabaseColumnType DataType { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool PrimaryKey { get; set; }
    }

    // yes, i know, this isn't by any means comprehensive or "correct" but it is all I need...
    public enum DatabaseColumnType
    {
        Nvarchar,
        Int,
        Money
    }
}
