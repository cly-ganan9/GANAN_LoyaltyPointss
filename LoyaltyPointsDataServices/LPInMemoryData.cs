using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    internal class LPInMemoryData : ILPDataServices
    {
        private List<Customer> customers = new List<Customer>();
        public void AddTransaction(Customer customer, string transaction)
        {
            customer.TransactionHistory.Add(transaction);
        }
        public void SaveCustomer(Customer customer)
        {
            var existing = GetCustomerByPassportID(customer.PassportId);

            if (existing == null)
            {
                customers.Add(customer);
            }
        }
        public Customer? GetCustomerByPassportID(string passportId)
        {
            return customers.FirstOrDefault(c => c.PassportId == passportId);
        }
        public List<Customer> GetCustomers()
        {
            return customers;
        }
        public void UpdateCustomer(Customer customer)
        {
            var existing = GetCustomerByPassportID(customer.PassportId);
            if (existing != null)
            {
                existing.CustomerName = customer.CustomerName;
                existing.LoyaltyPoints = customer.LoyaltyPoints;
                existing.TransactionHistory = customer.TransactionHistory;
            }
        }

    }
}
