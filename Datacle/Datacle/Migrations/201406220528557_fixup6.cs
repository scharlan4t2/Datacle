namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixup6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DtcViews", "ID", "dbo.DtcPaths");
            DropForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews");
            DropIndex("dbo.DtcViews", new[] { "ID" });
            DropPrimaryKey("dbo.DtcViews");
            AddColumn("dbo.DtcViews", "NextPathID", c => c.Int(nullable: false));
            AlterColumn("dbo.DtcViews", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DtcViews", "ID");
            CreateIndex("dbo.DtcViews", "NextPathID");
            AddForeignKey("dbo.DtcViews", "NextPathID", "dbo.DtcPaths", "ID");
            AddForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews");
            DropForeignKey("dbo.DtcViews", "NextPathID", "dbo.DtcPaths");
            DropIndex("dbo.DtcViews", new[] { "NextPathID" });
            DropPrimaryKey("dbo.DtcViews");
            AlterColumn("dbo.DtcViews", "ID", c => c.Int(nullable: false));
            DropColumn("dbo.DtcViews", "NextPathID");
            AddPrimaryKey("dbo.DtcViews", "ID");
            CreateIndex("dbo.DtcViews", "ID");
            AddForeignKey("dbo.DtcConnections", "ViewID", "dbo.DtcViews", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DtcViews", "ID", "dbo.DtcPaths", "ID");
        }
    }
}
