namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduserview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DtcUserViews",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ViewID = c.Guid(nullable: false),
                        Attrib_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DtcAttribs", t => t.Attrib_ID)
                .ForeignKey("dbo.DtcUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.DtcViews", t => t.ViewID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ViewID)
                .Index(t => t.Attrib_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DtcUserViews", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcUserViews", "UserID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserViews", "Attrib_ID", "dbo.DtcAttribs");
            DropIndex("dbo.DtcUserViews", new[] { "Attrib_ID" });
            DropIndex("dbo.DtcUserViews", new[] { "ViewID" });
            DropIndex("dbo.DtcUserViews", new[] { "UserID" });
            DropTable("dbo.DtcUserViews");
        }
    }
}
