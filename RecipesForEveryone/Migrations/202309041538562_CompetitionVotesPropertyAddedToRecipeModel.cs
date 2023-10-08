namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompetitionVotesPropertyAddedToRecipeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "CompetitionVotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "CompetitionVotes");
        }
    }
}
