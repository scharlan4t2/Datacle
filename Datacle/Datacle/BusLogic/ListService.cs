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
    public class TypeDisplay
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public bool isselect { get; set; }
        public string sort { get; set; }

        public AttribInfo attrib { get; set; }
        
        public TypeDisplay BuildListTypeDisplay(DtcListType listtype)
        {
            var listtypeDisplay = new TypeDisplay()
            {
                id = listtype.ID,
                isselect = SelectInfo.IsSelect(listtype.ID),
                title = listtype.Title,
                sort = "",
                attrib = AttribInfo.BuildAttribInfo(listtype.ID, listtype.Attrib),
            };
            return listtypeDisplay;
        }
        public TypeDisplay BuildViewTypeDisplay(DtcViewType viewtype)
        {
            var viewtypeDisplay = new TypeDisplay()
            {
                id = viewtype.ID,
                isselect = SelectInfo.IsSelect(viewtype.ID),
                sort = "",
                title = viewtype.Title,
                attrib = AttribInfo.BuildAttribInfo(viewtype.ID, viewtype.Attrib),
            };
            return viewtypeDisplay;
        }
        public TypeDisplay BuildUserTypeDisplay(DtcUserType listtype)
        {
            var listtypeDisplay = new TypeDisplay()
            {
                id = listtype.ID,
                title = listtype.Title
            };
            return listtypeDisplay;
        }
    }
    public class ListDisplay
    {
        public Guid? id { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public string title { get; set; }

        public string sort { get; set; }
        public bool isselect  { get; set; }
        public AttribInfo attrib { get; set; }
        public static ListDisplay BuildListDisplay(DtcList list)
        {
            var listDisplay = new ListDisplay()
            {
                id = list.ID,
                title = list.Title,
                type = list.ListType.Title,
                typeid = list.ListTypeID,
                isselect = SelectInfo.IsSelect(list.ID),
                sort = "",
                attrib = AttribInfo.BuildAttribInfo(list.ID, list.Attrib),
            };
            return listDisplay;
        }
    }
    public class ListItemsDisplay
    {
        public Guid? listId { get; set; }
        public List<ListItemDisplay> items { get; set; }
        public ListItemsDisplay BuildListItemDisplay(Guid ListId, List<DtcListItem> dtclistItems)
        {
            var listitems = new ListItemsDisplay();
            listitems.listId = ListId;
            listitems.items = dtclistItems.Select<DtcListItem, ListItemDisplay>
                    (li => ListItemDisplay.BuildListItemDisplay(li)).ToList();
            return listitems;
        }
    }
    public class ListService
    {
        public JsonResult AddListType(TypeDisplay addlisttype)
        {
            DtcListType dtcListType = null;
            using (var dtc = new DatacleContext())
            {
                if (addlisttype.id == Guid.Empty)
                {
                    var listtypeid = Guid.NewGuid();
                    dtcListType = new DtcListType()
                    {
                        ID = listtypeid,
                        Title = addlisttype.title
                    };
                    dtc.ListTypes.Add(dtcListType);
                }
                else
                {
                    dtcListType = dtc.ListTypes.First(lt => lt.ID == addlisttype.id);
                    dtcListType.Title = addlisttype.title;
                }
                updateListTypeInfo(dtcListType, addlisttype);
                dtc.SaveChanges();
                var displayType = new TypeDisplay();
                var listtypeDisplay = displayType.BuildListTypeDisplay(dtcListType);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(listtypeDisplay, Formatting.None, settings)
                };
            }
        }
        public void DeleteList(Guid listId)
        {
            using (var dtc = new DatacleContext())
            {
                DeleteList(listId, dtc);
            }
        }
        public void DeleteList(Guid listID, DatacleContext dtc)
        {
            var user = ShareService.loginUser(dtc);
            var userID = user.ID;
            var dtcList = dtc.Lists.First(ls => ls.ID == listID);
            var findUserList = dtc.UserLists.First(uv => uv.UserID==userID &&
                                                        uv.ListID == listID);
            if (dtcList.OwnerID.Equals(userID))
            {
                var dtcUserLists = dtc.UserLists.Where(uv => uv.ListID == listID).ToList();
                foreach (DtcUserList dtcUserList in dtcUserLists)
                {
                    dtc.UserLists.Remove(dtcUserList);
                }

                dtc.Lists.Remove(dtcList);
            }
            dtc.SaveChanges();
        }
        public void DeleteListType(Guid listtypeId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcListTypes = dtc.ListTypes.Where(vt => vt.ID == listtypeId).ToList();
                if (dtcListTypes.Count() > 0) { //if exists
                    var dtcListType = dtcListTypes.First();
                    var dtcLists = dtc.Lists.Where(vw => vw.ListTypeID == listtypeId).ToList();
                    foreach (DtcList dtcList in dtcLists) {
                        DeleteList(dtcList.ID, dtc);
                    }
                    dtc.ListTypes.Remove(dtcListType);
                    dtc.SaveChanges();
                }
            }
        }
        public JsonResult UpdateListType(Guid listtypeId, TypeDisplay updatelisttype)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcListType = dtc.ListTypes.First(ls => ls.ID == listtypeId);
                dtcListType.Title = updatelisttype.title;
                updateListTypeInfo(dtcListType, updatelisttype);
                dtc.SaveChanges();
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(updatelisttype, Formatting.None, settings)
                };
            }
        }
        private void updateListTypeInfo(DtcListType dtcListType, TypeDisplay updatelisttype)
        {
            if (dtcListType.Attrib == null)
            {
                dtcListType.Attrib = new DtcAttrib()
                {
                    ID = dtcListType.ID
                };
            }
            if (updatelisttype.attrib != null)
            {
                dtcListType.Attrib.Attrib = updatelisttype.attrib.attrib;
            }
        }

        public List<TypeDisplay> ListTypes()
        {
            using (var dtc = new DatacleContext())
            {
                var typeDisplay = new TypeDisplay();
                var listTypes = dtc.ListTypes.ToList();
                var displays = listTypes.Select<DtcListType, TypeDisplay>
                     (lityp => typeDisplay.BuildListTypeDisplay(lityp)).ToList();
                return displays;
            }
        }
        public JsonResult GetDisplayListType()
        {
            var listtypes = ListTypes();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listtypes, Formatting.None, settings)
            };
        }
        public JsonResult AddList(ListDisplay addlist)
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var listid = Guid.NewGuid();
                var dtcList = new DtcList()
                {
                    ID = listid,
                    OwnerID = user.ID,
                    Title = addlist.title,
                    ListTypeID = (Guid)addlist.typeid
                };
                updateListInfo(dtcList, addlist);
                dtc.Lists.Add(dtcList);
                dtcList.ListType = dtc.ListTypes.First(vt => vt.ID == dtcList.ListTypeID);
                var dtcUserList = new DtcUserList()
                {
                    ID = Guid.NewGuid(),
                    List = dtcList,
                    ListID = dtcList.ID,
                    //User = user,
                    UserID = user.ID                    
                };
                updateUserListInfo(dtcUserList, addlist);
                dtc.UserLists.Add(dtcUserList);
                dtc.SaveChanges();
                addlist.id = dtcList.ID;
                var display = ListDisplay.BuildListDisplay(dtcList);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateList(Guid listId, ListDisplay updatelist)
        {
            using (var dtc = new DatacleContext())
            {
                //var userlists = dtc.UserLists.First(ul => ul.ID == listId);
                var dtcList = dtc.Lists.First(ls => ls.ID == listId);
                dtcList.ListTypeID = (Guid)updatelist.typeid;
                dtcList.Title = updatelist.title;
                updateListInfo(dtcList, updatelist);
                dtc.SaveChanges();
                var display = ListDisplay.BuildListDisplay(dtcList);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        private void updateListInfo(DtcList dtcList, ListDisplay updatelist)
        {
            if (dtcList.Attrib == null)
            {
                dtcList.Attrib = new DtcAttrib()
                {
                    ID = dtcList.ID
                };
            }
            if (updatelist.attrib != null)
            {
                dtcList.Attrib.Attrib = updatelist.attrib.attrib;
            }
        }
        private void updateUserListInfo(DtcUserList dtcUserList, ListDisplay updatelist)
        {
            if (dtcUserList.Attrib == null)
            {
                dtcUserList.Attrib = new DtcAttrib()
                {
                    ID = dtcUserList.ID
                };
            }
            if (updatelist.attrib != null)
            {
                dtcUserList.Attrib.Attrib = updatelist.attrib.attrib;
            }
        }

        public List<ListDisplay> Lists()
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var userlists = dtc.UserLists.Where(ul => ul.UserID == user.ID);
                var lists = userlists.Select<DtcUserList,DtcList>(usl=>usl.List).ToList();
                var displays = lists.Select<DtcList, ListDisplay>
                     (li => ListDisplay.BuildListDisplay(li)).ToList();
                return displays;
            }
        }
        public ListItemsDisplay List(Guid ListId)
        {
            using (var dtc = new DatacleContext())
            {
                var listDisplay = new ListItemsDisplay();
                var listitems = dtc.ListItems.Where(li => li.ListID.Equals(ListId)).ToList();
                var display = listDisplay.BuildListItemDisplay(ListId, listitems);
                return display;
            }
        }
        public JsonResult GetDisplayList()
        {
            var lists = Lists();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayList(Guid listId)
        {
            var list = List(listId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(list, Formatting.None, settings)
            };
        }

    }

}