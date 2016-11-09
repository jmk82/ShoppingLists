namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeBought : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingListItems", "TimeBought", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingListItems", "TimeBought");
        }
    }
}
