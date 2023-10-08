namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FavoriteRecipeClassAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavoriteRecipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FavoriteRecipes");
        }
    }
}
