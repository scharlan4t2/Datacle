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
    public class ListJoinDisplay
    {
        public Guid? id { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public bool isselect { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public ListJoinDisplay BuildListJoinDisplay(DtcListJoin listjoin)
        {
            var listjoinDisplay = new ListJoinDisplay()
            {
                id = listjoin.ID,
                isselect = SelectInfo.IsSelect(listjoin.ID),
                desc = "Description",
            };
            return listjoinDisplay;
        }
    }
    public class ListJoinService
    {
        public JsonResult AddListJoin(ListJoinDisplay addlistjoin)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcListJoin = new DtcListJoin(){
                    ID = Guid.NewGuid()
                };
                dtc.ListJoins.Add(dtcListJoin);
                dtc.SaveChanges();
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(dtcListJoin, Formatting.None, settings)
                };
            }
        }
        public ListJoinDisplay ListJoin(Guid listJoinId)
        {
            using (var dtc = new DatacleContext())
            {
                var listjoinDisplay = new ListJoinDisplay();
                var listjoin = dtc.ListJoins.First(lj=>lj.ID==listJoinId);
                var displays = listjoinDisplay.BuildListJoinDisplay(listjoin);
                return displays;
            }
        }
        public List<ListJoinDisplay> ListJoins()
        {
            using (var dtc = new DatacleContext())
            {
                var listjoinDisplay = new ListJoinDisplay();
                var listjoinItems = dtc.ListJoins.ToList();
                var displays = listjoinItems.Select<DtcListJoin, ListJoinDisplay>
                     (lj => listjoinDisplay.BuildListJoinDisplay(lj)).ToList();
                return displays;
            }
        }
        public JsonResult GetDisplayListJoins()
        {
            var listjoins = ListJoins();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listjoins, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayListJoin(Guid ListJoinId)
        {
            var listjoinitem = ListJoinItem(ListJoinId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listjoinitem, Formatting.None, settings)
            };
        }

        private object ListJoinItem(Guid ListJoinId)
        {
            throw new NotImplementedException();
        }

    }

}