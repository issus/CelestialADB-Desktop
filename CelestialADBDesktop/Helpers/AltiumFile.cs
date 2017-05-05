using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.Helpers
{
    static class AltiumFile
    {
        public static AltiumDblibPath BrowseForDbLib()
        {
            AltiumDblibPath path = new AltiumDblibPath();

            var dialog = new CommonOpenFileDialog();
            dialog.EnsureFileExists = true;
            dialog.Title = "Locate DbLib File";
            dialog.Multiselect = false;
            dialog.Filters.Add(new CommonFileDialogFilter("Altium Database File", "*.DbLib"));

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                path.Error = "DbLib not selected.";
                return path;
            }

            path.Success = true;
            path.Path = dialog.FileName;

            try
            {
                Regex dblibRex = new Regex(@"^ConnectionString=FILE NAME=(?<File>.+)\s*$", RegexOptions.Multiline);
                string dblib = File.ReadAllText(path.Path);
                if (dblibRex.IsMatch(dblib))
                {
                    path.ConnectionStringPath = dblibRex.Match(dblib).Groups["File"].Value.Trim();
                }
            }
            catch (Exception err)
            {
                path.Error = err.Message;
                path.Success = false;
            }

            return path;
        }

        public static UdlCredentials ReadUdlFile(AltiumDblibPath p)
        {
            return ReadUdlFile(p.ConnectionStringPath);
        }

        public static UdlCredentials ReadUdlFile(string connectionStringPath)
        {
            string udl = "";
            UdlCredentials cred = new UdlCredentials();
            Regex tokenRex = new Regex(@"^(?=[^;])(?:(?<Token>.+?)=(?<Value>.+?);)+", RegexOptions.Multiline);

            if (!File.Exists(connectionStringPath))
            {
                cred.Error = "Connection file is missing.";
                return cred;
            }

            try
            {
                udl = File.ReadAllText(connectionStringPath);
                if (!tokenRex.IsMatch(udl))
                {
                    cred.Error = "UDL file has not been configured correctly.";
                    return cred;
                }
            }
            catch (Exception err)
            {
                cred.Error = err.Message;
                return cred;
            }
            
            var match = tokenRex.Match(udl);
            for (int i = 0; i < match.Groups["Token"].Captures.Count; i++)
            {
                if (match.Groups["Token"].Captures[i].Value == "User ID")
                    cred.Username= match.Groups["Value"].Captures[i].Value;
                else if (match.Groups["Token"].Captures[i].Value == "Password")
                    cred.Password = match.Groups["Value"].Captures[i].Value;
                else if (match.Groups["Token"].Captures[i].Value == "Data Source")
                    cred.Server = match.Groups["Value"].Captures[i].Value;
            }

            return cred;
        }

        public static bool CreateMsSqlUdlFile(string connectionStringPath, UdlCredentials cred)
        {
            if (cred.Server == "csql.database.windows.net" && !cred.Username.EndsWith("@csql"))
            {
                cred.Username = String.Format("{0}@csql", cred.Username);
            }

            try
            {
                StringBuilder udl = new StringBuilder();
                udl.AppendLine("[oledb]");
                udl.AppendLine("; Everything after this line is an OLE DB initstring");
                udl.AppendFormat("Provider = SQLNCLI11.1; Password = {0}; User ID = {1}; Initial Catalog = altium_library; Data Source = {2}; Initial File Name = \"\"; Server SPN = \"\"\r\n",
                    cred.Password, cred.Username, cred.Server);

                File.WriteAllText(connectionStringPath, udl.ToString());
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
                return false;
            }

            return true;
        }
    }

    class UdlCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }

        public string Error { get; set; }
    }

    class AltiumDblibPath
    {
        public AltiumDblibPath()
        {
            Success = false;
        }

        public string Path { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        
        public string ConnectionStringPath { get; set; }
    }
}
