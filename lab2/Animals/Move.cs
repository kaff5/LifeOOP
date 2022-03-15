using System;
using System.Linq;

namespace lab2
{
    public class Move
    {
        
        
        private static bool CheckMove(Cell _cell,Sides side, int rand)
        {
            if ((side == Sides.upLeft) && (rand == 3) && (_cell.Y >= 1) && (_cell.X >= 1) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y - 1].GetBiom() != Biom.Water))
                return true;
            
            if ((side == Sides.downLeft) && (rand == 2) && (_cell.X >= 1) && (_cell.Y < _cell._map.numOfCells - 1) &&
                (_cell._map.cells[_cell.X - 1, _cell.Y + 1].GetBiom() != Biom.Water))
                return true;
            
            if ((side == Sides.upRight) && (rand == 1) && (_cell.Y >= 1) && (_cell.X < _cell._map.numOfCells - 1) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y - 1].GetBiom() != Biom.Water))
                return true;
            
            if ((side == Sides.downRight) && (rand == 0) && (_cell.Y < _cell._map.numOfCells - 1) && (_cell.X < _cell._map.numOfCells - 1) &&
                (_cell._map.cells[_cell.X+1, _cell.Y + 1].GetBiom() != Biom.Water))
                return true;
            
            if (
                (side == Sides.up)
                && (rand == 3)
                && (_cell.Y >= 1)
                && (_cell._map.cells[_cell.X, _cell.Y - 1].GetBiom() != Biom.Water)
            )
                return true;
            
            if ((side == Sides.left) && (rand == 2) && (_cell.X >= 1) && 
                (_cell._map.cells[_cell.X - 1, _cell.Y].GetBiom() != Biom.Water))
                return true;
            
            if ((side == Sides.right) && (rand == 1) && (_cell.X < _cell._map.numOfCells - 1) &&
                (_cell._map.cells[_cell.X + 1, _cell.Y].GetBiom() != Biom.Water))
                return true;
            
            if ((side == Sides.down) && (rand == 0) && (_cell.Y < _cell._map.numOfCells - 1) &&
                (_cell._map.cells[_cell.X, _cell.Y + 1].GetBiom() != Biom.Water))
                return true;

            return false;
        }
        
        
        
        
        public static void MoveAnimalWithoutEatOrthogonal(Animal currentAnimal,Cell _cell)
        {
            Random randomForMove = new Random();
            
            currentAnimal.SetPreviousCell(_cell);
            if (CheckMove(_cell,Sides.up, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.left, randomForMove.Next(0, 10)))
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.right, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (CheckMove(_cell,Sides.down, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
        }

        
        

        public static void MoveAnimalWithoutEatDiagonal(Animal currentAnimal,Cell _cell)
        {
            Random randomForMove = new Random();
            
            currentAnimal.SetPreviousCell(_cell);
            if (CheckMove(_cell,Sides.upLeft, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.downLeft, randomForMove.Next(0, 10)))
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.upRight, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (CheckMove(_cell,Sides.downRight, randomForMove.Next(0, 10)))
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
        }
        
        

        public static void MoveAnimalWithoutEatDiagonalAndOrthogonal(Animal currentAnimal,Cell _cell)
        {
            Random randomForMove = new Random();
            
            currentAnimal.SetPreviousCell(_cell);
            if (CheckMove(_cell,Sides.upLeft, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.downLeft, randomForMove.Next(0, 20)))
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.upRight, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (CheckMove(_cell,Sides.downRight, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (CheckMove(_cell,Sides.up, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.left, randomForMove.Next(0, 20)))
            {

                Cell newCell = _cell._map.cells[_cell.X-1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (CheckMove(_cell,Sides.right, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X+1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (CheckMove(_cell,Sides.down, randomForMove.Next(0, 20)))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }


            if (currentAnimal is Man)
            {
                if (((Man)currentAnimal).GetBagForRes().Count < 10)
                {
                    if (currentAnimal._cell.GetCopperResource() != null)
                    {
                        ((Man)currentAnimal).AddInBagForRes(currentAnimal._cell.GetCopperResource().elem);
                        currentAnimal._cell._map.pointsCopper.Remove(currentAnimal._cell.GetCopperResource());
                        currentAnimal._cell.itemCopper = null;
                    }
                    else if (currentAnimal._cell.GetIronResource() != null)
                    {
                        ((Man)currentAnimal).AddInBagForRes(currentAnimal._cell.GetIronResource().elem);
                        currentAnimal._cell._map.pointsIron.Remove(currentAnimal._cell.GetIronResource());
                        currentAnimal._cell.itemIron = null;
                    }
                    else if (currentAnimal._cell.GetGoldResource() != null)
                    {
                        ((Man)currentAnimal).AddInBagForRes(currentAnimal._cell.GetGoldResource().elem);
                        currentAnimal._cell._map.pointsGold.Remove(currentAnimal._cell.GetGoldResource());
                        currentAnimal._cell.itemGold = null;
                    }
                    else if (currentAnimal._cell.GetRockResource() != null)
                    {
                        ((Man)currentAnimal).AddInBagForRes(currentAnimal._cell.GetRockResource().elem);
                        currentAnimal._cell._map.pointsRock.Remove(currentAnimal._cell.GetRockResource());
                        currentAnimal._cell.itemRock = null;
                    }
                }
            }
        }
        
        
        public static void MoveToForReproduction(Animal currentAnimal, Animal target,Cell _cell)
        {
            currentAnimal.SetPreviousCell(_cell);
            if (_cell.X > target._cell.X)
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X < target._cell.X)
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (_cell.Y > target._cell.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.Y < target._cell.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X == target._cell.X && _cell.Y == target._cell.Y)
            {
                foreach (var animal in _cell.GetAnimal())
                {
                    if (animal != currentAnimal)
                    {
                        currentAnimal.Reproduction();
                        return;
                    }
                }

                currentAnimal.FreeMove();
                currentAnimal.SetAnimalForReproduction(null);
            }
        }


        public static void MoveToEatForCorn(Animal currentAnimal, Animal target,Cell _cell)
        {
            currentAnimal.SetPreviousCell(_cell);
            if (_cell.X > target._cell.X)
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X < target._cell.X)
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (_cell.Y > target._cell.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.Y < target._cell.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X == target._cell.X && _cell.Y == target._cell.Y)
            {
                foreach (var animal in _cell.GetAnimal().ToList())
                {
                    if (animal != currentAnimal)
                    {
                        ((Cornivourus)currentAnimal).EatAnimal(animal);
                    }
                }

                ((Cornivourus)currentAnimal).FreeMove();
                currentAnimal.SetTargetAnimalForEat(null);
            }
        }
        
        
        
        
        public static void MoveToEatOmniv(Animal currentAnimal,Cell target, Cell _cell)
        {
            currentAnimal.SetPreviousCell(_cell);
            if (_cell.X > target.X)
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X < target.X)
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (_cell.Y > target.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.Y < target.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X == target.X && _cell.Y == target.Y)
            {
                if (_cell.plant != null)
                {
                    if (_cell.plant.plantCycle == PlantCycle.Flower || _cell.plant.plantCycle == PlantCycle.Germ)
                        ((Omnivourus)currentAnimal).EatPlant();
                }
                else if (_cell.GetFruit() != null)
                {
                    ((Omnivourus)currentAnimal).EatFruit();
                }
                else
                {
                    foreach (var animal in _cell.GetAnimal().ToList())
                    {
                        if (animal != currentAnimal)
                        {
                            ((Omnivourus)currentAnimal).EatAnimal(animal);
                            return;
                        }
                    }

                    ((Omnivourus)currentAnimal).FreeMove();
                    currentAnimal.SetTargetAnimalForEat(null);
                }
            }
        }
        
        
        
        
        
        
        
        public static void MoveToEatMan(Animal currentAnimal,Cell target, Cell _cell)
        {
            currentAnimal.SetPreviousCell(_cell);
            if (_cell.X > target.X)
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X < target.X)
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (_cell.Y > target.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.Y < target.Y)
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X == target.X && _cell.Y == target.Y)
            {
                if (_cell.plant != null)
                {
                    if (_cell.plant.plantCycle == PlantCycle.Flower || _cell.plant.plantCycle == PlantCycle.Germ)
                        ((Man)currentAnimal).TakePlant(_cell.GetPlant());
                }
                else if (_cell.GetFruit() != null)
                {
                    ((Man)currentAnimal).TakeFruit(_cell.GetFruit());
                }
                else
                {
                    if (_cell.GetMeat() != null)
                    {
                        ((Man)currentAnimal).TakeMeat(_cell.GetMeat());
                        return;
                    }

                    ((Man)currentAnimal).FreeMove();
                    currentAnimal.SetTargetAnimalForEat(null);
                }
            }
        }
        
        
        
        
        
        
        
        public static void MoveToEatForHerbiv(Animal currentAnimal,Cell target,Cell _cell)
        {
            currentAnimal.SetPreviousCell(_cell);
            if (_cell.X > target.X && !(_cell._map.cells[_cell.X - 1, _cell.Y].GetAnimal().Any()))
            {
                Cell newCell = _cell._map.cells[_cell.X - 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X < target.X && !(_cell._map.cells[_cell.X + 1, _cell.Y].GetAnimal().Any()))
            {
                Cell newCell = _cell._map.cells[_cell.X + 1, _cell.Y];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }
            else if (_cell.Y > target.Y && !(_cell._map.cells[_cell.X, _cell.Y - 1].GetAnimal().Any()))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y - 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.Y < target.Y && !(_cell._map.cells[_cell.X, _cell.Y + 1].GetAnimal().Any()))
            {
                Cell newCell = _cell._map.cells[_cell.X, _cell.Y + 1];
                newCell.GetAnimal().Add(currentAnimal);
                _cell.animal.Remove(currentAnimal);
                currentAnimal._cell = newCell;
            }

            else if (_cell.X == target.X && _cell.Y == target.Y)
            {
                if (_cell.plant != null)
                {
                    if (_cell.plant.plantCycle == PlantCycle.Flower || _cell.plant.plantCycle == PlantCycle.Germ)
                        ((Herbivore)currentAnimal).EatPlant();
                }
                else if (_cell.GetFruit() != null)
                {
                    ((Herbivore)currentAnimal).EatFruit();
                }
                else
                {
                    ((Herbivore)currentAnimal).FreeMove();
                    currentAnimal.SetTargetCell(null);
                }
            }
        }
    }
}