var shareservice = function ($http, infoservice) {

    //Share 
    var getListShare = function () {
        return share.list;
    }
    var loadListShare = function () {
        $http.get("/API/User")
        .then(function (response) {
            var users = JSON.parse(response.data.Data);
            share.user = getactiveuser(users);
            sharelist.getList(share.user.id);
            $http.get("/API/Share")
            .then(function (response) {
                share.list = [];
                share.list.push(share.user);
                var shares = JSON.parse(response.data.Data);
                for (var idx in shares) {
                    var shareuser = shares[idx];
                    share.list.push(shareuser)
                }
                loadShareLists();
            });
        });
    }
    loadListShare();
    loadShareLists = function () {
        for (var idx in share.list) {
            var user = share.list[idx];
            if (user.isselect) {
                sharelist.loadList(user.id);
            }
        }
    }
    getactiveuser = function (users) {
        for (var idx in users) {
            var user = users[idx];
            if (user.isuser) {
                return user;
            }
        }
    }

    //New Object
    var usernamedefault = function () { return { name: '' }; }
    var setNew = function () {
        this.shownew = !this.shownew;
    }
    var showNew = function () {
        return this.shownew;
    }
    var hideNew = function () {
        this.shownew = false;
    }
    var saveNew = function () {
        this.saveObj(this.newshare);
    }

    // Object
    var showObj = function (share) {
        return share.isselect;
    }
    var setType = function (type) {
        for (var idx in this.list) {
            var share = this.list[idx];
            if (share.type == type) {
                this.setObj(share);
            }
        }
    }
    var setObj = function (share) {
        share.isselect = !share.isselect
        infoservice.saveSelect(share);
        if (share.isselect) {
            if (!sharelist.list[share.id]) {
                loadShareList(share.id);
            }
            getShareList(share.id);
        }
    }
    var saveObj = function (share) {
        var self = this;
        return $http.post("/API/Share/", share)
        .then(function (response) {
            var newuser = JSON.parse(response.data.Data);
            self.list.push(newuser);
            share.newshare = usernamedefault();
        });
    }
    var showShared = function (shareid) {
        return this.showshared[shareid] == true;
    }
    var setShared = function (shareid) {
        if (shareid != share.user.id) {
            this.showshared[shareid] = !this.showshared[shareid];
        }
        if (this.showshared[shareid])
            loadSharedList(shareid);
    }

    //Return Share
    var share = {
        list: [], visible: [],
        user: {},
        showshared: [],

        //New Object
        shownew: false,
        newshare: usernamedefault(),
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNew,
        //Share
        getList: getListShare,
        setType: setType,
        setObj: setObj,
        showObj: showObj,
        saveObj: saveObj,
        //Share
        showShared: showShared,
        setShared: setShared
    };


    //Share List
    var getShareList = function (shareid) {
        return sharelist.list[shareid];

    }
    var loadShareList = function (shareid) {
        return $http.get("/API/Share/" + shareid)
        .then(function (response) {
            sharelist.list[shareid] = JSON.parse(response.data.Data);
        });
    }
    var saveObjList = function (shareid, share) {
        return $http.post("/API/Share/" + shareid, share)
        .then(function (response) {
            var newsharelist = JSON.parse(response.data.Data);
            loadShareList(shareid);
        });
    }
    var setTypeList = function (type) {
        var lists = sharelist.list[share.user.id];
        for (var idx in lists) {
            var list = lists[idx];
            if (list.type == type) {
                list.isselect = !list.isselect;
                return;
            }
        }
    }
    //select share list
    var setObjList = function (listid, shareid) {
        var lists = this.list[shareid];
        for (var idx in lists) {
            var list = lists[idx];
            if (list.id == listid) {
                list.isselect = !list.isselect;
                infoservice.saveSelect(list);
                return;
            }
        }
    }
    var showObjList = function (listid) {
        for (var useridx in this.list) {
            var lists = this.list[useridx];
            for (var idx in lists) {
                var list = lists[idx];
                if (list.listid == listid) {
                    return list.isselect;
                }
            }
        }
    }
    //Return Share List
    var sharelist = {
        list: [],
        showobj: [],
        getList: getShareList,
        loadList: loadShareList,
        setType: setTypeList,
        setObj: setObjList,
        showObj: showObjList,
        saveObj: saveObjList
    };
    //Return Shared List
    
    //Share List
    var getSharedList = function (shareid) {
        return sharedlist.list[shareid];
    }
    var loadSharedList = function (shareid) {
        return $http.get("/API/Shared/" + shareid)
        .then(function (response) {
            sharedlist.list[shareid] = JSON.parse(response.data.Data);
        });
    }

    var setTypeSharedList = function (type, shareid) {
        var lists = this.list[shareid];
        for (var idx in lists[idx]) {
            var list = lists[idx];
            if (list.type == type) {
                this.setSelectSharedList(list, shareid);
            }
        }
    }
    var setSelectSharedList = function (list, shareid) {
        list.shareid = shareid;
        list.isselect = !list.isselect;
        if (list.isselect) {
            return $http.post("/API/Shared/" + shareid, list)
            .then(function (response) {
                ; //sharedlist.list[shareid] = JSON.parse(response.data.Data);
            });
        }
        else {
            return $http.delete("/API/Shared/" + list.id)
        }
    }
    var saveSharedList = function (shareid, share) {
        return $http.post("/API/Share/" + shareid, share)
        .then(function (response) {
            var newsharelist = JSON.parse(response.data.Data);
            loadShareList(shareid);
        });
    }
    var sharedlist = {
        list: [],
        getList: getSharedList,
        loadList: loadSharedList,
        saveObj: saveSharedList,
        setObj: setSelectSharedList,
        setType: setSelectSharedList
    };

    return {
        sharedlist: sharedlist,
        sharelist: sharelist,
        share: share
    }
}

listModule.factory("shareservice", shareservice);
