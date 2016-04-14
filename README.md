MySQL for Excel 1.0
=========
MySQL for Excel is an Excel Add-In that is installed and accessed from within the MS Excel’s Data tab offering a wizard-like interface arranged in an elegant yet simple way to help users browse MySQL Schemas, Tables, Views and Procedures and perform data operations against them using MS Excel as the vehicle to drive the data in and out MySQL Databases.
Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.

## Installation

* Prerequisites:
	* Visual Studio 2010 or greater.
	* .NET Framework 4.0 (Client or Full Profile).
	* Microsoft Office Excel 2007 or greater, for Microsoft Windows.
	* Visual Studio 2010 Tools for Office SDK (later called Office Developer Tools).
	* WiX Toolset, for building the installer MSI.
* Open MySQLForExcel.sln in Visual Studio.

## Features

MySQL for Excel has been designed to be a simple and friendly tool for data analysts who want to harness the power of MS Excel to play with MySQL data without worrying about the technical details involved to reach the data they want, boosting productivity so they can focus on the data analysis and manipulation.

* Import Data
	* MySQL for Excel makes the task of getting MySQL data into Excel a very easy one; there are no intermediate CSV files required, only a couple of clicks and data will be imported to Excel. MySQL for Excel supports importing data from tables, views and stored procedures.
* Export Data
	* MySQL for Excel allows users to create a new MySQL table from selected Excel data; data types are automatically recognized and defaulted for the new table and column names can be created from the first row of data, speeding up the process and making it simple for non-technical users.
* Append Data
	* MySQL for Excel lets users save selected Excel data in existing tables; it will automatically attempt to map the columns of the selection with the ones in the MySQL table by column name or by data type, users can then review the mapping, manually change it and store it for later use. Like the Export Data feature, appending data into an existing table is very easy.

## Documentation

For further information about MySQL or additional documentation, see:
* http://www.mysql.com
* http://dev.mysql.com/doc/mysql-for-excel/en/

## License

License information can be found in the Installer/COPYING file.

__MySQL FOSS License Exception__
We want free and open source software applications under certain licenses to be able to use specified GPL-licensed MySQL client libraries despite the fact that not all such FOSS licenses are compatible with version 2 of the GNU General Public License.
Therefore there are special exceptions to the terms and conditions of the GPLv2 as applied to these client libraries, which are identified and described in more detail in the FOSS License Exception at http://www.mysql.com/about/legal/licensing/foss-exception.html.

This distribution may include materials developed by third parties.
For license and attribution notices for these materials, please refer to the documentation that accompanies this distribution (see the "Licenses for Third-Party Components" appendix) or view the online documentation at http://dev.mysql.com/doc/.

__GPLv2 Disclaimer__
For the avoidance of doubt, except that if any license choice other than GPL or LGPL is available it will apply instead, Oracle elects to use only the General Public License version 2 (GPLv2) at this time for any software where a choice of GPL  license versions is made available with the language indicating that GPLv2 or any later version may be used, or where a choice of which version of the GPL is applied is otherwise unspecified.