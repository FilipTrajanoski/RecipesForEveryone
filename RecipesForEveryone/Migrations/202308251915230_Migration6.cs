namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DishTypes", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.DishTypes", new[] { "RecipeId" });
            AddColumn("dbo.Recipes", "DishTypesAsString", c => c.String());
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
            
            DropColumn("dbo.Recipes", "DishTypesAsString");
            CreateIndex("dbo.DishTypes", "RecipeId");
            AddForeignKey("dbo.DishTypes", "RecipeId", "dbo.Recipes", "Id", cascadeDelete: true);
        }
    }
}
