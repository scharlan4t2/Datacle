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
    public class ViewConnDisplay
    {
        public Guid? id { get; set; }
        public Guid viewid { get; set; }
        public Guid versionid { get; set; }
        public ViewConnDisplay BuildViewConnDisplay(DtcViewConn viewConn)
        {
            var listDisplay = new ViewConnDisplay()
            {
                id = viewConn.ID,
                viewid = viewConn.ViewID,
                versionid = viewConn.VersionID
            };
            return listDisplay;
        }
    }
    public class ViewConnService
    {
        public void AddConnection(ViewConnDisplay addConn)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcConnection = new DtcViewConn()
                {
                    ViewID = addConn.viewid,
                    VersionID = addConn.versionid,
                    ID = Guid.NewGuid()
                };
                dtc.ViewConns.Add(dtcConnection);
                dtc.SaveChanges();
            }
        }
        public void UpdateConnection(Guid Id, ViewConnDisplay updateConn)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewConn = dtc.ViewConns.First(vw => vw.ID == Id);
                dtcViewConn.ViewID = updateConn.viewid;
                dtcViewConn.VersionID = updateConn.versionid;
                dtc.SaveChanges();
            }
        }
        public void DeleteConnection(Guid Id)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewConn = dtc.ViewConns.First(vw => vw.ID == Id);
                dtc.ViewConns.Remove(dtcViewConn);
                dtc.SaveChanges();
            }
        }
        public List<ViewConnDisplay> ViewConns()
        {
            var viewConnDisplay = new ViewConnDisplay();
            using (var dtc = new DatacleContext())
            {
                var conns = dtc.ViewConns.ToList();
                var displays = conns.Select<DtcViewConn, ViewConnDisplay>
                     (vc => viewConnDisplay.BuildViewConnDisplay(vc)).ToList();
                return displays;
            }
        }
        public ViewConnDisplay ViewConn(Guid Id)
        {
            var viewConnDisplay = new ViewConnDisplay();
            using (var dtc = new DatacleContext())
            {
                var dtcViewConn = dtc.ViewConns.First(vw => vw.ID == Id);
                var display = viewConnDisplay.BuildViewConnDisplay(dtcViewConn);
                return display;
            }
        }
        public JsonResult GetDisplayViewConns()
        {
            var viewConns = ViewConns();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(viewConns, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayViewConn(Guid Id)
        {
            var viewConn = ViewConn(Id);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(viewConn, Formatting.None, settings)
            };
        }
    }
}