namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEditAllowedPropertyToShare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingListShares", "EditAllowed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingListShares", "EditAllowed");
        }
    }
}
