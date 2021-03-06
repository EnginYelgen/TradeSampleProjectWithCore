﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TradeSampleProjectWithCore.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TradeSampleProjectWithCore.Models
{
    public class TradeSampleContext : DbContext
    //public class TradeSampleContext : IdentityDbContext<User, Role, string>
    {
        public TradeSampleContext(DbContextOptions<TradeSampleContext> options) : base(options) { }
        //public TradeSampleContext(DbContextOptions<TradeSampleContext> options)
        //    : base(options)
        //{ }

        public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetail> ShoppingCartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            //modelBuilder.Entity<Role>().ToTable("Role");
            //modelBuilder.Entity<UserRole>().ToTable("UserRole");

            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart");
            modelBuilder.Entity<ShoppingCartDetail>().ToTable("ShoppingCartDetail");
            modelBuilder.Entity<Order>().ToTable("Order");

            //modelBuilder.Ignore<IdentityUser<string>>();
            //modelBuilder.Ignore<IdentityRole<string>>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
            //modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityUserClaim<string>>();
            //modelBuilder.Ignore<IdentityUserToken<string>>();
            //modelBuilder.Ignore<IdentityRoleClaim<string>>();

            //modelBuilder.Ignore<User>();
            //modelBuilder.Ignore<Role>();
            //modelBuilder.Ignore<UserRole>();


            // PK
            //modelBuilder.Entity<User>()
            //    .Property(x => x.Id)
            //    .UseSqlServerIdentityColumn();
            //modelBuilder.Entity<UserRole>()
            //    .Property(x => x.UserRoleId)
            //    .UseSqlServerIdentityColumn();

            // Constraints
            //modelBuilder.Entity<User>()
            //    .HasIndex(e => e.MailAddress)
            //    .HasName("Index_User_MailAddress")
            //    .IsUnique(true);

            modelBuilder.Entity<Product>()
                .HasIndex(e => e.StockCode)
                .HasName("Index_Product_StockCode")
                .IsUnique(true);

            modelBuilder.Entity<Country>()
                .HasIndex(e => e.Name)
                .HasName("Index_Country_Name")
                .IsUnique(true);

            modelBuilder.Entity<City>()
                .HasIndex(e => new { e.Name, e.CountryId })
                .HasName("Index_City_NameAndCountryId")
                .IsUnique(true);

            // Keys
            modelBuilder.Entity<Address>()
                .HasOne(e => e.User)
                .WithMany(e => e.AddressesUser)
                .HasForeignKey(e => e.UserId);

            //modelBuilder.Entity<Address>()
            //    .HasOne(e => e.UpdateUser)
            //    .WithMany(e => e.AddressesUpdateUser)
            //    .HasForeignKey(e => e.UpdateUserId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict;
            }

        }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
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
        public string MailAddress { get; set; }
        [MaxLength(20)]
        public string Telephone { get; set; }
        [Required]
        public bool InUse { get; set; }
        public int? UpdateUserId { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }

        [ForeignKey("UpdateUserId")]
        public virtual User UpdateUser { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Address> AddressesUpdateUser { get; set; }
        public virtual ICollection<Address> AddressesUser { get; set; }
    }

    //public class User : IdentityUser
    //{
    //    //[Key]
    //    //public int UserId { get; set; }
    //    [Required]
    //    public DateTime Birthday { get; set; }
    //    [Required]
    //    public bool InUse { get; set; }
    //    //public string UpdateUserId { get; set; }
    //    [Required]
    //    public DateTime UpdateDate { get; set; }

    //    //[ForeignKey("UpdateUserId")]
    //    //public virtual User UpdateUser { get; set; }

    //    //public virtual ICollection<User> Users { get; set; }
    //    //public virtual ICollection<Address> AddressesUpdateUser { get; set; }
    //    public virtual ICollection<Address> AddressesUser { get; set; }
    //    public virtual ICollection<ShoppingCart> AShoppingCartsUser { get; set; }
    //}

    //public class Role : IdentityRole { }

    //public class UserRole : IdentityUserRole<string>
    //{
    //    [Key]
    //    public int UserRoleId { get; set; }
    //}

    public class Country : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }

    public class City : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }

    public class Address : BaseModel
    {
        //[Key]
        //public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string AddressName { get; set; }
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
        public bool IsDeleted { get; set; }
        //[Required]
        //public DateTime UpdateDate { get; set; }

        [Required]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [Required]
        public int UserId { get; set; }
        //[ForeignKey("UserId")]
        public virtual User User { get; set; }

        //[Required]
        //public string UpdateUserId { get; set; }
        ////[ForeignKey("UpdateUserId")]
        //public virtual User UpdateUser { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class ProductCategory : BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product : BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string StockCode { get; set; }
        [Required]
        public int StockNumber { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }

    public class ShoppingCart : BaseModel
    {
        public DateTime CreateDate { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }

    public class ShoppingCartDetail : BaseModel
    {
        [Required]
        public int NumberOfProduct { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public class Order : BaseModel
    {
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal Total { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}