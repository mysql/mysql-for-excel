// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using MySQL.Utility;
using System.ServiceProcess;

namespace MySQL.ForExcel
{
 public static class MySQLForExcelConnectionsFile
  {
    
   public static bool CreateXMLFile()
   {
      var connections = new MySqlWorkbenchConnectionCollection();      
      string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySQL\Workbench\connections.xml";

      try
      {
        if (!Directory.Exists(Path.GetDirectoryName(file)))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(file));        
        }
              
        //Create connections file
        XmlDocument doc = new XmlDocument();
        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
        doc.AppendChild(dec);
        XmlElement root = doc.CreateElement("data");
        root.SetAttribute("grt_format", "2.0");
        doc.AppendChild(root);
        XmlElement valueNode = doc.CreateElement("value");
        valueNode.SetAttribute("type", "list");
        valueNode.SetAttribute("content-type", "object");
        valueNode.SetAttribute("content-struct-name", "db.mgmt.Connection");
        valueNode.InnerText = string.Empty;
        root.AppendChild(valueNode);
        doc.Save(file);


        string serviceName = string.Empty;
        var services = Utility.Service.GetInstances(".*mysqld.*");
        MySqlWorkbenchConnection defaultConn = null;

        foreach (var item in services)
        {
          serviceName = item.Properties["Name"].Value.ToString();
          var winService = new ServiceController(serviceName);
          if (winService.Status == ServiceControllerStatus.Running)
          {
            var parameters = GetStartupParameters(winService);
            if (parameters.Port != 0 && !String.IsNullOrEmpty(parameters.HostName))
            {
              //Add default connection      
              defaultConn = new MySqlWorkbenchConnection();
              defaultConn.Name = "MySQLForExcelConnection";
              defaultConn.Host = parameters.HostName == "." ? "localhost" : parameters.HostName;
              defaultConn.UserName = "root";
              defaultConn.Port = parameters.Port;
              defaultConn.DriverType = parameters.NamedPipesEnabled ? MySqlWorkbenchConnectionType.NamedPipes : MySqlWorkbenchConnectionType.Tcp;
              break;
            }
          }
        }
        if (defaultConn != null)
        {
          Save(defaultConn);
          return true;
        }
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, "Error when loading database connections file", String.Format(@"Description Error: \""{0}\""", ex.Message));
        infoDialog.ShowDialog();
        return false;              
      }
      
      return false;
   }


   /// <summary>
   /// Add a new Connection to the connections.xml file
   /// in %AppData%\MySQL\Workbench path
   /// </summary>
   /// <param name="newConnection">The new connection to add to file</param>
   public static void Save(MySqlWorkbenchConnection newConnection)
    {            
      string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MySQL\Workbench\connections.xml";
      
      if (!File.Exists(file))      
        CreateXMLFile();
            
      MySqlWorkbench.Connections.Add(newConnection);
      MySqlWorkbench.Connections.Save();
    }


    private struct MySQLStartupParameters
    {
      public string DefaultsFile;
      public string HostName;
      public string HostIPv4;
      public int Port;
      public string PipeName;
      public bool NamedPipesEnabled;
    }


    private static MySQLStartupParameters GetStartupParameters(ServiceController winService)
    {

      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";
      
      // get our host information
      parameters.HostName = winService.MachineName == "." ? "localhost" : winService.MachineName;
      parameters.HostIPv4 = Utility.Utility.GetIPv4ForHostName(parameters.HostName);

      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", winService.ServiceName));
      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();

      if (imagepath == null) return parameters;
      string[] args = Utility.Utility.SplitArgs(imagepath);

      bool IsRealMySQLService = false;
      if (args.Length >= 1)
      {
        string cmd = args[0];
        IsRealMySQLService = cmd.EndsWith("mysqld.exe") || cmd.EndsWith("mysqld-nt.exe") || cmd.EndsWith("mysqld") || cmd.EndsWith("mysqld-nt");
      }

      if (IsRealMySQLService)
      {
        // Parse our command line args
        Mono.Options.OptionSet p = new Mono.Options.OptionSet()
        .Add("defaults-file=", "", v => parameters.DefaultsFile = v)
        .Add("port=|P=", "", v => Int32.TryParse(v, out parameters.Port))
        .Add("enable-named-pipe", v => parameters.NamedPipesEnabled = true)
        .Add("socket=", "", v => parameters.PipeName = v);

        p.Parse(args);

        if (parameters.DefaultsFile == null) return parameters;

        // we have a valid defaults file
        IniFile f = new IniFile(parameters.DefaultsFile);
        Int32.TryParse(f.ReadValue("mysqld", "port", parameters.Port.ToString()), out parameters.Port);
        parameters.PipeName = f.ReadValue("mysqld", "socket", parameters.PipeName);

        // now see if named pipes are enabled
        parameters.NamedPipesEnabled = parameters.NamedPipesEnabled || f.HasKey("mysqld", "enable-named-pipe");
      
      }
      return parameters;      
    }
  }
}
