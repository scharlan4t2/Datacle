namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DtcAttribs",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Attrib = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DtcConnItems",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ViewConnID = c.Guid(nullable: false),
                        ViewListID = c.Guid(nullable: false),
                        ListItemID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcViewConns", t => t.ViewConnID, cascadeDelete: false)
                .ForeignKey("dbo.DtcViewLists", t => t.ViewListID, cascadeDelete: false)
                .ForeignKey("dbo.DtcListItems", t => t.ListItemID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ViewConnID)
                .Index(t => t.ViewListID)
                .Index(t => t.ListItemID);
            
            CreateTable(
                "dbo.DtcListItems",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                        ListID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcLists", t => t.ListID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ListID);
            
            CreateTable(
                "dbo.DtcLists",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                        OwnerID = c.Guid(nullable: false),
                        ListTypeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcListTypes", t => t.ListTypeID, cascadeDelete: false)
                .ForeignKey("dbo.DtcUsers", t => t.OwnerID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.OwnerID)
                .Index(t => t.ListTypeID);
            
            CreateTable(
                "dbo.DtcListTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.DtcUsers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 60),
                        UserTypeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcUserTypes", t => t.UserTypeID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.UserTypeID);
            
            CreateTable(
                "dbo.DtcUserLists",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ListID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcLists", t => t.ListID, cascadeDelete: false)
                .ForeignKey("dbo.DtcUsers", t => t.UserID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.UserID)
                .Index(t => t.ListID);
            
            CreateTable(
                "dbo.DtcUserShares",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ShareID = c.Guid(nullable: false),
                        DtcUser_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcUsers", t => t.ShareID, cascadeDelete: false)
                .ForeignKey("dbo.DtcUsers", t => t.UserID, cascadeDelete: false)
                .ForeignKey("dbo.DtcUsers", t => t.DtcUser_ID)
                .Index(t => t.ID)
                .Index(t => t.UserID)
                .Index(t => t.ShareID)
                .Index(t => t.DtcUser_ID);
            
            CreateTable(
                "dbo.DtcUserTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.DtcUserViews",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ViewID = c.Guid(nullable: false),
                        DtcList_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcUsers", t => t.UserID, cascadeDelete: false)
                .ForeignKey("dbo.DtcViews", t => t.ViewID, cascadeDelete: false)
                .ForeignKey("dbo.DtcLists", t => t.DtcList_ID)
                .Index(t => t.ID)
                .Index(t => t.UserID)
                .Index(t => t.ViewID)
                .Index(t => t.DtcList_ID);
            
            CreateTable(
                "dbo.DtcViews",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                        ViewTypeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcViewTypes", t => t.ViewTypeID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ViewTypeID);
            
            CreateTable(
                "dbo.DtcViewConns",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ViewID = c.Guid(nullable: false),
                        VersionID = c.Guid(nullable: false),
                        ViewVersion_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcViews", t => t.ViewID, cascadeDelete: false)
                .ForeignKey("dbo.DtcViewVersions", t => t.ViewVersion_ID)
                .Index(t => t.ID)
                .Index(t => t.ViewID)
                .Index(t => t.ViewVersion_ID);
            
            CreateTable(
                "dbo.DtcViewVersions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                        ViewID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcViews", t => t.ViewID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ViewID);
            
            CreateTable(
                "dbo.DtcViewLists",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ViewID = c.Guid(nullable: false),
                        ListID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcLists", t => t.ListID, cascadeDelete: false)
                .ForeignKey("dbo.DtcViews", t => t.ViewID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ViewID)
                .Index(t => t.ListID);
            
            CreateTable(
                "dbo.DtcViewTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.DtcDescs",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DtcListJoins",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ListID = c.Guid(nullable: false),
                        JoinID = c.Guid(nullable: false),
                        ListItemID = c.Guid(nullable: false),
                        JoinItemID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.ID)
                .ForeignKey("dbo.DtcLists", t => t.JoinID, cascadeDelete: false)
                .ForeignKey("dbo.DtcListItems", t => t.JoinItemID, cascadeDelete: false)
                .ForeignKey("dbo.DtcLists", t => t.ListID, cascadeDelete: false)
                .ForeignKey("dbo.DtcListItems", t => t.ListItemID, cascadeDelete: false)
                .Index(t => t.ID)
                .Index(t => t.ListID)
                .Index(t => t.JoinID)
                .Index(t => t.ListItemID)
                .Index(t => t.JoinItemID);
            
            CreateTable(
                "dbo.DtcSelects",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.UserID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DtcListJoins", "ListItemID", "dbo.DtcListItems");
            DropForeignKey("dbo.DtcListJoins", "ListID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcListJoins", "JoinItemID", "dbo.DtcListItems");
            DropForeignKey("dbo.DtcListJoins", "JoinID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcListJoins", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcConnItems", "ListItemID", "dbo.DtcListItems");
            DropForeignKey("dbo.DtcUserViews", "DtcList_ID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcViews", "ViewTypeID", "dbo.DtcViewTypes");
            DropForeignKey("dbo.DtcViewTypes", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcViewLists", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcViewLists", "ListID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcConnItems", "ViewListID", "dbo.DtcViewLists");
            DropForeignKey("dbo.DtcViewLists", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcViewConns", "ViewVersion_ID", "dbo.DtcViewVersions");
            DropForeignKey("dbo.DtcViewVersions", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcViewVersions", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcViewConns", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcConnItems", "ViewConnID", "dbo.DtcViewConns");
            DropForeignKey("dbo.DtcViewConns", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUserViews", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcViews", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUserViews", "UserID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserViews", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUsers", "UserTypeID", "dbo.DtcUserTypes");
            DropForeignKey("dbo.DtcUserTypes", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUserShares", "DtcUser_ID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserShares", "UserID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserShares", "ShareID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserShares", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUserLists", "UserID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserLists", "ListID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcUserLists", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcLists", "OwnerID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUsers", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcLists", "ListTypeID", "dbo.DtcListTypes");
            DropForeignKey("dbo.DtcListTypes", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcListItems", "ListID", "dbo.DtcLists");
            DropForeignKey("dbo.DtcLists", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcListItems", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcConnItems", "ID", "dbo.DtcAttribs");
            DropIndex("dbo.DtcListJoins", new[] { "JoinItemID" });
            DropIndex("dbo.DtcListJoins", new[] { "ListItemID" });
            DropIndex("dbo.DtcListJoins", new[] { "JoinID" });
            DropIndex("dbo.DtcListJoins", new[] { "ListID" });
            DropIndex("dbo.DtcListJoins", new[] { "ID" });
            DropIndex("dbo.DtcViewTypes", new[] { "ID" });
            DropIndex("dbo.DtcViewLists", new[] { "ListID" });
            DropIndex("dbo.DtcViewLists", new[] { "ViewID" });
            DropIndex("dbo.DtcViewLists", new[] { "ID" });
            DropIndex("dbo.DtcViewVersions", new[] { "ViewID" });
            DropIndex("dbo.DtcViewVersions", new[] { "ID" });
            DropIndex("dbo.DtcViewConns", new[] { "ViewVersion_ID" });
            DropIndex("dbo.DtcViewConns", new[] { "ViewID" });
            DropIndex("dbo.DtcViewConns", new[] { "ID" });
            DropIndex("dbo.DtcViews", new[] { "ViewTypeID" });
            DropIndex("dbo.DtcViews", new[] { "ID" });
            DropIndex("dbo.DtcUserViews", new[] { "DtcList_ID" });
            DropIndex("dbo.DtcUserViews", new[] { "ViewID" });
            DropIndex("dbo.DtcUserViews", new[] { "UserID" });
            DropIndex("dbo.DtcUserViews", new[] { "ID" });
            DropIndex("dbo.DtcUserTypes", new[] { "ID" });
            DropIndex("dbo.DtcUserShares", new[] { "DtcUser_ID" });
            DropIndex("dbo.DtcUserShares", new[] { "ShareID" });
            DropIndex("dbo.DtcUserShares", new[] { "UserID" });
            DropIndex("dbo.DtcUserShares", new[] { "ID" });
            DropIndex("dbo.DtcUserLists", new[] { "ListID" });
            DropIndex("dbo.DtcUserLists", new[] { "UserID" });
            DropIndex("dbo.DtcUserLists", new[] { "ID" });
            DropIndex("dbo.DtcUsers", new[] { "UserTypeID" });
            DropIndex("dbo.DtcUsers", new[] { "ID" });
            DropIndex("dbo.DtcListTypes", new[] { "ID" });
            DropIndex("dbo.DtcLists", new[] { "ListTypeID" });
            DropIndex("dbo.DtcLists", new[] { "OwnerID" });
            DropIndex("dbo.DtcLists", new[] { "ID" });
            DropIndex("dbo.DtcListItems", new[] { "ListID" });
            DropIndex("dbo.DtcListItems", new[] { "ID" });
            DropIndex("dbo.DtcConnItems", new[] { "ListItemID" });
            DropIndex("dbo.DtcConnItems", new[] { "ViewListID" });
            DropIndex("dbo.DtcConnItems", new[] { "ViewConnID" });
            DropIndex("dbo.DtcConnItems", new[] { "ID" });
            DropTable("dbo.DtcSelects");
            DropTable("dbo.DtcListJoins");
            DropTable("dbo.DtcDescs");
            DropTable("dbo.DtcViewTypes");
            DropTable("dbo.DtcViewLists");
            DropTable("dbo.DtcViewVersions");
            DropTable("dbo.DtcViewConns");
            DropTable("dbo.DtcViews");
            DropTable("dbo.DtcUserViews");
            DropTable("dbo.DtcUserTypes");
            DropTable("dbo.DtcUserShares");
            DropTable("dbo.DtcUserLists");
            DropTable("dbo.DtcUsers");
            DropTable("dbo.DtcListTypes");
            DropTable("dbo.DtcLists");
            DropTable("dbo.DtcListItems");
            DropTable("dbo.DtcConnItems");
            DropTable("dbo.DtcAttribs");
        }
    }
}
