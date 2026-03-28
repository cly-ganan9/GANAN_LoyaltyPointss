using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public class LPDataServices
    {
        private ILPDataServices _dataService;
        public LPDataServices()
        {
            _dataService = new LPInMemoryData();
        }
        public LPDataServices(ILPDataServices dataService)
        {
            _dataService = dataService;
        }
        public void AddTransaction(Customer customer, string transaction)
        {
            _dataService.AddTransaction(customer, transaction);
        }
        public void SaveCustomer(Customer customer)
        {
            _dataService.SaveCustomer(customer);
        }
        public Customer? GetCustomerByPassportId(string passportId)
        {
            return _dataService.GetCustomerByPassportId(passportId);
        }
        public List<Customer> GetCustomers()
        {
            return _dataService.GetCustomers();
        }
        public void UpdateCustomer(Customer customer)
        {
            _dataService.UpdateCustomer(customer);
        }

    }
}
