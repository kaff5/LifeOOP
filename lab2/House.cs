using System.Collections.Generic;
using lab2.Properties;

namespace lab2
{
    public class House
    {
        public Cell _cell;
        public House(Cell cell)
        {
            _cell = cell;
        }

        private List<Sour<Resource>> storage = new List<Sour<Resource>>();
    }
}