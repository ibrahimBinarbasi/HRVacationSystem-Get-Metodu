using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRVacationSystemBL
{
    public class MyPasswordHasher
    {
        public static string GenerateHMAC(string password, string secretKeyString)
        {
            var encoder = new ASCIIEncoding();
            var messageBytes = encoder.GetBytes(password);
            var secretKeyBytes = new byte[secretKeyString.Length / 2];
            for (int index = 0; index < secretKeyBytes.Length; index++)
            {
                string byteValue = secretKeyString.Substring(index * 2, 2);
                secretKeyBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            var hmacsha512 = new HMACSHA512(secretKeyBytes);

            byte[] hashValue = hmacsha512.ComputeHash(messageBytes);

            string hmac ="";
            foreach (byte x in hashValue)
            {
                hmac += String.Format("{0:x2}", x);
            }

            return hmac.ToUpper();
        }
        public static bool ValidateUser(Personel personel, string password)
        {
           bool isCorrectPassword = (personel.Password == GenerateHMAC(password, personel.Salt));
              
                return isCorrectPassword ? true: false;
            
           
        }

        public static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public static string CreateSalt512()
        {
            var message = RandomString(512, false);
            return BitConverter.ToString((new SHA512Managed()).ComputeHash(Encoding.ASCII.GetBytes(message))).Replace("-" , "");
        }
    }
}
