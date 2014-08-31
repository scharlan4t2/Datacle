using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Datacle.DataAccess
{
    public class DtcUserShare
    {
        [Key]
        public Guid ID { get; set; }
        
        public Guid UserID { get; set; }
        public Guid ShareID { get; set; }

        [ForeignKey("UserID")]
        public virtual DtcUser User { get; set; }
        [ForeignKey("ShareID")]
        public virtual DtcUser Share { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
    }
    public class DtcUser
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }

        public Guid UserTypeID { get; set; }
        [ForeignKey("UserTypeID")]
        public virtual DtcUserType UserType { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual ICollection<DtcUserShare> UserShares { get; set; }
        
        public virtual ICollection<DtcList> Lists { get; set; }
        public virtual ICollection<DtcUserList> UserLists { get; set; }
        public virtual ICollection<DtcUserView> UserViews { get; set; }
    }
    public class DtcUserList
    {
        [Key]
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid ListID { get; set; }
        [ForeignKey("UserID")]
        public virtual DtcUser User { get; set; }
        [ForeignKey("ListID")]
        public virtual DtcList List { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
    }
    public class DtcUserView
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid ViewID { get; set; }

        [ForeignKey("UserID")]
        public virtual DtcUser User { get; set; }
        [ForeignKey("ViewID")]
        public virtual DtcView View { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
    }
    public class DtcUserType
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }

        public virtual DtcAttrib Attrib { get; set; }
        public virtual ICollection<DtcUser> Users { get; set; }
    }
}
