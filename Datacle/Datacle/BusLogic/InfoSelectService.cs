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
    public class SelectInfo
    {
        public static List<Guid> SelectList
        {   get
            {
                var selectList = (List<Guid>)HttpContext.Current.Session["select"];
                var selectUser = (string)HttpContext.Current.Session["selectUser"];
                var activeUser = (string)HttpContext.Current.Session["login"];
                if (selectList == null || selectUser != activeUser)
                {
                    selectList = new List<Guid>();
                    using (var dtc = new DatacleContext())
                    {
                        var user = ShareService.loginUser(dtc);
                        if( user!=null){
                            var userSelects = dtc.Selects.Where(ds => ds.UserID == user.ID);
                            selectList = userSelects.Select<DtcSelect,Guid>(ds=>ds.ID).ToList();
                            HttpContext.Current.Session["selectUser"] = activeUser;
                        }
                    }
                    HttpContext.Current.Session["select"] = selectList;
                }
                return selectList;
            }
        }
        public static bool IsSelect(Guid item)
        {
            return SelectList.Contains(item);
        }

        public Guid id { get; set; }
        public bool isselect { get; set; }
        
        public static SelectInfo BuildSelectInfo(Guid Id, bool isselect)
        {
            var selectInfo = new SelectInfo()
            {
                id = Id,
                isselect = isselect
            };
            return selectInfo;
        }
    }
    public class SelectService
    {
        public static void AddSelect(SelectInfo addselect)
        {
            using (var dtc = new DatacleContext())
            {
                AddSelect(addselect, dtc);
                dtc.SaveChanges();
            }
        }
        public static void AddSelect(Guid selectId, DatacleContext dtc)
        {
            var selectInfo = new SelectInfo()
            {
                id = selectId,
                isselect = true
            };
            AddSelect(selectInfo, dtc);
        }
        private static void AddSelect(SelectInfo addselect, DatacleContext dtc)
        {
            var user = ShareService.loginUser(dtc);

            var cur = dtc.Selects.Where(sel => sel.ID == addselect.id &&
                                        sel.UserID == user.ID);
            //clear existing
            if (cur.Count() > 0)
            {
                dtc.Selects.RemoveRange(cur);
                SelectInfo.SelectList.Remove(addselect.id);
            }
            if (addselect.isselect)
            {
                var dtcSelect = new DtcSelect()
                {
                    ID = (Guid)addselect.id,
                    UserID = (Guid)user.ID
                };
                SelectInfo.SelectList.Add(dtcSelect.ID);
                dtc.Selects.Add(dtcSelect);
            }
        }
        //public void UpdateSelect(Guid selectId, SelectInfo updateselect)
        //{
        //    using (var dtc = new DatacleContext())
        //    {
        //        var dtcSelect = dtc.Selects.First(vw => vw.ID == selectId);
        //        dtcSelect.UserID = (Guid)updateselect.userid;
        //        dtc.SaveChanges();
        //    }
        //}
        public void DeleteSelect(Guid Id)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcSelect = dtc.Selects.First(vw => vw.ID == Id);
                dtc.Selects.Remove(dtcSelect);
                dtc.SaveChanges();
            }
        }
        public List<SelectInfo> Selects()
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var selects = dtc.Selects.ToList();
                var displays = selects.Select<DtcSelect, SelectInfo>
                     (de => SelectInfo.BuildSelectInfo(de.ID, true)).ToList();
                return displays;
            }
        }
        public SelectInfo Select(Guid selectId)
        {
            using (var dtc = new DatacleContext())
            {
                var select = dtc.Selects.First(vw => vw.ID == selectId);
                var display = SelectInfo.BuildSelectInfo(select.ID, true);
                return display;
            }
        }
        public JsonResult GetInfoSelects()
        {
            var selects = Selects();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(selects, Formatting.None, settings)
            };
        }
        public JsonResult GetInfoSelect(Guid viewId)
        {
            var select = Select(viewId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(select, Formatting.None, settings)
            };
        }
    }
}