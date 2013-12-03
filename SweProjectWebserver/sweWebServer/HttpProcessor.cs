using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;

namespace sweWebServer
{
    public class HttpProcessor
    {
        public TcpClient socket;
        public webServer srv;
        private HTTPHeader myHead;
        private StreamReader inputStream;
        public StreamWriter outputStream;
        private ArrayList _loadedPlugins;
    //    private int clicks = 0;
        //public NetworkStream ourNetworkStream;

        
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, webServer srv)
        {
            this.socket = s;
            this.srv = srv;
            myHead = new HTTPHeader();
        }


        public void process()
        {
            inputStream = new StreamReader(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
   
                pluginMngr PluginManager = new pluginMngr();
                _loadedPlugins = PluginManager.LoadPlugins("/plugins/", "*.dll", typeof(sweWebServer.IPlugins));

                bool tried = false;

                foreach (IPlugins plug in _loadedPlugins)
                {
                    //if (plug.pluginName == WebRequest.RequestedPlugin)
                        if (plug.pluginName == myHead.UrlSubStrings[1])
                    {
                        tried = true;
                        Console.WriteLine("requested plugin: " + plug.pluginName);

                        plug.handleRequest(outputStream, myHead);
                        //plug.doSomething();
                    }
                }


                if (tried == false)
                {
                    string html = @"
<html>
    <head>
        <title>SWE Webserver Gruppe SEVILMIS - KAVLAK</title>
    </head>
    <body>
        <h1>Choose Plugin</h1>
        <p><a href='http://localhost:8080/TemperaturePlugin'>TemperaturePlugin</a></p>
        <p><a href='http://localhost:8080/StatPlugin'>StatPlugin</a></p>
        <p><a href='http://localhost:8080/NavPlugin'>NavPlugin</a></p>
        <br>
        <p><a href='http://localhost:8080/'>Default</a></p>
    </body>
</html>";

                    outputStream.WriteLine("HTTP/1.1 200 OK");
                    outputStream.WriteLine("Content-Type: text/html");
                    outputStream.WriteLine("Content-Length: " + html.Length);
                    outputStream.WriteLine("Connection: close");
                    outputStream.Write(System.Environment.NewLine);
                    outputStream.WriteLine(html);
                }

               
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }

        public void parseRequest()
        {
            //String request = streamReadLine(inputStream);
            String request = inputStream.ReadLine();
            string[] tokens = null;
            try
            {
                tokens = request.Split(' ');

                if (tokens.Length != 3)
                {
                    throw new Exception("invalid http request line");
                }
                myHead.Http_method = tokens[0].ToUpper();
                myHead.Http_url = tokens[1];
                myHead.Http_protocol_versionstring = tokens[2];

                myHead.splitURL();

                Console.WriteLine("starting: " + request);
            }
            catch (Exception)
            {

            }
        }

        public void readHeaders()
        {
            Console.WriteLine("readHeaders()");
            String line;
            //while ((line = streamReadLine(inputStream)) != null)
                while ((line = inputStream.ReadLine()) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        
        private const int BUF_SIZE = 4096;
    

        public void writeSuccess(string content_type = "text/html")
        {
            outputStream.WriteLine("HTTP/1.1 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.1 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }
}
