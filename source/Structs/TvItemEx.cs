// Copyright (c) 2014, 2016, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Runtime.InteropServices;

namespace MySQL.ForExcel.Structs
{
  /// <summary>
  /// Specifies or receives attributes of a tree-view item.
  /// </summary>
  /// <remarks>DO NOT change the order of the struct elements since C++ expects it in this specific order.</remarks>
  internal struct TvItemEx
  {
    /// <summary>
    /// Array of flags that indicate which of the other structure members contain valid data.
    /// When this structure is used with the TVM_GETITEM message, the mask member indicates the item attributes to retrieve.
    /// If used with the TVM_SETITEM message, the mask indicates the attributes to set.
    /// </summary>
    public int mask;

    /// <summary>
    /// Handle to the item.
    /// </summary>
    public IntPtr hItem;

    /// <summary>
    /// Set of bit flags and image list indexes that indicate the item's state.
    /// When setting the state of an item, the stateMask member indicates the valid bits of this member.
    /// When retrieving the state of an item, this member returns the current state for the bits indicated in the stateMask member.
    /// </summary>
    public int state;

    /// <summary>
    /// Bits of the state member that are valid. If you are retrieving an item's state, set the bits of the stateMask member
    /// to indicate the bits to be returned in the state member. If you are setting an item's state, set the bits of the stateMask
    /// member to indicate the bits of the state member that you want to set. To set or retrieve an item's overlay image index,
    /// set the TVIS_OVERLAYMASK bits. To set or retrieve an item's state image index, set the TVIS_STATEIMAGEMASK bits.
    /// </summary>
    public int stateMask;

    /// <summary>
    /// Pointer to a null-terminated string that contains the item text if the structure specifies item attributes.
    /// If this member is the LPSTR_TEXTCALLBACK value, the parent window is responsible for storing the name.
    /// In this case, the tree-view control sends the parent window a TVN_GETDISPINFO notification code when it needs
    /// the item text for displaying, sorting, or editing and a TVN_SETDISPINFO notification code when the item text changes.
    /// If the structure is receiving item attributes, this member is the address of the buffer that receives the item text.
    /// Note that although the tree-view control allows any length string to be stored as item text, only the first 260 characters are displayed.
    /// </summary>
    [MarshalAs(UnmanagedType.LPTStr)]
    public string lpszText;

    /// <summary>
    /// Size of the buffer pointed to by the pszText member, in characters.
    /// If this structure is being used to set item attributes, this member is ignored.
    /// </summary>
    public int cchTextMax;

    /// <summary>
    /// Index in the tree-view control's image list of the icon image to use when the item is in the nonselected state.
    /// If this member is the I_IMAGECALLBACK value, the parent window is responsible for storing the index. In this case,
    /// the tree-view control sends the parent a TVN_GETDISPINFO notification code to retrieve the index when it needs to display the image.
    /// </summary>
    public int iImage;

    /// <summary>
    /// Index in the tree-view control's image list of the icon image to use when the item is in the selected state.
    /// If this member is the I_IMAGECALLBACK value, the parent window is responsible for storing the index. In this case,
    /// the tree-view control sends the parent a TVN_GETDISPINFO notification code to retrieve the index when it needs to display the image.
    /// </summary>
    public int iSelectedImage;

    /// <summary>
    /// Flag that indicates whether the item has associated child items.
    /// </summary>
    public int cChildren;

    /// <summary>
    /// A value to associate with the item.
    /// </summary>
    public IntPtr lParam;

    /// <summary>
    /// Height of the item, in multiples of the standard item height (see TVM_SETITEMHEIGHT).
    /// For example, setting this member to 2 will give the item twice the standard height.
    /// The tree-view control does not draw in the extra area, which appears below the item content, but this space can be used by
    /// the application for drawing when using custom draw. Applications that are not using custom draw should set this value to 1,
    /// as otherwise the behavior is undefined.
    /// </summary>
    public int iIntegral;

    /// <summary>
    /// Initializes a new instance of <see cref="TvItemEx"/>
    /// </summary>
    /// <param name="mask">Array of flags that indicate which of the other structure members contain valid data.</param>
    /// <param name="hItem">Handle to the item.</param>
    /// <param name="iIntegral">Height of the item, in multiples of the standard item height (see TVM_SETITEMHEIGHT).</param>
    public TvItemEx(int mask, IntPtr hItem, int iIntegral)
    {
      lpszText = null;
      this.mask = mask;
      this.hItem = hItem;
      state = 0;
      stateMask = 0;
      cchTextMax = 0;
      iImage = 0;
      iSelectedImage = 0;
      cChildren = 0;
      lParam = new IntPtr();
      this.iIntegral = iIntegral;
    }
  }
}
