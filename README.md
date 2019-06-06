MySQL for Excel 1.3
=========
MySQL for Excel is an Excel Add-In that is installed and accessed from within the MS Excel’s Data tab offering a wizard-like interface arranged in an elegant yet simple way to help users browse MySQL Schemas, Tables, Views and Procedures and perform data operations against them using MS Excel as the vehicle to drive the data in and out MySQL Databases.
Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.

License information can be found in the Installer/LICENSE file.

## Installation

* Prerequisites:
	* Visual Studio 2015 or greater.
	* .NET Framework 4.5.2 (Client or Full Profile).
	* Microsoft Office Excel 2007 or greater, for Microsoft Windows.
	* Visual Studio 2010 Tools for Office SDK (later called Office Developer Tools).
	* WiX Toolset, for building the installer MSI.
	* MSBuild Community Tasks, for building the installer MSI.
* Open MySQLForExcel.sln or Package.sln in Visual Studio.

## Features

MySQL for Excel has been designed to be a simple and friendly tool for data analysts who want to harness the power of MS Excel to play with MySQL data without worrying about the technical details involved to reach the data they want, boosting productivity so they can focus on the data analysis and manipulation.

* Import Data
	* MySQL for Excel makes the task of getting MySQL data into Excel a very easy one; there are no intermediate CSV files required, only a couple of clicks and data will be imported to Excel. MySQL for Excel supports importing data from tables, views and stored procedures.
* Export Data
	* MySQL for Excel allows users to create a new MySQL table from selected Excel data; data types are automatically recognized and defaulted for the new table and column names can be created from the first row of data, speeding up the process and making it simple for non-technical users.
* Append Data
	* MySQL for Excel lets users save selected Excel data in existing tables; it will automatically attempt to map the columns of the selection with the ones in the MySQL table by column name or by data type, users can then review the mapping, manually change it and store it for later use. Like the Export Data feature, appending data into an existing table is very easy.
* Edit Data
	* MySQL for Excel now provides a way to edit a MySQL table's data directly within Excel using a new worksheet as a canvas to update existing data, insert new rows and delete existing ones in a very friendly and intuitive way. Changes are pushed back to the MySQL Server as a transaction batch with the click of a button, or can be pushed as soon as they are done with no further clicks if users prefer it. This is a powerful feature since Excel is a natural user interface to operate with data, and these changes can be reflected in the database immediately.

## Documentation

For further information about MySQL or additional documentation, see:
* http://www.mysql.com
* http://dev.mysql.com/doc/mysql-for-excel/en/