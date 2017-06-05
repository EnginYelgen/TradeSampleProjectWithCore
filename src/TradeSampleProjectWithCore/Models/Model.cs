﻿using System;
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
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetail> ShoppingCartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart");
            modelBuilder.Entity<ShoppingCartDetail>().ToTable("ShoppingCartDetail");
            modelBuilder.Entity<Order>().ToTable("Order");

            modelBuilder.Entity<User>()
                .HasIndex(e => e.MailAddress)
                .HasName("Index_MailAddress")
                .IsUnique(true);

            //modelBuilder.Entity<Address>()
            //    .HasIndex(e => e.User)
            //    .HasName("Index_User");

            modelBuilder.Entity<Product>()
                .HasIndex(e => e.StockCode)
                .HasName("Index_StockCode")
                .IsUnique(true);
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
        public int UpdateUserId { get; set; }
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
        public User UpdateUser { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

    public class Country : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<City> Cities { get; set; }
    }

    public class City : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }

        public ICollection<Address> Addresses { get; set; }
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
        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }
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

        public ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }

    public class ShoppingCart : BaseModel
    {
        public DateTime CreateDate { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }
    }

    public class ShoppingCartDetail : BaseModel
    {
        [Required]
        public int NumberOfProduct { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public ShoppingCart ShoppingCart { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
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
        public DateTime DeliveryDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public ShoppingCart ShoppingCart { get; set; }
    }
}