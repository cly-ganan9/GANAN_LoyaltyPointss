using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public class LPJsonData : ILPDataServices
    {
        private List<Customer> customers = new List<Customer>();

        private string _jsonFileName;

        public LPJsonData()
        {
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/Customers.json";


            RetrieveDataFromJsonFile();
        }

        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.OpenWrite(_jsonFileName))
            {
                JsonSerializer.Serialize<List<Customer>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { SkipValidation = true, Indented = true })
                    , customers);
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            string json = File.ReadAllText(_jsonFileName);

            customers = JsonSerializer.Deserialize<List<Customer>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Customer>();
        }
        public void AddTransaction(Customer customer, string transaction)
        {
            RetrieveDataFromJsonFile();

            var existing = GetCustomerByPassportId(customer.PassportId);
            if (existing != null)
            {
                existing.TransactionHistory.Add(transaction);
                SaveDataToJsonFile();
            }

        }
        public void SaveCustomer(Customer customer)
        {
            RetrieveDataFromJsonFile();

            var existing = GetCustomerByPassportId(customer.PassportId);
            if (existing == null)
            {
                customers.Add(customer);
                SaveDataToJsonFile();
            }
        }

        public Customer? GetCustomerByPassportId(string passportId)
        {
            return customers.FirstOrDefault(c => c.PassportId == passportId);
        }

        public List<Customer> GetCustomers()
        {
            RetrieveDataFromJsonFile();
            return customers;
        }
        public void UpdateCustomer(Customer customer)
        {
            RetrieveDataFromJsonFile();

            var existing = customers.FirstOrDefault(c => c.PassportId == customer.PassportId);
            if (existing != null)
            {
                existing.CustomerName = customer.CustomerName;
                existing.LoyaltyPoints = customer.LoyaltyPoints;
                existing.TransactionHistory = customer.TransactionHistory;
                SaveDataToJsonFile();
            }
        }

    }
}
