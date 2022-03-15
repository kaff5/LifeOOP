using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;


namespace lab2
{
    public class Cell
    {
        public class aQueue
        {
            public Point pointForMe = new Point();
            public Point pointParent = new Point();

            public aQueue(Point a,Point b)
            {
                pointForMe = a;
                pointParent = b;
            }
        }
        
        
        
        public ColorForMap color;
        public bool Lock;
        public AnimalGround Animal;
        public Plant plant;

        public ColorForMap colorAnimal;
        public ColorForMap plantColor;
        public List<aQueue> allCells = new List<aQueue>();

        public void addNewItemInQueue(Point a,Point b)
        {
            allCells.Add(new aQueue(a,b));
        }
        public Point getXYForMe(int index)
        {
            return allCells[index].pointForMe;
        }
        
        public Point getXYParent(int index)
        {
            return allCells[index].pointParent;
        }
        public void addWith100(Point a)
        {
            allCells.Add(new aQueue(a,new Point(-100,-100)));
        }
    

        

        
        
        public Cell()
        {
            color = ColorForMap.white;
            Lock = false;
            List<aQueue> allCells = new List<aQueue>();
        }
    }
    
    public class Map
    {

        int size = 10;
        int numOfCells = 100;
        int temperature;

        private List<Point> pointsPlants = new List<Point>();
        private List<Point> pointsAnimal = new List<Point>();


        
        private Random rnd = new Random();
        public Cell[,] cells = new Cell[1000, 1000];
        private SolidBrush Brush;
        private Rectangle Rect;
        private int newX;
        private int newY;
        private int newR;




        private void draw(int i, int j, Graphics g, Color color)
        {
            Rect = new Rectangle(size * i, size * j, size, size);
            Brush = new SolidBrush(color);
            g.FillRectangle(Brush, Rect);
        }

        public void GenerateMap(Graphics g)
        {
            for (int i = 0; i <= numOfCells; i++)
            {
                for (int j = 0; j <= numOfCells; j++)
                {
                    cells[i, j] = new Cell();
                    draw(i, j, g, Color.Yellow);
                    cells[i,j].addNewItemInQueue(new Point(i,j),new Point(-100,-100));
                    cells[i, j].color = ColorForMap.yellow;
                    if ((i - 99) * (i - 99) + j * j > 7000)
                    {
                        draw(i, j, g, Color.Blue);
                        cells[i, j].color = ColorForMap.blue;
                    }
                }
            }

            Brush = new SolidBrush(Color.DarkGreen);
            for (int i = 0; i < 10; i++)
            {

                newX = rnd.Next(40, 88);
                newY = rnd.Next(2, 60);
                newR = rnd.Next(5, 11);
                int x = newX - newR;
                int y = newY - newR;
                if (x < 0)
                {
                    x = 0;
                }

                if (y < 0)
                {
                    y = 0;
                }

                for (int pX = x; pX <= newX + newR; pX++)
                {
                    for (int pY = y; pY <= newY + newR; pY++)
                    {
                        if ((pY - newY) * (pY - newY) + (pX - newX) * (pX - newX) < newR * newR)
                        {
                            draw(pX, pY, g, Color.DarkGreen);
                            cells[pX, pY].color = ColorForMap.darkGreen;
                        }
                    }
                }
            }
        }
        public void makeNewAnimal(Graphics g)
        {
            
            for (var i = 0; i <= numOfCells; i++)
            {
                for (var j = 0; j <= numOfCells; j++)
                {
                    if (cells[i, j].color == ColorForMap.darkGreen && cells[i, j].Lock == false && rnd.Next(0, 200) == 5)
                    {
                        cells[i, j].colorAnimal = ColorForMap.red;
                        cells[i, j].Lock = true;
                        cells[i, j].Animal = new AnimalGround(cells[i, j]);
                        pointsAnimal.Add(new Point(i,j));
                        cells[i,j].addNewItemInQueue(new Point(i,j),new Point(-100,-100));
                        draw(i, j, g, Color.Red);
                    }
                }
            }
        }

        public void makeNewPlant(Graphics g)
        {
            for (var i = 0; i <= numOfCells; i++)
            {
                for (int j = 0; j <= numOfCells; j++)
                {
                    int makeorNotmake = rnd.Next(0, 200);
                    if (cells[i, j].color != ColorForMap.blue && cells[i, j].Animal == null &&
                        cells[i, j].Lock == false && makeorNotmake == 1)
                    {
                        draw(i, j, g, Color.LightSeaGreen);
                        cells[i, j].Lock = true;
                        cells[i, j].plantColor = ColorForMap.seedColor;
                        cells[i, j].plant = new Plant(cells[i, j]);
                        pointsPlants.Add(new Point(i,j));
                    }
                }
            }
        }



