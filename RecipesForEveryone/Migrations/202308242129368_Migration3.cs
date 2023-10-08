namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DishTypes", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.DishTypes", new[] { "RecipeId" });
            DropTable("dbo.DishTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DishTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.DishTypes", "RecipeId");
            AddForeignKey("dbo.DishTypes", "RecipeId", "dbo.Recipes", "Id", cascadeDelete: true);
        }
    }
}
