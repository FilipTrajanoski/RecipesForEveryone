namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompetitionLevelPropertyToRecipeClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "CompetitionLevel", c => c.Int(nullable: false));
            DropColumn("dbo.Recipes", "NumberOfVotes");
            DropColumn("dbo.Recipes", "TotalSumOfRatings");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipes", "TotalSumOfRatings", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "NumberOfVotes", c => c.Int(nullable: false));
            DropColumn("dbo.Recipes", "CompetitionLevel");
        }
    }
}
