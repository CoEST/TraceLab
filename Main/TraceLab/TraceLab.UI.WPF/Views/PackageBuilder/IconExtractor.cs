// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TraceLab.UI.WPF.Views.PackageBuilder
{
    [Flags]
    enum SHGFI : int
    {
        /// <summary>get icon</summary>
        Icon = 0x000000100,
        /// <summary>get display name</summary>
        DisplayName = 0x000000200,
        /// <summary>get type name</summary>
        TypeName = 0x000000400,
        /// <summary>get attributes</summary>
        Attributes = 0x000000800,
        /// <summary>get icon location</summary>
        IconLocation = 0x000001000,
        /// <summary>return exe type</summary>
        ExeType = 0x000002000,
        /// <summary>get system icon index</summary>
        SysIconIndex = 0x000004000,
        /// <summary>put a link overlay on icon</summary>
        LinkOverlay = 0x000008000,
        /// <summary>show icon in selected state</summary>
        Selected = 0x000010000,
        /// <summary>get only specified attributes</summary>
        Attr_Specified = 0x000020000,
        /// <summary>get large icon</summary>
        LargeIcon = 0x000000000,
        /// <summary>get small icon</summary>
        SmallIcon = 0x000000001,
        /// <summary>get open icon</summary>
        OpenIcon = 0x000000002,
        /// <summary>get shell size icon</summary>
        ShellIconSize = 0x000000004,
        /// <summary>pszPath is a pidl</summary>
        PIDL = 0x000000008,
        /// <summary>use passed dwFileAttribute</summary>
        UseFileAttributes = 0x000000010,
        /// <summary>apply the appropriate overlays</summary>
        AddOverlays = 0x000000020,
        /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
        OverlayIndex = 0x000000040,
    }

    class IconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFileInfo(
          string pszPath,
          int dwFileAttributes,
          out    SHFILEINFO psfi,
          uint cbfileInfo,
          SHGFI uFlags);

        /// <summary>Maximal Length of unmanaged Windows-Path-strings</summary>
        private const int MAX_PATH = 260;
        /// <summary>Maximal Length of unmanaged Typename</summary>
        private const int MAX_TYPE = 80;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public SHFILEINFO(bool b)
            {
                hIcon = IntPtr.Zero;
                iIcon = 0;
                dwAttributes = 0;
                szDisplayName = "";
                szTypeName = "";
            }
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_TYPE)]
            public string szTypeName;
        };

        private IconExtractor()
        {
        }

        /// <summary>
        /// Get the associated Icon for a file or application, this method always returns
        /// an icon.  If the strPath is invalid or there is no idonc the default icon is returned
        /// </summary>
        /// <param name="strPath">full path to the file</param>
        /// <param name="bSmall">if true, the 16x16 icon is returned otherwise the 32x32</param>
        /// <returns></returns>
        public static Icon GetIcon(string strPath, bool bSmall)
        {
            SHFILEINFO info = new SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags;
            if (bSmall)
                flags = SHGFI.Icon | SHGFI.SmallIcon;// | SHGFI.UseFileAttributes;
            else
                flags = SHGFI.Icon | SHGFI.LargeIcon;// | SHGFI.UseFileAttributes;

            SHGetFileInfo(strPath, 256, out info, (uint)cbFileInfo, flags);
            return Icon.FromHandle(info.hIcon);
        }
    }
}
