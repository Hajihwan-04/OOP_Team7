using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    public class CustomerManager
    {
        private Dictionary<string, Customer> customerDB = new Dictionary<string, Customer>();

        public bool IsCustomerExist(string phoneNum)
        {
            return customerDB.ContainsKey(phoneNum);
        }

        public Customer GetCustomer(string phoneNum)
        {
            return customerDB[phoneNum];
        }

        public Customer RegisterCustomer(string phoneNum, string name)
        {
            Customer newCustomer = new Customer(name);
            customerDB.Add(phoneNum, newCustomer);
            return newCustomer;
        }
    }
}
