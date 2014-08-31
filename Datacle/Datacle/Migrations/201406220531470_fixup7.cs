namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixup7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews");
            DropIndex("dbo.DtcViews", new[] { "NextPathID" });
            DropColumn("dbo.DtcViews", "ID");
            DropColumn("dbo.DtcPaths", "NextPathID");
            RenameColumn(table: "dbo.DtcViews", name: "NextPathID", newName: "ID");
            RenameColumn(table: "dbo.DtcPaths", name: "NextPath_ID", newName: "NextPathID");
            RenameIndex(table: "dbo.DtcPaths", name: "IX_NextPath_ID", newName: "IX_NextPathID");
            DropPrimaryKey("dbo.DtcViews");
            AlterColumn("dbo.DtcViews", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DtcViews", "ID");
            CreateIndex("dbo.DtcViews", "ID");
            AddForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews", "ID", cascadeDelete: true);
            DropColumn("dbo.DtcPaths", "PrevPathID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DtcPaths", "PrevPathID", c => c.Int());
            DropForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews");
            DropIndex("dbo.DtcViews", new[] { "ID" });
            DropPrimaryKey("dbo.DtcViews");
            AlterColumn("dbo.DtcViews", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DtcViews", "ID");
            RenameIndex(table: "dbo.DtcPaths", name: "IX_NextPathID", newName: "IX_NextPath_ID");
            RenameColumn(table: "dbo.DtcPaths", name: "NextPathID", newName: "NextPath_ID");
            RenameColumn(table: "dbo.DtcViews", name: "ID", newName: "NextPathID");
            AddColumn("dbo.DtcPaths", "NextPathID", c => c.Int(nullable: false));
            AddColumn("dbo.DtcViews", "ID", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.DtcViews", "NextPathID");
            AddForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews", "ID", cascadeDelete: true);
        }
    }
}
