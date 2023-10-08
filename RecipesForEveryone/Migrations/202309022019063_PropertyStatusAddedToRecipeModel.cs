namespace RecipesForEveryone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyStatusAddedToRecipeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "Status");
        }
    }
}
