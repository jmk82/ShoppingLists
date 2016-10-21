namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysToShares : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShoppingListShares", "ReceiverUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ShoppingListShares", "ShoppingListId");
            CreateIndex("dbo.ShoppingListShares", "ReceiverUserId");
            AddForeignKey("dbo.ShoppingListShares", "ReceiverUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ShoppingListShares", "ShoppingListId", "dbo.ShoppingLists", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingListShares", "ShoppingListId", "dbo.ShoppingLists");
            DropForeignKey("dbo.ShoppingListShares", "ReceiverUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ShoppingListShares", new[] { "ReceiverUserId" });
            DropIndex("dbo.ShoppingListShares", new[] { "ShoppingListId" });
            AlterColumn("dbo.ShoppingListShares", "ReceiverUserId", c => c.String());
        }
    }
}
