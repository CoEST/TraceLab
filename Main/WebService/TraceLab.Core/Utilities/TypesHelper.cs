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

namespace TraceLab.Core.Utilities
{
    public class TypesHelper
    {
        public static string GetFriendlyName(string type) 
        {
            //check if there is character ` that determines the generic type 
            //for example
            //System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]

            string friendlyname;

            int offset = type.IndexOf("`");
            if (offset >= 0)
            {
                //generic type
                StringBuilder builder = new StringBuilder();

                string[] token = type.Split(new char[]{'`'}, 2); //split for type name and its generic arguments

                //get just type name
                builder.Append(GetTypeName(token[0]));
                builder.Append("<");

                int current = 1;
                var genericArguments = GetGenericArguments(token[1]);
                foreach (string genericArgument in genericArguments)
                {
                    builder.Append(GetFriendlyName(genericArgument));
                    if (current < genericArguments.Count) builder.Append(", ");
                    current++;
                }

                builder.Append(">");
                friendlyname = builder.ToString();
            }
            else
            {
                friendlyname = GetTypeName(type);
            }
            return friendlyname;
        }

        /// <summary>
        /// Gets the name of the basic types, not generic types.
        /// </summary>
        /// <param name="basicType">Type of the basic.</param>
        /// <returns></returns>
        private static string GetTypeName(string basicType)
        {
            //cut off assembly qualified info and return last part of type name
            int startOfQualifedName = basicType.IndexOf(',');
            if (startOfQualifedName > -1)
            {
                basicType = basicType.Remove(startOfQualifedName);
            }
            return basicType.Substring(basicType.LastIndexOf('.') + 1);
        }

        private static List<string> GetGenericArguments(string rawgenericargs)
        {
            //get number of generic arguments
            int n = int.Parse(rawgenericargs.Substring(0, 1)); //the first character tells how many generic parameters there are

            //parse generics - we can remove first and very last bracket and process what's inside
            string genericargs = rawgenericargs.Substring(rawgenericargs.IndexOf('[') + 1, rawgenericargs.LastIndexOf(']') - 1);

            List<string> args = new List<string>();

            Stack<char> bracketsStack = new Stack<char>();

            char[] arr = genericargs.ToCharArray();

            int index = 0;
            StringBuilder argbuilder = new StringBuilder();
            while(args.Count < n)
            {
                //push first bracket
                if (arr[index++] == '[') bracketsStack.Push('[');
                while (bracketsStack.Count > 0)
                {
                    //find index of closing ]
                    if (arr[index] == '[') bracketsStack.Push('[');
                    if (arr[index] == ']') bracketsStack.Pop(); //take one
                    if(bracketsStack.Count > 0) argbuilder.Append(arr[index]); //collect
                    index++;
                }
                //found closing index so add the arguments to list
                args.Add(argbuilder.ToString());
                argbuilder.Clear();
                index++; //to skip comma ','
            }
            return args;
        }
    }
}
