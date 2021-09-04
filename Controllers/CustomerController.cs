using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Dynamic;
using User.Backend.Api.Core.Services;
using User.Backend.Api.Core.Exceptions;
using User.Backend.Api.Clases.Domain;
using User.Backend.Api.Core.Auth;
using Microsoft.AspNetCore.Authorization;

namespace User.Backend.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _userService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService userService,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Authenticate([FromBody]Customer customer)
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                var userDB = await _userService.GetCustomerAsync(customer.DNI, inDate);
                var token = await _jwtAuthenticationManager.AuthenticateAsync(userDB, customer.DNI, customer.Password, inDate);

                if ( token == "")
                {
                    return Unauthorized();
                }
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";
                obj.token = token;
                obj.user = userDB;

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }

        [HttpGet("GetCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCustomer(int dni)
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                var result = await _userService.GetCustomerAsync(dni, inDate);
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";
                obj.user = result;

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }

        [HttpGet("GetCustomers")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCustomer()
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                var result = await _userService.GetCustomersAsync(inDate);
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";
                obj.user = result;

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }

        [AllowAnonymous]
        [HttpPost("CreateCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                var result = await _userService.CreateCustomerAsync(customer, inDate); ;
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";
                obj.user = result;

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                var result = await _userService.UpdateCustomerAsync(customer, inDate); ;
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";
                obj.user = result;

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }

        [HttpDelete("DeleteCustomer")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCustomer(int dni)
        {
            string inDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

            try
            {
                await _userService.DeleteCustomerAsync(dni, inDate);
                dynamic obj = new ExpandoObject();
                obj.codigoError = "OK";

                return new OkObjectResult(obj);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException(e.Message + e.StackTrace);
            }
        }
    }
}
