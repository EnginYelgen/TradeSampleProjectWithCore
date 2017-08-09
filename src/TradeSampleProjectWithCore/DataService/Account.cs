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
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => new ViewModelAddress
                {
                    AddressId = x.Id,
                    AddressName = x.AddressName,
                    UserId = x.UserId,
                    Street = x.Street,
                    Number = x.No,
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    CountryId = x.CountryId,
                    CountryName = x.Country.Name,
                    PostCode = x.PostCode,
                    Description = x.Description,
                    InUse = x.InUse,
                    IsDeleted = x.IsDeleted
                }).ToList();
            }
            else
            {
                listVMAddress = new List<ViewModelAddress>();
            }

            return new ViewModelAccountDetail
            {
                AddressList = listVMAddress
            };
        }

        public List<ViewModelAddress> GetAddressList(int userId)
        {
            List<ViewModelAddress> vmAddressList = new List<ViewModelAddress>();

            if (this.DbContext.Addresses.Any(x => x.UserId == userId))
            {
                vmAddressList = this.DbContext.Addresses
                    .Include(x => x.City)
                    .Include(x => x.Country)
                    .Where(x => x.UserId == userId && !x.IsDeleted)
                    .Select(x => new ViewModelAddress
                    {
                        AddressId = x.Id,
                        AddressName = x.AddressName,
                        UserId = x.UserId,
                        Street = x.Street,
                        Number = x.No,
                        CityId = x.CityId,
                        CityName = x.City.Name,
                        CountryId = x.CountryId,
                        CountryName = x.Country.Name,
                        PostCode = x.PostCode,
                        Description = x.Description,
                        InUse = x.InUse,
                        IsDeleted = x.IsDeleted
                    }).ToList();
            }

            return vmAddressList;
        }

        public bool SaveAddress(Address address, out string errMess)
        {
            bool result = true;
            errMess = string.Empty;

            try
            {
                this.DbContext.Addresses.Add(new Address
                {
                    AddressName = address.AddressName,
                    CityId = address.CityId,
                    CountryId = address.CountryId,
                    Description = address.Description,
                    InUse = this.DbContext.Addresses.Any() ? false : address.InUse,
                    IsDeleted = false,
                    No = address.No,
                    Street = address.Street,
                    PostCode = address.PostCode,
                    UpdateDate = DateTime.Now,
                    UpdateUserId = address.UpdateUserId,
                    UserId = address.UserId
                });

                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                errMess = "İşlem sırasında hata oluştu.";
                result = false;
            }

            return result;
        }

        public bool SetActiveAddress(int addressId, int userId, out string errMess)
        {
            bool result = true;
            errMess = string.Empty;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Address address in this.DbContext.Addresses)
                    {
                        address.InUse = address.Id == addressId;
                        address.UpdateDate = DateTime.Now;
                        address.UpdateUserId = userId;

                        this.DbContext.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    errMess = "İşlem sırasında hata oluştu.";
                    result = false;
                    transaction.Rollback();
                }
            }

            return result;
        }

        public bool DeleteAddress(int addressId, int userId, out string errMess)
        {
            bool result = true;
            errMess = string.Empty;

            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    Address address = this.DbContext.Addresses.Where(x => x.Id == addressId).Single();
                    address.IsDeleted = true;
                    address.UpdateDate = DateTime.Now;
                    address.UpdateUserId = userId;

                    this.DbContext.SaveChanges();

                    if (address.InUse && this.DbContext.Addresses.Any(x => x.Id != addressId && !x.IsDeleted))
                    {
                        address = this.DbContext.Addresses.Where(x => x.Id != addressId && !x.IsDeleted).OrderByDescending(x => x.Id).Single();
                        address.InUse = true;
                        address.UpdateDate = DateTime.Now;
                        address.UpdateUserId = userId;

                        this.DbContext.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    errMess = "İşlem sırasında hata oluştu.";
                    result = false;
                    transaction.Rollback();
                }
            }

            return result;
        }
    }
}
