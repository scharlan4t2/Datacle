using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Datacle.DataAccess
{
    public class DtcAttrib
    {
        [Key]
        public Guid ID { get; set; }
        public string Attrib { get; set; }
    }
    public class DtcDesc
    {
        [Key]
        public Guid ID { get; set; }
        public string Desc { get; set; }
    }
    public class DtcSelect
    {
        [Key]
        public Guid ID { get; set; }
        [Key]
        public Guid UserID { get; set; }
    }
}