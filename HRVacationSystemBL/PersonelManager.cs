using HRVacationSystemDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRVacationSystemBL
{
    public class PersonelManager : IPersonelManager
    {
        //Burada context kullanılarak Personel tablosuyla ilgili işlemler yapılacak
        HRVacationEntities context = new HRVacationEntities();

        public bool PersonelSignIn(string email, string password)
        {
            try
            {
                var personel = context.Personel.FirstOrDefault(x =>
                x.Email.ToLower() == email.ToLower() && x.IsActive);
                //ToDo: Şuanda paroları açık bir şekilde görüyoruz. İlerleyen örneklerde 
                //parolalar HASHlenecektir.
               var sonuc= MyPasswordHasher.ValidateUser(personel, password);
                return personel != null && sonuc ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Personel GetPersonelProile(string email)
        {
            try
            {
                return context.Personel.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            }
            catch (Exception)
            {
                throw;
            }
        }
       

        public bool PersonelUpdate()
        {
            try
            {
                return context.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AddNewPersonel(Personel personel)
        {
            try
            {
                context.Personel.Add(personel);
                return context.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static string MD5EncryptPassword(string password)
        {
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(password);
                b = MD5.ComputeHash(b);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (byte x in b)

                    sb.Append(x.ToString("x2"));

                return sb.ToString();
            }
        }

        public static string SHA384Hash(string password)
        {
           
            using (SHA384 sha384Hash = SHA384.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha384Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

             
    }
}