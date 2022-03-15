using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Cornivourus : Animal
    {
        private int Hp;
        private int Hungry { get; set; }
        private CornivourusTypes typeAnimal;
        private Gender gender { get; set; }
        private int timerForReproduction;
        private int TimeOld;


        public override int GetHp()
        {
            return Hp;
        }

        public CornivourusTypes GetTypeAnimal()
        {
            return typeAnimal;
        }

        public override void SetHp(int _hp)
        {
            Hp = _hp;
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


        public Cornivourus(Cell cell, Cell targetCell, Cell previousCell, CornivourusTypes _typeAnimal, Gender _gender,
            bool sleepWinter)
            : base(cell, targetCell, previousCell, sleepWinter)
        {
            timerForReproduction = 300;
            typeAnimal = _typeAnimal;
            this.Hp = 150;
            this.Hungry = 250;
            gender = _gender;
            TimeOld = 600;
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
                        FindNewTargetAnimal();
                        SetAnimalForReproduction(null);
                        Hungry -= 1 * ratio;
                        break;
                    }


                    case < 70:
                    {
                        if (targetAnimalForEat == null)
                        {
                            FindNewTargetAnimal();
                        }
                        else if (targetAnimalForEat != null)
                        {
                            Move.MoveToEatForCorn(this, targetAnimalForEat, _cell);
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
                case CornivourusTypes.Wolf:
                {
                    Move.MoveAnimalWithoutEatDiagonal(this, this._cell);
                    break;
                }
                case CornivourusTypes.Tiger:

                    Move.MoveAnimalWithoutEatDiagonalAndOrthogonal(this, this._cell);
                    break;

                case CornivourusTypes.Fox:
                {
                    Move.MoveAnimalWithoutEatOrthogonal(this, this._cell);
                    break;
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

            SetAnimalForReproduction(timeVar);
        }


        private void FindNewTargetAnimal()
        {
            Animal findTargetAnimal = FindAnimalForEatOrReproduction();
            if (findTargetAnimal == null)
            {
                FreeMove();
            }

            targetAnimalForEat = findTargetAnimal;
        }

        public void EatAnimal(Animal target)
        {
            Hungry = 250;
            _cell._map.pointsAnimal.Remove(target);
            _cell.SetAnimalForMove(target);
            Hp = 150;
            targetAnimalForEat = null;
            _cell._map.deletedAnimals.Add(target);
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

        private bool CheckAnimalInVisit(List<Cell> target)
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


        private Animal GetAnimalFromVisit(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (this.Hungry == 140 && animal is Cornivourus &&
                            animal.GetHungry() > 130 && animal.GetGender() != this.GetGender() &&
                            animal.GetTimerForReproduction() == 0)
                        {
                            return animal;
                        }

                        if (this.Hungry <= 70 && animal != this)
                        {
                            if (animal is not Cornivourus)
                            {
                                return animal;
                            }
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
            while (visit.Count is > 0 and < 31 && !CheckAnimalInVisit(visit))
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

            return GetAnimalFromVisit(visit);
        }
    }
}