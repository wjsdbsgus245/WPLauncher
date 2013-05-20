using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace WPLauncher
{
    public partial class Form1 : Form
    {
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
                    DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\do minecraft.jar\");
                    string searchPattern = "*.*";
                    files = di.GetFiles(searchPattern, SearchOption.AllDirectories);
                    foreach (FileInfo f in files)
                    {
                        zFile.Add(f.FullName, f.FullName.Replace(Directory.GetCurrentDirectory() + @"\do minecraft.jar\", ""));
                    }
                    zFile.CommitUpdate();
                    zFile.Close();


                    //Now Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\do .minecraft\", "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(Directory.GetCurrentDirectory() + @"\do .minecraft\", textBox1.Text + @"\" ));

                    
                    di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\do .minecraft\");
                    files = di.GetFiles(searchPattern, SearchOption.AllDirectories);
                    
                    foreach (FileInfo f in files)
                    {
                        
                        File.Copy(f.FullName, textBox1.Text + f.FullName.Replace(Directory.GetCurrentDirectory() + @"\do .minecraft", ""), true);
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
    }
}
