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
using System.Xml.Serialization;

namespace TraceLab.Core.PackageSystem
{
    [Serializable]
    public class PackageReference : IXmlSerializable, TraceLabSDK.PackageSystem.IPackageReference
    {
        public PackageReference()
        {
        }

        public PackageReference(TraceLabSDK.PackageSystem.IPackage package)
        {
            ID = package.ID;
            Name = package.Name;
        }

        public string Name
        {
            get;
            private set;
        }

        public string ID
        {
            get;
            private set;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            ID = reader.GetAttribute("ID");
            Name = reader.GetAttribute("Name");
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("ID", ID);
            writer.WriteAttributeString("Name", Name);
        }

        #endregion

        public override bool Equals(object obj)
        {
            bool equals = false;
            PackageReference other = obj as PackageReference; 
            if (other != null)
            {
                if (string.Equals(ID, other.ID, StringComparison.InvariantCultureIgnoreCase))
                {
                    equals = string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return equals;
        }

        public override int GetHashCode()
        {
            return (ID + Name).GetHashCode();
        }
    }
}
