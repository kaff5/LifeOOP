using System;
using System.Linq;

namespace lab2
{
    public class Plant: AddInBag
    {
        private int timerForDead;
        public PlantCycle plantCycle { get; private set; }
        private bool edible { get; }
        public bool poisonous{ get; private set; }
        public bool reproducing{ get; private set; }
        
        private const int germ = 150;
        private const int flower = 100;
        private const int dried = 50;
        private const int forSrinkle = 20;
        private const int forReproducing = 70;
        private const int plantDead = 1;
        private int liveCycleFruit;

        public Cell _cell;
        public Plant(Cell cell,bool reproducing,bool poisonous)
        {
            timerForDead = 250;
            plantCycle = PlantCycle.Seed;
            _cell = cell;
            this.reproducing = reproducing;
            this.poisonous = poisonous;
        }

        private void ReduceTimerForDead()
        {
            timerForDead -= 1;
        }





        public void CheckGrowth()
        {
            ReduceTimerForDead();
            
            if (timerForDead <= germ)
            {
                plantCycle = PlantCycle.Germ;
                if (timerForDead <= flower)
                {
                    plantCycle = PlantCycle.Flower;
                    liveCycleFruit += 1;
                    if (liveCycleFruit == forSrinkle)
                    {
                        FallFruits();
                    }

                    if (timerForDead == forReproducing)
                    {
                        ReproducingPlants();
                    }
                    if (timerForDead <= dried)
                    {
                        plantCycle = PlantCycle.DriedPlant;
                    }
                }
            }
            
            if (timerForDead <= plantDead)
            {
                PlantDead();
            }
        }

        public void WinterForPlant()
        {
            if (poisonous && reproducing && timerForDead <= germ )
            {
                PlantDead();
            }
        }


        public void PlantDead()
        {
            _cell.plant = null;
            _cell._map.deletedPlants.Add(_cell);
            _cell._map.pointsPlants.Remove(this);
        }


        private bool CheckForFallFruits(Sides side,int rand)
        {
            if ((side == Sides.up) && (rand == 3) && (_cell.Y >= 1) &&
                !(_cell._map.cells[_cell.X, _cell.Y - 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X, _cell.Y - 1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X, _cell.Y - 1].GetBiom() != Biom.Water) && 
                _cell._map.cells[_cell.X, _cell.Y - 1].GetFruit() == null)
                return true;
            if ((side == Sides.left) && (rand == 2) && (_cell.X >= 1) &&
                !(_cell._map.cells[_cell.X - 1, _cell.Y].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y].GetPlant() == null) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X - 1, _cell.Y].GetFruit() == null)
                return true;
            if ((side == Sides.right) && (rand == 1) && (_cell.X < _cell._map.numOfCells - 1) &&
                !(_cell._map.cells[_cell.X + 1, _cell.Y].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y].GetPlant() == null) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X + 1, _cell.Y].GetFruit() == null)
                return true;
            if ((side == Sides.down) && (rand == 0) && (_cell.Y < _cell._map.numOfCells - 1) &&
                !(_cell._map.cells[_cell.X, _cell.Y + 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X, _cell.Y + 1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X, _cell.Y + 1].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X, _cell.Y + 1].GetFruit() == null)
                return true;
            return false;
        }



        private void FallFruits()
        {
            Random randomForMove = new Random();
            
            if (CheckForFallFruits(Sides.up, randomForMove.Next(0, 5)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.SetFruit(_cell.plant.poisonous);
                _cell._map.pointsFruits.Add(newCell.GetFruit());
            }

            else if (CheckForFallFruits(Sides.left, randomForMove.Next(0, 5)))
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y];
                newCell.SetFruit(_cell.plant.poisonous);
                _cell._map.pointsFruits.Add(newCell.GetFruit());
            }

            if (CheckForFallFruits(Sides.right, randomForMove.Next(0, 5)))
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y];
                newCell.SetFruit(_cell.plant.poisonous);
                _cell._map.pointsFruits.Add(newCell.GetFruit());
            }
            else if (CheckForFallFruits(Sides.down, randomForMove.Next(0, 5)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.SetFruit(_cell.plant.poisonous);
                _cell._map.pointsFruits.Add(newCell.GetFruit());
            }
        }



        private bool CheckCanReproducingInCell(Sides side,int rand)
        {
            if ((side ==Sides.upLeft) && (_cell.Y >= 1) && (_cell.X >= 1) &&
                !(_cell._map.cells[_cell.X - 1, _cell.Y - 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X-1, _cell.Y - 1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X-1, _cell.Y - 1].GetBiom() != Biom.Water) && 
                _cell._map.cells[_cell.X-1, _cell.Y - 1].GetFruit() == null)
                return true;
            if ((side == Sides.downLeft) && (rand == 0) && (_cell.X >= 1) && (_cell.Y < _cell._map.numOfCells - 1) && 
                !(_cell._map.cells[_cell.X - 1, _cell.Y + 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y+1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y+1].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X - 1, _cell.Y+1].GetFruit() == null)
                return true;
            if ((side == Sides.upRight) && (rand == 1) && (_cell.Y >= 1) && (_cell.X < _cell._map.numOfCells - 1) &&
                !(_cell._map.cells[_cell.X + 1, _cell.Y - 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y - 1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y - 1].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X + 1, _cell.Y - 1].GetFruit() == null)
                return true;
            if ((side == Sides.downRight) && (_cell.Y < _cell._map.numOfCells - 1) && (_cell.X < _cell._map.numOfCells - 1) && 
                !(_cell._map.cells[_cell.X+1, _cell.Y + 1].GetAnimal().Any()) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y + 1].GetPlant() == null) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y + 1].GetBiom() != Biom.Water)&& 
                _cell._map.cells[_cell.X + 1, _cell.Y + 1].GetFruit() == null)
                return true;
            return false;
        }
        
        
        private void ReproducingPlants()
        {
            Random randomForMove = new Random();
            
            if (CheckCanReproducingInCell(Sides.upLeft, randomForMove.Next(0, 4)) && reproducing != false)
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y - 1];
                newCell.SetPlant(reproducing,poisonous);
                _cell._map.pointsPlants.Add(newCell.GetPlant());
            }
            if (CheckCanReproducingInCell(Sides.downLeft, randomForMove.Next(0, 4)) && reproducing != false)
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y + 1];
                newCell.SetPlant(reproducing,poisonous);
                _cell._map.pointsPlants.Add(newCell.GetPlant());
            }

            if (CheckCanReproducingInCell(Sides.upRight, randomForMove.Next(0, 4)) && reproducing != false)
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y - 1];
                newCell.SetPlant(reproducing,poisonous);
                _cell._map.pointsPlants.Add(newCell.GetPlant());
            }
            
            if (CheckCanReproducingInCell(Sides.downRight, randomForMove.Next(0, 4)) && reproducing != false)
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y + 1];
                newCell.SetPlant(reproducing,poisonous);
                _cell._map.pointsPlants.Add(newCell.GetPlant());
            }
        }
    }
}