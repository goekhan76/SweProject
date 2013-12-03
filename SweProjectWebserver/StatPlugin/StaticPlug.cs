using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using sweWebServer;

namespace StatPlugin
{
    public class StatP : IPlugins
    {
        private string mypluginName = "StatPlugin";

        public void handleRequest(StreamWriter writeOut, HTTPHeader head)
        {
            if (head.UrlSubStrings.Count() == 2)
            {
                string[] filePaths = Directory.GetFiles(".");

                string files = "";

                foreach (string s in filePaths)
                {
                    files += "<tr><td><a href='StatPlugin/" + s.Substring(2) + "'>" + s.Substring(2) + "</a></td></tr>";
                }

                string html = @"
<html>
    <head>
        <title>swe WebServer SEVILMIS - Kavlak</title>
    </head>
    <body>
        <h1>StatPlugin</h1>
        <h1>Choose your file</h1>
        <p>html, xml, plaintext, png, jpeg Unterstuetzung<br>Andere Formate fuehren zu einem Download</p>
        <table border='1'>" +
            files +
        @"</table>
        <br>
        <p><a href='http://localhost:8080/'>Default</a></p>
    </body>
</html>";

                writeOut.WriteLine("HTTP/1.1 200 OK");
                writeOut.WriteLine("Content-Type: text/html");
                writeOut.WriteLine("Content-Length: " + html.Length);
                writeOut.WriteLine("Connection: close");
                writeOut.Write(System.Environment.NewLine);
                writeOut.WriteLine(html);
                writeOut.Flush();
            }

            else
            {
                byte[] file;
                FileStream fileStream = new FileStream(head.UrlSubStrings[2], FileMode.Open, FileAccess.Read);
                
                
                file = new byte[fileStream.Length];

                fileStream.Read(file, 0, Convert.ToInt32(fileStream.Length));



                string[] fileparts = head.UrlSubStrings[2].Split('.');

                if (fileparts[fileparts.Length - 1] == "jpeg" || fileparts[fileparts.Length - 1] == "jpg")
                {
                    //jpeg
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: image/jpeg");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
                else if (fileparts[fileparts.Length - 1] == "png")
                {
                    //png
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: image/png");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
                else if (fileparts[fileparts.Length - 1] == "gif")
                {
                    //gif
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: image/gif");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
                else if (fileparts[fileparts.Length - 1] == "html" || fileparts[fileparts.Length - 1] == "htm" || fileparts[fileparts.Length - 1] == "xhtml")
                {
                    //html
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: text/html");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
                else if (fileparts[fileparts.Length - 1] == "xml")
                {
                    //xml
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: text/xml");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
                else if (fileparts[fileparts.Length - 1] == "txt" || fileparts[fileparts.Length - 1] == "ini" || fileparts[fileparts.Length - 1] == "config")
                {
                    //rawtext
                    writeOut.WriteLine("HTTP/1.1 200 OK");
                    writeOut.WriteLine("Content-Type: text/plain");
                    writeOut.WriteLine("Content-Length: " + file.Length);
                    writeOut.WriteLine("Connection: close");
                }
               // writeOut.BaseStream.Write(file, 0, file.Length);
                writeOut.Write(System.Environment.NewLine);
                writeOut.Flush();
                writeOut.BaseStream.Write(file, 0, file.Length);
                
                
                //memory.Close();
            }

        }

        public string pluginName
        {
            get
            {
                return mypluginName;
            }
        }
    }
}
