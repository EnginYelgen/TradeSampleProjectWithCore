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

            int adminId = 0;
            DateTime currentDate = DateTime.Now;

            if (!context.Users.Any())
            {
                string hashedPass = Common.Security.GetHashed("admin");

                User entity_U = new User
                {
                    Name = "Administrator",
                    Surname = "Administrator",
                    MailAddress = "admin@tradesample.com",
                    Birthday = DateTime.Parse("2017-06-01"),
                    Password = hashedPass,
                    Telephone = "",
                    InUse = true,
                    UpdateDate = currentDate
                };

                context.Users.Add(entity_U);
                context.SaveChanges();

                adminId = entity_U.Id;
            }

            if (!context.Countries.Any())
            {
                Country entity_C = new Country
                {
                    Name = "Türkiye",
                    InUse = true,
                    UpdateDate = currentDate,
                    UpdateUserId = adminId
                };

                context.Countries.Add(entity_C);
                context.SaveChanges();

                context.Cities.AddRange(new[] {
                    new City {Name="İstanbul", CountryId=entity_C.Id,InUse=true,UpdateDate= currentDate,UpdateUserId=adminId },
                    new City {Name="Ankara", CountryId=entity_C.Id,InUse=true,UpdateDate= currentDate,UpdateUserId=adminId },
                    new City {Name="İzmir", CountryId=entity_C.Id,InUse=true,UpdateDate= currentDate,UpdateUserId=adminId },
                    new City {Name="Denizli", CountryId=entity_C.Id,InUse=true,UpdateDate= currentDate,UpdateUserId=adminId },
                });
                context.SaveChanges();
            }

            if (!context.ProductCategories.Any())
            {
                ProductCategory entity_PC = new ProductCategory
                {
                    Name = "Laptop",
                    Description = string.Empty,
                    InUse = true,
                    UpdateDate = currentDate,
                    UpdateUserId = adminId
                };

                context.ProductCategories.Add(entity_PC);
                context.SaveChanges();

                context.Products.AddRange(new[] {
                    new Product {Name="Asus",Description=string.Empty,ProductCategoryId=entity_PC.Id,UnitPrice=3250,StockCode="LASUS001",StockNumber=10,InUse=true,UpdateDate=currentDate,UpdateUserId=adminId},
                    new Product {Name="HP",Description=string.Empty,ProductCategoryId=entity_PC.Id,UnitPrice=3500,StockCode="LHP001",StockNumber=8,InUse=true,UpdateDate=currentDate,UpdateUserId=adminId},
                    new Product {Name="Toshiba",Description=string.Empty,ProductCategoryId=entity_PC.Id,UnitPrice=3350,StockCode="LTOSHIBA001",StockNumber=8,InUse=true,UpdateDate=currentDate,UpdateUserId=adminId},
                    new Product {Name="MAC",Description=string.Empty,ProductCategoryId=entity_PC.Id,UnitPrice=5800,StockCode="LMAC001",StockNumber=2,InUse=true,UpdateDate=currentDate,UpdateUserId=adminId}
                });
                context.SaveChanges();
            }
        }
    }
}
