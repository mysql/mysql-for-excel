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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class InfoDialog : AutoStyleableBaseDialog
  {
    private const int MIN_WIDTH = 580;
    private const int COLLAPSED_HEIGHT = 215;
    private const int EXPANDED_HEIGHT = 350;

    public enum InfoType { Success = 0, Error = 1, Warning = 2, Info = 3 };

    public bool ExpandedState { get; set; }
    public InfoType OperationType { get; set; }

    public string OperationStatusText
    {
      get { return lblOperationStatus.Text; }
      set { lblOperationStatus.Text = value; }
    }
    public string OperationSummaryText
    {
      get { return lblOperationSummary.Text; }
      set 
      { 
        lblOperationSummary.Text = value;
        lblOperationSummary.Height = 17 * ((value.Length / 64) + 1);
      }
    }
    public string OperationSummarySubText
    {
      get { return lblOperationSummarySub.Text; }
      set { lblOperationSummarySub.Text = value; }
    }
    public string OperationDetailsText
    {
      get { return txtDetails.Text; }
      set 
      {
        if (!String.IsNullOrEmpty(value))
        {
          txtDetails.Text = value;
          btnShowDetails.Visible = true;
        }
        else
          btnShowDetails.Visible = false;
      }
    }
    public bool WordWrapDetails
    {
      get { return txtDetails.WordWrap; }
      set { txtDetails.WordWrap = value; }
    }

    public InfoDialog(InfoType operationsType, string operationSummary, string operationDetails)
    {
      InitializeComponent();
      OperationType = operationsType;
      picLogo.Image = iconsList.Images[(int)operationsType];
      switch (operationsType)
      {
        case InfoType.Success:
          OperationStatusText = "Operation Completed Successfully";
          btnOK.Text = "OK";
          btnOK.DialogResult = DialogResult.OK;
          break;
        case InfoType.Warning:
          OperationStatusText = "Operation Completed With Warnings";
          btnOK.Text = "OK";
          btnOK.DialogResult = DialogResult.OK;
          break;
        case InfoType.Error:
          OperationStatusText = "An Error Ocurred";
          btnOK.Text = "Back";
          btnOK.DialogResult = DialogResult.Cancel;
          break;
        case InfoType.Info:
          OperationStatusText = "Information";
          btnOK.Text = "OK";
          btnOK.DialogResult = DialogResult.OK;
          break;
      }
      OperationSummaryText = operationSummary;
      OperationDetailsText = operationDetails;
      OperationSummarySubText = String.Format("Press {0} to continue.", btnOK.Text);
      ExpandedState = false;
      ChangeHeight();
    }

    public InfoDialog(bool operationSuccessful, string operationSummary, string operationDetails)
      : this((operationSuccessful ? InfoType.Success : InfoType.Error), operationSummary, operationDetails)
    {
    }

    private void ChangeHeight()
    {
      txtDetails.Visible = ExpandedState;
      Size = MinimumSize = new Size(MIN_WIDTH, (ExpandedState ? EXPANDED_HEIGHT : COLLAPSED_HEIGHT));
      MaximumSize = (ExpandedState ? new Size(0, 0) : MinimumSize);
    }

    private void btnShowDetails_Click(object sender, EventArgs e)
    {
      ExpandedState = !ExpandedState;
      ChangeHeight();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      DialogResult = btnOK.DialogResult;
    }

  }
}
