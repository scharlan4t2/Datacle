using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Datacle.DataAccess
{
    public class DtcView
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }
        public Guid ViewTypeID { get; set; }
        [ForeignKey("ViewTypeID")]
        public virtual DtcViewType ViewType { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual ICollection<DtcUserView> UserViews { get; set; }
        public virtual List<DtcViewVersion> ViewVersions { get; set; }
        public virtual List<DtcViewList> ViewLists { get; set; }
        public virtual List<DtcViewConn> ViewConns { get; set; }
    }
    public class DtcViewType
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual ICollection<DtcView> Views { get; set; }
    }

    public class DtcViewList
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ViewID { get; set; }
        public virtual DtcView View { get; set; }

        public Guid ListID { get; set; }
        public virtual DtcList List { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual List<DtcConnItem> ConnItems { get; set; }
    }
    public class DtcViewConn
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ViewID { get; set; }
        public virtual DtcView View { get; set; }
        public Guid VersionID { get; set; }
        public virtual DtcViewVersion ViewVersion { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual List<DtcConnItem> ConnItems { get; set; }
    }
    public class DtcViewVersion
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }

        public Guid ViewID { get; set; }
        public virtual DtcView View { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

        public virtual List<DtcViewConn> ViewConns { get; set; }
    }
    
    public class DtcConnItem
    {
        [Key]
        public Guid ID { get; set; }

        public Guid ViewConnID { get; set; }
        [ForeignKey("ViewConnID")]
        public virtual DtcViewConn ViewConn { get; set; }

        public Guid ViewListID { get; set; }
        [ForeignKey("ViewListID")]
        public virtual DtcViewList ViewList { get; set; }

        public Guid ListItemID { get; set; }
        [ForeignKey("ListItemID")]
        public virtual DtcListItem ListItem { get; set; }

        public virtual DtcAttrib Attrib { get; set; }

    }
}