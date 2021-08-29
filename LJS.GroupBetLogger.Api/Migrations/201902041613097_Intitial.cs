namespace LJS.GroupBetLogger.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Group.Bets",
                c => new
                    {
                        BetId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BetId)
                .ForeignKey("Group.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "Group.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "Group.GroupUsers",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Admin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.UserId })
                .ForeignKey("Group.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Group.Selections",
                c => new
                    {
                        SelectionId = c.Int(nullable: false, identity: true),
                        BetId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SelectionId)
                .ForeignKey("Group.Bets", t => t.BetId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.BetId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Group.Selections", "UserId", "dbo.AspNetUsers");
            DropForeignKey("Group.Selections", "BetId", "Group.Bets");
            DropForeignKey("Group.GroupUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("Group.GroupUsers", "GroupId", "Group.Groups");
            DropForeignKey("Group.Bets", "GroupId", "Group.Groups");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("Group.Selections", new[] { "UserId" });
            DropIndex("Group.Selections", new[] { "BetId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("Group.GroupUsers", new[] { "UserId" });
            DropIndex("Group.GroupUsers", new[] { "GroupId" });
            DropIndex("Group.Bets", new[] { "GroupId" });
            DropTable("dbo.AspNetRoles");
            DropTable("Group.Selections");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("Group.GroupUsers");
            DropTable("Group.Groups");
            DropTable("Group.Bets");
        }
    }
}
