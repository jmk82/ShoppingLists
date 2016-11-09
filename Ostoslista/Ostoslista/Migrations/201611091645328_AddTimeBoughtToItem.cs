namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeBoughtToItem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShoppingListItems", "TimeBought", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShoppingListItems", "TimeBought", c => c.DateTime(nullable: false));
        }
    }
}
