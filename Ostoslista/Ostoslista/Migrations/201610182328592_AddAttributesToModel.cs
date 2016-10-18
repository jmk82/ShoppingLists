namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttributesToModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShoppingLists", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ShoppingListItems", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShoppingListItems", "Name", c => c.String());
            AlterColumn("dbo.ShoppingLists", "Name", c => c.String());
        }
    }
}
