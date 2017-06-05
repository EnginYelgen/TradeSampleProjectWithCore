using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class DbInitialize
    {
        public static void Initialize(TradeSampleContext context)
        {
            context.Database.EnsureCreated();

            //if (!context.Countries.Any())
            //{
            //    string hashedPass = Common.Security.GetHashed("admin");

            //    context.Users.Add(
            //        new User {Name="Administrator", Surname= "Administrator", MailAddress="admin@tradesample.com", Birthday=DateTime.Parse("2017-06-01"), Password= hashedPass, Telephone="",InUse = true, UpdateDate= DateTime.Now, UpdateUserId})
            //}

            //if (!context.Countries.Any())
            //{
            //    context.Countries.AddRange(
            //        new Country { Name = "Türkiye", InUse = true, UpdateDate = DateTime.Now, })
            //}
        }
    }
}
