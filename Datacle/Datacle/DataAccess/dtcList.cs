using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Datacle.DataAccess
{
    public class DtcList
    {
        [Key]
        public Guid ID { get; set; }
        public string Title {get;set;}
        public Guid OwnerID { get; set; }

        public Guid ListTypeID { get; set; }
        [ForeignKey("OwnerID")]
        public virtual DtcUser Owner { get; set; }
        [ForeignKey("ListTypeID")]
        public virtual DtcListType ListType { get; set; }
        public virtual DtcAttrib Attrib { get; set; }
        
        
        public virtual ICollection<DtcUserList> UserLists { get; set; }
//public virtual ICollection<DtcUserView> UserViews { get; set; }
        public virtual ICollection<DtcListItem> ListItems { get; set; }
        public virtual ICollection<DtcViewList> ViewLists { get; set; }
    }
    public class DtcListType
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
                        
        public virtual ICollection<DtcList> Lists { get; set; }
    }

    public class DtcListItem
    {
        [Key]
        public Guid ID { get; set; }
        public string Title {get;set;}

        public Guid ListID { get; set; }
        [ForeignKey("ListID")]
        public virtual DtcList List { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
    }

    public class DtcListJoin
    {
        [Key]
        public Guid ID { get; set; }
        public Guid ListID { get; set; }
        public Guid JoinID { get; set; }

        [ForeignKey("ListID")]
        public DtcList List { get; set; }
        [ForeignKey("JoinID")]
        public DtcList Join { get; set; }

        public Guid ListItemID { get; set; }
        public Guid JoinItemID { get; set; }
        [ForeignKey("ListItemID")]
        public DtcListItem ListItem { get; set; }
        [ForeignKey("JoinItemID")]
        public DtcListItem JoinItem { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
    }
}