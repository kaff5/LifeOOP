using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Map map;
        private Drawer _drawer;
        private bool press = true;
        private int normalWidth = 3883;
        private int scaleWidth = 8882;
        private int size = 3883;
        private string text;
        private Seasons forCheckSeason = Seasons.Summer;
        private int X = 1;
        private int Y = 1;


        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Invalidate();
            map = new Map();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
                return;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            _drawer = new Drawer(map, g);


            timer2.Start();
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Size.Width != size)
            {
                if (press == true)
                {
                    size = normalWidth;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                    pictureBox1.Size = new System.Drawing.Size(3883, 3033);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    size = scaleWidth;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                    pictureBox1.Size = new System.Drawing.Size(8882, 8033);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                _drawer = new Drawer(map, g);
            }

            map.GlobalGame();
            if (map.seasons != forCheckSeason)
            {
                _drawer.ChangeSeason();
                forCheckSeason = map.seasons;
            }

            _drawer.DrawItemGlobalGame();
            pictureBox1.Refresh();
            PrintText();
        }

        private void PrintText()
        {
            textBox1.Text = "";
            text = "X: " + X + " Y: " + Y;
            text += "\r\n";

            text += map.cells[X, Y].GetBiom().ToString();
            text += "\r\n";

            foreach (var animal in map.cells[X, Y].GetAnimal())
            {
                switch (animal)
                {
                    case Man man:
                    {
                        if (man.GetPartner() != null)
                        {
                            text += "Hp: " + man.GetHp();
                            text += "\r\n";
                            text += "Hungry: " + man.GetHungry();
                            text += "\r\n";
                            text += "Partner: X:" + man.GetPartner()._cell.X + "Y: " +
                                    man.GetPartner()._cell.X;
                            text += "\r\n";
                        }
                        else
                        {
                            text += "null";
                            text += "\r\n";
                        }


                        text += "Bag: " + man.GetBag().Count + " / " + Man.MaxSizeBag;
                        text += "\r\n";
                        text += "BagForRes: " + man.GetBagForRes().Count + " / " + Man.MaxSizeBag;
                        text += "\r\n";
                        break;
                    }
                    case Cornivourus:
                        text += "Hp: " + animal.GetHp();
                        text += "\r\n";
                        text += "Hungry: " + animal.GetHungry();
                        text += "\r\n";
                        text += "Cornivourus";
                        text += "\r\n";
                        break;
                    case Omnivourus:
                        text += "Hp: " + animal.GetHp();
                        text += "\r\n";
                        text += "Hungry: " + animal.GetHungry();
                        text += "\r\n";
                        text += "Omnivourus";
                        text += "\r\n";
                        break;
                    case Herbivore:
                        text += "Hp: " + animal.GetHp();
                        text += "\r\n";
                        text += "Hungry: " + animal.GetHungry();
                        text += "\r\n";
                        text += "Herbivore";
                        text += "\r\n";
                        break;
                }
            }

            if (map.cells[X, Y].GetPlant() != null)
            {
                text += "Plant";
                text += "\r\n";
            }

            if (map.cells[X, Y].GetFruit() != null)
            {
                text += "Fruit";
                text += "\r\n";
            }
            if (map.cells[X, Y].GetCopperResource() != null)
            {
                text += "Copper";
                text += "\r\n";
            }
            if (map.cells[X, Y].GetIronResource() != null)
            {
                text += "Iron";
                text += "\r\n";
            }
            if (map.cells[X, Y].GetGoldResource() != null)
            {
                text += "Gold";
                text += "\r\n";
            }    
            if (map.cells[X, Y].GetRockResource() != null)
            {
                text += "Rock";
                text += "\r\n";
            }
            

            textBox1.Text = text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (press == false)
            {
                press = true;
                size = normalWidth;
            }
            else
            {
                press = false;
                size = scaleWidth;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            Point coordinates = me.Location;
            double AnsX;
            double AnsY;
            if (size == 8882)
            {
                AnsX = coordinates.X / (8882 / 1000);
                AnsY = coordinates.Y / (8033 / 1000);
            }
            else
            {
                AnsX = coordinates.X / (3883 / 1000);
                AnsY = coordinates.Y / (3033 / 1000);
            }


            AnsX = Math.Floor(AnsX);
            AnsY = Math.Floor(AnsY);
            X = (int) AnsX;
            Y = (int) AnsY;
        }
    }
}