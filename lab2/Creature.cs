using System;
using System.Collections.Generic;
using System.Drawing;

namespace lab2
{
    public class AnimalGround
    {
        private int HP = 100;
        private int Hungry = 150;
        
        
        public AnimalGround(Cell cell)
        {
            this._cell = cell;
        }
        
        public List<Point> way = new List<Point>();
        
        private Cell _cell;
        
        public void setHp(int a)
        {
            HP = a;
        }

        public void setWay(List<Point> a)
        {
            way = a;
        }
        
        public List<Point> getWay()
        {
            return way;
        }

        public AnimalGround copy()
        {
            return new AnimalGround(_cell);
        }
        
        
        public int getHp()
        {
            return HP;
        }        
        
        public void setHungry(int a)
        {
            Hungry = a;
        }

        public int getHungry()
        {
            return Hungry;
        }

    }
    
    
}