        private bool checkMove(int a, int b, int srand, string side)
        {
            if ((side == "up") && (srand == 1) && (b > 2) && (cells[a, b - 1].color != ColorForMap.blue) &&
                (cells[a, b - 1].Animal == null))
                return true;
            if ((side == "left") && (a > 2) && (srand == 2) && (cells[a - 1, b].color != ColorForMap.blue) &&
                (cells[a - 1, b].Animal == null))
                return true;
            if ((side == "right") && (srand == 3) && (a < numOfCells - 1) &&
                (cells[a + 1, b].color != ColorForMap.blue) && (cells[a + 1, b].Animal == null))
                return true;
            if ((side == "down") && (b < numOfCells - 1) && (srand == 0) &&
                (cells[a, b + 1].color != ColorForMap.blue) && (cells[a, b + 1].Animal == null))
                return true;
            return false;
        }

        public void moveAnimal(int index)
        {
            if (checkMove(pointsAnimal[index].X, pointsAnimal[index].Y, rnd.Next(0, 9), "up"))
            {
                cells[pointsAnimal[index].X, pointsAnimal[index].Y - 1].Animal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y - 1].colorAnimal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y - 1].Lock = true;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal = null;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal = ColorForMap.colorLess;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Lock = false;
                pointsAnimal.Add(new Point(pointsAnimal[index].X, pointsAnimal[index].Y - 1));
                Point ce = cells[pointsAnimal[index].X, pointsAnimal[index].Y].getXYForMe(0);
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].allCells.Clear();
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].addWith100(ce);
                pointsAnimal.RemoveAt(index);
            }

            if (checkMove(pointsAnimal[index].X, pointsAnimal[index].Y, rnd.Next(0, 9), "left"))
            {
                cells[pointsAnimal[index].X - 1, pointsAnimal[index].Y].Animal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal;
                cells[pointsAnimal[index].X - 1, pointsAnimal[index].Y].colorAnimal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal;
                cells[pointsAnimal[index].X - 1, pointsAnimal[index].Y].Lock = true;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal = null;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal = ColorForMap.colorLess;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Lock = false;
                pointsAnimal.Add(new Point(pointsAnimal[index].X - 1, pointsAnimal[index].Y));
                Point ce = cells[pointsAnimal[index].X, pointsAnimal[index].Y].getXYForMe(0);
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].allCells.Clear();
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].addWith100(ce);
                pointsAnimal.RemoveAt(index);
            }

            if (checkMove(pointsAnimal[index].X, pointsAnimal[index].Y, rnd.Next(0, 9), "right"))
            {
                cells[pointsAnimal[index].X + 1, pointsAnimal[index].Y].Animal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal;
                cells[pointsAnimal[index].X + 1, pointsAnimal[index].Y].colorAnimal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal;
                cells[pointsAnimal[index].X + 1, pointsAnimal[index].Y].Lock = true;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal = null;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal = ColorForMap.colorLess;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Lock = false;
                pointsAnimal.Add(new Point(pointsAnimal[index].X + 1, pointsAnimal[index].Y));
                Point ce = cells[pointsAnimal[index].X, pointsAnimal[index].Y].getXYForMe(0);
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].allCells.Clear();
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].addWith100(ce);
                pointsAnimal.RemoveAt(index);
            }

            if (checkMove(pointsAnimal[index].X, pointsAnimal[index].Y, rnd.Next(0, 9), "down"))
            {
                cells[pointsAnimal[index].X, pointsAnimal[index].Y + 1].Animal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y + 1].colorAnimal =
                    cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y + 1].Lock = true;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Animal = null;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].colorAnimal = ColorForMap.colorLess;
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].Lock = false;
                pointsAnimal.Add(new Point(pointsAnimal[index].X, pointsAnimal[index].Y + 1));
                Point ce = cells[pointsAnimal[index].X, pointsAnimal[index].Y].getXYForMe(0);
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].allCells.Clear();
                cells[pointsAnimal[index].X, pointsAnimal[index].Y].addWith100(ce);
                pointsAnimal.RemoveAt(index);

            }

        }



        public void drawAgain(Graphics g)
        {
            for (var i = 0; i <= numOfCells; i++)
            {
                for (int j = 0; j <= numOfCells; j++)
                {
                    
                    if (cells[i, j].color == ColorForMap.darkGreen)
                    {
                        draw(i, j, g, Color.DarkGreen);
                    }

                    if (cells[i, j].color == ColorForMap.yellow)
                    {
                        draw(i, j, g, Color.Yellow);
                    }
                    
                    if (cells[i, j].colorAnimal == ColorForMap.red)
                    {
                        draw(i, j, g, Color.Red);
                    }
                    
                    if (cells[i, j].plant != null)
                    {
                        if (cells[i, j].plant.getPlantCycle() == PlantCycle.DriedPlant)
                        {
                            draw(i, j, g, Color.Black);
                        }

                        if (cells[i, j].plant.getPlantCycle() == PlantCycle.Flower)
                        {
                            draw(i, j, g, Color.BlueViolet);
                        }

                        if (cells[i, j].plant.getPlantCycle() == PlantCycle.Germ)
                        {
                            draw(i, j, g, Color.Cyan);
                        }

                        if (cells[i, j].plant.getPlantCycle() == PlantCycle.Seed)
                        {
                            draw(i, j, g, Color.LightSeaGreen);
                        }
                    }

                }
            }
        }


        private void reproductionOfPlants(int a, int b)
        {
            if ((rnd.Next(0, 2) == 1) && (b > 2) && (cells[a, b - 1].color != ColorForMap.blue) &&
                (cells[a, b - 1].Animal == null) && (cells[a, b - 1].plant == null))
            {
                cells[a, b - 1].Lock = true;
                cells[a, b - 1].plantColor = ColorForMap.seedColor;
                cells[a, b - 1].plant = new Plant(cells[a, b - 1]);
                pointsPlants.Add(new Point(a,b-1));
            }

            if ((rnd.Next(0, 2) == 1) && (a > 2) && (cells[a - 1, b].color != ColorForMap.blue) &&
                (cells[a - 1, b].Animal == null) && (cells[a - 1, b].plant == null))
            {
                cells[a - 1, b].Lock = true;
                cells[a - 1, b].plantColor = ColorForMap.seedColor;
                cells[a - 1, b].plant = new Plant(cells[a - 1, b]);
                pointsPlants.Add(new Point(a-1,b));
            }

            if ((rnd.Next(0, 2) == 1) && (a < numOfCells - 1) && (cells[a + 1, b].color != ColorForMap.blue) &&
                (cells[a + 1, b].Animal == null) && (cells[a + 1, b].plant == null))
            {
                cells[a + 1, b].Lock = true;
                cells[a + 1, b].plantColor = ColorForMap.seedColor;
                cells[a + 1, b].plant = new Plant(cells[a + 1, b]);
                pointsPlants.Add(new Point(a,b+1));
            }

            if ((rnd.Next(0, 2) == 1) && (b < numOfCells - 1) && (cells[a, b + 1].color != ColorForMap.blue) &&
                (cells[a, b + 1].Animal == null) && (cells[a, b + 1].plant == null))
            {
                cells[a, b + 1].Lock = true;
                cells[a, b + 1].plantColor = ColorForMap.seedColor;
                cells[a, b + 1].plant = new Plant(cells[a, b + 1]);
                pointsPlants.Add(new Point(a,b+1));
            }
        }

        public void killPlantOrLivePlant(Graphics g)
        {
            for (int i = 0; i < pointsPlants.Count; i++)
            {
                if (cells[pointsPlants[i].X, pointsPlants[i].Y].plant != null)
                {
                    if ( cells[pointsPlants[i].X, pointsPlants[i].Y].plant.getTimerForDead() == 0)
                    {
                        cells[pointsPlants[i].X, pointsPlants[i].Y].plant = null;
                        cells[pointsPlants[i].X, pointsPlants[i].Y].plantColor = ColorForMap.colorLess;
                        cells[pointsPlants[i].X, pointsPlants[i].Y].Lock = false;
                        reproductionOfPlants(pointsPlants[i].X, pointsPlants[i].Y);
                        pointsPlants.RemoveAt(i);
                    
                    }
                    else
                    {
                        cells[pointsPlants[i].X, pointsPlants[i].Y].plant.setTimerForDead(cells[pointsPlants[i].X, pointsPlants[i].Y].plant.getTimerForDead() - 1);
                        cells[pointsPlants[i].X, pointsPlants[i].Y].plant.checkTimeForDead();
                    }
                }
            }
        }



        private bool checkUsed(List<Point> used, int a, int b)
        {
            for (int p = 0; p < used.Count; p++)
            {
                if (used[p].X == a && used[p].Y == b)
                {
                    return true;
                }
            }
            return false;
        }

        private List<Point> searchPlant(int z, int x)
        {
            int t = 0;
            var k= 0;
            var l = 0;
            var cycle = 0;
            List<Point> used = new List<Point>();
            bool NASHEL = false;

            used.Add(new Point(z, x));
            for (int u = 0; u < cells[z, x].allCells.Count; u++)
            {

                Point test = cells[z, x].getXYForMe(u);

                if ((test.Y > 1) && (cells[test.X, test.Y - 1].color != ColorForMap.blue) &&
                    (cells[test.X, test.Y - 1].Animal == null) &&
                    checkUsed(used, test.X, test.Y - 1) == false)
                {
                    used.Add(new Point(test.X, test.Y - 1));
                    cells[z, x].addNewItemInQueue(new Point(test.X, test.Y - 1), new Point(test.X, test.Y));
                }

                if ((test.X > 1) && (cells[test.X - 1, test.Y].color != ColorForMap.blue) &&
                    (cells[test.X - 1, test.Y].Animal == null) &&
                    checkUsed(used, test.X - 1, test.Y) == false)
                {
                    used.Add(new Point(test.X - 1, test.Y));
                    cells[z, x].addNewItemInQueue(new Point(test.X - 1, test.Y), new Point(test.X, test.Y));
                }

                if ((test.X < numOfCells - 1) && (cells[test.X + 1, test.Y].color != ColorForMap.blue) &&
                    (cells[test.X + 1, test.Y].Animal == null) && checkUsed(used, test.X + 1, test.Y) == false)
                {
                    used.Add(new Point(test.X + 1, test.Y));
                    cells[z, x].addNewItemInQueue(new Point(test.X + 1, test.Y), new Point(test.X, test.Y));
                }

                if ((test.Y < numOfCells - 1) && (cells[test.X, test.Y + 1].color != ColorForMap.blue) &&
                    (cells[test.X, test.Y + 1].Animal == null) && checkUsed(used, test.X, test.Y + 1) == false)
                {
                    used.Add(new Point(test.X, test.Y + 1));
                    cells[z, x].addNewItemInQueue(new Point(test.X, test.Y + 1), new Point(test.X, test.Y));
                }

                if (cells[test.X, test.Y].plant != null)
                {
                    k = test.X;
                    l = test.Y;
                    cycle = u;
                    u = 100000000;
                }
                if (u == 9999)
                {
                    return null;
                }
            }


            List <Point> myWay = returnMyWay(k,l,cycle,z,x);
            return myWay;
        }


        private int searchParent(int q, int w,int a,int b)
        {
            for (int j = 0; j < cells[q, w].allCells.Count; j++)
            {
                Point forSearch = cells[q, w].getXYForMe(j);
                if (a == forSearch.X && forSearch.Y == b)
                {
                    return j;
                }
            }

            return -5;
        }

        private List<Point> returnMyWay(int a, int b,int index, int mainX,int mainY)
        {            
            List<Point> way = new List<Point>();
            way.Insert(0, cells[mainX, mainY].getXYForMe(index));
            while (a != mainX || b != mainY)
            {
                Point past = cells[mainX, mainY].getXYParent(index);
                a = past.X;
                b = past.Y;

                index = searchParent(mainX, mainY, a, b);
                
                way.Insert(0, cells[mainX, mainY].getXYForMe(index));
            }
            return way;
        }




        private List<Point> MoveAnimalAndEat(List<Point> d,int b, int n,int index)
        {

            List<Point> bvb = new List<Point>();
            for (int pol = 0; pol < d.Count; pol++)
            {

                Point m = new Point(d[pol].X,d[pol].Y);
                bvb.Add(m);
            }
                
            cells[d[0].X, d[0].Y].Animal = cells[b, n].Animal.copy();
            cells[d[0].X , d[0].Y].colorAnimal = cells[b,n].colorAnimal;
            cells[d[0].X, d[0].Y].Lock = true;

            cells[b, n].colorAnimal = ColorForMap.colorLess; 
            
            cells[bvb[0].X, bvb[0].Y].Animal.setHp(cells[b,n].Animal.getHp());
            cells[bvb[0].X, bvb[0].Y].Animal.setHungry(cells[b,n].Animal.getHungry());
            cells[b, n].Animal.way.Clear();
            cells[b, n].Animal = null;
            

            cells[bvb[0].X, bvb[0].Y].colorAnimal = ColorForMap.red; 
            cells[b,n].Lock = false;
            if (cells[bvb[0].X, bvb[0].Y].plant != null)
            {
                cells[bvb[0].X, bvb[0].Y].Animal.setHp(100);
                cells[bvb[0].X, bvb[0].Y].Animal.setHungry(150);
                cells[bvb[0].X, bvb[0].Y].plant = null;
                cells[bvb[0].X, bvb[0].Y].plantColor = ColorForMap.colorLess;
                cells[bvb[0].X, bvb[0].Y].Lock = false;

                for (int h = 0; h < pointsAnimal.Count; h++)
                {
                    if (pointsAnimal[h].X == b && pointsAnimal[h].Y == n)
                    {
                        pointsAnimal.RemoveAt(h);
                    }
                }
                for (int h = 0; h < pointsPlants.Count; h++)
                {
                    if (pointsPlants[h].X == bvb[0].X && pointsPlants[h].Y == bvb[0].Y)
                    {
                        pointsPlants.RemoveAt(h);
                    }
                }
                pointsAnimal.Add(new Point(bvb[0].X, bvb[0].Y));
                cells[bvb[0].X, bvb[0].Y].Animal.way.Clear();

            }
            else
            {

                for (int h = 0; h < pointsAnimal.Count; h++)
                {
                    if (pointsAnimal[h].X == b && pointsAnimal[h].Y == n)
                    {
                        pointsAnimal.RemoveAt(h);
                    }
                }
                pointsAnimal.Add(new Point(bvb[0].X, bvb[0].Y));
                if (cells[bvb[0].X, bvb[0].Y].Animal != null)
                {
                    cells[bvb[0].X, bvb[0].Y].Animal.setWay(bvb);
                }



            }







            if (cells[bvb[0].X, bvb[0].Y].Animal.way.Count != 0)
            {
                cells[bvb[0].X, bvb[0].Y].Animal.way.RemoveAt(0);
                return bvb;
            }
                
            bvb.Clear();
            return bvb;
        }







        public void killAnimalOrLive(Graphics g)
        {
            for (int i = 0; i < pointsAnimal.Count; i++)
            {
                if (cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHp() != 0)
                {
                    if (cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHungry() > 50)
                    {
                        cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.setHungry(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHungry() - 1);
                        moveAnimal(i);
                        
                        cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.way.Clear();
                    }
                    else if (cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHungry() <= 50)
                    {
                        if (cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHp() >= 0)
                        {
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal
                                .setHungry(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHungry() - 1);
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal
                                .setHp(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHp() - 1);
                            // Нахожу кратчайший путь до цветка
                            
                            if (cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.way.Count != 0)
                            {
                                cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.setWay(MoveAnimalAndEat(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getWay(), pointsAnimal[i].X, pointsAnimal[i].Y, i));
                            }
                            else
                            {
                                List<Point> beg = searchPlant(pointsAnimal[i].X, pointsAnimal[i].Y);

                                if (beg == null)
                                {
                                    cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal
                                        .setHungry(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHungry() - 1);
                                    cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal
                                        .setHp(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getHp() - 1);
                                }
                                else
                                {
                                    cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal
                                        .setWay(searchPlant(pointsAnimal[i].X, pointsAnimal[i].Y));

                                    cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.way.RemoveAt(0);
                                    cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.setWay(
                                        MoveAnimalAndEat(cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal.getWay(),
                                            pointsAnimal[i].X, pointsAnimal[i].Y, i));
                                }
                            }
                            
                            Point ce = cells[pointsAnimal[i].X, pointsAnimal[i].Y].getXYForMe(0);
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].allCells.Clear();
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].addWith100(ce);
                        }
                        else
                        {
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].Animal = null;
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].colorAnimal = ColorForMap.colorLess;
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].Lock = false;
                            Point ce = cells[pointsAnimal[i].X, pointsAnimal[i].Y].getXYForMe(0);
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].allCells.Clear();
                            cells[pointsAnimal[i].X, pointsAnimal[i].Y].addWith100(ce);
                            pointsAnimal.RemoveAt(i);
                        }
                    }
                }
            }
        }

    }
}