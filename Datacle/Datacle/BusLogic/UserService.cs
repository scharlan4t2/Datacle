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
    public class UserTypeDisplay
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public bool isselect { get; set; }
        
        public AttribInfo attrib { get; set; }
       
        public static UserTypeDisplay BuildUserTypeDisplay(DtcUserType usertype)
        {
            var listtypeDisplay = new UserTypeDisplay()
            {
                id = usertype.ID,
                title = usertype.Title,
                isselect = SelectInfo.IsSelect(usertype.ID),
                attrib = AttribInfo.BuildAttribInfo(usertype.ID, usertype.Attrib)
            };
            return listtypeDisplay;
        }
    }
    public class UserDisplay
    {
        public Guid? id { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public string title { get; set; }
        public bool isselect { get; set; }
        public bool isuser { get; set; }
        
        public AttribInfo attrib { get; set; }
        public static UserDisplay BuildUserDisplay(Guid userID, DtcUser user)
        {
            var typeDisplay = new UserDisplay()
            {
                id = user.ID,
                title = user.Title,
                type = user.UserType.Title,
                typeid = user.UserTypeID,
                isselect = SelectInfo.IsSelect(user.ID),
                isuser = (user.ID == userID),
                attrib = AttribInfo.BuildAttribInfo(user.ID, user.Attrib),
            };
            return typeDisplay;
        }
    }
    public class UserService
    {
        public Guid GetUserID(string Username)
        {
            using (var dtc = new DatacleContext())
            {
                var users = dtc.Users.Where(us => us.Title == Username);
                if (users.Count() > 0)
                    return dtc.Users.First(us => us.Title == Username).ID;
                else
                    return Guid.Empty;
            }
        }
        public JsonResult AddUserType(TypeDisplay addtype)
        {
            using (var dtc = new DatacleContext())
            {
                //var listType = dtc.Types.First(lt => lt.Title == addlist.litypTitle);
                var dtcType = new DtcUserType()
                {
                    Title = addtype.title,
                    ID = (Guid)addtype.id
                };
                updateTypeInfo(dtcType, addtype);
                dtc.UserTypes.Add(dtcType);
                dtc.SaveChanges();
                var displayType = new UserTypeDisplay();
                var typedisplay = UserTypeDisplay.BuildUserTypeDisplay(dtcType);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(typedisplay, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateUserType(Guid typeId, TypeDisplay updatetype)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcType = dtc.UserTypes.First(us => us.ID == typeId);
                dtcType.Title = updatetype.title;
                updateTypeInfo(dtcType, updatetype);
                dtc.SaveChanges();

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(dtcType, Formatting.None, settings)
                };
            }
        }
        private void updateTypeInfo(DtcUserType dtcUserType, TypeDisplay updatetype)
        {
            if (dtcUserType.Attrib == null)
            {
                dtcUserType.Attrib = new DtcAttrib()
                {
                    ID = dtcUserType.ID
                };
            }
            if (updatetype.attrib != null)
            {
                dtcUserType.Attrib.Attrib = updatetype.attrib.attrib;
            }
        }

        public List<UserTypeDisplay> Types()
        {
            using (var dtc = new DatacleContext())
            {
                var listTypes = dtc.UserTypes.ToList();
                var displays = listTypes.Select<DtcUserType, UserTypeDisplay>
                     (lityp => UserTypeDisplay.BuildUserTypeDisplay(lityp)).ToList();
                return displays;
            }
        }
        public JsonResult AddUser(UserDisplay adduser)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcUser = new DtcUser()
                {
                    UserTypeID = (Guid)adduser.typeid,
                    Title = adduser.title,
                    ID = Guid.NewGuid()
                };
                updateUserInfo(dtcUser, adduser);
                dtc.Users.Add(dtcUser);
                dtc.SaveChanges();
                dtcUser.UserType = dtc.UserTypes.First(ut => ut.ID == dtcUser.UserTypeID);
                var userdisplay = UserDisplay.BuildUserDisplay(dtcUser.ID, dtcUser);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(userdisplay, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateUser(Guid userId, UserDisplay updateuser)
        {
            using (var dtc = new DatacleContext())
            {
                var displayUser = new UserDisplay();
                var dtcUser = dtc.Users.First(us => us.ID == userId);
                dtcUser.UserTypeID = (Guid)updateuser.typeid;
                dtcUser.Title = updateuser.title;
                updateUserInfo(dtcUser, updateuser);
                dtc.SaveChanges();
                var userdisplay = UserDisplay.BuildUserDisplay(userId, dtcUser);

                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(userdisplay, Formatting.None, settings)
                };
            }
        }
        private void updateUserInfo(DtcUser dtcUser, UserDisplay updateuser)
        {
            if (dtcUser.Attrib == null)
            {
                dtcUser.Attrib = new DtcAttrib()
                {
                    ID = dtcUser.ID
                };
            }
            if (updateuser.attrib != null)
            {
                dtcUser.Attrib.Attrib = updateuser.attrib.attrib;
            }
        }

        public void DeleteUser(Guid userId)
        {
            using (var dtc = new DatacleContext())
            {
                DeleteUser(userId, dtc);
            }
        }

        public void DeleteUser(Guid userId, DatacleContext dtc)
        {
            var dtcUser = dtc.Users.First(us => us.ID == userId);
            dtc.Users.Remove(dtcUser);
            dtc.SaveChanges();
        }
        public void DeleteUserType(Guid usertypeId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcUserTypes = dtc.UserTypes.Where(vt => vt.ID == usertypeId);
                if (dtcUserTypes.Count() > 0)
                { //if exists
                    var dtcUsers = dtc.Users.Where(vw => vw.UserTypeID == usertypeId);
                    foreach (DtcUser dtcUser in dtcUsers)
                    {
                        DeleteUser(dtcUser.ID, dtc);
                    }
                    var dtcUserType = dtcUserTypes.First();
                    dtc.UserTypes.Remove(dtcUserType);
                    dtc.SaveChanges();
                }
            }
        }
        public List<UserDisplay> Users()
        {
            List<UserDisplay> displays = new List<UserDisplay>();
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                if (user != null)
                {
                    var users = dtc.Users.ToList();
                    displays = users.Select<DtcUser, UserDisplay>
                         (li => UserDisplay.BuildUserDisplay(user.ID, li)).ToList();
                    var userdisplay = displays.Where(ud => ud.id == user.ID);
                    if (userdisplay.Count() > 0)
                        userdisplay.First().isselect = true;
                }
            }
            return displays;
        }

        public List<UserDisplay> UserShares()
        {
            using (var dtc = new DatacleContext())
            {
                var user = ShareService.loginUser(dtc);
                var lists = dtc.Users.ToList();
                var displays = lists.Select<DtcUser, UserDisplay>
                     (us => UserDisplay.BuildUserDisplay(user.ID, us)).ToList();
                return displays;
            }
        }
        public JsonResult GetDisplayUserType()
        {
            var listtypes = Types();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(listtypes, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayUser()
        {
            var lists = Users();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplayShare()
        {
            var lists = UserShares();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }

    }
}