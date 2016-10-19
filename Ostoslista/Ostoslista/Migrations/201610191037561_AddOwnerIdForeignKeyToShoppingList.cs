namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnerIdForeignKeyToShoppingList : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ShoppingLists", name: "Owner_Id", newName: "OwnerId");
            RenameIndex(table: "dbo.ShoppingLists", name: "IX_Owner_Id", newName: "IX_OwnerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ShoppingLists", name: "IX_OwnerId", newName: "IX_Owner_Id");
            RenameColumn(table: "dbo.ShoppingLists", name: "OwnerId", newName: "Owner_Id");
        }
    }
}
