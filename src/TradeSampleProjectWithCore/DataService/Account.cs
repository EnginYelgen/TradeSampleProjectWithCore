using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.DataService
{
    public class Account : BaseClasses.BaseService
    {
        public Account(TradeSampleContext context) : base(context) { }

        public ViewModelAccountDetail GetAccountDetail(int userId)
        {
            List<ViewModelAddress> listVMAddress;

            if (this.DbContext.Addresses.Any(x => x.UserId == userId))
            {
                listVMAddress = this.DbContext.Addresses
                .Include(x => x.City)
                .Include(x => x.Country)
                .Where(x => x.UserId == userId)
                .Select(x => new ViewModelAddress
                {
                    AddressId = x.Id,
                    UserId = x.UserId,
                    Street = x.Street,
                    Number = x.No,
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    //CityList = new SelectList(this.DbContext.Cities.Where(y => y.CountryId == x.CountryId).ToList(), "Id", "Name", x.CityId),
                    CountryId = x.CountryId,
                    CountryName = x.Country.Name,
                    //CountryList = new SelectList(this.DbContext.Countries.ToList(), "Id", "Name", x.CountryId),
                    PostCode = x.PostCode,
                    Description = x.Description,
                    InUse = x.InUse
                }).ToList();
            }
            else
            {
                listVMAddress = new List<ViewModelAddress>();
            }


            return new ViewModelAccountDetail
            {
                AddressList = listVMAddress,
                CityList = new SelectList(this.DbContext.Cities, "Id", "Name"),
                CountryList = new SelectList(this.DbContext.Countries, "Id", "Name")
            };
        }
    }
}
