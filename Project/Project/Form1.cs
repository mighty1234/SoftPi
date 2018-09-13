using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Project
{
    public partial class Form1 : Form
    {
        Stopwatch timer = new Stopwatch();
        public Form1()
        {
            InitializeComponent();       
        }

        private void Start_Click(object sender, EventArgs e)
        {
            int iterator = 0;


             GetData.GetLocation("178.255.215.79");
           // Whois.Lookup("178.255.215.79");
            //using (FileStream fs = new FileStream(@"C:\Users\Vlad\Desktop\access_log", FileMode.Open, FileAccess.Read))
            //{
            //    using (StreamReader sr = new StreamReader(fs))
            //    {
            //        timer.Start();
            //        List<string> buffer =new List<string>();          
            //        while (!sr.EndOfStream)
            //        {
            //            var x = sr.ReadLine();
            //           buffer.Add(x);
            //            iterator++;
            //            if (iterator == 15)
            //            {
            //                iterator = 0;
            //                Save(buffer);

            //                // AsyncHelper.RunAsyncOperation(() => Save(buffer));
            //                buffer.Clear();
            //            }
            //        }
            //        timer.Stop();
            //        MessageBox.Show((timer.ElapsedMilliseconds / 1000.0).ToString());
            //        timer.Reset();


            //    }

            //}
            //if (buffer.Count!=0)
            //{
            //Save(buffer);
            //buffer.Clear();           
            //}
        }
        
        }

    }
    





