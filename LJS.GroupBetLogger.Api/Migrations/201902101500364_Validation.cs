namespace LJS.GroupBetLogger.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Group.Selections", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Group.Selections", "Name", c => c.String());
        }
    }
}
