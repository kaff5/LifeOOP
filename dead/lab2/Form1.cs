using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {


        public Map el = new Map();

        public Graphics g;


        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Invalidate();
        }

        int timerc = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (timer2.Enabled)
                return;
            
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            
            el.GenerateMap(g);
            el.makeNewAnimalForDebugSearchFood(g);
            el.makeNewPlant(g);

            
            timer2.Start();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //el.moveAnimal();
            //el.killPlantOrLivePlant(g);

            el.killAnimalOrLive(g);
            el.drawAgain(g);
            timerc += 1;
            if (timerc == 300)
            {
                el.makeNewPlant(g);
                timerc = 0;
            }
            pictureBox1.Refresh();
        }

    }


}
