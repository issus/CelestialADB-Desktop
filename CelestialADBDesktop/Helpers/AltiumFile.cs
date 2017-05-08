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
        static Regex dblibConnStrRex = new Regex(@"^(?<key>ConnectionString=FILE NAME=)(?<File>.+)\s*$", RegexOptions.Multiline);

        public static AltiumDblibPath BrowseForDbLib()
        {
            AltiumDblibPath path = new AltiumDblibPath();

            var dialog = new CommonOpenFileDialog()
            {
                EnsureFileExists = true,
                Title = "Locate DbLib File",
                Multiselect = false
            };

            dialog.Filters.Add(new CommonFileDialogFilter("Altium Database File", "*.DbLib"));

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                path.Error = "DbLib not selected.";
                return path;
            }

            path.Success = true;
            path.Path = dialog.FileName;

            path.ConnectionStringPath = ReadUdlLocation(path.Path);

            return path;
        }
        public static string ReadUdlLocation(string dbLibPath)
        {
            try
            {
                string dblib = File.ReadAllText(dbLibPath);
                if (dblibConnStrRex.IsMatch(dblib))
                {
                    return dblibConnStrRex.Match(dblib).Groups["File"].Value.Trim();
                }
            }
            catch
            {
                return "";
            }

            return "";
        }

        public static bool SetUdlLocationInDbLib(string dbLibPath, string udlPath)
        {
            if (!File.Exists(dbLibPath))
                return false;

            if (!File.Exists(udlPath))
                return false;

            string dblib = "";

            try
            {
                 dblib = File.ReadAllText(dbLibPath);

                if (dblibConnStrRex.IsMatch(dblib))
                {
                    if (udlPath == dblibConnStrRex.Match(dblib).Groups["File"].Value.Trim())
                        return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            dblib = dblibConnStrRex.Replace(dblib, String.Format("${{key}}{0}", udlPath));

            File.WriteAllText(dbLibPath, dblib);

            return true;
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
                    cred.Username = match.Groups["Value"].Captures[i].Value;
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
                udl.AppendFormat("Provider=SQLNCLI11.1; Password=\"{0}\";User ID={1}; Initial Catalog=altium_library; Data Source={2}; Initial File Name=\"\"; Server SPN=\"\"\r\n",
                    cred.Password, cred.Username, cred.Server);

                File.WriteAllText(connectionStringPath, udl.ToString(), Encoding.Unicode);
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
