using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using WPLauncher;
using System.Threading;

namespace MCLoginLib
{
    public class Login
    {
        static WebRequest req;
        public static string[] Session = null;

        public static void generateSession(string username, string password, int clientVer)
        {
            httpGET("https://login.minecraft.net?user=" + username + "&password=" + password + "&version=" + clientVer);
        }

        public static void httpGET(string URI)
        {
            req = WebRequest.Create(URI);
            //WebResponse resp = req.GetResponse();
            req.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
        }

        static void FinishWebRequest(IAsyncResult result)
        {
            WebResponse resp = req.EndGetResponse(result);
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            Session = sr.ReadToEnd().Trim().Split(':');
        }


        public static void startMinecraft(bool mode, int ramMin, int ramMax, string username, string sessionID, bool debug, string mcPatch)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
            string appData = mcPatch + @"\";
            Process proc = new Process();
            if (debug == true)
            {
                proc.StartInfo.FileName = "java";
            }
            else
            {
                proc.StartInfo.FileName = "javaw";
            }

            if (mode == true)
            {
                proc.StartInfo.Arguments = "-Xms" + ramMin + "M -Xmx" + ramMax + "M -Djava.library.path=" + appData + "bin\\natives -cp " + appData + "bin\\minecraft.jar;" + appData + "bin\\jinput.jar;" + appData + "bin\\lwjgl.jar;" + appData + "bin\\lwjgl_util.jar net.minecraft.client.Minecraft " + username + " " + sessionID;
            }
            else
            {
                proc.StartInfo.Arguments = "-Xms" + ramMin + "M -Xmx" + ramMax + "M -Djava.library.path=" + appData + "bin/natives -cp " + appData + "bin/minecraft.jar;" + appData + "bin/jinput.jar;" + appData + "bin/lwjgl.jar;" + appData + "bin/lwjgl_util.jar net.minecraft.client.Minecraft " + username;
            }
            proc.Start();
        }
    }
}