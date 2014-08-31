namespace Datacle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setUserViews : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DtcUserViews", "Attrib_ID", "dbo.DtcAttribs");
            DropIndex("dbo.DtcUserViews", new[] { "Attrib_ID" });
            DropColumn("dbo.DtcUserViews", "Attrib_ID");
            DropPrimaryKey("dbo.DtcUserViews");
            AlterColumn("dbo.DtcUserViews", "ID", c => c.Guid(nullable: false));
           AddPrimaryKey("dbo.DtcUserViews", "ID");
            CreateIndex("dbo.DtcUserViews", "ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DtcUserViews", new[] { "ID" });
            DropPrimaryKey("dbo.DtcUserViews");
            AlterColumn("dbo.DtcUserViews", "ID", c => c.Guid());
            AddPrimaryKey("dbo.DtcUserViews", "ID");
            RenameColumn(table: "dbo.DtcUserViews", name: "ID", newName: "Attrib_ID");
            AddColumn("dbo.DtcUserViews", "ID", c => c.Guid(nullable: false));
            CreateIndex("dbo.DtcUserViews", "Attrib_ID");
        }
    }
}
