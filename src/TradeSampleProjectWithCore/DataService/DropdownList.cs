using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.DataService
{
    public class DropdownList : BaseClasses.BaseService
    {
        public DropdownList(TradeSampleContext context) : base(context) { }

        public SelectList GetCountryList(object selectedValue = null, bool hasEmptyItem = false)
        {
            var list = this.DbContext.Countries.Select(x => new { x.Id, x.Name }).ToList();
            if (hasEmptyItem)
            {
                list.Insert(0, new { Id = 0, Name = "" });
            }

            return new SelectList(
                list,
                "Id",
                "Name",
                selectedValue);
        }

        public SelectList GetCityList(int countryId, object selectedValue = null, bool hasEmptyItem = false)
        {
            var list = this.DbContext.Cities.Where(x => x.CountryId == countryId).Select(x => new { x.Id, x.Name }).ToList();
            if (hasEmptyItem)
            {
                list.Insert(0, new { Id = 0, Name = "" });
            }

            return new SelectList(
                list,
                "Id",
                "Name",
                selectedValue);
        }
    }
}
