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

        public SelectList GetCountryList(object selectedValue = null)
        {
            return new SelectList(
                this.DbContext.Countries.Select(x => new { x.Id, x.Name }).ToList(),
                "Id",
                "Name",
                selectedValue);
        }

        public SelectList GetCityList(int countryId, object selectedValue = null)
        {
            return new SelectList(
                this.DbContext.Cities.Where(x => x.CountryId == countryId).Select(x => new { x.Id, x.Name }).ToList(),
                "Id",
                "Name",
                selectedValue);
        }
    }
}
