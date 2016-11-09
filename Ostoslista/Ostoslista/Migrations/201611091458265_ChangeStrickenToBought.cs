namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStrickenToBought : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingListItems", "Bought", c => c.Boolean(nullable: false));
            DropColumn("dbo.ShoppingListItems", "Striked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShoppingListItems", "Striked", c => c.Boolean(nullable: false));
            DropColumn("dbo.ShoppingListItems", "Bought");
        }
    }
}
