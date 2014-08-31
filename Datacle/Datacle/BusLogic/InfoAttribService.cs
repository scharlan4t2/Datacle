using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datacle.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Mvc;

namespace Datacle.BusLogic
{
    public class AttribInfo
    {
        public Guid? id { get; set; }
        public string attrib { get; set; }
        public static AttribInfo BuildAttribInfo(Guid Id, DtcAttrib dtcattrib)
        {
            var attribInfo = new AttribInfo()
            {
                id = Id
            };
            if (dtcattrib != null)
            {
                attribInfo.attrib = dtcattrib.Attrib;
                if (attribInfo.attrib == null)
                {
                    attribInfo.attrib = "{}";
                }
            }
            return attribInfo;
        }
    }
    public class AttribService
    {
        public void AddAttrib(AttribInfo addattrib)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcAttrib = new DtcAttrib()
                {
                    Attrib = addattrib.attrib,
                    ID = Guid.NewGuid()
                };
                dtc.Attribs.Add(dtcAttrib);
                dtc.SaveChanges();
            }
        }
        public void UpdateAttrib(Guid attribId, AttribInfo updateattrib)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcAttrib = dtc.Attribs.First(vw=>vw.ID==attribId);

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                dtcAttrib.Attrib = JsonConvert.SerializeObject(updateattrib.attrib, Formatting.None, settings);
                dtc.SaveChanges();
            }
        }
        public void DeleteAttrib(Guid Id)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcAttrib = dtc.Attribs.First(vw => vw.ID == Id);
                dtc.Attribs.Remove(dtcAttrib);
                dtc.SaveChanges();
            }
        }
        public List<AttribInfo> Attribs()
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var UserId = user.ID;
                var attribs = dtc.Attribs.ToList();
                var displays = attribs.Select<DtcAttrib, AttribInfo>
                     (at => AttribInfo.BuildAttribInfo(at.ID, at)).ToList();
                return displays;
            }
        }
        public AttribInfo Attrib(Guid attribId)
        {
            using (var dtc = new DatacleContext())
            {
                var attrib = dtc.Attribs.First(at => at.ID == attribId);
                var display = AttribInfo.BuildAttribInfo(attrib.ID, attrib);
                return display;
            }
        }
        public JsonResult GetInfoAttribs()
        {
            var attribs = Attribs();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(attribs, Formatting.None, settings)
            };
        }
        public JsonResult GetInfoAttrib(Guid viewId)
        {
            var attrib = Attrib(viewId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(attrib, Formatting.None, settings)
            };
        }
    }
}