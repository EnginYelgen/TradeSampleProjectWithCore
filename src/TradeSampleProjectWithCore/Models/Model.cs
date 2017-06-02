using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TradeSampleProjectWithCore.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TradeSampleProjectWithCore.Models
{
    public class TradeSampleContext : DbContext
    {
        public TradeSampleContext(DbContextOptions<TradeSampleContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }

    public class User : BaseModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(200)]
        [Index(IsUnique = true)]
        public string MailAddress { get; set; }
        [MaxLength(20)]
        public string Telephone { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }

        public List<Address> Addresses { get; set; }
    }

    public class Country : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<Address> Addresses { get; set; }
    }

    public class City : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<Address> Addresses { get; set; }
    }

    public class Address : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(50)]
        public string No { get; set; }
        [Required]
        [MaxLength(20)]
        public string PostCode { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [Index]
        public User User { get; set; }
    }
}