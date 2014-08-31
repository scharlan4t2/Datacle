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
    public class UserRequestDisplay
    {
        public Guid? id { get; set; }
        public string name { get; set; }
        public AttribInfo attrib { get; set; }
    }
    public class ShareDisplay
    {
        public Guid? id { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public string title { get; set; }
        public bool isselect { get; set; }

        public AttribInfo attrib { get; set; }

        public static ShareDisplay BuildUserDisplay(DtcUserShare usershare)
        {
            var share = usershare.Share;                
            var shareDisplay = new ShareDisplay()
            {
                id = usershare.ID,
                title = share.Title,
                type = share.UserType.Title,
                typeid = share.UserTypeID,
                isselect = SelectInfo.IsSelect(usershare.ID),
                attrib = AttribInfo.BuildAttribInfo(share.ID, share.Attrib),
            };
            return shareDisplay;
        }
    }
    public class SharedListDisplay : ShareListDisplay
    {
        public Guid? shareid { get; set; }
        
    }
    public class ShareListDisplay
    {
        public Guid? id { get; set; }
        public Guid? listid { get; set; }
        public string type { get; set; }
        public Guid? typeid { get; set; }
        public string title { get; set; }
        public bool isselect { get; set; }

        public AttribInfo attrib { get; set; }
        public static ShareListDisplay BuildShareListDisplay(DtcUserList userlist)
        {
            DtcList list = userlist.List;
            var sharelistDisplay = new ShareListDisplay()
            {
                id = userlist.ID,
                listid = list.ID,
                title = list.Title,
                type = list.ListType.Title,
                typeid = list.ListType.ID,
                isselect = SelectInfo.IsSelect(userlist.ID),
                attrib = AttribInfo.BuildAttribInfo(userlist.ID, userlist.Attrib),
            };
            return sharelistDisplay;
        }
        public static ShareListDisplay BuildShareListDisplay(DtcUserList userlist, Guid shareID, DatacleContext dtc)
        {
            DtcList list = userlist.List;
            var usershareID = dtc.UserShares.First(us => us.ID == shareID).ShareID;
            var sharelist = dtc.UserLists.Where(ul => ul.UserID == usershareID && ul.ListID == userlist.ListID);
            var userlistID = Guid.NewGuid();
            if (sharelist.Count() > 0)
                userlistID = sharelist.First().ID;
            var sharelistDisplay = new ShareListDisplay()
            {
                id = userlistID,
                listid = list.ID,
                title = list.Title,
                type = list.ListType.Title,
                typeid = list.ListType.ID,
                isselect = sharelist.Count()>0,
                attrib = AttribInfo.BuildAttribInfo(list.ID, list.Attrib),
            };
            return sharelistDisplay;
        }
    }
    public class ShareService
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
        public JsonResult AddShare(UserRequestDisplay userName)
        {
            using (var dtc = new DatacleContext())
            {
                var shareID = GetUserID(userName.name);
                var shareuser = dtc.Users.First(us => us.ID == shareID);
                var user = ShareService.loginUser(dtc);
                var dtcUserShare = new DtcUserShare()
                {
                    ID = Guid.NewGuid(),
                    UserID = user.ID,
                    ShareID = shareID
                };
                updateUserShareInfo(dtcUserShare, userName);
                dtc.UserShares.Add(dtcUserShare);
                SelectService.AddSelect(dtcUserShare.ID, dtc);
                dtc.SaveChanges();
                var sharedisplay = UserDisplay.BuildUserDisplay(user.ID, shareuser);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(sharedisplay, Formatting.None, settings)
                };
            }
        }
        public JsonResult UpdateShare(SharedListDisplay sharedList)
        {
            //insert toggle


            using (var dtc = new DatacleContext())
            {
                var shareID = sharedList.shareid;
                var share = dtc.UserShares.First(us => us.ID == shareID);
                var shareuserid = share.ShareID;
                var user = ShareService.loginUser(dtc);
                var dtcUserList = new DtcUserList()
                {
                    ID = Guid.NewGuid(),
                    UserID = shareuserid,
                    ListID = (Guid)sharedList.listid
                };
                updateUserListInfo(dtcUserList, sharedList);
                dtc.UserLists.Add(dtcUserList);
                SelectService.AddSelect(dtcUserList.ID, dtc);
                dtc.SaveChanges();
                dtcUserList = dtc.UserLists.Include("List").First(ul => ul.ID == dtcUserList.ID);
                var sharedisplay = ShareListDisplay.BuildShareListDisplay(dtcUserList);
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                return new JsonResult()
                {
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(sharedisplay, Formatting.None, settings)
                };
            }
        }
        private void updateUserListInfo(DtcUserList dtcUserList, SharedListDisplay sharedlist)
        {
            if (dtcUserList.Attrib == null )
            {
                dtcUserList.Attrib = new DtcAttrib()
                {
                    ID = dtcUserList.ID
                };
            }
            if (sharedlist.attrib != null)
            {
                dtcUserList.Attrib.Attrib = sharedlist.attrib.attrib;
            }
        }
        private void updateUserShareInfo(DtcUserShare dtcUserShare, UserRequestDisplay updateusershare)
        {
            if (dtcUserShare.Attrib == null)
            {
                dtcUserShare.Attrib = new DtcAttrib()
                {
                    ID = dtcUserShare.ID
                };
            }
            if (updateusershare.attrib != null)
            {
                dtcUserShare.Attrib.Attrib = updateusershare.attrib.attrib;
            }
        }

        public JsonResult UpdateShare(Guid userId, ShareDisplay updateuser)
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
        private void updateUserInfo(DtcUser dtcUser, ShareDisplay updateuser)
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

        public bool DeleteShareList(Guid userListId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcUserList = dtc.UserLists.First(us => us.ID == userListId);
                dtc.UserLists.Remove(dtcUserList);
                dtc.SaveChanges();
                return true;
            }
        }
        public bool DeleteShare(Guid userShareId)
        {
            using (var dtc = new DatacleContext())
            {
                var dtcUserShare = dtc.UserShares.First(us => us.ID == userShareId);
                dtc.UserShares.Remove(dtcUserShare);
                dtc.SaveChanges();
                return true;
            }
        }
        public List<UserDisplay> Shares(Guid userId)
        {
            using (var dtc = new DatacleContext())
            {
                var user = dtc.Users.First(ud => ud.ID == userId);
                var lists = user.UserShares.Select<DtcUserShare, DtcUser>(us => us.User).ToList();
                lists.Add(user);
                var displays = lists.Select<DtcUser, UserDisplay>
                     (li => UserDisplay.BuildUserDisplay(userId, li)).ToList();
                var userdisplay = displays.Where(ud => ud.id == userId);
                if (userdisplay.Count() > 0)
                    userdisplay.First().isselect = true;
                return displays;
            }
        }

        public List<ShareDisplay> UserShares()
        {
            using (var dtc = new DatacleContext())
            {
                //set user
                var user = ShareService.loginUser(dtc);
                //set user shares
                var usershares = dtc.UserShares.Where(us=>us.UserID==user.ID).ToList();
                var displays = usershares.Select<DtcUserShare, ShareDisplay>
                     (us => ShareDisplay.BuildUserDisplay(us)).ToList();
                //select login user user
                var userdisplay = displays.Where(ud => ud.id == user.ID);
                if (userdisplay.Count() > 0)
                    userdisplay.First().isselect = true;
                // return user and usershares
                return displays;
            }
        }

        public List<ShareListDisplay> UserShareLists(Guid shareID)
        {
            using (var dtc = new DatacleContext())
            {
                //set user shares
                var user = ShareService.loginUser(dtc);
                var ownerID = user.ID;
                if (ownerID != shareID) //is user share id
                {
                    var userlists = dtc.UserShares.First(ul => ul.ID == shareID);
                    ownerID = userlists.ShareID;
                }
                var shareLists = dtc.UserLists.Where(us => us.UserID == user.ID && 
                                                    us.List.OwnerID== ownerID).ToList();
                var displays = shareLists.Select<DtcUserList, ShareListDisplay>
                     (us => ShareListDisplay.BuildShareListDisplay(us)).ToList();
                // return user and usershares
                return displays;
            }
        }
        public List<ShareListDisplay> UserSharedLists(Guid shareID)
        {
            using (var dtc = new DatacleContext())
            {
                //set user shares
                var user = ShareService.loginUser(dtc);
                var userlists = dtc.UserLists.Where(ul => ul.UserID == user.ID &&
                                                    ul.List.OwnerID == user.ID).ToList();
                var displays = userlists.Select<DtcUserList, ShareListDisplay>
                     (us => ShareListDisplay.BuildShareListDisplay(us, shareID, dtc)).ToList();
                // return user and usershares
                return displays;
            }
        }
        public JsonResult GetDisplayShareList(Guid userId)
        {
            var lists = UserShareLists(userId);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult()
            {
                ContentType = "application/json",
                Data = JsonConvert.SerializeObject(lists, Formatting.None, settings)
            };
        }
        public JsonResult GetDisplaySharedList(Guid userId)
        {
            var lists = UserSharedLists(userId);
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

        //List User View Controllers
        public static DtcUser loginUser(DatacleContext dtc)
        {
            var user = (DtcUser)HttpContext.Current.Session["User"];
            var userName = HttpContext.Current.User.Identity.Name;
            if (user == null || user.Title.ToUpper() != userName.ToUpper())
            {
                var users = dtc.Users.Include("UserShares").Where(us => us.Title== userName);
                if (users.Count() > 0)
                {
                    user = users.First();
                    HttpContext.Current.Session["User"] = user;
                }
            }
            return user;
        }
    }
}