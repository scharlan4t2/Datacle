var listservice = function ($http, shareservice, infoservice) {
    var dtShare = shareservice.share;
    var dtShareList = shareservice.sharelist;

    var listdefault = function () { return { id: '', typeid: '', title: '', desc: '', attr: '' }; }
    var typedefault = function () { return { title: '', desc: '', type: 'list' }; }

    var loadListType = function () {
        $http.get("/API/Reference/ListType", { type: 'list' })
        .then(function (response) {
            var listtypes = JSON.parse(response.data.Data);

            listtype.list = listtypes;
            for (var idx in listtypes) {
                var item = listtypes[idx];
                var attrib = item.attrib.attrib;
                var attr = infoservice.cleanAttrib(attrib);
                item.attrib.attrib = attr;
                if (attrib != "{}") {
                    item.sort = item.attrib.attrib.sort;
                }
            }
        });
    }
    //default after list call
    var onLoad = function () {
    }
    var loadList = function () {
        var self = this;
        $http.get("/API/List/")
        .then(function (response) {
            var lists = JSON.parse(response.data.Data);
            list.list = lists;
            for (var idx in lists) {
                var item = lists[idx];
                var attrib = item.attrib.attrib;
                var attr = infoservice.cleanAttrib(attrib);
                item.attrib.attrib = attr;
                if (attrib != "{}") {
                    item.sort = item.attrib.attrib.sort;
                }
                if( item.isselect){
                    list.onListLoad(item.id);
                }
            }
        });
    }

    
    var getListType = function () {
        return this.list;
    }
    var getList = function () {
        return this.list;
    }
    var hold = function(){
        var users = dtShareList.list;
        var listidxs = [];
        //get list index
        for (var idx in this.list) {
            listidxs[this.list[idx].id] = idx;
        }
        //scan shares
        for (var userid in users) {
            var lists = users[userid];
            for (var idx in lists) {
                var list = lists[idx];
                if (!this.haslist[list.id]) {
                    listidxs[list.id] = this.list.length;
                    var attrib = list.attrib.attrib;
                    var attr = {};
                    if( attrib!="{}"){
                        attr = infoservice.cleanAttrib(attrib);
                    }
                    this.list.push({
                        id: list.listid,
                        sort: attr.sort,
                        title: list.title,
                        isselect: list.isselect,
                        attrib: { id: list.id, attrib: attr },
                        type: list.type, typeid: list.typeid
                    });
                    this.haslist[list.id] = true;
                    if (this.loadItem != null && list.isselect) {
                        this.loadItem(list.id);
                    }
        
                }
                var listidx = listidxs[list.id];
                var idxlist = this.list[listidx];
                typevisible = listtype.showObj(list.typeid);
                idxlist.isselect = list.isselect && typevisible;
            }
        }
        return this.list;
    }



    var setObjType = function (listtype) {
        listtype.isselect = !listtype.isselect;
        infoservice.saveSelect(listtype);
    }
    //New Object
    var setObjList = function (list) {
        list.isselect = !list.isselect;
        infoservice.saveSelect(list);
    }

    var setEdit = function (list) {
        this.showedit = !this.showedit;
        if (this.showedit) {
            this.showmouse[list.id] = false;
            this.edititem = list;
        }
    }
    var setNew = function () {
        this.shownew = !this.shownew;
    }
    var setMouse = function (typeid, set) {
        if (this.showmouse[typeid] != set) {
            this.showmouse[typeid] = set;
        }
        return false;
    }


    var showObjType = function (listtypeid) {
        for (var idx in this.list) {
            var objType = this.list[idx];
            if (objType.id == listtypeid) {
                return objType.isselect;
            }
        }
    }
    var showObjList = function (list) {
        var typevisible = listtype.showObj(list.typeid);
        var sharevisible = dtShareList.showObj(list.id);
        return typevisible && sharevisible;
    }
    var showDesc = function (list) {
        return list.attrib.attrib.showdesc;
    }
    var showNew = function () {
        return this.shownew;
    }
    var showEdit = function () {
        return this.showedit;
    }
    var showMouse = function (typeid) {
        return this.showmouse[typeid];
    }
    var getWidthList = function (list) {
        if( list.attrib.attrib.width)
            return list.attrib.attrib.width;
        return 2;
    }  

    var hideNew = function () {
        this.shownew = false;
    }
    var hideEdit = function () {
        this.showedit = false;
    }



    var saveNewType = function () {
        var self = this;
        return $http.post("/API/Reference/ListType/", this.newtype)
        .then(function (response) {
            var listtype = JSON.parse(response.data.Data);
            var newtype = self.newtype;
            if (newtype.attrib) {
                listtype.attrib.attrib = newtype.attrib.attrib;
            }
            self.list.push(listtype);
            infoservice.saveAttrib(listtype);
            self.newtype = typedefault();
            return response.data;
        });
    }
    var addShareList = function (list) {
        var users = dtShare.list;
        for (var useridx in users) {
            var user = users[useridx];
            if (user.isuser) {
                var userid = user.id;
            }
        }
        dtShareList.loadList(userid);
    }
    var saveNewList = function () {
        var self = this;
        return $http.post("/API/List/", self.newlist)
            .then(function (response) {
                var list = JSON.parse(response.data.Data);
                var newlist = self.newlist;
                if (newlist.attrib) {
                    list.attrib.attrib = newlist.attrib.attrib;
                }
                infoservice.saveAttrib(list);
                addShareList(list);
                self.getList();
                self.newlist = listdefault();
                return response.data;
            });
    }
    var saveEditType = function () {
        var self = this;
        var listtype = this.edititem;
        listtype.type = "list";
        return $http.post("/API/Reference/ListType/", listtype)
        .then(function (response) {
            infoservice.saveAttrib(listtype);
            self.hideEdit();
        });
    }
    var saveEditList = function () {
        var self = this;
        var list = this.edititem;
        return $http.post("/API/List/", list)
        .then(function (response) {
            infoservice.saveAttrib(list);
            self.hideEdit();
            return response.data;
        });
    }
    var delEditType = function () {
        var self = this;
        listtype = this.edititem;
        return $http.delete("/API/Reference/" + listtype.id)
        .then(function (response) {
            var idx = self.list.indexOf(listtype);
            self.list.splice(idx, 1);
            self.hideEdit();
        });
    }
    var removeShareList = function (listid) {
        var users = dtShareList.list;
        for (var userid in users) {
            var lists = users[userid];
            for (var idx in lists) {
                var list = lists[idx];
                if (list.id == listid) {
                    var listidx = lists.indexOf(list);
                    lists.splice(listidx, 1);
                }
            }
        }
    }
    var delEditList = function () {
        var self = this;
        var listitem = this.edititem;
        return $http.delete("/API/List/" + listitem.id)
        .then(function (response) {
            var listidx = self.list.indexOf(listitem);
            self.list.splice(listidx, 1);
            removeShareList(listitem.id);
            self.hideEdit();
            return response.data;
        });
    }


    var listtype = {
        list: [],
        hastype: [],
        showmouse: [],
        getList: getListType,
        loadList: loadListType,
        setObj: setObjType,
        showObj: showObjType,
        setMouse: setMouse,
        showMouse: showMouse,
        // edit Object
        showedit: false,
        edititem: {},
        setEdit: setEdit,
        showEdit: showEdit,
        hideEdit: hideEdit,
        delEdit: delEditType,
        saveEdit: saveEditType,
        // New Object
        shownew: false,
        newtype: typedefault(),
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNewType
    };

    var list = {
        list: [],
        haslist: [],
        showmouse: [],
        getList: getList,
        onListLoad: onLoad,
        setObj: setObjList,
        showObj: showObjList,
        showDesc: showDesc,
        getWidth: getWidthList,
        setMouse: setMouse,
        showMouse: showMouse,
        // edit Object
        showedit: false,
        edititem: {},
        setEdit: setEdit,
        showEdit: showEdit,
        hideEdit: hideEdit,
        delEdit: delEditList,
        saveEdit: saveEditList,
        //New Object
        shownew: false,
        newlist: listdefault(),
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNewList
    };

    loadListType();
    loadList();
    return {
        list: list,
        listtype: listtype,
    }
};

//listModule = angular.module("list",[]);
listModule.factory("listservice", listservice);
