using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public interface ILPDataServices
    {
        void AddTransaction(Customer customer, string transaction);
        void SaveCustomer(Customer customer);
        Customer? GetCustomerByPassportId(string passportId);
        List<Customer> GetCustomers();
        void UpdateCustomer(Customer customer);
    }
}
