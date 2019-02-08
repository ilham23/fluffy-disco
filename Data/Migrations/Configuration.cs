namespace Data.Migrations
{
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.DbContext context)
        {
            CreateProductCategorySample(context);
            CreateUser(context);
        }


        private void CreateUser(DbContext context)
        {
            var manager = new UserManager<AspNetUser>(new UserStore<AspNetUser>(new DbContext()));
            if (manager.Users.Count() == 0)
            {
                var roleManager = new RoleManager<AspNetRole>(new RoleStore<AspNetRole>(new DbContext()));

                var user = new AspNetUser()
                {
                    UserName = "admin",
                    Email = "admin@example.com.vn",
                    EmailConfirmed = true,
                    Name = "Administrator",
                    Gender = true,
                    Status = true
                };
                if (manager.Users.Count(x => x.UserName == "admin") == 0)
                {
                    manager.Create(user, "Admin@123");

                    if (!roleManager.Roles.Any())
                    {
                        roleManager.Create(new AspNetRole { Name = "Admin", Description = "Administrator" });
                        roleManager.Create(new AspNetRole { Name = "Member", Description = "User" });
                    }

                    var adminUser = manager.FindByName("admin");

                    manager.AddToRoles(adminUser.Id, new string[] { "Admin", "Member" });
                }
            }
        }

        private void CreateProductCategorySample(Data.DbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name="Male",Alias="ao-nam",Status=true },
                    new ProductCategory() { Name="Female",Alias="ao-nu",Status=true },
                };
                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }

    }
}