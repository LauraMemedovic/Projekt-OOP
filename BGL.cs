using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OTTER
{
    /// <summary>
    /// -
    /// </summary>
    public partial class BGL : Form
    {
        /* ------------------- */
        

        #region Environment Variables

        List<Func<int>> GreenFlagScripts = new List<Func<int>>();

        /// <summary>
        /// Uvjet izvršavanja igre. Ako je <c>START == true</c> igra će se izvršavati.
        /// </summary>
        /// <example><c>START</c> se često koristi za beskonačnu petlju. Primjer metode/skripte:
        /// <code>
        /// private int MojaMetoda()
        /// {
        ///     while(START)
        ///     {
        ///       //ovdje ide kod
        ///     }
        ///     return 0;
        /// }</code>
        /// </example>
        public static bool START = true;

        //public int Bodovi;
        //public bool Kraj = false;
        //public bool UpisVozaca = false;


        //sprites
        /// <summary>
        /// Broj likova.
        /// </summary>
        public static int spriteCount = 0, soundCount = 0;

        /// <summary>
        /// Lista svih likova.
        /// </summary>
        //public static List<Sprite> allSprites = new List<Sprite>();
        public static SpriteList<Sprite> allSprites = new SpriteList<Sprite>();

        //sensing
        int mouseX, mouseY;
        Sensing sensing = new Sensing();

        //background
        List<string> backgroundImages = new List<string>();
        int backgroundImageIndex = 0;
        string ISPIS = "";

        SoundPlayer[] sounds = new SoundPlayer[1000];
        TextReader[] readFiles = new StreamReader[1000];
        TextWriter[] writeFiles = new StreamWriter[1000];
        bool showSync = false;
        int loopcount;
        DateTime dt = new DateTime();
        String time;
        double lastTime, thisTime, diff;

        #endregion
        /* ------------------- */
        #region Events

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            try
            {
                foreach (Sprite sprite in allSprites)
                {
                    if (sprite != null)
                        if (sprite.Show == true)
                        {
                            g.DrawImage(sprite.CurrentCostume, new Rectangle(sprite.X, sprite.Y, sprite.Width, sprite.Heigth));
                        }
                    if (allSprites.Change)
                        break;
                }
                if (allSprites.Change)
                    allSprites.Change = false;
            }
            catch
            {
                //ako se doda sprite dok crta onda se mijenja allSprites
                MessageBox.Show("Greška!");
            }
        }

        private void startTimer(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            Init();
        }

        private void updateFrameRate(object sender, EventArgs e)
        {
            updateSyncRate();
        }

        /// <summary>
        /// Crta tekst po pozornici.
        /// </summary>
        /// <param name="sender">-</param>
        /// <param name="e">-</param>
        public void DrawTextOnScreen(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var brush = new SolidBrush(Color.WhiteSmoke);
            string text = ISPIS;

            SizeF stringSize = new SizeF();
            Font stringFont = new Font("Arial", 14);
            stringSize = e.Graphics.MeasureString(text, stringFont);

            using (Font font1 = stringFont)
            {
                RectangleF rectF1 = new RectangleF(0, 0, stringSize.Width, stringSize.Height);
                e.Graphics.FillRectangle(brush, Rectangle.Round(rectF1));
                e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
            }
        }

        private void mouseClicked(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = false;
            sensing.MouseDown = false;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            //sensing.MouseX = e.X;
            //sensing.MouseY = e.Y;
            //Sensing.Mouse.x = e.X;
            //Sensing.Mouse.y = e.Y;
            sensing.Mouse.X = e.X;
            sensing.Mouse.Y = e.Y;

        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            sensing.Key = e.KeyCode.ToString();
            sensing.KeyPressedTest = true;
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            sensing.Key = "";
            sensing.KeyPressedTest = false;
        }

        private void Update(object sender, EventArgs e)
        {
            if (sensing.KeyPressed(Keys.Escape))
            {
                START = false;
            }

            if (START)
            {
                this.Refresh();
            }
        }

        #endregion
        /* ------------------- */
        #region Start of Game Methods

        //my
        #region my

        //private void StartScriptAndWait(Func<int> scriptName)
        //{
        //    Task t = Task.Factory.StartNew(scriptName);
        //    t.Wait();
        //}

        //private void StartScript(Func<int> scriptName)
        //{
        //    Task t;
        //    t = Task.Factory.StartNew(scriptName);
        //}

        private int AnimateBackground(int intervalMS)
        {
            while (START)
            {
                setBackgroundPicture(backgroundImages[backgroundImageIndex]);
                Game.WaitMS(intervalMS);
                backgroundImageIndex++;
                if (backgroundImageIndex == 3)
                    backgroundImageIndex = 0;
            }
            return 0;
        }

        private void KlikNaZastavicu()
        {
            foreach (Func<int> f in GreenFlagScripts)
            {
                Task.Factory.StartNew(f);
            }
        }

        #endregion

        /// <summary>
        /// BGL
        /// </summary>
        public BGL()
        {
            InitializeComponent();
        }

        public BGL(string imevozaca, int bodovivozaca, int zivotivozaca)
        {
            InitializeComponent();
            this.imev = imevozaca;
            this.bodovi = bodovivozaca;
            this.zivoti = zivotivozaca;

        }



        /// <summary>
        /// Pričekaj (pauza) u sekundama.
        /// </summary>
        /// <example>Pričekaj pola sekunde: <code>Wait(0.5);</code></example>
        /// <param name="sekunde">Realan broj.</param>
        public void Wait(double sekunde)
        {
            int ms = (int)(sekunde * 1000);
            Thread.Sleep(ms);
        }

        //private int SlucajanBroj(int min, int max)
        //{
        //    Random r = new Random();
        //    int br = r.Next(min, max + 1);
        //    return br;
        //}

        /// <summary>
        /// -
        /// </summary>
        public void Init()
        {
            if (dt == null) time = dt.TimeOfDay.ToString();
            loopcount++;
            //Load resources and level here
            this.Paint += new PaintEventHandler(DrawTextOnScreen);
            SetupGame();
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="val">-</param>
        public void showSyncRate(bool val)
        {
            showSync = val;
            if (val == true) syncRate.Show();
            if (val == false) syncRate.Hide();
        }

        /// <summary>
        /// -
        /// </summary>
        public void updateSyncRate()
        {
            if (showSync == true)
            {
                thisTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                diff = thisTime - lastTime;
                lastTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

                double fr = (1000 / diff) / 1000;

                int fr2 = Convert.ToInt32(fr);

                syncRate.Text = fr2.ToString();
            }

        }

        //stage
        #region Stage

        /// <summary>
        /// Postavi naslov pozornice.
        /// </summary>
        /// <param name="title">tekst koji će se ispisati na vrhu (naslovnoj traci).</param>
        public void SetStageTitle(string title)
        {
            this.Text = title;
        }

        /// <summary>
        /// Postavi boju pozadine.
        /// </summary>
        /// <param name="r">r</param>
        /// <param name="g">g</param>
        /// <param name="b">b</param>
        public void setBackgroundColor(int r, int g, int b)
        {
            this.BackColor = Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Postavi boju pozornice. <c>Color</c> je ugrađeni tip.
        /// </summary>
        /// <param name="color"></param>
        public void setBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        /// <summary>
        /// Postavi sliku pozornice.
        /// </summary>
        /// <param name="backgroundImage">Naziv (putanja) slike.</param>
        public void setBackgroundPicture(string backgroundImage)
        {
            this.BackgroundImage = new Bitmap(backgroundImage);
        }

        /// <summary>
        /// Izgled slike.
        /// </summary>
        /// <param name="layout">none, tile, stretch, center, zoom</param>
        public void setPictureLayout(string layout)
        {
            if (layout.ToLower() == "none") this.BackgroundImageLayout = ImageLayout.None;
            if (layout.ToLower() == "tile") this.BackgroundImageLayout = ImageLayout.Tile;
            if (layout.ToLower() == "stretch") this.BackgroundImageLayout = ImageLayout.Stretch;
            if (layout.ToLower() == "center") this.BackgroundImageLayout = ImageLayout.Center;
            if (layout.ToLower() == "zoom") this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        #endregion

        //sound
        #region sound methods

        /// <summary>
        /// Učitaj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        /// <param name="file">-</param>
        public void loadSound(int soundNum, string file)
        {
            soundCount++;
            sounds[soundNum] = new SoundPlayer(file);
        }

        /// <summary>
        /// Sviraj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        public void playSound(int soundNum)
        {
            sounds[soundNum].Play();
        }

        /// <summary>
        /// loopSound
        /// </summary>
        /// <param name="soundNum">-</param>
        public void loopSound(int soundNum)
        {
            sounds[soundNum].PlayLooping();
        }

        /// <summary>
        /// Zaustavi zvuk.
        /// </summary>
        /// <param name="soundNum">broj</param>
        public void stopSound(int soundNum)
        {
            sounds[soundNum].Stop();
        }

        #endregion

        //file
        #region file methods

        /// <summary>
        /// Otvori datoteku za čitanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToRead(string fileName, int fileNum)
        {
            readFiles[fileNum] = new StreamReader(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToRead(int fileNum)
        {
            readFiles[fileNum].Close();
        }

        /// <summary>
        /// Otvori datoteku za pisanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToWrite(string fileName, int fileNum)
        {
            writeFiles[fileNum] = new StreamWriter(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToWrite(int fileNum)
        {
            writeFiles[fileNum].Close();
        }

        /// <summary>
        /// Zapiši liniju u datoteku.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <param name="line">linija</param>
        public void writeLine(int fileNum, string line)
        {
            writeFiles[fileNum].WriteLine(line);
        }

        /// <summary>
        /// Pročitaj liniju iz datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća pročitanu liniju</returns>
        public string readLine(int fileNum)
        {
            return readFiles[fileNum].ReadLine();
        }

        /// <summary>
        /// Čita sadržaj datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća sadržaj</returns>
        public string readFile(int fileNum)
        {
            return readFiles[fileNum].ReadToEnd();
        }

        #endregion

        //mouse & keys
        #region mouse methods

        /// <summary>
        /// Sakrij strelicu miša.
        /// </summary>
        public void hideMouse()
        {
            Cursor.Hide();
        }

        /// <summary>
        /// Pokaži strelicu miša.
        /// </summary>
        public void showMouse()
        {
            Cursor.Show();
        }

        /// <summary>
        /// Provjerava je li miš pritisnut.
        /// </summary>
        /// <returns>true/false</returns>
        public bool isMousePressed()
        {
            //return sensing.MouseDown;
            return sensing.MouseDown;
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">naziv tipke</param>
        /// <returns></returns>
        public bool isKeyPressed(string key)
        {
            if (sensing.Key == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">tipka</param>
        /// <returns>true/false</returns>
        public bool isKeyPressed(Keys key)
        {
            if (sensing.Key == key.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
        /* ------------------- */

        /* ------------ GAME CODE START ------------ */

        /* Game variables */

        private string imev;
        private int bodovi,zivoti;
        Lik vozac;
        
        Znakovi dobar1, dobar2, los1, los2, los3, los4, los6;
        List<Znakovi> dobriZnakovi,losiZnakovi;
        Osobe osobe1, osobe2;

        /* Initialization */
        public delegate void TouchHandler();
        public static event TouchHandler DodirZnaka;

  

        private void SetupGame()
        {
            //1. setup stage
            SetStageTitle("PMF");
            setBackgroundColor(Color.WhiteSmoke);
            setBackgroundPicture("backgrounds\\cesta1.jpeg");
            
            START = true;

            //none, tile, stretch, center, zoom
            setPictureLayout("stretch");


            //2. add sprites
            vozac = new Lik("sprites\\vozac1.jpg.png", 200, 100);
            Game.AddSprite(vozac);
            vozac.RotationStyle = "AllAround";
            vozac.Ime = imev;
            vozac.Bodovi = bodovi;
            vozac.SetSize(80);


            //Instanciranje znakova koji padaju
            dobriZnakovi = new List<Znakovi>();
            dobriZnakovi.Add(new Znakovi("sprites\\naredba3.png", 0, 0));
            dobriZnakovi.Add(new Znakovi("sprites\\naredba4.png", 0, 0));

            losiZnakovi = new List<Znakovi>();
            losiZnakovi.Add(new Znakovi("sprites\\obavijest.png", 0, 0));
            losiZnakovi.Add(new Znakovi("sprites\\obavijest1.png", 0, 0));
            losiZnakovi.Add(new Znakovi("sprites\\opasnost2.png", 0, 0));
            losiZnakovi.Add(new Znakovi("sprites\\opasnost3.png", 0, 0));
 
            losiZnakovi.Add(new Znakovi("sprites\\auto2.png", 0, 0));

            osobe1 = new Osobe("sprites\\osobe1.png", 0, 0);
            Game.AddSprite(osobe1);
            osobe1.SetSize(30);
            osobe2 = new Osobe("sprites\\osobe2.png", 0, 0);
            Game.AddSprite(osobe2);
            osobe2.SetSize(20);

            DodirZnaka += DodirZ;

            dobar1 = new Znakovi("sprites\\naredba3.png", 0, 0);
            dobar1.SetSize(70);
            dobar2 = new Znakovi("sprites\\naredba4.png", 0, 0);
            dobar2.SetSize(70);

            los1 = new Znakovi("sprites\\obavijest1.png", 0, 0);
            los1.SetSize(80);
            los2 = new Znakovi("sprites\\opasnost3.png", 0, 0);
            los2.SetSize(70);
            los3 = new Znakovi("sprites\\obavijest.png", 0, 0);
            los3.SetSize(20);
            los4 = new Znakovi("sprites\\opasnost2.png", 0, 0);
            los4.SetSize(70);
            
            los6 = new Znakovi("sprites\\auto2.png", 0, 0);
            los6.SetSize(20);

            Game.StartScript(Kretanjevozaca);
            Game.StartScript(KretanjeZnakova);


        }

        private void DodirZ()
        {
            if (vozac.TouchingSprite(los1))
                los1.Y = 0;
            if (vozac.TouchingSprite(los2))
                los2.Y = 0;
            if (vozac.TouchingSprite(los3))
                los3.Y = 0;
            if (vozac.TouchingSprite(los4))
                los4.Y = 0;
            if (vozac.TouchingSprite(los6))
                los6.Y = 0;
            if (vozac.TouchingSprite(dobar1))
                dobar1.Y = 0;
            if (vozac.TouchingSprite(dobar2))
                dobar2.Y = 0;

            if (vozac.TouchingSprite(osobe1))
                osobe1.X = 0;
            if (vozac.TouchingSprite(osobe2))
                osobe2.X = 0;

        }

        private int Kretanjevozaca()
        {
            vozac.Y = (GameOptions.DownEdge);
            while (START)
            {
                if (sensing.KeyPressed(Keys.Up))
                {
                    try
                    {
                        vozac.Y -= vozac.Brzina;
                        vozac.SetHeading(0);
                    }
                    catch (ArgumentException)
                    {
                        vozac.Y = GameOptions.UpEdge;
                    }
                }
                if (sensing.KeyPressed(Keys.Down))
                {
                    try
                    {
                        vozac.Y += vozac.Brzina;
                        vozac.SetHeading(180);
                    }
                    catch (ArgumentException)
                    {
                        vozac.Y = GameOptions.DownEdge;
                    }
                }
                if (sensing.KeyPressed(Keys.Left))
                {
                    vozac.X -= vozac.Brzina;
                    vozac.SetHeading(270);
                }
                if (sensing.KeyPressed(Keys.Right))
                {
                    vozac.X += vozac.Brzina;
                    vozac.SetHeading(90);
                }
            }
            return 0;
        }


        Random g = new Random();
        private bool dodirlos1;
        private bool dodirlos2;
        private bool dodirlos3;
        private bool dodirlos4;
        private bool dodirlos6;
        private bool dodirdobar1;
        private bool dodirdobar2;

        private bool dodirosobe1;
        private bool dodirosobe2;


        private int KretanjeZnakova()
        {
            Game.AddSprite(dobar1);
            Game.AddSprite(dobar2);

            Game.AddSprite(los1);
            Game.AddSprite(los2);
            Game.AddSprite(los3);
            Game.AddSprite(los4);
            Game.AddSprite(los6);
            Game.AddSprite(osobe1);
            Game.AddSprite(osobe2);

            ISPIS = String.Format("Vozac: {0}, Bodovi: {1}, Zivoti: {2},", vozac.Ime, vozac.Bodovi, vozac.Zivoti);
            while (START)
            {
                los1.Y += 5;
                los2.Y += 7;
                los3.Y += 10;
                los4.Y += 5;
                los6.Y += 12;

                dobar1.Y += 10;
                dobar2.Y += 8;
                osobe1.X += 8;
                osobe2.X += 10;
                Wait(0.1);
                if (los1.TouchingSprite(vozac))
                {
                    dodirlos1 = true;
                    ProvjeraLosegZnaka(dodirlos1);
                }
                else if (los2.TouchingSprite(vozac))
                {
                    dodirlos2 = true;
                    ProvjeraLosegZnaka(dodirlos2);
                }
                else if (los3.TouchingSprite(vozac))
                {
                    dodirlos3 = true;
                    ProvjeraLosegZnaka(dodirlos3);
                }
                else if (los4.TouchingSprite(vozac))
                {
                    dodirlos4 = true;
                    ProvjeraLosegZnaka(dodirlos4);
                }
                
                else if (los6.TouchingSprite(vozac))
                {
                    dodirlos6 = true;
                    ProvjeraLosegZnaka(dodirlos6);
                }
                else if (osobe1.TouchingSprite(vozac))
                {
                    dodirosobe1 = true;
                    ProvjeraLosegZnaka(dodirosobe1);
                }
                else if (osobe2.TouchingSprite(vozac))
                {
                    dodirosobe2 = true;
                    ProvjeraLosegZnaka(dodirosobe2);
                }


                else if (dobar1.TouchingSprite(vozac))
                {
                    dodirdobar1 = true;
                    ProvjeraDobrogZnaka(dodirdobar1);
                }
                else if (dobar2.TouchingSprite(vozac))
                {
                    dodirdobar2 = true;
                    ProvjeraDobrogZnaka(dodirdobar2);
                }

                ISPIS = String.Format("Vozac: {0}, Bodovi: {1}, Zivoti: {2},", vozac.Ime, vozac.Bodovi, vozac.Zivoti);
                ZivotiVozac();
                Provjera();
            }
            return 0;
        }


        private void ProvjeraDobrogZnaka(bool dodir)
        {
            DodirZnaka.Invoke();


            if (dodir == dodirdobar1)
            {
                vozac.BrojTocnih += 1;
                vozac.Bodovi += dobar1.Bodoviznaka;
                vozac.MaxBodovi += dobar1.Bodoviznaka;
                dobar1.SetX(g.Next(0, GameOptions.RightEdge - dobar1.Width));
            }
            if (dodir == dodirdobar2)
            {
                vozac.BrojTocnih += 1;
                vozac.Bodovi += dobar2.Bodoviznaka;
                vozac.MaxBodovi += dobar2.Bodoviznaka;
                dobar2.SetX(g.Next(0, GameOptions.RightEdge - dobar2.Width));
            }


        }

        private void ProvjeraLosegZnaka(bool d)
        {
            DodirZnaka.Invoke();
            if (d == dodirlos1)
            {
                vozac.Bodovi -= los1.Bodoviznaka;
                vozac.MaxBodovi -= los1.Bodoviznaka;
                vozac.BrojNetocnih += 1;
                los1.SetX(g.Next(0, GameOptions.RightEdge) - los1.Width);
            }
            if (d == dodirlos2)
            {
                vozac.Bodovi -= los2.Bodoviznaka;
                vozac.MaxBodovi -= los2.Bodoviznaka;
                vozac.BrojNetocnih += 1;
                los2.SetX(g.Next(0, GameOptions.RightEdge) - los2.Width);
            }
            if (d == dodirlos3)
            {
                vozac.Bodovi -= los3.Bodoviznaka;
                vozac.MaxBodovi -= los3.Bodoviznaka;
                vozac.BrojNetocnih += 1;
                los3.SetX(g.Next(0, GameOptions.RightEdge) - los3.Width);
            }
            if (d == dodirlos4)
            {
                vozac.Bodovi -= los4.Bodoviznaka;
                vozac.MaxBodovi -= los4.Bodoviznaka;
                vozac.BrojNetocnih += 1;
                los4.SetX(g.Next(0, GameOptions.RightEdge) - los4.Width);
            }
            
            if (d == dodirlos6)
            {
                vozac.Bodovi -= los6.Bodoviznaka;
                vozac.MaxBodovi -= los6.Bodoviznaka;
                vozac.BrojNetocnih += 1;
                los6.SetX(g.Next(0, GameOptions.RightEdge) - los6.Width);
            }
            if (d == dodirosobe1)
            {
               
                MessageBox.Show("Zgazili ste osobu! Igra završena!");
                Application.Restart();
            }
            if (d == dodirosobe2)
            {
                
                MessageBox.Show("Zgazili ste osobu! Igra završena!");
                Application.Restart();
            }


        }

        private void ZivotiVozac()
        {
            if (vozac.BrojTocnih != 0)
            {
                
                vozac.BrojTocnih = 0;
            }
            if (vozac.BrojNetocnih != 0)
            {
                vozac.Zivoti -= 1;
                vozac.BrojNetocnih = 0;
            }
            if (vozac.Bodovi >= 100)
            {
                using (StreamWriter sw = File.AppendText(GameOptions.datoteka))
                {
                    vozac.Ime = imev;
                    sw.WriteLine(vozac.Ime + "-" + vozac.Bodovi);
                }
                
                allSprites.Clear();
                Kraj zadnja = new Kraj();
                zadnja.ShowDialog();
                //bodovi = vozac.Bodovi;
                
            }
            ISPIS = String.Format("Vozac: {0}, Bodovi: {1}, Zivoti: {2},", vozac.Ime, vozac.Bodovi, vozac.Zivoti);
            if (vozac.Zivoti == 0)
            {
                MessageBox.Show("Nažalost pali ste ispit. Pokušajte ponovo!");
                
                allSprites.Clear();
                Application.Restart();
                //bodovi = vozac.Bodovi;
                
            }
        }

        private void Provjera()
        {
            if (dobar1.X == dobar2.X || dobar1.X == los1.X || dobar1.X == los2.X || dobar1.X == los3.X || dobar1.X == los4.X || dobar1.X == los6.X || dobar1.TouchingSprite(dobar2) || dobar1.TouchingSprite(los1) || dobar1.TouchingSprite(los2) || dobar1.TouchingSprite(los3) || dobar1.TouchingSprite(los4) || dobar1.TouchingSprite(los6))
            {
                dobar1.SetX(g.Next(0, GameOptions.RightEdge - dobar1.Width));
            }
            if (dobar2.X == dobar1.X || dobar2.X == los1.X || dobar2.X == los2.X || dobar2.X == los3.X || dobar2.X == los4.X || dobar2.X == los6.X || dobar2.TouchingSprite(dobar1) || dobar2.TouchingSprite(los1) || dobar2.TouchingSprite(los2) || dobar2.TouchingSprite(los3) || dobar2.TouchingSprite(los4) || dobar2.TouchingSprite(los6))
            {
                dobar2.SetX(g.Next(0, GameOptions.RightEdge - dobar2.Width));
            }
            
            if (los1.X == los2.X || los1.X == los3.X || los1.X == los3.X || los1.X == los4.X || los1.TouchingSprite(los2) || los1.TouchingSprite(los3) || los1.TouchingSprite(los4) || los1.TouchingSprite(los6))
            {
                los1.SetX(g.Next(0, GameOptions.RightEdge - los1.Width));
            }
            if (osobe1.X == osobe2.X || osobe1.TouchingSprite(osobe2))
            {
                osobe1.SetX(g.Next(0, GameOptions.DownEdge - osobe1.Width));
            }

        }












        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }





    }
}
     

       



        /* ------------ GAME CODE END ------------ */


  

