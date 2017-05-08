using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    // yes this is going to make easy to transfer components around in a nicely formatted way. 
    // No this is not going to make life even remotely easy to pump these into a database which altium can deal with....
    public class AltiumComponent
    {
        public AltiumComponent()
        {
            FootprintReferences = new List<AltiumComponentPathRef>();
            SymbolReferences = new List<AltiumComponentPathRef>();
            Parametrics = new List<AltiumComponentParameter>();
            Suppliers = new List<AltiumComponentSupplier>();
            Links = new List<AltiumComponentLink>();
        }

        /// <summary>
        /// Part's ID in the database.
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// ComponentType Column is used for view mapping
        /// </summary>
        public string ComponentType { get; set; }
        /// <summary>
        /// ComponentType Column is used for view mapping
        /// </summary>
        public string ComponentSubType { get; set; }

        /// <summary>
        /// Altium Comment field, usually "=Value" or "='Part Number'"
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Supplier description. Digikey style short description preferred.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Manufacturers name
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// Manufacturers Part Number
        /// </summary>
        public string MfrPartNo { get; set; }

        /// <summary>
        /// Footprints available for this part. Database currently supports a maximum of 3.
        /// </summary>
        public List<AltiumComponentPathRef> FootprintReferences { get; set; }

        /// <summary>
        /// Schematic symbols available for this part. You should not need more than one, but it is supported by altium...
        /// </summary>
        public List<AltiumComponentPathRef> SymbolReferences { get; set; }

        /// <summary>
        /// All other parameters for the component which will be stored in the database.
        /// </summary>
        public List<AltiumComponentParameter> Parametrics { get; set; }

        /// <summary>
        /// List of suppliers the part can be sourced from.
        /// </summary>
        public List<AltiumComponentSupplier> Suppliers { get; set; }

        /// <summary>
        /// List of documentation links that will be shown as references in Altium.
        /// </summary>
        public List<AltiumComponentLink> Links { get; set; }

        public override string ToString()
        {
            return string.Format("[{0} - {1}] {2} from {3}", PartId, MfrPartNo, Description, Manufacturer);
        }
    }

    /// <summary>
    /// Used for Footprint/Symbol references
    /// </summary>
    public class AltiumComponentPathRef
    {
        public AltiumComponentPathRef()
        {
        }
        public AltiumComponentPathRef(string reference, string path)
        {
            Reference = reference;
            Path = path;
        }
        public string Reference { get; set; }
        public string Path { get; set; }
    }

    /// <summary>
    /// Parametric information about the component from the supplier
    /// </summary>
    public class AltiumComponentParameter
    {
        public AltiumComponentParameter()
        {
        }
        public AltiumComponentParameter(string column, string val)
        {
            ColumnName = column;
            Value = val;
        }
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = {1}", ColumnName, Value);
        }
    }

    /// <summary>
    /// Supplier information for a part
    /// </summary>
    public class AltiumComponentSupplier
    {
        public AltiumComponentSupplier()
        {
        }

        public AltiumComponentSupplier(string name, string partno)
        {
            Name = name;
            PartNumber = partno;
        }

        public string Name { get; set; }
        public string PartNumber { get; set; }
        public string Link { get; set; }
    }

    /// <summary>
    /// Used for documentation/supplier links
    /// </summary>
    public class AltiumComponentLink
    {
        public AltiumComponentLink()
        {
        }
        public AltiumComponentLink(string desc, string url)
        {
            Description = desc;
            Url = url;
        }

        public string Description { get; set; }
        public string Url { get; set; }
    }
}
