using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;


namespace User.Backend.Api.Core.Auth
{

    public static class Util
    {
        public static string Encrypt(string password, int DNI)
        {
            var key = string.Format("{0}{1}", password, DNI);
            byte[] encData_byte = new byte[key.Length];
            encData_byte = Encoding.UTF8.GetBytes(key);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
    }
}