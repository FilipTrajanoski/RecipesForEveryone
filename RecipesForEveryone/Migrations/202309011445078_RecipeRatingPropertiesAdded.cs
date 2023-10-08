namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecipeRatingPropertiesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "NumberOfRatingVotes", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "TotalSumOfRatings", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "TotalSumOfRatings");
            DropColumn("dbo.Recipes", "NumberOfRatingVotes");
        }
    }
}
