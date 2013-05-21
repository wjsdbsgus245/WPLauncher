using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.IO;

using System.Net;

namespace WPLauncher
{
    public partial class Form1 : Form
    {
        Point LastMousePos;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text + "/bin/minecraft.jar"))
            {
                
                using (ZipFile zFile = new ZipFile(textBox1.Text + "/bin/minecraft.jar"))
                {
                    zFile.BeginUpdate();
                    try
                    {
                        zFile.Delete("META-INF/MANIFEST.MF");
                    }
                    catch
                    { };
                    try
                    {
                        zFile.Delete("META-INF/MOJANG_C.SF");
                    }
                    catch
                    { };
                    try
                    {
                        zFile.Delete("META-INF/MOJANG_C.DSA");
                    }
                    catch
                    { };

                    FileInfo[] files = null;
                    DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Klient\do minecraft.jar\");
                    string searchPattern = "*.*";
                    files = di.GetFiles(searchPattern, SearchOption.AllDirectories);
                    foreach (FileInfo f in files)
                    {
                        zFile.Add(f.FullName, f.FullName.Replace(Directory.GetCurrentDirectory() + @"\Klient\do minecraft.jar\", ""));
                    }
                    zFile.CommitUpdate();
                    zFile.Close();


                    //Now Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Klient\do .minecraft\", "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(Directory.GetCurrentDirectory() + @"\Klient\do .minecraft\", textBox1.Text + @"\" ));

                    
                    di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Klient\do .minecraft\");
                    files = di.GetFiles(searchPattern, SearchOption.AllDirectories);
                    
                    foreach (FileInfo f in files)
                    {
                        
                        File.Copy(f.FullName, textBox1.Text + f.FullName.Replace(Directory.GetCurrentDirectory() + @"\Klient\do .minecraft", ""), true);
                    }
                }
                MessageBox.Show("Zrobione!");
            }
            else 
            {
                MessageBox.Show("Błąd! Niepoprawna ścieżka do pliku lub folder nie zawiera minecraft.jar");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DesktopLocation = new Point(DesktopLocation.X + Cursor.Position.X - LastMousePos.X, DesktopLocation.Y + Cursor.Position.Y - LastMousePos.Y);
            }

            LastMousePos = Cursor.Position;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if(File.Exists("Klient.7z"))
            {
                File.Delete("Klient.7z");
            }

            progressBar1.Visible = true;

            WebClient client = new WebClient();
            
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            Uri remoteFile = new Uri("https://dl.dropboxusercontent.com/s/7y51nd0pcits718/Warpack%202.0%20pre2%20Klient.7z");
            client.DownloadFileAsync(remoteFile, "Klient.7z");

        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = (int)(((float)e.BytesReceived / 60000000.0f) * 100);
            if (e.BytesReceived == e.TotalBytesToReceive)
            {
                progressBar1.Visible = false;
                UnZip("Klient.zip");
            }
        }

        public static void UnZip(string SrcFile)
        {
            FastZip FZ = new FastZip();
            FZ.ExtractZip(SrcFile, "Klient", null);
        }

    }
}
