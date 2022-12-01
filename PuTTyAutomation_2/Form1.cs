using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace PuTTyAutomation_2
{
    public partial class Form1 : Form
    {
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        //Thread readThread;


        public Form1()
        {
            InitializeComponent();
           //Console.WriteLine("yoyo");
        }

        System.IO.Ports.SerialPort sport;


        // Thread readThread = new Thread(Read);
        string message;
        private void button1_Click(object sender, EventArgs e)
        {
            sport = new System.IO.Ports.SerialPort(textBox1.Text, 9600,
                                                                                System.IO.Ports.Parity.None,
                                                                                8,
                                                                                System.IO.Ports.StopBits.One);

            /* System.Diagnostics.Process cmd = new System.Diagnostics.Process();

            cmd.StartInfo.FileName = @"C:\Program Files\PuTTY\putty.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true; */ 

            try
            {
                sport.Open();

                Passwords();
                sport.WriteLine("en");
                Passwords();

                message = sport.ReadLine();
                
                if (stringComparer.Equals("% Bad Secrets", message))
                {
                    MessageBox.Show("Must Reset the password");
                   // UnknownRouterPwd();
                }

                else
                {
                    SimpleCommands();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }

            sport.Close();

        }

        private void SimpleCommands()
        {
            sport.WriteLine("wr er");
            SendKeys.Send("{ENTER}");
            //Thread.Sleep(1000);
            SendKeys.Send("{ENTER}");
            sport.WriteLine("\nreload");
            sport.WriteLine("n");
            sport.WriteLine("\nreload");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
           // SendKeys.Send("{ENTER}");
        }

        private void Passwords()
        {
            sport.WriteLine("cisco");
            sport.WriteLine("{ENTER}");
            sport.WriteLine("class");
        }

        private void UnknownRouterPwd()
        {
            SendKeys.Send("BREAK");
            SendKeys.Send("BREAK");
            SendKeys.Send("BREAK");
            sport.WriteLine("\nconfreg 0x2142");
            sport.WriteLine("reset");
            Thread.Sleep(360000);
            sport.WriteLine("en");
            sport.WriteLine("config t");
            sport.WriteLine("config-register 0x2102");
            sport.WriteLine("exit");
            sport.WriteLine("wr er");
            SendKeys.Send("ENTER");
            Thread.Sleep(1000);
            sport.WriteLine("\nwr memory");
            Thread.Sleep(1000);
            sport.WriteLine("\nreload");

            sport.Close();

        }

         private void button2_Click(object sender, EventArgs e)
        {

            sport = new System.IO.Ports.SerialPort(textBox1.Text, 9600,
                                                                               System.IO.Ports.Parity.None,
                                                                               8,
                                                                               System.IO.Ports.StopBits.One);

            // router reset pass
            sport.Open();
            UnknownRouterPwd();
            sport.Close();
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sport = new System.IO.Ports.SerialPort(textBox1.Text, 9600,
                                                                               System.IO.Ports.Parity.None,
                                                                               8,
                                                                               System.IO.Ports.StopBits.One);

            // switch pass reset 
            sport.Open();

            MessageBox.Show("Restart the switch and hold the button until all lights stop flashing.");
            sport.WriteLine("flash_init");
            sport.WriteLine("dir flash:");

            /*extracting all files with .text extension and deleting them at once
            string[] filesToDelete = Directory.GetFiles("dir flash:", "*.text");
            filesToDelete.ToList().ForEach(file => File.Delete(file)); */

            sport.Write("delete flash:config.text");
            sport.Write("y");
            Thread.Sleep(1000);
            sport.WriteLine("dir flash:");
            sport.WriteLine("boot");

            sport.Close(); 
        } 
    } 
}
