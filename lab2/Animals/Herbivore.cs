using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Herbivore : Animal
    {
        private int Hp { get; set; }
        private int Hungry { get; set; }
        private Gender gender;
        private HerbivoreTypes typeAnimal;
        private int timerForReproduction;
        private int TimeOld;
        
        
        public override void SetHp(int _hp)
        {
            Hp = _hp;
        }
        public override int GetHp()
        {
            return Hp;
        }

        public override int GetTimerForReproduction()
        {
            return timerForReproduction;
        }

        protected override void SetTimerForReproduction(int time)
        {
            timerForReproduction = time;
        }

        public override int GetHungry()
        {
            return Hungry;
        }

        public override Gender GetGender()
        {
            return gender;
        }

        public Herbivore(Cell cell, Cell targetCell, Cell previousCell, HerbivoreTypes _typeAnimal, Gender _gender, bool sleepWinter) :
            base(cell, targetCell, previousCell,sleepWinter)
        {
            timerForReproduction = 300;
            typeAnimal = _typeAnimal;
            this.Hp = 200;
            this.Hungry = 350;
            gender = _gender;
            TimeOld = 600;
        }

        public HerbivoreTypes GetTypeAnimal()
        {
            return typeAnimal;
        }

        public override void FreeMove()
        {
            switch (typeAnimal)
            {
                case HerbivoreTypes.Elephant:
                {
                    Move.MoveAnimalWithoutEatDiagonal(this, _cell);
                    break;
                }
                case HerbivoreTypes.Rabbit:

                    Move.MoveAnimalWithoutEatDiagonalAndOrthogonal(this, _cell);
                    break;

                case HerbivoreTypes.Zebra:
                {
                    Move.MoveAnimalWithoutEatOrthogonal(this, _cell);
                    break;
                }
            }
        }


        public override void IWantEatOrNot(int ratio)
        {
            if (TimeOld != 0)
            {
                TimeOld -= 1;
            }
            else
            {
                KillAnimal();
            }
            if (timerForReproduction != 0)
            {
                timerForReproduction -= 1;
            }

            if (Hp <= 0)
            {
                KillAnimal();
            }
            else
            {
                switch (Hungry)
                {
                    case 140:
                    {
                        if (targetAnimalForReproduction == null && GetTimerForReproduction() == 0 &&
                            GetGender() ==
                            Gender.Male)
                        {
                            FindForReproduction();
                        }
                        else if (targetAnimalForReproduction != null)
                        {
                            Move.MoveToForReproduction(this, targetAnimalForReproduction, _cell);
                        }

                        Hungry -= 1 * ratio;
                        break;
                    }


                    case > 70:
                    {
                        if (targetAnimalForReproduction == null)
                            FreeMove();
                        else
                        {
                            Move.MoveToForReproduction(this, targetAnimalForReproduction, _cell);
                        }

                        Hungry -= 1 * ratio;
                        break;
                    }


                    case 70:
                    {
                        FindNewTargetCell();
                        SetAnimalForReproduction(null);
                        Hungry -= 1 * ratio;
                        break;
                    }


                    case < 70:
                    {
                        if (targetCell == null)
                        {
                            FindNewTargetCell();
                        }
                        else if (targetCell != null)
                        {
                            Move.MoveToEatForHerbiv(this, targetCell, _cell);
                        }

                        Hungry -= 1 * ratio;
                        Hp -= 1;
                        break;
                    }
                }
            }
        }

        private void FindForReproduction()
        {
            Animal timeVar = FindAnimalForEatOrReproduction();
            if (timeVar == null)
            {
                FreeMove();
            }

            targetAnimalForReproduction = timeVar;
        }
        


        private void FindNewTargetCell()
        {
            Cell timeVar = FindPlant();
            if (timeVar == null)
            {
                FreeMove();
            }

            targetCell = timeVar;
        }


        public void EatFruit()
        {
            if (_cell.GetFruit().poisonous == false)
            {
                Hungry = 250;
                _cell._map.pointsFruits.Remove(_cell.GetFruit());
                _cell.SetFruitForDead();
                Hp = 100;
                targetCell = null;
            }
            else
            {
                _cell._map.pointsFruits.Remove(_cell.GetFruit());
                _cell.SetFruitForDead();
                Hp -= 10;
                targetCell = null;
            }
        }

        public void EatPlant()
        {
            if (_cell.GetPlant().poisonous == false)
            {
                Hungry = 250;
                Hp = 200;

            }
            else
            {
                Hungry += 10;
                Hp -= 20;
            }
            
            _cell.GetPlant().PlantDead();
            targetCell = null;
        }


        private bool CheckRepetitions(List<Cell> target, int x, int y)
        {
            foreach (var _cell in target)
            {
                if (_cell.X == x && _cell.Y == y)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckPlantInVisit(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.plant != null)
                {
                    if (_cell.plant.plantCycle == PlantCycle.Flower || _cell.plant.plantCycle == PlantCycle.Germ)
                        return true;
                }
                else if (_cell.GetFruit() != null)
                {
                    return true;
                }
            }

            return false;
        }


        private Cell GetPlantFromVisit(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.plant != null)
                {
                    if (_cell.plant.plantCycle == PlantCycle.Flower || _cell.plant.plantCycle == PlantCycle.Germ)
                        return _cell;
                }
                else if (_cell.GetFruit() != null)
                {
                    return _cell;
                }
            }

            return null;
        }


        private Cell FindPlant()
        {
            List<Cell> used = new List<Cell>();
            List<Cell> visit = new List<Cell>();

            used.Add(_cell);
            visit.Add(_cell);
            while (visit.Count is > 0 and < 20 && !CheckPlantInVisit(visit))
            {
                if (!CheckRepetitions(used, visit[0].X, visit[0].Y - 1) && visit[0].Y - 1 > 0 &&
                    !(_cell._map.cells[_cell.X, _cell.Y - 1].GetAnimal().Any()) &&
                    _cell._map.cells[visit[0].X, visit[0].Y - 1].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X, visit[0].Y - 1]);
                    visit.Add(_cell._map.cells[visit[0].X, visit[0].Y - 1]);
                }

                if (!CheckRepetitions(used, visit[0].X - 1, visit[0].Y) && visit[0].X - 1 > 0 &&
                    !(_cell._map.cells[_cell.X - 1, _cell.Y].GetAnimal().Any()) &&
                    _cell._map.cells[visit[0].X - 1, visit[0].Y].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X - 1, visit[0].Y]);
                    visit.Add(_cell._map.cells[visit[0].X - 1, visit[0].Y]);
                }

                if (!CheckRepetitions(used, visit[0].X + 1, visit[0].Y) && visit[0].X + 1 < _cell._map.numOfCells &&
                    !(_cell._map.cells[_cell.X + 1, _cell.Y].GetAnimal().Any()) &&
                    _cell._map.cells[visit[0].X + 1, visit[0].Y].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X + 1, visit[0].Y]);
                    visit.Add(_cell._map.cells[visit[0].X + 1, visit[0].Y]);
                }

                if (!CheckRepetitions(used, visit[0].X, visit[0].Y + 1) && visit[0].Y + 1 < _cell._map.numOfCells &&
                    !(_cell._map.cells[_cell.X, _cell.Y + 1].GetAnimal().Any()) &&
                    _cell._map.cells[visit[0].X, visit[0].Y + 1].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X, visit[0].Y + 1]);
                    visit.Add(_cell._map.cells[visit[0].X, visit[0].Y + 1]);
                }

                visit.RemoveAt(0);
            }

            if (visit.Count >= 19)
            {
                return null;
            }

            return GetPlantFromVisit(visit);
        }


        private bool CheckInVisitForReproduction(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.GetAnimal().Any() && !_cell.GetAnimal().Contains(this))
                {
                    return true;
                }
            }

            return false;
        }


        private Animal GetVisitsForReproduction(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (this.Hungry == 140 && animal is Herbivore &&
                            animal.GetHungry() > 130 && animal.GetGender() != this.GetGender() &&
                            animal.GetTimerForReproduction() == 0)
                        {
                            return animal;
                        }
                    }
                }
            }

            return null;
        }


        private Animal FindAnimalForEatOrReproduction()
        {
            List<Cell> used = new List<Cell>();
            List<Cell> visit = new List<Cell>();

            used.Add(_cell);
            visit.Add(_cell);
            while (visit.Count is > 0 and < 31 && !CheckInVisitForReproduction(visit))
            {
                if (!CheckRepetitions(used, visit[0].X, visit[0].Y - 1) && visit[0].Y - 1 > 0 &&
                    _cell._map.cells[visit[0].X, visit[0].Y - 1].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X, visit[0].Y - 1]);
                    visit.Add(_cell._map.cells[visit[0].X, visit[0].Y - 1]);
                }

                if (!CheckRepetitions(used, visit[0].X - 1, visit[0].Y) && visit[0].X - 1 > 0 &&
                    _cell._map.cells[visit[0].X - 1, visit[0].Y].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X - 1, visit[0].Y]);
                    visit.Add(_cell._map.cells[visit[0].X - 1, visit[0].Y]);
                }

                if (!CheckRepetitions(used, visit[0].X + 1, visit[0].Y) && visit[0].X + 1 < _cell._map.numOfCells &&
                    _cell._map.cells[visit[0].X + 1, visit[0].Y].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X + 1, visit[0].Y]);
                    visit.Add(_cell._map.cells[visit[0].X + 1, visit[0].Y]);
                }

                if (!CheckRepetitions(used, visit[0].X, visit[0].Y + 1) && visit[0].Y + 1 < _cell._map.numOfCells &&
                    _cell._map.cells[visit[0].X, visit[0].Y + 1].GetBiom() != Biom.Water)
                {
                    used.Add(_cell._map.cells[visit[0].X, visit[0].Y + 1]);
                    visit.Add(_cell._map.cells[visit[0].X, visit[0].Y + 1]);
                }

                visit.RemoveAt(0);
            }

            if (visit.Count >= 30)
            {
                return null;
            }

            return GetVisitsForReproduction(visit);
        }
    }
}