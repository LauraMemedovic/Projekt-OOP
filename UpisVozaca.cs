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
    public partial class UpisVozaca : Form
    {
        

        public UpisVozaca()
        {
            InitializeComponent();
        }

        public string imeVozaca = "Vozac";
        public int bodoviVozaca = 0;
        public int zivotiVozaca = 5;
        
        public string datoteka = "vozaci.txt";

        
        
        
        
        private void UpisVozaca_Load(object sender, EventArgs e)
        {
            if(!File.Exists(datoteka))
            {
                using (FileStream fs = File.Create(datoteka))
                {
                    Console.WriteLine("Datoteka je stvorena.");
                }
            }
            
        }
    

        private void Button1_Click(object sender, EventArgs e)
        {
            imeVozaca = textBox1.Text;
            if(imeVozaca!="")
            {
                if(imeVozaca.Contains(" "))
                {
                    MessageBox.Show("Ime mora biti jedna riječ!");
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Morate upisati ime.");
                return;
            }
            BGL bgl = new BGL(imeVozaca,bodoviVozaca,zivotiVozaca);
            BGL.allSprites.Clear();
            this.Hide();
            bgl.ShowDialog();







        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
