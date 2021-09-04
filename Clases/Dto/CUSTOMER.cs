using System;
namespace User.Backend.Api.Clases.Dto
{
    public class CUSTOMER
    {
        public string CUST_GUID { get; set; }
        public DateTime CUST_BIRTHDAY { get; set; }
        public int CUST_DNI { get; set; }
        public string CUST_NAME { get; set; }
        public int CUST_PHONE { get; set; }
        public string CUST_EMAIL { get; set; }
        public bool CUST_ACTIVE { get; set; }
        public string CUST_PASSWORD { get; set; }
        public string CUST_AVATAR { get; set; }
    }
}
