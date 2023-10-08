namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewModelClassAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        CommentText = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        RecipeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Reviews", new[] { "RecipeId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropTable("dbo.Reviews");
        }
    }
}
