using System.Collections.Generic;
using System.Threading.Tasks;
using User.Backend.Api.Clases.Domain;

namespace User.Backend.Api.Core.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(string inDate);
        Task<Customer> GetCustomerAsync(int dni, string inDate);
        Task<Customer> CreateCustomerAsync(Customer customer, string inDate);
        Task<Customer> UpdateCustomerAsync(Customer customer, string inDate);
        Task DeleteCustomerAsync(int dni, string inDate);
    }
}
