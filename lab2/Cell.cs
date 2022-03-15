using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lab2.Properties;

namespace lab2
{
    public class Cell
    {
        public List<Animal> animal = new List<Animal>();
        public Plant plant;
        public Map _map;
        private Biom biom;
        private Fruit fruit;
        private Random randomForCreateAnimalType = new Random();
        private Random randomForGenderAnimal = new Random();
        private Random randomForSleepInWinter = new Random();
        private Animal meat;
        public Sour<Gold> itemGold;
        public Sour<Iron> itemIron;
        public Sour<Copper> itemCopper;
        public Sour<Rock> itemRock;

        public void SetGoldResource()
        {
            itemGold = new Sour<Gold>(this);
        }
        public void SetIronResource()
        {
            itemIron = new Sour<Iron>(this);
        }
        public void SetCopperResource()
        {
            itemCopper = new Sour<Copper>(this);
        }
        public void SetRockResource()
        {
            itemRock = new Sour<Rock>(this);
        }
        public Sour<Gold> GetGoldResource()
        {
            return itemGold;
        }
        public Sour<Iron> GetIronResource()
        {
            return itemIron;
        }
        public Sour<Copper> GetCopperResource()
        {
            return itemCopper;
        }
        public Sour<Rock> GetRockResource()
        {
            return itemRock;
        }
        

        private House building;
        public int X { get; }
        public int Y { get; }

        public Animal GetMeat()
        {
            return meat;
        }
        public void SetMeat(Animal _meat)
        {
            meat = _meat;
        }

        public Cell(Map map, Biom biom, int x, int y)
        {
            this.animal = new List<Animal>();
            this.plant = null;
            _map = map;
            this.biom = biom;
            X = x;
            Y = y;
        }

        public Biom GetBiom()
        {
            return biom;
        }

        public void AddNewAnimalForReproduction(Animal parent)
        {
            if (parent is Cornivourus)
            {
                GetAnimal().Add(new Cornivourus(this, null, null, ((Cornivourus) parent).GetTypeAnimal(),
                    (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),parent.GetSleepWinter()));
                _map.pointsAnimal.Add(animal.Last());
            }
            else if (parent is Omnivourus)
            {
                GetAnimal().Add(new Omnivourus(this, null, null, ((Omnivourus) parent).GetTypeAnimal(),
                    (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),parent.GetSleepWinter()));
                _map.pointsAnimal.Add(animal.Last());
            }
            else if (parent is Herbivore)
            {
                GetAnimal().Add(new Herbivore(this, null, null, ((Herbivore) parent).GetTypeAnimal(),
                    (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),parent.GetSleepWinter()));
                _map.pointsAnimal.Add(animal.Last());
            }
        }


        public void SetNewAnimal(int type)
        {
            switch (type)
            {
                case 0:
                {
                    switch (randomForCreateAnimalType.Next(0, 3))
                    {
                        case 0:
                        {
                            animal.Add(new Cornivourus(this, null, null, CornivourusTypes.Tiger,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 1:
                        {
                            animal.Add(new Cornivourus(this, null, null, CornivourusTypes.Fox,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 2:
                        {
                            animal.Add(new Cornivourus(this, null, null, CornivourusTypes.Wolf,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                    }

                    break;
                }

                case 1:
                {
                    switch (randomForCreateAnimalType.Next(0, 3))
                    {
                        case 0:
                        {
                            animal.Add(new Omnivourus(this, null, null, OmnivourusTypes.Bear,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 1:
                        {
                            animal.Add(new Omnivourus(this, null, null, OmnivourusTypes.Crow,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 2:
                        {
                            animal.Add(new Omnivourus(this, null, null, OmnivourusTypes.Monkey,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                    }

                    break;
                }

                case 2:
                {
                    switch (randomForCreateAnimalType.Next(0, 3))
                    {
                        case 0:
                        {
                            animal.Add(new Herbivore(this, null, null, HerbivoreTypes.Elephant,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 1:
                        {
                            animal.Add(new Herbivore(this, null, null, HerbivoreTypes.Rabbit,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                        case 2:
                        {
                            animal.Add(new Herbivore(this, null, null, HerbivoreTypes.Zebra,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                    }

                    break;
                }
                
                case 3:
                {
                    switch (randomForCreateAnimalType.Next(0, 2))
                    {
                        case 0:
                        {
                            animal.Add(new Man(this, null, null, OmnivourusTypes.Man,
                                (Gender) randomForGenderAnimal.Next(Enum.GetNames(typeof(Gender)).Length),randomForSleepInWinter.Next(2) == 1));
                            break;
                        }
                    }

                    break;
                }
            }
        }

        public void SetAnimalForKill(Animal animal)
        {
            this.animal.Remove(animal);
        }

        public List<Animal> GetAnimal()
        {
            return animal;
        }

        public void SetPlant(bool reproducing, bool poisonous)
        {
            plant = new Plant(this, reproducing, poisonous);
        }
        // public void SetResource(int typeRes)
        // {
        //     switch (typeRes)
        //     {
        //         case 0:
        //             item = new Sour<Resource>();
        //     }
        //
        // }

        public Plant GetPlant()
        {
            return plant;
        }

        public void SetFruit(bool edible)
        {
            fruit = new Fruit(this, edible);
        }

        public void SetFruitForDead()
        {
            this.fruit = null;
        }

        public Fruit GetFruit()
        {
            return fruit;
        }

        public void SetAnimalForMove(Animal animal)
        {
            this.animal.Remove(animal);
        }
        public void SetHouse()
        {
            building = new House(this);
        }
        public House GetHouse()
        {
            return building;
        }
        
    }
}