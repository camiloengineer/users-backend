using System;
namespace User.Backend.Api.Clases.Domain
{
    public class Customer
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int DNI { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
    }
}
