using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Walletify.Models.Entities;

namespace Walletify.ApplicationDbContext
{
    public class AppDbContext: IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration, DbContextOptions<AppDbContext> options) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food & Drink", CategoryType = CategoryType.Spending },
                new Category { Id = 2, Name = "Personal Care", CategoryType = CategoryType.Spending },
                new Category { Id = 3, Name = "Transportation", CategoryType = CategoryType.Spending },
                new Category { Id = 4, Name = "Shopping", CategoryType = CategoryType.Spending },
                new Category { Id = 5, Name = "Investment", CategoryType = CategoryType.Income },
                new Category { Id = 6, Name = "Housing", CategoryType = CategoryType.Spending },
                new Category { Id = 7, Name = "Health Care", CategoryType = CategoryType.Spending },
                new Category { Id = 8, Name = "Bonus", CategoryType = CategoryType.Income },
                new Category { Id = 9, Name = "Salary", CategoryType = CategoryType.Income },
                new Category { Id = 10, Name = "Other", CategoryType = CategoryType.Spending },
                new Category { Id = 11, Name = "Other", CategoryType = CategoryType.Income }
            );

            modelBuilder.Entity<IdentityUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<IdentityUser>()
                .HasIndex(u => u.UserName);


        }

    }
}
