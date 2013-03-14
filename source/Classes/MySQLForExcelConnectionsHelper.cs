//
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.ServiceProcess;
  using System.Text;
  using System.Xml;
  using Microsoft.Win32;
  using MySQL.Utility;

  /// <summary>
  /// Contains methods used to work with connections used by MySQL for Excel to connect to a MySQL Server instance.
  /// </summary>
  public static class MySQLForExcelConnectionsHelper
  {
    /// <summary>
    /// Relative path and file name for the connections file used by MySQL for Excel only.
    /// </summary>
    public const string MYSQL_FOR_EXCEL_CONNECTIONS_FILE = @"\Oracle\MySQL For Excel\connections.xml";

    /// <summary>
    /// Relative path and file name for the connections file used by MySQL Workbench and shared with MySQL for Excel.
    /// </summary>
    public const string WORKBENCH_CONNECTIONS_FILE = @"\MySQL\Workbench\connections.xml";

    /// <summary>
    /// Gets the file path of the connections file used by MySQL for Excel only.
    /// </summary>
    public static string MySQLForExcelConnectionsFile
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + MYSQL_FOR_EXCEL_CONNECTIONS_FILE;
      }
    }

    /// <summary>
    /// Gets the file path of the connections file used by MySQL Workbench and shared with MySQL for Excel.
    /// </summary>
    public static string WorkbenchConnectionsFile
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + WORKBENCH_CONNECTIONS_FILE;
      }
    }

    /// <summary>
    /// Creates the connections file used by MySQL for Excel to connect to a MySQL Server instance.
    /// </summary>
    /// <param name="useWorkbenchConnectionsFile">Flag indicating if the connections file used is the MySQL Workbench one.</param>
    /// <returns><see cref="true"/> if the file was created and connections saved successfully, <see cref="false"/> otherwise.</returns>
    public static bool CreateXMLFile(bool useWorkbenchConnectionsFile)
    {
      bool success = true;
      var connections = new MySqlWorkbenchConnectionCollection();
      string connectionsFile = useWorkbenchConnectionsFile ? WorkbenchConnectionsFile : MySQLForExcelConnectionsFile;

      try
      {
        if (!Directory.Exists(Path.GetDirectoryName(connectionsFile)))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(connectionsFile));
        }

        //// Create connections file
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
        var services = Service.GetInstances(".*mysqld.*");
        MySqlWorkbenchConnection defaultConn = null;

        foreach (var item in services)
        {
          serviceName = item.Properties["Name"].Value.ToString();
          var winService = new ServiceController(serviceName);
          if (winService.Status == ServiceControllerStatus.Running)
          {
            var parameters = GetStartupParameters(winService);
            if (parameters.Port != 0 && !string.IsNullOrEmpty(parameters.HostName))
            {
              //// Add default connection
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

    /// <summary>
    /// Creates the connections file used by MySQL for Excel to connect to a MySQL Server instance.
    /// </summary>
    /// <returns><see cref="true"/> if the file was created and connections saved successfully, <see cref="false"/> otherwise.</returns>
    public static bool CreateXMLFile()
    {
      return CreateXMLFile(MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    /// <summary>
    /// Gets a list of connections that MySQL of Excel can use to connect to MySQL Server instances.
    /// </summary>
    /// <param name="useWorkbenchConnectionsFile">Flag indicating if the connections file used is the MySQL Workbench one.</param>
    /// <param name="reloadConnections">Flag indicating if connections are to be re-read from the connections file.</param>
    /// <returns>A list of <see cref="MySqlWorkbenchConnection"/> objects.</returns>
    public static List<MySqlWorkbenchConnection> GetConnections(bool useWorkbenchConnectionsFile, bool reloadConnections = false)
    {
      if (useWorkbenchConnectionsFile)
      {
        if (!File.Exists(WorkbenchConnectionsFile) && !CreateXMLFile(true))
        {
          return null;
        }

        if (reloadConnections)
        {
          MySqlWorkbench.LoadData();
        }
      }
      else
      {
        if (!File.Exists(MySQLForExcelConnectionsFile))
        {
          if (MySqlWorkbench.Connections == null)
          {
            return null;
          }

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
        {
          MySqlWorkbench.LoadExternalConnections(MySQLForExcelConnectionsFile);
        }
      }

      if (MySqlWorkbench.Connections == null)
      {
        return null;
      }

      return MySqlWorkbench.Connections.Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
    }

    /// <summary>
    /// Gets a list of connections that MySQL of Excel can use to connect to MySQL Server instances.
    /// </summary>
    /// <param name="reloadConnections">Flag indicating if connections are to be re-read from the connections file.</param>
    /// <returns>A list of <see cref="MySqlWorkbenchConnection"/> objects.</returns>
    public static List<MySqlWorkbenchConnection> GetConnections(bool reloadConnections = false)
    {
      return GetConnections(MySqlWorkbench.AllowsExternalConnectionsManagement, reloadConnections);
    }

    /// <summary>
    /// Migrates connections from the MySQL for Excel connections file to the MySQL Workbench connections one.
    /// </summary>
    public static void MigrateConnectionsFromMySQLForExcelToWorkbench()
    {
      //// If local connections file does not exist it means we already migrated existing connections or they were never created, no need to migrate.
      if (!File.Exists(MySQLForExcelConnectionsFile) || !MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        return;
      }

      //// Inform users we are about to migrate connections
      InfoDialog errorDlg = new InfoDialog(InfoDialog.InfoType.Info, Properties.Resources.MigrateConnectionsToWorkbenchInfoTitle, Properties.Resources.MigrateConnectionsToWorkbenchInfoDetail);
      errorDlg.OperationStatusText = Properties.Resources.MigrateConnectionsToWorkbenchInfoHeader;
      errorDlg.WordWrapDetails = true;
      errorDlg.ShowDialog();

      //// If Workbench is running we won't be able to migrate since the file will be in use, issue an error and exit, attempt to migrate next time.
      if (MySqlWorkbench.IsRunning)
      {
        errorDlg = new InfoDialog(false, Properties.Resources.UnableToMergeConnectionsErrorTitle, Properties.Resources.UnableToMergeConnectionsErrorDetail);
        errorDlg.WordWrapDetails = true;
        errorDlg.ShowDialog();
        return;
      }

      //// Check for duplicate names and if so add a suffix to local connections before migrating them
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
        {
          conn.Name = proposedConnectionName;
        }
      }

      //// Attempt to Rename Local connections file, if we can rename it we proceed with saving the connections in Workbench connections file.
      try
      {
        File.Move(MySQLForExcelConnectionsFile, MySQLForExcelConnectionsFile + ".bak");
      }
      catch (Exception ex)
      {
        errorDlg = new InfoDialog(false, Properties.Resources.UnableToDeleteLocalConnectionsFileError, "Description Error: \"" + ex.Message + "\"");
        errorDlg.WordWrapDetails = true;
        errorDlg.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        return;
      }

      //// Save the connections in Workbench, if we could do that we delete the renamed connections file, otherwise we revert it back.
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
        infoTitle = string.Format(Properties.Resources.ExcelConnectionsMigratedSuccessfullyTitle, localConnectionsList.Count);
        infoDetail.Append(Properties.Resources.ExcelConnectionsMigratedSuccessfullyDetail);
        infoDetail.Append(Environment.NewLine);
        infoDetail.Append(Environment.NewLine);
        foreach (MySqlWorkbenchConnection conn in localConnectionsList)
        {
          infoDetail.AppendFormat(conn.Name + Environment.NewLine);
        }
      }
      else
      {
        File.Move(MySQLForExcelConnectionsFile + ".bak", MySQLForExcelConnectionsFile);
        infoTitle = Properties.Resources.ExcelConnectionsMigrationErrorTitle;
        infoDetail.AppendFormat(Properties.Resources.ExcelConnectionsMigrationErrorDetail);
        infoDetail.AppendFormat(Environment.NewLine);
        infoDetail.AppendFormat(Environment.NewLine);
        infoDetail.Append(saveException.Message);
      }

      //// Inform users the results of the migration
      errorDlg = new InfoDialog(saveException == null, infoTitle, infoDetail.ToString());
      errorDlg.WordWrapDetails = true;
      errorDlg.ShowDialog();

      //// Load Connections Again so they are ready for use in Excel
      MySqlWorkbench.LoadData();
    }

    /// <summary>
    /// Removes a connection from the list of connections used by MySQL for Excel and saves the change in disk.
    /// </summary>
    /// <param name="connectionID">ID of the connection to remove from the list.</param>
    /// <param name="useWorkbenchConnectionsFile">Flag indicating if the connections file used is the MySQL Workbench one.</param>
    /// <returns><see cref="true"/> if the removal and saving were successful, <see cref="false"/> otherwise.</returns>
    public static bool RemoveConnection(string connectionID, bool useWorkbenchConnectionsFile)
    {
      bool success = true;

      try
      {
        if (useWorkbenchConnectionsFile)
        {
          if (MySqlWorkbench.IsRunning)
          {
            InfoDialog infoDlg = new InfoDialog(false, Properties.Resources.UnableToDeleteConnectionsWhenWBRunning, string.Empty);
            infoDlg.OperationSummarySubText = Properties.Resources.CloseWBAdviceToDelete;
            infoDlg.ShowDialog();
            success = false;
          }
          else
          {
            MySqlWorkbench.Connections.Remove(connectionID);
          }
        }
        else
        {
          MySqlWorkbench.Connections.Remove(connectionID, MySQLForExcelConnectionsFile);
        }
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, Properties.Resources.UnableToDeleteConnectionError, "Description Error: \"" + ex.Message + "\"");
        infoDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Removes a connection from the list of connections used by MySQL for Excel and saves the change in disk.
    /// </summary>
    /// <param name="connectionID">ID of the connection to remove from the list.</param>
    /// <returns><see cref="true"/> if the removal and saving were successful, <see cref="false"/> otherwise.</returns>
    public static bool RemoveConnection(string connectionID)
    {
      return RemoveConnection(connectionID, MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    /// <summary>
    /// Adds a new connection to the list of connections used by MySQL for Excel and saves the change in disk.
    /// </summary>
    /// <param name="newConnection">The connection to be added to the connections list.</param>
    /// <param name="useWorkbenchConnectionsFile">Flag indicating if the connections file used is the MySQL Workbench one.</param>
    /// <returns><see cref="true"/> if the addition and saving were successful, <see cref="false"/> otherwise.</returns>
    public static bool SaveConnection(MySqlWorkbenchConnection newConnection, bool useWorkbenchConnectionsFile)
    {
      bool success = true;
      string connectionsFile = useWorkbenchConnectionsFile ? WorkbenchConnectionsFile : MySQLForExcelConnectionsFile;

      if (!File.Exists(connectionsFile) && !CreateXMLFile())
      {
        return false;
      }

      try
      {
        MySqlWorkbench.Connections.Add(newConnection);
        if (useWorkbenchConnectionsFile)
        {
          MySqlWorkbench.Connections.Save();
        }
        else
        {
          MySqlWorkbench.Connections.Save(MySQLForExcelConnectionsFile);
        }
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

    /// <summary>
    /// Adds a new connection to the list of connections used by MySQL for Excel and saves the change in disk.
    /// </summary>
    /// <param name="newConnection">The connection to be added to the connections list.</param>
    /// <returns><see cref="true"/> if the addition and saving were successful, <see cref="false"/> otherwise.</returns>
    public static bool SaveConnection(MySqlWorkbenchConnection newConnection)
    {
      return SaveConnection(newConnection, MySqlWorkbench.AllowsExternalConnectionsManagement);
    }

    /// <summary>
    /// Gets the connection properties for a existing MySQL Servers instance installed as a Windows service in the local computer.
    /// </summary>
    /// <param name="winService">Windows service for a MySQL Server instance.</param>
    /// <returns>A <see cref="MySQLStartupParameters"/> struct with the connection properties.</returns>
    private static MySQLStartupParameters GetStartupParameters(ServiceController winService)
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      //// Get our host information
      parameters.HostName = winService.MachineName == "." ? "localhost" : winService.MachineName;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + winService.ServiceName);
      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();

      if (imagepath == null)
      {
        return parameters;
      }

      string[] args = Utility.SplitArgs(imagepath);
      bool isRealMySQLService = false;
      if (args.Length >= 1)
      {
        string cmd = args[0];
        isRealMySQLService = cmd.EndsWith("mysqld.exe") || cmd.EndsWith("mysqld-nt.exe") || cmd.EndsWith("mysqld") || cmd.EndsWith("mysqld-nt");
      }

      if (isRealMySQLService)
      {
        //// Parse our command line args
        Mono.Options.OptionSet p = new Mono.Options.OptionSet()
        .Add("defaults-file=", "", v => parameters.DefaultsFile = v)
        .Add("port=|P=", "", v => Int32.TryParse(v, out parameters.Port))
        .Add("enable-named-pipe", v => parameters.NamedPipesEnabled = true)
        .Add("socket=", "", v => parameters.PipeName = v);

        p.Parse(args);
        if (parameters.DefaultsFile == null)
        {
          return parameters;
        }

        //// We have a valid defaults file
        IniFile f = new IniFile(parameters.DefaultsFile);
        Int32.TryParse(f.ReadValue("mysqld", "port", parameters.Port.ToString()), out parameters.Port);
        parameters.PipeName = f.ReadValue("mysqld", "socket", parameters.PipeName);

        //// Now see if named pipes are enabled
        parameters.NamedPipesEnabled = parameters.NamedPipesEnabled || f.HasKey("mysqld", "enable-named-pipe");
      }

      return parameters;
    }

    /// <summary>
    /// Contains connection parameters extracted from a Windows service of a MySQL Server instance.
    /// </summary>
    private struct MySQLStartupParameters
    {
      /// <summary>
      /// The default INI file used by a MySQL Server instance containing initialization parameters and values.
      /// </summary>
      public string DefaultsFile;

      /// <summary>
      /// The connection IP of the MySQL Server instance.
      /// </summary>
      public string HostIPv4;

      /// <summary>
      /// The connection host name of the MySQL Server instance.
      /// </summary>
      public string HostName;

      /// <summary>
      /// Flag indicating if names pipes are enabled for the MySQL Server connection.
      /// </summary>
      public bool NamedPipesEnabled;

      /// <summary>
      /// The name of the pipe used by the connection.
      /// </summary>
      public string PipeName;

      /// <summary>
      /// The connection port of the MySQL Server instance.
      /// </summary>
      public int Port;
    }
  }
}