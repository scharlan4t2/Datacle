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
    public class ListItemDisplay
    {
        public Guid? id { get; set; }
        public string list { get; set; }
        public Guid? listid { get; set; }
        public string title { get; set; }
        
        public string sort { get; set; }
        public bool isselect { get; set; }
        public AttribInfo attrib { get; set; }
        public static ListItemDisplay BuildListItemDisplay(DtcListItem item)
        {
            ListItemDisplay listitem = new ListItemDisplay()
            {
                id = item.ID,
                list = item.List.Title,
                listid = item.ListID,
                title = item.Title,
                isselect = SelectInfo.IsSelect(item.ID),
                sort = "",
                attrib = AttribInfo.BuildAttribInfo(item.ID, item.Attrib),
            };
            return listitem;
        }
    }
    public class ListItemService
    {
        public string AddListType(ListItemDisplay addlist)
        {
            using (var dtc = new DatacleContext())
            {
                //var listType = dtc.ListTypes.First(lt => lt.Title == addlist.litypTitle);
                var dtcListType = new DtcListType()
                {
                    Title = addlist.title,
                    ID = (Guid)addlist.id
                };
                dtc.ListTypes.Add(dtcListType);
                dtc.SaveChanges();
                var displayType = new TypeDisplay();
                displayType.BuildListTypeDisplay(dtcListType);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return JsonConvert.SerializeObject(dtcListType, Formatting.None, settings);
            }
        }
        public JsonResult AddListItem(ListItemDisplay addlistitem)
        {
            using (var dtc = new DatacleContext())
            {
                //var listid = dtc.UserLists.First(ul => ul.ID == addlistitem.listid).ListID;
                var listid = addlistitem.listid;
                var list = dtc.Lists.First(ls => ls.ID == listid);
                var dtcListItem = new DtcListItem()
                {
                    ListID = (Guid)addlistitem.listid,
                    List = list,
                    Title = addlistitem.title,
                    ID = Guid.NewGuid()
                };
                updateListItemInfo(dtcListItem, addlistitem);
                dtc.ListItems.Add(dtcListItem);
                
                dtc.SaveChanges();
                var display = ListItemDisplay.BuildListItemDisplay(dtcListItem);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateListItem(Guid listitemId, ListItemDisplay updatelistitem)
        {
            using (var dtc = new DatacleContext())
            {
                //var listid = dtc.UserLists.First(ul => ul.ID == updatelistitem.listid).ListID;
                var listid = updatelistitem.listid;
                var dtcListItem = dtc.ListItems.First(li => li.ID == listitemId);
                dtcListItem.ListID = (Guid)listid;
                dtcListItem.Title = updatelistitem.title;                
                updateListItemInfo(dtcListItem, updatelistitem);
                dtc.SaveChanges();
                var display = ListItemDisplay.BuildListItemDisplay(dtcListItem);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        private void updateListItemInfo(DtcListItem dtcListItem, ListItemDisplay updatelistitem){
            if (dtcListItem.Attrib == null)
            {
                dtcListItem.Attrib = new DtcAttrib()
                {
                    ID = dtcListItem.ID
                };
            }
            if (updatelistitem.attrib != null)
            {
                if (updatelistitem.attrib.attrib != null)
                {
                    dtcListItem.Attrib.Attrib = updatelistitem.attrib.attrib;
                }
            }
        }
        
        public JsonResult DeleteListItem(Guid listitemId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcListItem = dtc.ListItems.First(li => li.ID == listitemId);
                var display = new ListItemDisplay() { listid = dtcListItem.ListID};
                dtc.ListItems.Remove(dtcListItem);
                dtc.SaveChanges();
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(display, Formatting.None, settings)
                };
            }
        }
        public List<ListItemDisplay> ListItems()
        {
            using (var dtc = new DatacleContext())
            {
                var listItems = dtc.ListItems.OrderBy(li=>li.Title).ToList();
                var displays = listItems.Select<DtcListItem, ListItemDisplay>
                     (li => ListItemDisplay.BuildListItemDisplay(li)).ToList();
                return displays;
            }
        }
        public List<ListItemDisplay> ListItems(Guid listId)
        {
            using (var dtc = new DatacleContext())
            {
                var listItems = dtc.ListItems.Where(li => li.ListID.Equals(listId))
                                .OrderBy(li => li.Title).ToList();
                var displays = listItems.Select<DtcListItem, ListItemDisplay>
                     (li => ListItemDisplay.BuildListItemDisplay(li)).ToList();
                return displays;
            }
        }
        public ListItemDisplay ListItem(Guid listItemId)
        {
            using (var dtc = new DatacleContext())
            {
                var listItem = dtc.ListItems.First(li => li.ID.Equals(listItemId));
                var displays = ListItemDisplay.BuildListItemDisplay(listItem);
                return displays;
            }
        }
        public JsonResult GetDisplayListItems()
        {
            var listitems = ListItems();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listitems, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayListItems(Guid listId)
        {
            var listitems = ListItems(listId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listitems, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayListItem(Guid listItemId)
        {
            var listitems = ListItems(listItemId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listitems, Formatting.None, settings)
            };
        }

    }

}