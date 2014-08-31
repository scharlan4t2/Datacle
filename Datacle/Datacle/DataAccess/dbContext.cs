using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Datacle.DataAccess
{
    public class DatacleContext:DbContext
    {
        public DbSet<DtcUser> Users { get; set; }
        public DbSet<DtcUserType> UserTypes { get; set; }
        public DbSet<DtcUserShare> UserShares { get; set; }
        public DbSet<DtcUserList> UserLists { get; set; }
        public DbSet<DtcUserView> UserViews { get; set; }
        public DbSet<DtcList> Lists { get; set; }
        public DbSet<DtcListItem> ListItems { get; set; }
        public DbSet<DtcListType> ListTypes { get; set; }
        public DbSet<DtcListJoin> ListJoins { get; set; }
        public DbSet<DtcViewConn> ViewConns { get; set; }
        public DbSet<DtcView> Views { get; set; }
        public DbSet<DtcViewType> ViewTypes { get; set; }
        public DbSet<DtcViewList> ViewLists { get; set; }
        public DbSet<DtcConnItem> ConnItems { get; set; }
        public DbSet<DtcAttrib> Attribs { get; set; }
        public DbSet<DtcDesc> Descs { get; set; }
        public DbSet<DtcSelect> Selects { get; set; }
        public DatacleContext()
            : base("DatacleContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DtcAttrib>()
                .HasKey(t => t.ID);
            modelBuilder.Entity<DtcDesc>()
                .HasKey(t => t.ID);
            modelBuilder.Entity<DtcSelect>()
                .HasKey(d => new { d.ID, d.UserID });
            modelBuilder.Entity<DtcUser>()
                .HasKey(d => d.ID);
            modelBuilder.Entity<DtcListType>()
                .HasKey(d => d.ID);
            // UserShare
            // User

            modelBuilder.Entity<DtcUser>().Property(v => v.Title).HasMaxLength(60);
            modelBuilder.Entity<DtcUser>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);

            // UserType
            modelBuilder.Entity<DtcUserType>().Property(v => v.Title).HasMaxLength(30);
            modelBuilder.Entity<DtcUserType>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);

            modelBuilder.Entity<DtcUserList>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);

            modelBuilder.Entity<DtcUserView>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);

            // UserView - List Items
            modelBuilder.Entity<DtcUserShare>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            
            
            
            // List
            modelBuilder.Entity<DtcList>().Property(v => v.Title).HasMaxLength(40);
            
            modelBuilder.Entity<DtcList>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            // ListType
            modelBuilder.Entity<DtcListType>().Property(v => v.Title).HasMaxLength(40);
            modelBuilder.Entity<DtcListType>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            // ListItem
            modelBuilder.Entity<DtcListItem>().Property(v => v.Title).HasMaxLength(40);
            modelBuilder.Entity<DtcListItem>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            
            modelBuilder.Entity<DtcListJoin>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            

            modelBuilder.Entity<DtcView>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            
            modelBuilder.Entity<DtcViewType>().Property(v => v.Title).HasMaxLength(40);
            modelBuilder.Entity<DtcViewType>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            modelBuilder.Entity<DtcViewConn>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            modelBuilder.Entity<DtcViewVersion>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            modelBuilder.Entity<DtcViewList>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);
            
            // View 
            modelBuilder.Entity<DtcView>().Property(v => v.Title).HasMaxLength(40);
            // Version
            modelBuilder.Entity<DtcViewVersion>().Property(v => v.Title).HasMaxLength(40);
            modelBuilder.Entity<DtcConnItem>().HasRequired(us => us.Attrib).WithOptional()
                .WillCascadeOnDelete(value: false);


        }
    }
}