
namespace lab2
{
    public abstract class Animal: AddInBag
    {
        
        private protected Cell targetCell;
        private protected Animal targetAnimalForEat;
        private protected Animal targetAnimalForReproduction;
        public Cell _cell;
        private Cell previousCell;
        private readonly bool sleepWinter;



        protected void KillAnimal()
        {
            if (this is Man)
            {
                ((Man)this).SetHome(null);
                ((Man) this).GetPartner().SetHome(null);
                ((Man) this).GetPartner().SetPartner(null);
                ((Man) this).SetPartner(null);

            }
            _cell._map.deletedAnimals.Add(this);
            targetAnimalForEat = null;
            targetAnimalForReproduction = null;
            _cell._map.pointsAnimal.Remove(this);
            _cell.SetAnimalForKill(this);
            _cell.SetMeat(this);
            
        }
        
        
        public void GoInSleep()
        {
            SetTargetAnimalForEat(null);
            SetTargetCell(null);
        }

        public bool GetSleepWinter()
        {
            return sleepWinter;
        }


        public void SetPreviousCell(Cell _previousCell)
        {
            previousCell = _previousCell;
        }
        
        public void Reproduction()
        {
            if (_cell.animal.Count > 1)
            {
                _cell.AddNewAnimalForReproduction(this);
                SetTimerForReproduction(300);
                targetAnimalForReproduction.SetTimerForReproduction(300);
                targetAnimalForReproduction.SetAnimalForReproduction(null);
                this.SetAnimalForReproduction(null);
            }
        }


        protected Animal(Cell cell, Cell _targetCell, Cell _previousCell,bool _sleepWinter)
        {
            targetCell = _targetCell;
            previousCell = _previousCell;
            _cell = cell;
            sleepWinter = _sleepWinter;
        }

        public void SetAnimalForReproduction(Animal _animal)
        {
            targetAnimalForReproduction = _animal;
        }
        public void SetTargetAnimalForEat(Animal _animal)
        {
            targetAnimalForEat = _animal;
        }



        public Cell GetPreviousCell()
        {
            return previousCell;
        }       
        public void SetTargetCell(Cell cell)
        {
            targetCell = cell;
        }


        public abstract void IWantEatOrNot(int ratio);

        public abstract int GetHungry();
        public abstract int GetTimerForReproduction();
        protected abstract void SetTimerForReproduction(int time);
        public abstract Gender GetGender();
        public abstract void FreeMove();
        public abstract void SetHp(int hp);
        public abstract int GetHp();
    }
}
