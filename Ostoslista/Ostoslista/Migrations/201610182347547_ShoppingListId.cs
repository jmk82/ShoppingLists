namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingListId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShoppingListItems", "ShoppingList_Id", "dbo.ShoppingLists");
            DropIndex("dbo.ShoppingListItems", new[] { "ShoppingList_Id" });
            RenameColumn(table: "dbo.ShoppingListItems", name: "ShoppingList_Id", newName: "ShoppingListId");
            AlterColumn("dbo.ShoppingListItems", "ShoppingListId", c => c.Int(nullable: false));
            CreateIndex("dbo.ShoppingListItems", "ShoppingListId");
            AddForeignKey("dbo.ShoppingListItems", "ShoppingListId", "dbo.ShoppingLists", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingListItems", "ShoppingListId", "dbo.ShoppingLists");
            DropIndex("dbo.ShoppingListItems", new[] { "ShoppingListId" });
            AlterColumn("dbo.ShoppingListItems", "ShoppingListId", c => c.Int());
            RenameColumn(table: "dbo.ShoppingListItems", name: "ShoppingListId", newName: "ShoppingList_Id");
            CreateIndex("dbo.ShoppingListItems", "ShoppingList_Id");
            AddForeignKey("dbo.ShoppingListItems", "ShoppingList_Id", "dbo.ShoppingLists", "Id");
        }
    }
}
