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
using System.IO;
using System.Security.Permissions;

namespace TraceLab.Core.Workspaces.Serialization
{
    [Serializable]
    public class StreamManager : MarshalByRefObject
    {
        private Dictionary<string, Stream> m_streams;

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        private StreamManager()
        {
            m_streams = new Dictionary<string, Stream>();
        }

        static readonly object padlock = new object();

        public static StreamManager CreateNewManager()
        {
            return new StreamManager();
        }

        /// <summary>
        /// Returns the stream associated with the given path. Writes the cache to disc.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A stream</returns>
        /// <exception cref="System.ArgumentException">Thrown if path is either null, empty, or not set to a full rooted path.</exception>
        /// <exception cref="System.IO.IOException">Thrown if an error occurs while attempting to open the path for reading or writing.</exception>
        public Stream GetStream(string path)
        {
            return GetStream(path, false);
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="isTemporary">if set to <c>true</c> the return stream is a memory stream, and not file stream. The stream is not write to disc</param>
        /// <returns></returns>
        public Stream GetStream(string path, bool isTemporary)
        {
            ValidatePath(path);
          
            lock (m_streams)
            {
                if (!m_streams.ContainsKey(path))
                {
                    if (isTemporary)
                    {
                        m_streams.Add(path, new MemoryStream());
                    }
                    else
                    {
                        m_streams.Add(path, new FileWrapperStream(path));
                    }
                }
                return m_streams[path];
            }
        }

        private void ValidatePath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "Path cannot be null");
            if (path.Length == 0)
                throw new ArgumentException("Path cannot be empty", "path");
            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
                throw new ArgumentException("Path contains illegal characters.", "path");
            if (System.IO.Path.IsPathRooted(path) == false)
                throw new ArgumentException("FileWrapper Path must be absolute", "path");
            if (System.IO.Path.GetFileName(path).IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException("File name contains illegal characters.", "path");
        }

        /// <summary>
        /// Moves the stream within the stream manager. If old path does not exist nothing happens
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        public void MoveStream(string oldPath, string newPath)
        {
            ValidatePath(oldPath);
            ValidatePath(newPath);

            lock (m_streams)
            {
                Stream stream;
                if (m_streams.TryGetValue(oldPath, out stream))
                {
                    m_streams.Remove(oldPath);

                    Stream streamToBeOverriden;
                    if (m_streams.TryGetValue(newPath, out streamToBeOverriden))
                    {
                        streamToBeOverriden.Close();
                        if (File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }
                        m_streams[newPath] = stream;
                    }
                    else
                    {
                        m_streams.Add(newPath, stream);
                    }
                    //rename data files to the new files names
                    if (File.Exists(oldPath))
                    {
                        File.Move(oldPath, newPath);
                    }
                }
            }
        }

        public void CloseStream(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("Stream path must be valid", "path");

            lock (m_streams)
            {
                if (m_streams.ContainsKey(path))
                {
                    m_streams[path].Close();
                    m_streams.Remove(path);
                }
            }
        }


        public void Flush(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("Stream path must be valid", "path");

            lock (m_streams)
            {
                Stream stream;
                m_streams.TryGetValue(path, out stream);
                if (stream == null)
                {
                    throw new ArgumentException("StreamManager doesn't contain the stream of the given path " + path + "!");
                }
                FlushStream(stream);
            }
        }

        public void FlushAll()
        {
            lock (m_streams)
            {
                foreach (Stream stream in m_streams.Values)
                {
                    if (stream != null)
                    {
                        FlushStream(stream);
                    }
                }
            }
        }

        public void Clear()
        {
            lock (m_streams)
            {
                m_streams.Clear();
            }
        }

        public bool IsEmpty()
        {
            lock (m_streams)
            {
                return m_streams.Count.Equals(0);
            }
        }

        private static void FlushStream(Stream stream)
        {
            var wrapper = stream as FileWrapperStream;
            if (wrapper != null)
            {
                wrapper.Flush(true);
            }
            else
            {
                stream.Flush();
            }
        }
    }
}
