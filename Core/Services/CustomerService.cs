using System;
using System.Collections.Generic;
using AutoMapper;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using User.Backend.Api.Clases.Domain;
using User.Backend.Api.Clases.Dto;
using User.Backend.Api.Core.Exceptions;
using User.Backend.Api.Core.Auth;

namespace User.Backend.Api.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private IDynamoDBContext _dynamoDBContext;


        public CustomerService(IMapper mapper, IDynamoDBContext dynamoDBContext)
        {
            _mapper = mapper;
            _dynamoDBContext = dynamoDBContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string inDate)
        {
            try
            {
                return await Task.Run(async () =>
                {

                    var dto = await _dynamoDBContext
                                    .ScanAsync<CUSTOMER>(null).GetNextSetAsync();

                    var result = _mapper.Map<List<CUSTOMER>, List<Customer>>(dto);

                    return result;
                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("{0} => {1} $ {2}", ex.Message, ex.StackTrace, inDate));
            }
        }

        public async Task<Customer> GetCustomerAsync(int dni, string inDate)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    var dto = await _dynamoDBContext
                                    .LoadAsync<CUSTOMER>(dni);

                    var result = _mapper.Map<CUSTOMER, Customer>(dto);

                    return result;
                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("${0} => {1} $ {2}",  ex.Message, ex.StackTrace  ,inDate));
            }
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer, string inDate)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    var dto = new CUSTOMER();

                    dto.CUST_BIRTHDAY = customer.Birthdate;
                    dto.CUST_DNI = customer.DNI;
                    dto.CUST_EMAIL = customer.Email;
                    dto.CUST_GUID = Guid.NewGuid().ToString("N");
                    dto.CUST_NAME = customer.Name;
                    dto.CUST_PHONE = customer.Phone;
                    dto.CUST_ACTIVE = customer.Active;
                    dto.CUST_PASSWORD = Util.Encrypt( customer.Password, customer.DNI );
                    dto.CUST_AVATAR = customer.Avatar;

                    await _dynamoDBContext.SaveAsync<CUSTOMER>(dto);

                    var result = _mapper.Map<CUSTOMER, Customer>(dto);

                    return result;
                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("${0} => {1} $ {2}", ex.Message, ex.StackTrace, inDate));
            }
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer, string inDate)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    var dto = new CUSTOMER();

                    dto.CUST_BIRTHDAY = customer.Birthdate;
                    dto.CUST_DNI = customer.DNI;
                    dto.CUST_EMAIL = customer.Email;
                    dto.CUST_GUID = customer.Guid;
                    dto.CUST_NAME = customer.Name;
                    dto.CUST_PHONE = customer.Phone;
                    dto.CUST_ACTIVE = customer.Active;
                    dto.CUST_AVATAR = customer.Avatar;

                    if (!string.IsNullOrEmpty(customer.NewPassword))
                    {
                        dto.CUST_PASSWORD = Util.Encrypt(customer.NewPassword, customer.DNI);
                    }
                    else dto.CUST_PASSWORD = customer.Password;


                    await _dynamoDBContext.SaveAsync(dto);

                    var result = _mapper.Map<CUSTOMER, Customer>(dto);

                    return result;
                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("${0} => {1} $ {2}", ex.Message, ex.StackTrace, inDate));
            }
        }

        public async Task DeleteCustomerAsync(int dni, string inDate)
        {
            try
            {
                await Task.Run(async () =>
                {
                    await _dynamoDBContext
                                    .DeleteAsync<CUSTOMER>(dni);
                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("${0} => {1} $ {2}", ex.Message, ex.StackTrace, inDate));
            }
        }
    }
}
