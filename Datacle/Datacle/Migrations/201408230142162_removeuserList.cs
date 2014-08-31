namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeuserList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DtcUserViews", "ID", "dbo.DtcAttribs");
            DropForeignKey("dbo.DtcUserViews", "UserID", "dbo.DtcUsers");
            DropForeignKey("dbo.DtcUserViews", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcUserViews", "DtcList_ID", "dbo.DtcLists");
            DropIndex("dbo.DtcUserViews", new[] { "ID" });
            DropIndex("dbo.DtcUserViews", new[] { "UserID" });
            DropIndex("dbo.DtcUserViews", new[] { "ViewID" });
            DropIndex("dbo.DtcUserViews", new[] { "DtcList_ID" });
            DropTable("dbo.DtcUserViews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DtcUserViews",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        ViewID = c.Guid(nullable: false),
                        DtcList_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.DtcUserViews", "DtcList_ID");
            CreateIndex("dbo.DtcUserViews", "ViewID");
            CreateIndex("dbo.DtcUserViews", "UserID");
            CreateIndex("dbo.DtcUserViews", "ID");
            AddForeignKey("dbo.DtcUserViews", "DtcList_ID", "dbo.DtcLists", "ID");
            AddForeignKey("dbo.DtcUserViews", "ViewID", "dbo.DtcViews", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DtcUserViews", "UserID", "dbo.DtcUsers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DtcUserViews", "ID", "dbo.DtcAttribs", "ID");
        }
    }
}
