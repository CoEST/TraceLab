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
//
//TLAB-67 TLAB-68 TLAB-69
//Herzum - TraceLab Challenge
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace TraceLab.Core.Utilities
{
    public static class Crypto
    {
        private static string key = "J?&H?I?3";

        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory (IntPtr Destination, int Length);
        // Function to Generate a 64 bits Key.
        static string GenerateKey ()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create ();
            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString (desCrypto.Key);
        }

        public static bool EncryptFile (Stream streamToEncrypt, string sOutputFilename) {           
            try {
                FileStream fsEncrypted = new FileStream (sOutputFilename,
                                                         FileMode.Create,
                                                         FileAccess.Write);
                
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider ();
                DES.Key = ASCIIEncoding.ASCII.GetBytes (key);
                DES.IV = ASCIIEncoding.ASCII.GetBytes (key);
                ICryptoTransform desencrypt = DES.CreateEncryptor ();
                
                CryptoStream cryptostream = new CryptoStream (fsEncrypted,
                                                              desencrypt,
                                                              CryptoStreamMode.Write);
                MemoryStream outputStream = streamToEncrypt as MemoryStream;
                
                // Set the position to the beginning of the stream.
                outputStream.Seek (0, SeekOrigin.Begin);
                byte[] byteArray = new byte[outputStream.Length];
                int count = 0; 
                
                while (count < outputStream.Length) {
                    byteArray [count++] = Convert.ToByte (outputStream.ReadByte ());
                }
                
                cryptostream.Write (byteArray, 0, byteArray.Length);
                cryptostream.Close ();
                fsEncrypted.Close ();

            } catch (Exception ex) {
                return false;
            } 
     
            return true;
        }

        public static Stream DecryptFile (string sInputFilename, string sOutputFilename) {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider ();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes (key);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes (key);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream (sInputFilename,
                                               FileMode.Open,
                                               FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor ();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream (fsread,
                                                             desdecrypt,
                                                             CryptoStreamMode.Read);
            
            MemoryStream memoryFile;
            string s = new StreamReader (cryptostreamDecr).ReadToEnd ();
        
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
            Byte[] bytes = encoding.GetBytes (s);

            memoryFile = new MemoryStream (bytes);
            return memoryFile;
        }     
    }
}