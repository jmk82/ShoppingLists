namespace Ostoslista.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShoppingListShare : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingListShares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShoppingListId = c.Int(nullable: false),
                        ReceiverUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShoppingListShares");
        }
    }
}
