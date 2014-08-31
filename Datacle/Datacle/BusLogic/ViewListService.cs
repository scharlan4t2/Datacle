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
    public class ViewListDisplay
    {
        public Guid? id { get; set; }
        public Guid viewid { get; set; }
        public Guid listid { get; set; }
        public bool isselect { get; set; }
        public static ViewListDisplay BuildViewListDisplay(DtcViewList list)
        {
            var listDisplay = new ViewListDisplay()
            {
                id = list.ID,
                viewid = list.ViewID,
                isselect = SelectInfo.IsSelect(list.ID),
                listid = list.ListID
            };
            return listDisplay;
        }
    }
    public class ViewListService
    {
        public void AddViewList(ViewListDisplay viewList)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewList = new DtcViewList()
                {
                    ID = Guid.NewGuid(),
                    ListID = viewList.listid,
                    ViewID = viewList.viewid
                };
                dtc.ViewLists.Add(dtcViewList);
                dtc.SaveChanges();
            }
        }
        public void DeleteViewList(Guid Id)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewList = dtc.ViewLists.First(vw => vw.ID == Id);
                dtc.ViewLists.Remove(dtcViewList);
                dtc.SaveChanges();
            }
        }
        public void UpdateViewList(Guid Id, ViewListDisplay updateList)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewList = dtc.ViewLists.First(vw => vw.ID == Id);
                dtcViewList.ListID = updateList.listid;
                dtcViewList.ViewID = updateList.viewid;
                dtc.SaveChanges();
            }
        }
        public List<ViewListDisplay> ViewLists()
        {
            using (var dtc = new DatacleContext())
            {
                var lists = dtc.ViewLists.ToList();
                var displays = lists.Select<DtcViewList, ViewListDisplay>
                     (vl => ViewListDisplay.BuildViewListDisplay(vl)).ToList();
                return displays;
            }
        }
        public ViewListDisplay ViewLists(Guid viewListId)
        {
            using (var dtc = new DatacleContext())
            {
                var viewlist = dtc.ViewLists.First(vl => vl.ID==viewListId);
                var display = ViewListDisplay.BuildViewListDisplay(viewlist);
                return display;
            }
        }
        private bool IsViewListPath(DtcViewList list, List<ViewListDisplay> paths)
        {
            return paths.Count(p => p.listid == list.ID) > 0;
        }
        public JsonResult GetDisplayViewList()
        {
            var lists = ViewLists();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayViewList(Guid viewListId)
        {
            var lists = ViewLists();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
    }
}