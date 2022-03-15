using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using lab2.Properties;


namespace lab2
{
    public class Map
    {
        public int numOfCells { get; private set; } = 1000;
        public List<Plant> pointsPlants = new List<Plant>();
        public List<Animal> pointsAnimal = new List<Animal>();
        public List<Fruit> pointsFruits = new List<Fruit>();
        public List<Sour<Gold>> pointsGold = new List<Sour<Gold>>();
        public List<Sour<Iron>> pointsIron = new List<Sour<Iron>>();
        public List<Sour<Copper>> pointsCopper = new List<Sour<Copper>>();
        public List<Sour<Rock>> pointsRock = new List<Sour<Rock>>();
        
        
        
        
        public List<Animal> deletedAnimals = new List<Animal>();
        public List<Cell> deletedPlants = new List<Cell>();
        public List<House> allHouse = new List<House>();
        private Random rnd = new Random();
        private Random rndResource = new Random();
        public Cell[,] cells = new Cell[1000, 1000];
        private const int ratioOfFoodWinter = 2;
        private const int ratioOfFoodSummer = 1;
        private int TimerForChangeSeason = 200;
        public Seasons seasons { get; private set; }

        public Map()
        {
            seasons = Seasons.Summer;
            GenerateMap();
            MakeNewAnimal();
            MakeNewPlant();
            MakeNewResource();
        }


        private void GenerateMap()
        {
            for (int i = 0; i < numOfCells; i++)
            {
                for (int j = 0; j < numOfCells; j++)
                {
                    cells[i, j] = new Cell(this, Biom.Field, i, j);
                    if ((i - 99) * (i - 99) + j * j > 500000)
                    {
                        cells[i, j] = new Cell(this, Biom.Water, i, j);
                    }
                }
            }

            for (int i = 0; i < 30; i++)
            {
                int newX = rnd.Next(20, 448);
                int newY = rnd.Next(2, 488);
                int newR = rnd.Next(5, 88);
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
                            cells[pX, pY] = new Cell(this, Biom.Forest, pX, pY);
                        }
                    }
                }
            }
        }


        private void MakeNewAnimal()
        {
            for (var i = 0; i < numOfCells; i++)
            {
                for (var j = 0; j < numOfCells; j++)
                {
                    if (cells[i, j].GetBiom() == Biom.Forest && rnd.Next(0, 500) == 5)
                    {
                        int typeAnimal = rnd.Next(0, 4);
                         if (typeAnimal == 0)
                         {
                             cells[i, j].SetNewAnimal(0);
                         }
                         else if (typeAnimal == 1)
                         {
                             cells[i, j].SetNewAnimal(1);
                         }
                        
                         if (typeAnimal == 2)
                         {
                             cells[i, j].SetNewAnimal(2);
                         }

                        if (typeAnimal == 3)
                        {
                            cells[i, j].SetNewAnimal(3);
                        }


                        pointsAnimal.AddRange(cells[i, j].GetAnimal());
                    }
                }
            }
        }
        private void MakeNewResource()
        {
            for (var i = 0; i < numOfCells; i++)
            {
                for (var j = 0; j < numOfCells; j++)
                {
                    if (cells[i, j].GetAnimal().Any() == false && rnd.Next(0, 100) == 5)
                    {
                        int typeRes = rndResource.Next(0, 4);
                        switch (typeRes)
                        {
                            case 0:
                                cells[i,j].SetGoldResource();
                                pointsGold.Add(cells[i,j].GetGoldResource());
                                break;
                            case 1:
                                cells[i,j].SetIronResource();
                                pointsIron.Add(cells[i,j].GetIronResource());
                                break;
                            case 2:
                                cells[i, j].SetCopperResource();
                                pointsCopper.Add(cells[i,j].GetCopperResource());
                                break;
                            case 3:
                                cells[i,j].SetRockResource();
                                pointsRock.Add(cells[i,j].GetRockResource());
                                break;
                        }
                        

                        pointsAnimal.AddRange(cells[i, j].GetAnimal());
                    }
                }
            }
        }
        

        private void MakeNewPlant()
        {
            for (var i = 0; i < numOfCells; i++)
            {
                for (var j = 0; j < numOfCells; j++)
                {
                    if (cells[i, j].GetBiom() != Biom.Water && cells[i, j].GetAnimal().Any() == false &&
                        rnd.Next(0, 300) == 5)
                    {
                        cells[i, j].SetPlant(rnd.Next(2) == 1, rnd.Next(2) == 1);
                        pointsPlants.Add(cells[i, j].GetPlant());
                    }
                }
            }
        }


        public void GlobalGame()
        {
            deletedAnimals = new List<Animal>();
            deletedPlants = new List<Cell>();

            switch (TimerForChangeSeason)
            {
                case > 0:
                    TimerForChangeSeason -= 1;
                    break;
                case 0:
                    TimerForChangeSeason = 200;
                    switch (seasons)
                    {
                        case Seasons.Summer:
                            seasons = Seasons.Winter;
                            break;
                        case Seasons.Winter:
                            seasons = Seasons.Summer;
                            break;
                    }

                    break;
            }

            GlobalGameForAnimal();
            GlobalGameForPlant();
        }

        private void GlobalGameForAnimal()
        {
            foreach (var animal in pointsAnimal.ToList())
            {
                if (animal is Man)
                {
                    ((Man) animal).IWantEatOrNot();
                }
                else
                {
                    switch (seasons)
                    {
                        case Seasons.Winter when !animal.GetSleepWinter():
                            animal.IWantEatOrNot(ratioOfFoodWinter);
                            break;
                        case Seasons.Winter when animal.GetSleepWinter():
                        {
                            if (TimerForChangeSeason == 200)
                            {
                                animal.GoInSleep();
                            }

                            animal.SetHp(animal.GetHp() + 1);
                            break;
                        }
                        case Seasons.Summer:
                            animal.IWantEatOrNot(ratioOfFoodSummer);
                            break;
                    }
                }
                
            }
        }

        private void GlobalGameForPlant()
        {
            foreach (var plant in pointsPlants.ToList())
            {
                switch (seasons)
                {
                    case Seasons.Winter:
                        plant.WinterForPlant();
                        break;
                    case Seasons.Summer:
                        plant.CheckGrowth();
                        break;
                }
            }
        }
    }
}