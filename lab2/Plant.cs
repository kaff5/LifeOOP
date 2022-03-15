namespace lab2
{
    public class Plant
    {
        private int timerForDead = 200;
        private PlantCycle plantCycle = PlantCycle.Seed;

        
        public Plant(Cell cell)
        {
            this._cell = cell;
        }

        private Cell _cell;

        public void setPlantCycle(PlantCycle a)
        {
            plantCycle = a;
        }
        public PlantCycle getPlantCycle()
        {
            return plantCycle;
        }
        public int getTimerForDead()
        {
            return timerForDead;
        }
        
        public void setTimerForDead(int a)
        {
            timerForDead = a;
        }


        public void checkTimeForDead()
        {
            if (timerForDead <= 200)
            {
                plantCycle = PlantCycle.Seed;
                if (timerForDead <= 150)
                {
                    plantCycle = PlantCycle.Germ;
                    if (timerForDead <= 100)
                    {
                        plantCycle = PlantCycle.Flower;
                        if (timerForDead <= 50)
                        {
                            plantCycle = PlantCycle.DriedPlant;
                        }
                    }
                }
            }
        }
    }
}