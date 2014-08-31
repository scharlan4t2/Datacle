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
    public class ViewDisplay
    {
        public Guid? id { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public string title { get; set; }

        public bool isselect { get; set; }
        public string sort { get; set; }
        public List<ViewListDisplay> lists { get; set; }
        public List<ListDisplay> addlists { get; set; }
        public AttribInfo attrib { get; set; }
        public static ViewDisplay BuildViewDisplay(DtcView view)
        {
            //var selview = view.ViewLists.Select<DtcViewList, ViewListDisplay>(vl => ViewListDisplay.BuildViewListDisplay(vl));
            //var viewlist = selview.ToList();
            var viewDisplay = new ViewDisplay()
            {
                id = view.ID,
                title = view.Title,
                //lists = viewlist,
                type = view.ViewType.Title,
                typeid = view.ViewTypeID,
                sort = "",
                isselect = SelectInfo.IsSelect(view.ID),
                attrib = AttribInfo.BuildAttribInfo(view.ID, view.Attrib),
            };
            return viewDisplay;
        }
        public static ViewDisplay BuildViewDisplay(DtcView view, DatacleContext dtc)
        {
            var selview = view.ViewLists.Select<DtcViewList, ViewListDisplay>(vl => ViewListDisplay.BuildViewListDisplay(vl));
            var viewlist = selview.ToList();
            var user = ShareService.loginUser(dtc);
            var seladds = user.UserLists.ToList();
            var addlists = seladds.Select<DtcUserList, ListDisplay>(ls => ListDisplay.BuildListDisplay(ls.List)).ToList();
            var listDisplay = new ViewDisplay()
            {
                id = view.ID,
                title = view.Title,
                lists = viewlist,
                addlists = addlists,
                sort = "",
                isselect = SelectInfo.IsSelect(view.ID),
                attrib = AttribInfo.BuildAttribInfo(view.ID, view.Attrib),
            };
            return listDisplay;
        }
    }
    public class ViewConnsDisplay
    {
        public Guid? viewId { get; set; }
        public List<ViewConnDisplay> conns { get; set; }
        public ViewConnsDisplay BuildViewConnsDisplay(Guid ViewId, List<DtcViewConn> dtcviewconns)
        {
            var viewconnDisplay = new ViewConnDisplay();
            var viewDisplay = new ViewConnsDisplay();
            viewDisplay.viewId= ViewId;
            viewDisplay.conns = dtcviewconns.Select<DtcViewConn,ViewConnDisplay>
                (vc=>viewconnDisplay.BuildViewConnDisplay(vc)).ToList();
            return viewDisplay;
        }
    }
    public class ViewListsDisplay
    {
        public Guid? viewId { get; set; }
        public List<ViewListDisplay> conns { get; set; }
        public ViewListsDisplay BuildViewListsDisplay(Guid ViewId, List<DtcViewList> dtcviewconns)
        {
            var viewDisplay = new ViewListsDisplay();
            viewDisplay.viewId = ViewId;
            viewDisplay.conns = dtcviewconns.Select<DtcViewList, ViewListDisplay>
                (vc => ViewListDisplay.BuildViewListDisplay(vc)).ToList();
            return viewDisplay;
        }
    }
    public class ViewService
    {
        public JsonResult AddViewType(TypeDisplay addviewtype)
        {
            using (var dtc = new DatacleContext())
            {
                //var viewType = 
                DtcViewType dtcViewType = null;
                if (addviewtype.id == Guid.Empty)
                {
                    var viewtypeid = Guid.NewGuid();
                    dtcViewType = new DtcViewType()
                    {
                        ID = viewtypeid,
                        Title = addviewtype.title
                    };
                    dtc.ViewTypes.Add(dtcViewType);
                }
                else
                {
                    dtcViewType = dtc.ViewTypes.First(lt => lt.ID == addviewtype.id);
                    dtcViewType.Title = addviewtype.title;
                }
                updateViewTypeInfo(dtcViewType, addviewtype);
                dtc.SaveChanges();
                var displayType = new TypeDisplay();
                var viewtypeDisplay = displayType.BuildViewTypeDisplay(dtcViewType);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(viewtypeDisplay, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateViewType(Guid viewtypeId, TypeDisplay updateviewtype)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewType = dtc.ViewTypes.First(ls => ls.ID == viewtypeId);
                dtcViewType.Title = updateviewtype.title;
                updateViewTypeInfo(dtcViewType, updateviewtype);
                dtc.SaveChanges();
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(updateviewtype, Formatting.None, settings)
                };
            }
        }
        private void updateViewTypeInfo(DtcViewType dtcViewType, TypeDisplay updateviewtype)
        {
            if (dtcViewType.Attrib == null)
            {
                dtcViewType.Attrib = new DtcAttrib()
                {
                    ID = dtcViewType.ID
                };
            }
            if (updateviewtype.attrib != null)
            {
                dtcViewType.Attrib.Attrib = updateviewtype.attrib.attrib;
            }
        }
        public List<TypeDisplay> ViewTypes()
        {
            using (var dtc = new DatacleContext())
            {
                var typeDisplay = new TypeDisplay();
                var viewTypes = dtc.ViewTypes.ToList();
                var displays = viewTypes.Select<DtcViewType, TypeDisplay>
                     (lityp => typeDisplay.BuildViewTypeDisplay(lityp)).ToList();
                return displays;
            }
        }
        public JsonResult GetDisplayViewType()
        {
            var viewtypes = ViewTypes();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(viewtypes, Formatting.None, settings)
            };
        }
        public JsonResult AddView(ViewDisplay addview)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcView = new DtcView()
                {
                    ID = Guid.NewGuid(),
                    Title = addview.title,
                    ViewTypeID= (Guid)addview.typeid
                };
                updateViewInfo(dtcView, addview);
                dtc.Views.Add(dtcView);
                dtc.SaveChanges();
                dtcView.ViewType = dtc.ViewTypes.First(vt => vt.ID == dtcView.ViewTypeID);
                var user = ShareService.loginUser(dtc);
                var userView = new DtcUserView()
                {
                    ID = Guid.NewGuid(),
                    ViewID = dtcView.ID,
                    UserID = user.ID
                };
                updateUserViewInfo(userView, addview);
                dtc.UserViews.Add(userView);
                dtc.SaveChanges();
                var display = ViewDisplay.BuildViewDisplay(dtcView);                
                
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateView(Guid viewId, ViewDisplay updateview)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcView = dtc.Views.First(vw=>vw.ID==viewId);
                dtcView.Title = updateview.title;
                dtcView.ViewTypeID = (Guid)updateview.typeid;
                updateViewInfo(dtcView, updateview);
                dtc.SaveChanges();

                var display = ViewDisplay.BuildViewDisplay(dtcView);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        private void updateViewInfo(DtcView dtcView, ViewDisplay updateview)
        {
            if (dtcView.Attrib == null)
            {
                dtcView.Attrib = new DtcAttrib()
                {
                    ID = dtcView.ID
                };
            }
            if (updateview.attrib != null)
            {
                dtcView.Attrib.Attrib = updateview.attrib.attrib;
            }
        }
        private void updateUserViewInfo(DtcUserView dtcUserView, ViewDisplay updateview)
        {
            if (dtcUserView.Attrib == null)
            {
                dtcUserView.Attrib = new DtcAttrib()
                {
                    ID = dtcUserView.ID
                };
            }
            if (updateview.attrib != null)
            {
                dtcUserView.Attrib.Attrib = updateview.attrib.attrib;
            }
        }
        public void DeleteView(Guid Id)
        {
            using (var dtc = new DatacleContext())
            {
                DeleteView(Id, dtc);
            }
        }
        public void DeleteView(Guid Id, DatacleContext dtc)
        {
                var dtcUserViews = dtc.UserViews.Where(uv => uv.ViewID == Id).ToList();
                foreach (DtcUserView dtcUserView in dtcUserViews)
                {
                    dtc.UserViews.Remove(dtcUserView);
                }
                var dtcView = dtc.Views.First(vw => vw.ID == Id);
                dtc.Views.Remove(dtcView);
                dtc.SaveChanges();
        }
        public void DeleteViewType(Guid viewtypeId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcViewTypes = dtc.ViewTypes.Where(vt => vt.ID == viewtypeId).ToList();
                if (dtcViewTypes.Count() > 0)
                { //if exists
                    var dtcViews = dtc.Views.Where(vw => vw.ViewTypeID == viewtypeId).ToList();
                    foreach (DtcView dtcView in dtcViews) {
                        DeleteView(dtcView.ID, dtc);
                    }
                    var dtcViewType = dtcViewTypes.First();
                    dtc.ViewTypes.Remove(dtcViewType);
                    dtc.SaveChanges();
                }
            }
        }
        public List<ViewDisplay> Views()
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var userviews = dtc.UserViews.Where(us => user.ID == user.ID);
                var views = userviews.Select<DtcUserView, DtcView>(usv => usv.View).ToList();
                var displays = views.Select<DtcView, ViewDisplay>
                     (vw => ViewDisplay.BuildViewDisplay(vw)).ToList();
                return displays;
            }
        }
        public ViewDisplay View(Guid viewId)
        {
            using (var dtc = new DatacleContext())
            {
                var view = dtc.Views.First(vw => vw.ID==viewId);
                var display = ViewDisplay.BuildViewDisplay(view,dtc);
                return display;
            }
        }
        public JsonResult GetDisplayViews()
        {
            var lists = Views();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayView(Guid viewId)
        {
            var view = View(viewId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(view, Formatting.None, settings)
            };
        }
    }
}