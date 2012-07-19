using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQL.ForExcel.Properties;
using MySQL.Utility;

namespace MySQL.ForExcel
{
  public class MySQLForExcelSettings : CustomSettingsProvider
  {
    public override string ApplicationName
    {
      get { return Resources.AppName; }
      set { } 
    }

    public override string SettingsPath
    {
      get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL For Excel\settings.config"; }
    }
  }
}
