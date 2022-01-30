using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTTER
{
    public class Likovi : Sprite
    {
        protected string ime;
        //protected int bodovi;
        protected int brzina;
        protected int zivoti;

        public string Ime { get; set; }
        //public int Bodovi { get; set; }
        public int Brzina
        {
            get { return brzina; }
            set { brzina = value; }
        }
        public int Zivoti
        {
            get { return zivoti; }
            set { zivoti = value; }
        }

        public Likovi(string path,int xcor, int ycor) : base(path,xcor,ycor)
        {
            this.ime = "Vozac";
            //this.bodovi = 0;
            this.brzina = 10;
            this.zivoti = 3;
        }

    }

    public class Lik : Likovi
    {
        private int bodovi;
        private int maxbodovi;
        private int brtocnih,brnetocnih;

        public int BrojTocnih { get; set; }
        public int BrojNetocnih { get; set; }

        public int Bodovi { get; set; }
        public int MaxBodovi
        {
            get { return maxbodovi; }
            set { maxbodovi = value; }
        }
        public int Zivoti
        {
            get { return zivoti; }
            set
            {
                zivoti = value;
            }
        }
        public Lik(string path,int xcor, int ycor) : base(path,xcor,ycor)
        {
            this.bodovi = 0;
            this.maxbodovi = 0;
            this.brzina = 7;
            this.zivoti = 3;
            this.brtocnih = 0;
            this.brnetocnih = 0;

        }

        

        public override int X
        {
            get { return this.x; }
            
            set
            {
                if (value > GameOptions.RightEdge + this.Width)
                {
                    x = GameOptions.RightEdge + this.Width;
                }
                else if (value < GameOptions.LeftEdge)
                {
                    x = GameOptions.LeftEdge;
                }
                else
                {
                    x = value;
                }
            }
        }
        public override int Y
        {
            get { return this.y; }

            set
            {
                if (value > GameOptions.DownEdge + 250)
                {
                    y = GameOptions.DownEdge + 250;
                }
                else if (value < GameOptions.UpEdge)
                {
                    y = GameOptions.UpEdge;
                }
                else
                {
                    y = value;
                }
            }
        }


    }

    public class Znakovi : Sprite
    {
        protected int bodoviznaka;
        private int brzinaznaka;

        public int Bodoviznaka
        {
            get { return bodoviznaka; }
            set
            {
                bodoviznaka = value;
            }
        }
        public Znakovi(string path, int xcor, int ycor,int bodoviZnaka=10,int brzinaZnaka=20) : base(path,xcor,ycor)
        {
            this.bodoviznaka = bodoviZnaka;
            this.brzinaznaka = brzinaZnaka;
        }

        public override int Y
        {
            get { return base.Y; }
            set
            {
                if (value > GameOptions.DownEdge+150)
                {
                    this.y = 0;
                    this.X = g.Next(GameOptions.LeftEdge, GameOptions.RightEdge);

                }
                else
                {
                    base.Y = value;
                }
            }
        }
        Random g = new Random();
    }
    public class Osobe : Sprite
    {
        private int brzina;

        public Osobe(string path, int xcor, int ycor, int brzinaOsobe = 15) : base(path, xcor, ycor)
        {
            this.brzina = brzinaOsobe;
        }

        public override int X
        {
            get { return base.X; }
            set
            {
                if (value > GameOptions.RightEdge)
                {
                    this.x = 0;
                    this.Y = br.Next(GameOptions.UpEdge, GameOptions.DownEdge);
                }
                else
                {
                    base.X = value;
                }

            }
        }
        Random br = new Random();
    }





}
