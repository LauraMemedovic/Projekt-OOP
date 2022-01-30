using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OTTER
{
    public partial class Kraj : Form
    {
        
        public Kraj()
        {
            InitializeComponent();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
        public string datoteka = "vozaci.txt";

        private void Kraj_Load(object sender, EventArgs e)
        {
            

            using (StreamReader sr = File.OpenText(datoteka))
            {
                string linija = sr.ReadLine();
                while(linija!=null)
                {
                    string[] popis = linija.Split('-');
                    string vozac = popis[0];
                    int bodovi = int.Parse(popis[1]);
                    if(bodovi!=0)
                    {
                        listBox1.Items.Add(linija + "\n");
                    }
                    linija = sr.ReadLine();
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
