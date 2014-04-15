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

namespace TraceLab.Core.Workspaces.Serialization
{
    internal sealed class FileWrapperStream : System.IO.Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileWrapperStream"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentException">Thrown if path is either null, empty, or not set to a full rooted path.</exception>
        /// <exception cref="System.IO.IOException">Thrown if an error occurs while attempting to open the path for reading or writing.</exception>
        public FileWrapperStream(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path cannot be null");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty", "path");
            }
            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Path contains illegal characters.", "path");
            }
            if (System.IO.Path.IsPathRooted(path) == false)
            {
                throw new ArgumentException("FileWrapper Path must be absolute", "path");
            }
            if (System.IO.Path.GetFileName(path).IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("File name contains illegal characters.", "path");
            }


            Path = path;

            Buffer = new MemoryStream();
            System.IO.FileStream stream = null;
            try
            {
                stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);

                // Pull the entire file into memory
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Buffer.Write(data, 0, data.Length);
                IsAwaitingDiskFlush = false;
                Position = 0;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

        }

        public string Path
        {
            get;
            private set;
        }

        private MemoryStream Buffer
        {
            get;
            set;
        }

        public override bool CanRead
        {
            get { return Buffer.CanRead; }
        }

        public override bool CanSeek
        {
            get { return Buffer.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return Buffer.CanWrite; }
        }

        public bool IsAwaitingDiskFlush
        {
            get;
            private set;
        }

        /// <summary>
        /// Flushes the buffer if it has been modified.
        /// </summary>
        public override void Flush()
        {
            Flush(false);
        }

        /// <summary>
        /// Flushes the buffer if it has been modified.
        /// </summary>
        /// <param name="toDisk">True to flush the buffer to disk</param>
        /// <remarks>
        /// There is not much reason to call this function without passing in True - it is mainly provided
        /// so that the default behavior of Flush() can mimic that of a MemoryStream and do nothing.  This prevents
        /// constant writing to disk when using StreamWriters.
        /// </remarks>
        public void Flush(bool toDisk)
        {
            // Only dump to disk if we've actually changed the data.
            if (IsAwaitingDiskFlush == true && toDisk)
            {
                using (FileStream outputStream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    long pos = Position;
                    Position = 0;
                    Buffer.WriteTo(outputStream);
                    Position = pos;

                    IsAwaitingDiskFlush = false;
                }
            }
        }


        public override long Length
        {
            get { return Buffer.Length; }
        }

        public override long Position
        {
            get
            {
                return Buffer.Position;
            }
            set
            {
                Buffer.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Buffer.Read(buffer, offset, count);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return Buffer.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            Buffer.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Buffer.Write(buffer, offset, count);
            IsAwaitingDiskFlush = true;
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
            IsAwaitingDiskFlush = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Flush(true);
                Buffer.Dispose();
            }
        }
    }
}
