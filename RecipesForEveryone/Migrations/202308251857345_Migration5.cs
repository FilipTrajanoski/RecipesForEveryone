namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DishTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DishTypes", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.DishTypes", new[] { "RecipeId" });
            DropTable("dbo.DishTypes");
        }
    }
}
