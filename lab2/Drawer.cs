using System;
using System.Configuration;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace lab2
{
    public class Drawer
    {
        private SolidBrush Brush;
        private Rectangle Rect;
        private Map mapForDraw;
        private Graphics _graphics;
        private readonly int sizeCell = 3;


        private Bitmap Tiger = new(Properties.Resources.Tiger);
        private Bitmap Fox = new(Properties.Resources.Fox);
        private Bitmap Wolf = new(Properties.Resources.Wiu2Q0Phgco);
        private Bitmap Monkey = new(Properties.Resources.Monkey);
        private Bitmap Bear = new(Properties.Resources.Bear);
        private Bitmap Crow = new(Properties.Resources.Crow);
        private Bitmap Zebra = new(Properties.Resources.images);
        private Bitmap Elephant = new(Properties.Resources.Elephant);
        private Bitmap Rabbit = new(Properties.Resources.rabbit);
        private Bitmap Mans = new(Properties.Resources.Rock);

        private Bitmap SeedPosRep = new(Properties.Resources.SeedPeashooter1);
        private Bitmap SeedPos_Rep = new(Properties.Resources.SeedSun_shroom2);
        private Bitmap SeedPosRep_ = new(Properties.Resources.SeedSunflower1);
        private Bitmap SeedPos_Rep_ = new(Properties.Resources.SeedMissile_Toe2);
        private Bitmap GermPosRep = new(Properties.Resources.GermConceal_mint2);
        private Bitmap GermPos_Rep = new(Properties.Resources.GermCold_Snapdragon2);
        private Bitmap GermPosRep_ = new(Properties.Resources.GermFire_Peashooter2);
        private Bitmap GermPos_Rep_ = new(Properties.Resources.GermGold_Leaf2);
        private Bitmap FlowerPosRep = new(Properties.Resources.FlowerTorchwood2);
        private Bitmap FlowerPos_Rep = new(Properties.Resources.FlowerWall_nut1);
        private Bitmap FlowerPosRep_ = new(Properties.Resources.FlowerPower_Lily2);
        private Bitmap FlowerPos_Rep_ = new(Properties.Resources.FlowerSweet_Potato2);
        private Bitmap DriedPosRep = new(Properties.Resources.Dried);

        private Bitmap FruitPos_ = new(Properties.Resources.FruitPos_);
        private Bitmap FruitPos = new(Properties.Resources.FruitPos);


        public Drawer(Map map, Graphics graphics)
        {
            mapForDraw = map;
            _graphics = graphics;
            DrawAllMap();
        }

        private void DrawPixel(int i, int j, Color color)
        {
            Rect = new Rectangle(sizeCell * i, sizeCell * j, sizeCell, sizeCell);
            Brush = new SolidBrush(color);
            _graphics.FillRectangle(Brush, Rect);
        }


        private void DrawAnimals()
        {
            foreach (var animal in mapForDraw.pointsAnimal)
            {
                switch (animal)
                {
                    case Cornivourus cornivourus:
                    {
                        if (cornivourus.GetTypeAnimal() == CornivourusTypes.Fox)
                            _graphics.DrawImage(Fox, sizeCell * cornivourus._cell.X, sizeCell * cornivourus._cell.Y,
                                sizeCell, sizeCell);
                        if (cornivourus.GetTypeAnimal() == CornivourusTypes.Tiger)
                            _graphics.DrawImage(Tiger, sizeCell * cornivourus._cell.X, sizeCell * cornivourus._cell.Y,
                                sizeCell, sizeCell);
                        if (cornivourus.GetTypeAnimal() == CornivourusTypes.Wolf)
                            _graphics.DrawImage(Wolf, sizeCell * cornivourus._cell.X, sizeCell * cornivourus._cell.Y,
                                sizeCell, sizeCell);
                        break;
                    }
                    case Herbivore herbivore:
                    {
                        if (herbivore.GetTypeAnimal() == HerbivoreTypes.Elephant)
                            _graphics.DrawImage(Elephant, sizeCell * herbivore._cell.X, sizeCell * herbivore._cell.Y,
                                sizeCell, sizeCell);
                        if (herbivore.GetTypeAnimal() == HerbivoreTypes.Rabbit)
                            _graphics.DrawImage(Rabbit, sizeCell * herbivore._cell.X, sizeCell * herbivore._cell.Y,
                                sizeCell, sizeCell);
                        if (herbivore.GetTypeAnimal() == HerbivoreTypes.Zebra)
                            _graphics.DrawImage(Zebra, sizeCell * herbivore._cell.X, sizeCell * herbivore._cell.Y,
                                sizeCell, sizeCell);
                        break;
                    }
                    case Omnivourus omnivourus:
                    {
                        if (animal is Man)
                        {
                            _graphics.DrawImage(Mans, sizeCell * animal._cell.X, sizeCell * animal._cell.Y,
                                sizeCell, sizeCell);
                            break;
                        }

                        if (omnivourus.GetTypeAnimal() == OmnivourusTypes.Bear)
                            _graphics.DrawImage(Bear, sizeCell * omnivourus._cell.X, sizeCell * omnivourus._cell.Y,
                                sizeCell, sizeCell);
                        if (omnivourus.GetTypeAnimal() == OmnivourusTypes.Crow)
                            _graphics.DrawImage(Crow, sizeCell * omnivourus._cell.X, sizeCell * omnivourus._cell.Y,
                                sizeCell, sizeCell);
                        if (omnivourus.GetTypeAnimal() == OmnivourusTypes.Monkey)
                            _graphics.DrawImage(Monkey, sizeCell * omnivourus._cell.X, sizeCell * omnivourus._cell.Y,
                                sizeCell, sizeCell);
                        break;
                    }
                }

                if (animal.GetPreviousCell() != null)
                {
                    if (animal.GetPreviousCell().X != animal._cell.X || animal.GetPreviousCell().Y != animal._cell.Y)
                    {
                        switch (animal.GetPreviousCell().GetBiom())
                        {
                            case Biom.Field:
                                switch (mapForDraw.seasons)
                                {
                                    case Seasons.Summer:
                                        DrawPixel(animal.GetPreviousCell().X, animal.GetPreviousCell().Y,
                                            Color.BlanchedAlmond);
                                        break;
                                    case Seasons.Winter:
                                        DrawPixel(animal.GetPreviousCell().X, animal.GetPreviousCell().Y, Color.Gray);
                                        break;
                                }

                                break;


                            case Biom.Forest:
                                DrawPixel(animal.GetPreviousCell().X, animal.GetPreviousCell().Y, Color.DarkGreen);
                                break;

                            case Biom.Water:
                                DrawPixel(animal.GetPreviousCell().X, animal.GetPreviousCell().Y, Color.Blue);
                                break;
                        }
                    }
                }
            }
        }

        private void DrawDeletedAnimals()
        {
            foreach (var deletedAnimal in mapForDraw.deletedAnimals)
            {
                switch (deletedAnimal._cell.GetBiom())
                {
                    case Biom.Field:
                        switch (mapForDraw.seasons)
                        {
                            case Seasons.Summer:
                                DrawPixel(deletedAnimal._cell.X, deletedAnimal._cell.Y, Color.BlanchedAlmond);
                                break;
                            case Seasons.Winter:
                                DrawPixel(deletedAnimal._cell.X, deletedAnimal._cell.Y, Color.Gray);
                                break;
                        }

                        break;

                    case Biom.Forest:
                        DrawPixel(deletedAnimal._cell.X, deletedAnimal._cell.Y, Color.DarkGreen);
                        break;

                    case Biom.Water:
                        DrawPixel(deletedAnimal._cell.X, deletedAnimal._cell.Y, Color.Blue);
                        break;
                }
            }
        }

        private void DrawDeletePlants()
        {
            foreach (var deletedPlant in mapForDraw.deletedPlants)
            {
                switch (deletedPlant.GetBiom())
                {
                    case Biom.Field:

                        switch (mapForDraw.seasons)
                        {
                            case Seasons.Summer:
                                DrawPixel(deletedPlant.X, deletedPlant.Y, Color.BlanchedAlmond);
                                break;
                            case Seasons.Winter:
                                DrawPixel(deletedPlant.X, deletedPlant.Y, Color.Gray);
                                break;
                        }

                        break;

                    case Biom.Forest:
                        DrawPixel(deletedPlant.X, deletedPlant.Y, Color.DarkGreen);
                        break;

                    default:
                        DrawPixel(deletedPlant.X, deletedPlant.Y, Color.Blue);
                        break;
                }
            }
        }

        private void DrawPlants()
        {
            foreach (var plant in mapForDraw.pointsPlants)
            {
                switch (plant.plantCycle)
                {
                    case PlantCycle.Seed:
                        if (plant.reproducing && plant.poisonous) // размножающееся, ядовитое
                        {
                            _graphics.DrawImage(SeedPosRep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing && plant.poisonous == false)
                        {
                            _graphics.DrawImage(SeedPos_Rep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false && plant.poisonous)
                        {
                            _graphics.DrawImage(SeedPosRep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false && plant.poisonous == false)
                        {
                            _graphics.DrawImage(SeedPos_Rep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        break;


                    case PlantCycle.Germ:
                        if (plant.reproducing && plant.poisonous)
                        {
                            _graphics.DrawImage(GermPosRep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing &&
                                 plant.poisonous == false)
                        {
                            _graphics.DrawImage(GermPos_Rep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false &&
                                 plant.poisonous)
                        {
                            _graphics.DrawImage(GermPosRep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false &&
                                 plant.poisonous == false)
                        {
                            _graphics.DrawImage(GermPos_Rep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        break;

                    case PlantCycle.Flower:
                        if (plant.reproducing && plant.poisonous)
                        {
                            _graphics.DrawImage(FlowerPosRep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing && plant.poisonous == false)
                        {
                            _graphics.DrawImage(FlowerPos_Rep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false && plant.poisonous)
                        {
                            _graphics.DrawImage(FlowerPosRep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        else if (plant.reproducing == false && plant.poisonous == false)
                        {
                            _graphics.DrawImage(FlowerPos_Rep_, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                                sizeCell, sizeCell);
                        }

                        break;

                    case PlantCycle.DriedPlant:
                        _graphics.DrawImage(DriedPosRep, sizeCell * plant._cell.X, sizeCell * plant._cell.Y,
                            sizeCell, sizeCell);
                        break;
                }
            }
        }


        private void DrawFruits()
        {
            foreach (var fruit in mapForDraw.pointsFruits)
            {
                if (fruit.poisonous == true)
                {
                    _graphics.DrawImage(FruitPos, sizeCell * fruit._cell.X, sizeCell * fruit._cell.Y,
                        sizeCell, sizeCell);
                }
                else
                {
                    _graphics.DrawImage(FruitPos_, sizeCell * fruit._cell.X, sizeCell * fruit._cell.Y,
                        sizeCell, sizeCell);
                }
            }
        }

        private void DrawHouses()
        {
            foreach (var houses in mapForDraw.allHouse)
            {
                DrawPixel(houses._cell.X, houses._cell.Y, Color.Red);
            }
        }

        private void DrawResources()
        {
            foreach (var copper in mapForDraw.pointsCopper)
            {
                DrawPixel(copper.cell.X, copper.cell.Y, Color.Orange);
            }
            foreach (var Iron in mapForDraw.pointsIron)
            {
                DrawPixel(Iron.cell.X, Iron.cell.Y, Color.LightGray);
            }
            foreach (var Gold in mapForDraw.pointsGold)
            {
                DrawPixel(Gold.cell.X, Gold.cell.Y, Color.Yellow);
            }
            foreach (var Rock in mapForDraw.pointsRock)
            {
                DrawPixel(Rock.cell.X, Rock.cell.Y, Color.DarkGray);
            }
        }

        public void DrawItemGlobalGame()
        {
            
            DrawAnimals();
            DrawDeletedAnimals();
            DrawDeletePlants();
            DrawPlants();
            DrawHouses();
            DrawResources();
        }


        public void ChangeSeason()
        {
            for (int i = 0; i < mapForDraw.numOfCells; i++)
            {
                for (int j = 0; j < mapForDraw.numOfCells; j++)
                {
                    if (mapForDraw.cells[i, j].GetBiom() == Biom.Field)
                    {
                        switch (mapForDraw.seasons)
                        {
                            case Seasons.Summer:
                                DrawPixel(i, j, Color.BlanchedAlmond);
                                break;
                            case Seasons.Winter:
                                DrawPixel(i, j, Color.Gray);
                                break;
                        }
                    }
                }
            }
        }

        private void DrawAllMap()
        {
            for (int i = 0; i < mapForDraw.numOfCells; i++)
            {
                for (int j = 0; j < mapForDraw.numOfCells; j++)
                {
                    switch (mapForDraw.cells[i, j].GetBiom())
                    {
                        case Biom.Field:
                            switch (mapForDraw.seasons)
                            {
                                case Seasons.Summer:
                                    DrawPixel(i, j, Color.BlanchedAlmond);
                                    break;
                                case Seasons.Winter:
                                    DrawPixel(i, j, Color.Gray);
                                    break;
                            }

                            break;

                        case Biom.Forest:
                            DrawPixel(i, j, Color.DarkGreen);
                            break;

                        case Biom.Water:
                            DrawPixel(i, j, Color.Blue);
                            break;
                    }
                }
            }
        }
    }
}