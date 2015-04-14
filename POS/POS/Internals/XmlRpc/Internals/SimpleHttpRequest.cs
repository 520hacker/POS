namespace Rpc.Internals
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net.Sockets;

    ///<summary>Very basic HTTP request handler.</summary>
    ///<remarks>This class is designed to accept a TcpClient and treat it as an HTTP request.
    /// It will do some basic header parsing and manage the input and output streams associated
    /// with the request.</remarks>
    internal class SimpleHttpRequest
    {
        private String _httpMethod = null;
        private String _filePathFile = null;
        private String _filePathDir = null;
        private String __filePath;
        private Hashtable _headers;

        /// <summary>A constructor which accepts the TcpClient.</summary>
        /// <remarks>It creates the associated input and output streams, determines the request type,
        /// and parses the remaining HTTP header.</remarks>
        /// <param name="client">The <c>TcpClient</c> associated with the HTTP connection.</param>
        public SimpleHttpRequest(TcpClient client)
        {
            this.Client = client;
            this.Output = new StreamWriter(client.GetStream());
            this.Input = new StreamReader(client.GetStream());
            this.GetRequestMethod();
            this.GetRequestHeaders();
        }

        /// <summary>The output <c>StreamWriter</c> associated with the request.</summary>
        public StreamWriter Output { get; private set; }

        /// <summary>The input <c>StreamReader</c> associated with the request.</summary>
        public StreamReader Input { get; private set; }

        /// <summary>The <c>TcpClient</c> with the request.</summary>
        public TcpClient Client { get; private set; }

        /// <summary>The type of HTTP request (i.e. PUT, GET, etc.).</summary>
        public String HttpMethod 
        {
            get
            {
                return this._httpMethod;
            }
        }

        /// <summary>The level of the HTTP protocol.</summary>
        public String Protocol { get; private set; }

        /// <summary>The "path" which is part of any HTTP request.</summary>
        public String FilePath
        {
            get
            {
                return this._filePath;
            }
        }

        /// <summary>The file portion of the "path" which is part of any HTTP request.</summary>
        public String FilePathFile
        {
            get
            {
                if (this._filePathFile != null)
                {
                    return this._filePathFile;
                }

                int i = this.FilePath.LastIndexOf("/");

                if (i == -1)
                {
                    return "";
                }
	    
                i++;
                this._filePathFile = this.FilePath.Substring(i, this.FilePath.Length - i);
                return this._filePathFile;
            }
        }

        /// <summary>The directory portion of the "path" which is part of any HTTP request.</summary>
        public String FilePathDir
        {
            get
            {
                if (this._filePathDir != null)
                {
                    return this._filePathDir;
                }

                int i = this.FilePath.LastIndexOf("/");

                if (i == -1)
                {
                    return "";
                }
	    
                i++;
                this._filePathDir = this.FilePath.Substring(0, i);
                return this._filePathDir;
            }
        }

        private String _filePath
        {
            get
            {
                return this.__filePath;
            }
            set 
            {
                this.__filePath = value;
                this._filePathDir = null;
                this._filePathFile = null;
            }
        }

        /// <summary>
        /// Format the object contents into a useful string representation.
        /// </summary>
        ///<returns><c>String</c> representation of the <c>SimpleHttpRequest</c> as the <i>HttpMethod FilePath Protocol</i>.</returns>
        override public String ToString()
        {
            return string.Format("{0} {1} {2}", this.HttpMethod, this.FilePath, this.Protocol);
        }

        /// <summary>
        /// Close the <c>SimpleHttpRequest</c>. This flushes and closes all associated io streams.
        /// </summary>
        public void Close()
        {
            this.Output.Flush();
            this.Output.Close();
            this.Input.Close();
            this.Client.Close();
        }

        private void GetRequestMethod()
        {
            string req = this.Input.ReadLine();
            if (req == null)
            {
                throw new ApplicationException("Void request.");
            }

            if (0 == String.Compare("GET ", req.Substring(0, 4), true))
            {
                this._httpMethod = "GET";
            }
            else if (0 == String.Compare("POST ", req.Substring(0, 5), true))
            {
                this._httpMethod = "POST";
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unrecognized method in query: {0}", req));
            }

            req = req.TrimEnd();
            int idx = req.IndexOf(' ') + 1;
            if (idx >= req.Length)
            {
                throw new ApplicationException("What do you want?");
            }

            string page_protocol = req.Substring(idx);
            int idx2 = page_protocol.IndexOf(' ');
            if (idx2 == -1)
            {
                idx2 = page_protocol.Length;
            }
		
            this._filePath = page_protocol.Substring(0, idx2).Trim();
            this.Protocol = page_protocol.Substring(idx2).Trim();
        }

        private void GetRequestHeaders()
        {
            String line;
            int idx;

            this._headers = new Hashtable();

            while ((line = this.Input.ReadLine()) != "") 
            {
                if (line == null)
                {
                    break;
                }

                idx = line.IndexOf(':');
                if (idx == -1 || idx == line.Length - 1)
                {
                    Logger.WriteEntry(string.Format("Malformed header line: {0}", line), LogLevel.Information);
                    continue;
                }

                String key = line.Substring(0, idx);
                String value = line.Substring(idx + 1);

                try 
                {
                    this._headers.Add(key, value);
                }
                catch (Exception) 
                {
                    Logger.WriteEntry(string.Format("Duplicate header key in line: {0}", line), LogLevel.Information);
                }
            }
        }
    }
}