using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Omnivourus : Animal
    {
        protected int Hp { get; set; }
        protected int Hungry { get; set; }
        private OmnivourusTypes typeAnimal;
        private Gender gender;
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
        public OmnivourusTypes GetTypeAnimal()
        {
            return typeAnimal;
        }

        public override int GetTimerForReproduction()
        {
            return timerForReproduction;
        }

        protected override void SetTimerForReproduction(int time)
        {
            timerForReproduction = time;
        }

        public override Gender GetGender()
        {
            return gender;
        }


        public Omnivourus(Cell cell, Cell targetCell, Cell previousCell, OmnivourusTypes _typeAnimal, Gender _gender,bool sleepWinter) :
            base(cell, targetCell, previousCell,sleepWinter)
        {
            timerForReproduction = 300;
            typeAnimal = _typeAnimal;
            Hp = 300;
            Hungry = 250;
            gender = _gender;
            TimeOld = 600;
        }

        public override int GetHungry()
        {
            return Hungry;
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
                            Move.MoveToEatOmniv(this, targetCell, _cell);
                        }

                        Hungry -= 1 * ratio;
                        Hp -= 1;
                        break;
                    }
                }
            }
        }


        public override void FreeMove()
        {
            switch (typeAnimal)
            {
                case OmnivourusTypes.Bear:
                {
                    Move.MoveAnimalWithoutEatDiagonal(this, this._cell);
                    break;
                }
                case OmnivourusTypes.Crow:
                {
                    Move.MoveAnimalWithoutEatDiagonalAndOrthogonal(this, this._cell);
                    break;
                }
                case OmnivourusTypes.Monkey:
                {
                    Move.MoveAnimalWithoutEatOrthogonal(this, this._cell);
                    break;
                }
                case OmnivourusTypes.Man:
                {
                    Move.MoveAnimalWithoutEatDiagonalAndOrthogonal(this, this._cell);
                    break;
                }
            }
        }

        public void FindForReproduction()
        {
            Animal timeVar = FindPartnerForReproduction();
            if (timeVar == null)
            {
                FreeMove();
            }

            targetAnimalForReproduction = timeVar;
        }

        

        protected void FindNewTargetCell()
        {
            Cell timeVar = FindPlantOrAnimalForEat();
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
                Hp = 300;

            }
            else
            {
                Hungry += 10;
                Hp -= 20;
            }
            
            _cell.GetPlant().PlantDead();
            targetCell = null;
        }

        public void EatAnimal(Animal target)
        {
            _cell._map.deletedAnimals.Add(target);
            Hungry = 350;
            _cell._map.pointsAnimal.Remove(target);
            _cell.animal.Remove(target);
            Hp = 300;
            targetAnimalForEat = null;
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

        private bool CheckPlantOrAnimalInVisitForEat(List<Cell> target)
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
                else if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (animal is not Omnivourus && animal != this)
                            return true;
                    }
                }
            }

            return false;
        }


        private Cell GetPlantOrAnimalFromVisitForEat(List<Cell> target)
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

                if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (this.Hungry <= 70 && animal != this)
                        {
                            if (animal is not Omnivourus)
                            {
                                return _cell;
                            }
                        }
                    }
                }
            }

            return null;
        }


        private Cell FindPlantOrAnimalForEat()
        {
            List<Cell> used = new List<Cell>();
            List<Cell> visit = new List<Cell>();

            used.Add(_cell);
            visit.Add(_cell);
            while (visit.Count is > 0 and < 24 && !CheckPlantOrAnimalInVisitForEat(visit))
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

            if (visit.Count >= 23)
            {
                return null;
            }

            return GetPlantOrAnimalFromVisitForEat(visit);
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


        private Animal GetFromVisitsForReproduction(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (this.Hungry == 140 && animal is Omnivourus && GetTypeAnimal() == ((Omnivourus)animal).GetTypeAnimal() && 
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


        private Animal FindPartnerForReproduction()
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

            return GetFromVisitsForReproduction(visit);
        }
    }
}