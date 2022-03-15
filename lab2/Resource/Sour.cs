namespace lab2
{
    public class Sour<T> where T: Resource
    {
        public Cell cell;
        public Sour(Cell _cell)
        {
            cell = _cell;
        }
        

        
        
        public T elem { get; set; }
    }
}