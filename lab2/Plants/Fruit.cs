namespace lab2
{
    public class Fruit: AddInBag
    {
        public Cell _cell;
        public bool poisonous;


        public Fruit(Cell cell,bool poisonous)
        {
            _cell = cell;
            this.poisonous = poisonous;
            
        }
    }
}