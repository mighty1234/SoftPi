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
          List<string> buffer = new List<string>();
        Stopwatch timer = new Stopwatch();
        public Form1()
        {
            InitializeComponent();       
        }

        private void Start_Click(object sender, EventArgs e)
        {
            int iterator = 0;


           
            // GetData.GetLocation("178.255.215.79");
            List<string> buffer = new List<string>();
            using (FileStream fs = new FileStream(@"C:\Users\Vlad\Desktop\tariscope.com.log", FileMode.Open, FileAccess.Read))
            {
           
                using (StreamReader sr = new StreamReader(fs))
                {
                    timer.Start();
                   

                    while (!sr.EndOfStream)
                    {
                        var x = sr.ReadLine();
                        buffer.Add(x);
                        iterator++;

                     
                        if (iterator == 10)
                        {
                            iterator = 0;
                            GetData.Save(buffer);                         

                
                         
                                                       // AsyncHelper.RunAsyncOperation(() => Save(buffer));

                            buffer.Clear();
                        }
                    }
                    timer.Stop();
                    MessageBox.Show((timer.ElapsedMilliseconds / 1000.0).ToString());
                    timer.Reset();


                }

            }
            if (buffer.Count != 0)
            {
                        

                Save(buffer);

                buffer.Clear();
            }
        }

        public void Save(List<string> logModels)
        {
            LogModel n = new LogModel();
            foreach (var x in logModels)
            {
                n = (GetData.ParseToModel(x));
                if (n.Isvalid == false)
                {
                    n = null;
                }
                else
                {
                    GetData.LoadToDB(n);
                }
            }
        }
    }
    

    }
    





