namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "NumberOfVotes", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "TotalSumOfRatings", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "TotalSumOfRatings");
            DropColumn("dbo.Recipes", "NumberOfVotes");
        }
    }
}
