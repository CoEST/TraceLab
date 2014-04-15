using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TraceLabSDK
{
    /// <summary>
    /// Provides information about TraceLab runtime and operating system.
    /// </summary>
    public class RuntimeInfo
    {
        /// <summary>
        /// Initializes the static properties of Application, in particular detects Mono runtime
        /// </summary>
        static RuntimeInfo()
        {
            // Detect if we're running in Mono - if not, then we can use WPF
            Type t = Type.GetType("Mono.Runtime");
            m_isRunInMono = (t != null);

            //Detect operating system
            if (m_isRunInMono)
            {
                if (IsRunningOnMac())
                {
                    m_operatingSystem = OS.Mac;
                }
                else if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    m_operatingSystem = OS.Linux;
                }
                else
                {
                    m_operatingSystem = OS.Windows;
                }
            }
            else
            {
                m_operatingSystem = OS.Windows;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this application is run in Mono runtime
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this application is run in mono; otherwise, <c>false</c>.
        /// </value>
        public static bool IsRunInMono
        {
            get { return m_isRunInMono; }
        }

        /// <summary>
        /// Gets the operating system MacOS, Linux or Windows.
        /// </summary>
        public static OS OperatingSystem
        {
            get { return m_operatingSystem; }
        }

        //From Managed.Windows.Forms/XplatUI
        [DllImport("libc")]
        static extern int uname(IntPtr buf);

        /// <summary>
        /// Determines whether TraceLab is running on Mac by checking uname output.
        /// On MacOS output of uname is Darwin. 
        /// 
        /// Method found in Pinta source code. Seems to be hacky, but works.
        /// Checking Environment.OSVersion.Platform return Unix for both Mac and Linux. 
        /// </summary>
        /// <returns>
        ///   <c>true</c> if TraceLab is running on Mac; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsRunningOnMac()
        {
            
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    string os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
            return false;
        }

        /// <summary>
        /// Operating System
        /// </summary>
        public enum OS
        {
            Windows,
            Mac,
            Linux
        }

        private static readonly OS m_operatingSystem;
        private static readonly bool m_isRunInMono;
    }
}
