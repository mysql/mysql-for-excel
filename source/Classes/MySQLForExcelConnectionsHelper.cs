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
  public static class MySQLForExcelConnectionsHelper
  {
    public const string WORKBENCH_CONNECTIONS_FILE = @"\MySQL\Workbench\connections.xml";
    public const string MYSQL_FOR_EXCEL_CONNECTIONS_FILE = @"\Oracle\MySQL For Excel\connections.xml";

    public static string WorkbenchConnectionsFile
    {
      get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + WORKBENCH_CONNECTIONS_FILE; }
    }

    public static string MySQLForExcelConnectionsFile
    {
      get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + MYSQL_FOR_EXCEL_CONNECTIONS_FILE; }
    }

    public static bool CreateXMLFile(bool useWorkbenchConnectionsFile)
    {
      bool success = true;
      var connections = new MySqlWorkbenchConnectionCollection();
      string connectionsFile = (useWorkbenchConnectionsFile ? WorkbenchConnectionsFile : MySQLForExcelConnectionsFile);

      try
      {
        if (!Directory.Exists(Path.GetDirectoryName(connectionsFile)))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(connectionsFile));        
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
        doc.Save(connectionsFile);

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
          SaveConnection(defaultConn);
          success = true;
        }
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.DatabaseConnectionsFileLoadingErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        success = false;              
      }
      
      return success;
    }

    public static bool CreateXMLFile()
    {
      return CreateXMLFile(MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    public static List<MySqlWorkbenchConnection> GetConnections(bool useWorkbenchConnectionsFile)
    {
      if (useWorkbenchConnectionsFile)
      {
        if (!File.Exists(WorkbenchConnectionsFile))
          if (!CreateXMLFile(true))
            return null;
      }
      else
      {
        if (!File.Exists(MySQLForExcelConnectionsFile))
        {
          if (MySqlWorkbench.Connections == null)
            return null;
          try
          {
            CreateXMLFile(false);
            foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections)
            {
              conn.New = true;
              conn.Id = Guid.NewGuid().ToString();
            }
            MySqlWorkbench.Connections.Save(MySQLForExcelConnectionsFile);
          }
          catch (Exception ex)
          {
            InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.UnableToSaveConnectionsFileError, ex.Message);
            errorDialog.WordWrapDetails = true;
            errorDialog.ShowDialog();
            MiscUtilities.WriteAppErrorToLog(ex);
          }
        }
        else
          MySqlWorkbench.LoadExternalConnections(MySQLForExcelConnectionsFile);
      }
      if (MySqlWorkbench.Connections == null)
        return null;
      return MySqlWorkbench.Connections.Where(c => !String.IsNullOrEmpty(c.Name)).ToList();
    }

    public static List<MySqlWorkbenchConnection> GetConnections()
    {
      return GetConnections(MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    public static bool SaveConnection(MySqlWorkbenchConnection newConnection, bool useWorkbenchConnectionsFile)
    {
      bool success = true;
      string connectionsFile = (useWorkbenchConnectionsFile ? WorkbenchConnectionsFile : MySQLForExcelConnectionsFile);

      if (!File.Exists(connectionsFile))
        if (!CreateXMLFile())
          return false;
      
      try
      {
        MySqlWorkbench.Connections.Add(newConnection);
        if (useWorkbenchConnectionsFile)
          MySqlWorkbench.Connections.Save();
        else
          MySqlWorkbench.Connections.Save(MySQLForExcelConnectionsFile);
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.ConnectionsFileSavingErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        success = false;              
      }
      return success;
    }

    public static bool SaveConnection(MySqlWorkbenchConnection newConnection)
    {
      return SaveConnection(newConnection, MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    public static bool RemoveConnection(string connectionID, bool useWorkbenchConnectionsFile)
    {
      bool success = true;

      try
      {
        if (useWorkbenchConnectionsFile)
        {
          if (MySqlWorkbench.IsRunning)
          { 
            InfoDialog infoDlg = new InfoDialog(false, Properties.Resources.UnableToDeleteConnectionsWhenWBRunning, String.Empty);
            infoDlg.OperationSummarySubText = Properties.Resources.CloseWBAdviceToDelete;
            infoDlg.ShowDialog();
            success = false;
          }
          else
            MySqlWorkbench.Connections.Remove(connectionID);
        }
        else
          MySqlWorkbench.Connections.Remove(connectionID, MySQLForExcelConnectionsFile);
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, Properties.Resources.UnableToDeleteConnectionError, String.Format(@"Description Error: \""{0}\""", ex.Message));
        infoDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        success = false;
      }
      return success;
    }

    public static bool RemoveConnection(string connectionID)
    {
      return RemoveConnection(connectionID, MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    public static void MigrateConnectionsFromMySQLForExcelToWorkbench()
    {
      // If local connections file does not exist it means we already migrated existing connections or they were never created, no need to migrate.
      if (!File.Exists(MySQLForExcelConnectionsFile) || !MySqlWorkbench.AllowsExternalConnectionsManagement)
        return;

      // Inform users we are about to migrate connections
      InfoDialog errorDlg = new InfoDialog(InfoDialog.InfoType.Info, Properties.Resources.MigrateConnectionsToWorkbenchInfoTitle, Properties.Resources.MigrateConnectionsToWorkbenchInfoDetail);
      errorDlg.OperationStatusText = Properties.Resources.MigrateConnectionsToWorkbenchInfoHeader;
      errorDlg.WordWrapDetails = true;
      errorDlg.ShowDialog();

      // If Workbench is running we won't be able to migrate since the file will be in use, issue an error and exit, attempt to migrate next time.
      if (MySqlWorkbench.IsRunning)
      {
        errorDlg = new InfoDialog(false, Properties.Resources.UnableToMergeConnectionsErrorTitle, Properties.Resources.UnableToMergeConnectionsErrorDetail);
        errorDlg.WordWrapDetails = true;
        errorDlg.ShowDialog();
        return;
      }

      // Check for duplicate names and if so add a suffix to local connections before migrating them
      List<MySqlWorkbenchConnection> workbenchConnectionsList = GetConnections(true);
      List<MySqlWorkbenchConnection> localConnectionsList = GetConnections(false);
      foreach (MySqlWorkbenchConnection conn in localConnectionsList)
      {
        conn.New = true;
        conn.Id = Guid.NewGuid().ToString();
        string proposedConnectionName = conn.Name;
        int suffix = 2;
        while (workbenchConnectionsList.Any(c => c.Name == proposedConnectionName))
        {
          proposedConnectionName = conn.Name + "_" + suffix++;
        }
        if (conn.Name != proposedConnectionName)
          conn.Name = proposedConnectionName;
      }

      // Attempt to Rename Local connections file, if we can rename it we proceed with saving the connections in Workbench connections file.
      try
      {
        File.Move(MySQLForExcelConnectionsFile, MySQLForExcelConnectionsFile + ".bak");
      }
      catch (Exception ex)
      {
        errorDlg = new InfoDialog(false, Properties.Resources.UnableToDeleteLocalConnectionsFileError, String.Format(@"Description Error: \""{0}\""", ex.Message));
        errorDlg.WordWrapDetails = true;
        errorDlg.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        return;
      }

      // Save the connections in Workbench, if we could do that we delete the renamed connections file, otherwise we revert it back.
      Exception saveException = null;
      try
      {
        MySqlWorkbench.Connections.Save();
      }
      catch (Exception ex)
      {
        saveException = ex;
        MiscUtilities.WriteAppErrorToLog(ex);
      }
      string infoTitle;
      StringBuilder infoDetail = new StringBuilder();
      if (saveException == null)
      {
        File.Delete(MySQLForExcelConnectionsFile + ".bak");
        infoTitle = String.Format("{0} database connections were migrated successfully.", localConnectionsList.Count);
        infoDetail.AppendFormat("The following connections were migrated to the MySQL Workbench connections file: {0}{0}", Environment.NewLine);
        foreach (MySqlWorkbenchConnection conn in localConnectionsList)
          infoDetail.AppendFormat(String.Format("{0}{1}", conn.Name, Environment.NewLine));
      }
      else
      {
        File.Move(MySQLForExcelConnectionsFile + ".bak", MySQLForExcelConnectionsFile);
        infoTitle = "Local database connections could not be migrated.";
        infoDetail.AppendFormat("Database connections were not migrated because the following error ocurred:{0}{0}", Environment.NewLine);
        infoDetail.Append(saveException.Message);
      }

      // Inform users the results of the migration
      errorDlg = new InfoDialog(saveException == null, infoTitle, infoDetail.ToString());
      errorDlg.WordWrapDetails = true;
      errorDlg.ShowDialog();

      // Load Connections Again so they are ready for use in Excel
      MySqlWorkbench.LoadData();
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

      bool isRealMySQLService = false;
      if (args.Length >= 1)
      {
        string cmd = args[0];
        isRealMySQLService = cmd.EndsWith("mysqld.exe") || cmd.EndsWith("mysqld-nt.exe") || cmd.EndsWith("mysqld") || cmd.EndsWith("mysqld-nt");
      }

      if (isRealMySQLService)
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
