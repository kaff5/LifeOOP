using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace lab2
{
    public class Man : Omnivourus
    {

        
        private OmnivourusTypes typeAnimal;
        private Gender gender;
        private int timerForReproduction;
        private List<AddInBag> bagForEat = new List<AddInBag>();
        private List<AddInBag> bagForRes = new List<AddInBag>();
        private Man partner;
        private int TimeOld;
        public const int MaxSizeBag = 10;
        private House home;
        private bool possibilityToBuild = true;
        

        public Man(Cell cell, Cell targetCell, Cell previousCell, OmnivourusTypes _typeAnimal, Gender _gender, bool
            sleepWinter) :
            base(cell, targetCell, previousCell, _typeAnimal, _gender, sleepWinter)
        {
            typeAnimal = _typeAnimal;
            gender = _gender;
            partner = null;
            TimeOld = 600;
            timerForReproduction = 300;
        }

        public void SetHome(House _home)
        {
            home = _home;
        }

        public void AddInBagForRes(AddInBag _item)
        {
            bagForRes.Add(_item);
        }
        public List<AddInBag> GetBagForRes()
        {
            return bagForRes;
        }


        public Man GetPartner()
        {
            return partner;
        }

        public List<AddInBag> GetBag()
        {
            return bagForEat;
        }

        public void SetPartner(Man _partner)
        {
            partner = _partner;
        }

        private void IWantCreateHouse()
        {
            if (possibilityToBuild && _cell.GetPlant() == null && _cell.GetFruit() == null)
            {
                possibilityToBuild = false;
                _cell.SetHouse();
                _cell._map.allHouse.Add(_cell.GetHouse());
                home = _cell.GetHouse();
                partner.SetHome(home);
            }
        }


        public void IWantEatOrNot()
        {

            Hungry -=1;
            
            if (TimeOld != 0)
            {
                TimeOld -= 1;
            }
            else
            {
                KillAnimal();
            }

            if (partner == null)
            {
                partner = FindPartnerForReproduction();
                if (partner != null)
                {
                    partner.partner = this;
                    IWantCreateHouse();
                }

            }
            else
            {
                if (timerForReproduction != 0)
                {
                    timerForReproduction -= 1;
                }
                else if(partner.GetTimerForReproduction() == 0)
                {
                    Move.MoveToForReproduction(this,partner,_cell);
                }
            }

            if (Hp <= 0)
            {
                KillAnimal();
                partner.SetPartner(null);
                SetPartner(null);
            }
            else
            {
                if (bagForEat.Count is > 0 and < MaxSizeBag)
                {
                    FreeMove();
                    EatFromBag();
                }
                else
                {
                    if (targetCell == null)
                    {
                        FindCell();
                    }
                    else if (targetCell != null)
                    {
                        Move.MoveToEatMan(this, targetCell, _cell);
                    }


                }
            }
            if (Hungry <= 0)
            {
                Hp -= 1;
            }
        }


        private void FindCell()
        {
            Cell timeVar = Find();
            if (timeVar == null)
            {
                FreeMove();
            }

            targetCell = timeVar;
        }



        public void TakePlant(Plant plant)
        {
            bagForEat.Add(plant);
            plant.PlantDead();
            targetCell = null;
        }
        public void TakeFruit(Fruit fruit)
        {
            bagForEat.Add(fruit);
            _cell._map.pointsFruits.Remove(_cell.GetFruit());
            _cell.SetFruitForDead(); 
            targetCell = null;
        }
        
        public void TakeMeat(Animal meat)
        {
            bagForEat.Add(meat);
            _cell.SetMeat(null);
            targetCell = null;
        }
        
        
        
        
        
        
        private void EatFromBag()
        {
            if (Hungry < 60)
            {
                bagForEat.Remove(bagForEat.First());
                Hungry = 250;
            }
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


        private Man GetFromVisitsForReproduction(List<Cell> target)
        {
            foreach (var _cell in target)
            {
                if (_cell.GetAnimal().Any())
                {
                    foreach (var animal in _cell.GetAnimal())
                    {
                        if (animal is Man && animal.GetGender() != this.GetGender() && ((Man)animal).GetPartner() == null)
                        {
                            return (Man)animal;
                        }
                    }
                }
            }

            return null;
        }
        
        


        private Man FindPartnerForReproduction()
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
                if (_cell.GetMeat() != null)
                {
                    return true;
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

                if (_cell.GetMeat() != null)
                {
                    return _cell;
                }
            }

            return null;
        }


        private Cell Find()
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

    }
}