namespace Ostoslista.Migrations
{
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Ostoslista.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Ostoslista.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.ShoppingLists.AddOrUpdate(sl => sl.Name, new ShoppingList
            {
                Name = "Testilista",
                Added = DateTime.Now,
                Updated = DateTime.Now,
            });

            context.ShoppingListItems.AddOrUpdate(i => i.Name,
                    new ShoppingListItem
                    {
                        Name = "Maito",
                        Quantity = 2,
                        Added = DateTime.Now,
                        ShoppingListId = 1
                    },
                    new ShoppingListItem
                    {
                        Name = "Leipä",
                        Quantity = 3,
                        Added = DateTime.Now,
                        ShoppingListId = 1
                    }
                );
        }
    }
}
