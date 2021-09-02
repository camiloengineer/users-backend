using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace User.Backend.Api.Core.Enveloped
{
    public class EnvelopedObject
    {
        public class Enveloped
        {
            public Header Header { get; set; }
            public dynamic body { get; set; }
        }

        public class EnvelopedError
        {
            public Header header { get; set; }
            public Body body { get; set; }
        }

        public class Header
        {
            public Consumerdata consumerData { get; set; }
            public Transactiondata transactionData { get; set; }
        }

        public class Consumerdata
        {
      
        }

        public class Transactiondata
        {
            public string idTransaction { get; set; }
            public string idTransaction_Rel { get; set; }
            public string userType { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string date { get; set; }
        }

        public class Body
        {
            public Error error { get; set; }
        }

        public class Error
        {
            public string type { get; set; }
            public string description { get; set; }
            public List<Detail> detail { get; set; }
        }

        public class Detail
        {
            public string level { get; set; }
            public string type { get; set; }
            public string backend { get; set; }
            public string code { get; set; }
            public string description { get; set; }
        }
    }
}
