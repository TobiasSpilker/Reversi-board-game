using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Reversi
{
    class Program
    {
        static void Main()
        {
            Screen scherm = new Screen();
            Application.Run(scherm);
        }
    }


    class Screen : Form
    {

        Panel paneel;                  
        Pen pen = new Pen(Color.Black);
        Pen rodepen = new Pen(Color.DarkRed, 3);
        Pen blauwepen = new Pen(Color.DarkBlue, 3);
        int size = 8;                   
        int[,] SpeelveldArray;          
        int ArrayX, ArrayY;             
        int ClickTracker = 0;           
        Label BeurtLabel1;              
        Label BeurtLabel2;              
        int AantalRodeStenen;          
        int AantalBlauweStenen;         
        Label PuntenRood;               
        Label PuntenBlauw;              
        bool helper = false;            
        int SkipTracker;                

        Label RoodGewonnenLabel;
        Label BlauwGewonnenLabel;
        Label GelijkspelLabel;

        int beurt;
        int ander;
        bool Geldig;

        public Screen()
        {
            this.Text = "Tobias Spilker & Stijn van Huet - Reversi";
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(700, 600);
            this.CenterToScreen();

            OptionsInterface();
            StatsInterface();
            SizeInterface();
            this.Paint += InterfaceGraphics;

            Panel();
            ArrayMaker();
            AantalBerekener();
            paneel.MouseClick += klik;

        }

        private void OptionsInterface()
        {
            Label OptionsLabel = new Label();
            OptionsLabel.Text = "OPTIONS:";
            OptionsLabel.Location = new Point(8, 18);
            OptionsLabel.Size = new Size(60, 25);
            this.Controls.Add(OptionsLabel);

            Button RestartButton = new Button();
            RestartButton.Size = new Size(55, 20);
            RestartButton.Location = new Point(80, 15);
            RestartButton.Text = "restart";
            RestartButton.BackColor = Color.LightBlue;
            RestartButton.Click += this.restartgame;
            this.Controls.Add(RestartButton);

            Button HelpButton = new Button();
            HelpButton.Size = new Size(55, 20);
            HelpButton.Location = new Point(150, 15);
            HelpButton.Text = "help";
            HelpButton.BackColor = Color.LightBlue;
            //HelpButton.Click += this.help;
            this.Controls.Add(HelpButton);

            Button SkipBeurtButton = new Button();
            SkipBeurtButton.Size = new Size(55, 20);
            SkipBeurtButton.Location = new Point(220, 15);
            SkipBeurtButton.Text = "skip";
            SkipBeurtButton.BackColor = Color.LightBlue;
            SkipBeurtButton.Click += this.skipbeurt;
            this.Controls.Add(SkipBeurtButton);
        }

        private void restartgame(object sender, EventArgs e)
        { Application.Restart(); }
        private void help(object sender, EventArgs e)
        { if (helper == false) { helper = true; } else if (helper == true) { helper = false; } paneel.Invalidate(); }
        private void skipbeurt(object sender, EventArgs e)
        { ClickTracker++; SkipTracker++; this.Winnaar(); paneel.Invalidate(); StatsInterface(); AantalBerekener(); }


        private void StatsInterface()
        {
            Label StatsLabel = new Label();
            StatsLabel.Text = "STATS:";
            StatsLabel.Location = new Point(550, 5);
            StatsLabel.Size = new Size(200, 15);
            this.Controls.Add(StatsLabel);

            if (ClickTracker % 2 == 0)
            {
                BeurtLabel1 = new Label();
                BeurtLabel1.Location = new Point(550, 30);
                BeurtLabel1.Size = new Size(100, 20);
                BeurtLabel1.ForeColor = Color.Red;
                BeurtLabel1.Text += "Turn:   Player 1";
                this.Controls.Remove(BeurtLabel2);
                this.Controls.Add(BeurtLabel1);
            }

            else
            {
                BeurtLabel2 = new Label();
                BeurtLabel2.Location = new Point(550, 30);
                BeurtLabel2.Size = new Size(100, 20);
                BeurtLabel2.ForeColor = Color.Blue;
                BeurtLabel2.Text += "Turn:   Player 2";
                this.Controls.Remove(BeurtLabel1);
                this.Controls.Add(BeurtLabel2);
            }

        }

        private void Puntentelling(int Rode, int Blauwe)
        {
            PuntenRood = new Label();
            PuntenRood.Location = new Point(580, 58);
            PuntenRood.Size = new Size(50, 20);
            PuntenRood.Text = "" + Rode.ToString();
            this.Controls.Add(PuntenRood);

            PuntenBlauw = new Label();
            PuntenBlauw.Location = new Point(580, 88);
            PuntenBlauw.Size = new Size(50, 20);
            PuntenBlauw.Text = "" + Blauwe.ToString();
            this.Controls.Add(PuntenBlauw);
        }


        private void InterfaceGraphics(object o, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;

            gr.DrawRectangle(pen, 540, 20, 130, 100);

            gr.FillEllipse(Brushes.Red, 550, 55, 20, 20);
            gr.DrawEllipse(rodepen, 550, 55, 20, 20);

            gr.FillEllipse(Brushes.Blue, 550, 85, 20, 20);
            gr.DrawEllipse(blauwepen, 550, 85, 20, 20);
        }

        private void SizeInterface()
        {
            Label playfieldsizelabel = new Label();
            playfieldsizelabel.Text = "SIZE:";
            playfieldsizelabel.Location = new Point(8, 578);
            playfieldsizelabel.Size = new Size(50, 25);
            this.Controls.Add(playfieldsizelabel);

            Button Size8Button = new Button();
            Size8Button.Size = new Size(55, 20);
            Size8Button.Location = new Point(60, 575);
            Size8Button.Text = "large";
            Size8Button.BackColor = Color.LightBlue;
            Size8Button.Click += this.changesizeto8;
            this.Controls.Add(Size8Button);

            Button Size6Button = new Button();
            Size6Button.Size = new Size(55, 20);
            Size6Button.Location = new Point(130, 575);
            Size6Button.Text = "medium";
            Size6Button.BackColor = Color.LightBlue;
            Size6Button.Click += this.changesizeto6;
            this.Controls.Add(Size6Button);

            Button Size4Button = new Button();
            Size4Button.Size = new Size(55, 20);
            Size4Button.Location = new Point(200, 575);
            Size4Button.Text = "small";
            Size4Button.BackColor = Color.LightBlue;
            Size4Button.Click += this.changesizeto4;
            this.Controls.Add(Size4Button);
        }

        private void changesizeto8(object sender, EventArgs e)
        { size = 8; this.ArrayMaker(); paneel.Invalidate(); }
        private void changesizeto6(object sender, EventArgs e)
        { size = 6; this.ArrayMaker(); paneel.Invalidate(); }
        private void changesizeto4(object sender, EventArgs e)
        { size = 4; this.ArrayMaker(); paneel.Invalidate(); }

        void Panel()
        {
            paneel = new Panel();
            paneel.BackColor = Color.White;
            paneel.Paint += Speelveld;
            Controls.Add(paneel);
        }

        void Speelveld(object o, PaintEventArgs pea)
        {

            Graphics gr = pea.Graphics;

            int counterX;
            int counterY;

            int Xloc = 0;
            int Yloc = 0;

            for (counterX = 0; counterX < size; counterX++)
            {
                for (counterY = 0; counterY < size; counterY++)
                {
                    Xloc = counterX * 50;
                    Yloc = counterY * 50;

                    gr.DrawRectangle(pen, Xloc, Yloc, 50, 50);

                    if (SpeelveldArray[counterX, counterY] == 1)
                    {
                        gr.FillEllipse(Brushes.Red, Xloc + 2, Yloc + 2, 45, 45);
                        gr.DrawEllipse(rodepen, Xloc + 2, Yloc + 2, 45, 45);
                    }

                    if (SpeelveldArray[counterX, counterY] == 2)
                    {
                        gr.FillEllipse(Brushes.Blue, Xloc + 2, Yloc + 2, 45, 45);
                        gr.DrawEllipse(blauwepen, Xloc + 2, Yloc + 2, 45, 45);
                    }

                    if (this.MagPlaatsen(counterX, counterY) && helper == true)
                    {
                        gr.FillEllipse(Brushes.LightYellow, Xloc + 15, Yloc + 15, 20, 20);
                        gr.DrawEllipse(pen, Xloc + 15, Yloc + 15, 20, 20);
                    }
                }
            }

            paneel.Location = new Point(350 - (size * 25), 350 - (size * 25));
            paneel.Size = new Size((size * 50) + 1, (size * 50) + 1);
        }


        private void ArrayMaker()
        {

            SpeelveldArray = new int[size, size];

            for (int counterX = 0; counterX < size; counterX++)
            {
                for (int counterY = 0; counterY < size; counterY++)
                {
                    SpeelveldArray[counterX, counterY] = 0;
                }
            }

            SpeelveldArray[(size / 2) - 1, (size / 2) - 1] = 2;
            SpeelveldArray[size / 2, size / 2] = 2;
            SpeelveldArray[(size / 2) - 1, size / 2] = 1;
            SpeelveldArray[size / 2, (size / 2) - 1] = 1;
        }

        private void AantalBerekener()
        {
            int RodeStenenTemp = 0;
            int BlauweStenenTemp = 0;

            for (int counterX = 0; counterX < size; counterX++)
            {
                for (int counterY = 0; counterY < size; counterY++)
                {
                    if (SpeelveldArray[counterX, counterY] == 1) { RodeStenenTemp++; }

                    if (SpeelveldArray[counterX, counterY] == 2) { BlauweStenenTemp++; }
                }
            }

            AantalRodeStenen = RodeStenenTemp;
            AantalBlauweStenen = BlauweStenenTemp;
            this.Controls.Remove(PuntenRood);
            this.Controls.Remove(PuntenBlauw);
            this.Puntentelling(AantalRodeStenen, AantalBlauweStenen);
        }

        private void klik(object o, MouseEventArgs mea)
        {
            ArrayX = mea.Location.X / 50;
            ArrayY = mea.Location.Y / 50;

            if (this.MagPlaatsen(ArrayX, ArrayY) || omringing(ArrayX, ArrayY) == true)
            {
                if (ClickTracker % 2 == 0)
                    SpeelveldArray[ArrayX, ArrayY] = 1;

                else
                    SpeelveldArray[ArrayX, ArrayY] = 2;

                omringing(ArrayX, ArrayY);
                ClickTracker++;
                paneel.Invalidate();
                StatsInterface();
                AantalBerekener();
                SkipTracker = 0;

                Winnaar();
            }
        }

        private bool MagPlaatsen(int PX, int PY)
        {
            if (SpeelveldArray[PX, PY] == 0)
            {
                return true;
            }

            else
                return false;
        }

        private bool omringing(int aX, int bY)
        {

            if (ClickTracker % 2 == 0)
            {
                beurt = 1;
                ander = 2;
            }
            else
            {
                beurt = 2;
                ander = 1;
            }

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {

                    if (dx != 0 || dy != 0)
                    {
                        int p = verander_Array(aX + dx, bY + dy, dx, dy, beurt);
                        if (p > 0)
                        {
                            Geldig = true;
                        }
                    }
                }
            }
            return Geldig;
        }

        private int verander_Array(int midx, int midy, int dx, int dy, int beurt)
        {
            if (0 <= midx && midx <= size - 1 && 0 <= midy && midy <= size - 1)
            {


                int stappen;
                if (SpeelveldArray[midx, midy] == 0)
                {
                    return -1;
                }

                else if (SpeelveldArray[midx, midy] == beurt)
                {
                    return 0;
                }
                else if (SpeelveldArray[midx, midy] != beurt)
                {
                    stappen = verander_Array(midx + dx, midy + dy, dx, dy, beurt);
                    if (stappen >= 0)
                    {
                        SpeelveldArray[midx, midy] = beurt;
                        return stappen + 1;

                    }
                    else
                        return stappen;
                }

                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }



        private void Winnaar()
        {

            if (AantalBlauweStenen + AantalRodeStenen == size * size || SkipTracker == 2)
            {
                if (AantalRodeStenen > AantalBlauweStenen)
                {
                    Controls.Remove(paneel);
                    this.WinnerInterface();
                    Controls.Add(RoodGewonnenLabel);
                    this.WinnerPaneel();
                }

                else if (AantalRodeStenen < AantalBlauweStenen)
                {
                    Controls.Remove(paneel);
                    this.WinnerInterface();
                    Controls.Add(BlauwGewonnenLabel);
                    this.WinnerPaneel();
                }

                else
                {
                    Controls.Remove(paneel);
                    this.WinnerInterface();
                    Controls.Add(GelijkspelLabel);
                    this.WinnerPaneel();
                }
            }
        }

        private void WinnerPaneel()
        {
            Panel WinnerPaneel = new Panel();
            WinnerPaneel.BackColor = Color.White;
            WinnerPaneel.Size = new Size(400, 250);
            WinnerPaneel.Location = new Point(150, 175);
            WinnerPaneel.BorderStyle = BorderStyle.FixedSingle;
            WinnerPaneel.Paint += this.WinnerGraphics;
            Controls.Add(WinnerPaneel);
        }

        void WinnerGraphics(object o, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;
            Pen RodeDikkePen = new Pen(Color.Red, 5);
            Pen BlauweDikkePen = new Pen(Color.Blue, 5);

            for (int i = 0; i < 5; i++)
            {
                gr.DrawLine(BlauweDikkePen, 0 + (80 * i), 0, 20 + (80 * i), 30);
                gr.DrawLine(BlauweDikkePen, 20 + (80 * i), 30, 40 + (80 * i), 0);

                gr.DrawLine(RodeDikkePen, 0 + (80 * i) + 40, 0, 20 + (80 * i) + 40, 30);
                gr.DrawLine(RodeDikkePen, 20 + (80 * i) + 40, 30, 40 + (80 * i) + 40, 0);
            }
        }

        private void WinnerInterface()
        {
            RoodGewonnenLabel = new Label();
            RoodGewonnenLabel.Location = new Point(180, 290);
            RoodGewonnenLabel.Size = new Size(350, 100);
            RoodGewonnenLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            RoodGewonnenLabel.ForeColor = Color.Red;
            RoodGewonnenLabel.BackColor = Color.White;
            RoodGewonnenLabel.Text += "Player 1 has won!";

            BlauwGewonnenLabel = new Label();
            BlauwGewonnenLabel.Location = new Point(180, 290);
            BlauwGewonnenLabel.Size = new Size(350, 100);
            BlauwGewonnenLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            BlauwGewonnenLabel.ForeColor = Color.Blue;
            BlauwGewonnenLabel.BackColor = Color.White;
            BlauwGewonnenLabel.Text += "Player 2 has won!";

            GelijkspelLabel = new Label();
            GelijkspelLabel.Location = new Point(234, 290);
            GelijkspelLabel.Size = new Size(240, 100);
            GelijkspelLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            GelijkspelLabel.ForeColor = Color.Black;
            GelijkspelLabel.BackColor = Color.White;
            GelijkspelLabel.Text += "It's a draw!";
        }
    }

}
