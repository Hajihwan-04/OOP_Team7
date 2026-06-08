using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 1. 일반 클래스 (2/10)
    public class Customer
    {
        
        private string Name { get; set; }
        private int id = 0;

        public Customer (string name)
        {
            this.Name = name;
            id = id + 1;
        }

        public string GetName()
        {
            return Name;
        }

        

        //public void MakeOrder(Order order)
        //{

        //}
    }
}
