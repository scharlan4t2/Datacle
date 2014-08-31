var listitemservice = function ($http, listservice, infoservice) {
    var dtList = listservice.list;
    var dtListType = listservice.listtype;

    var itemdefault = function () { return { id: '', listid: '', title: '', desc: '', attr: '' }; }
    var editdefault = function (item) {
        if (item) {
            return {
                id: item.id, listid: item.listid, title: item.title,
                desc: item.desc, attr: item.attr
            };
        }
        return {
            id: '', listid: '', title: '',
            desc: '', attr: ''
        };
    }

    
    var loadListItems = function (listid) {
        return $http.get("/API/ListItem/" + listid)
        .then(function (response) {
            listitem.hideNew(listid);
            var listitems = JSON.parse(response.data.Data);

            listitem.list[listid] = listitems;
            for (var idx in listitems) {
                var item = listitems[idx];
                var attrib = item.attrib.attrib;
                var attr = infoservice.cleanAttrib(attrib);
                item.attrib.attrib = attr;
                item.sort = item.attrib.attrib.sort;
            }

        });
    };
    //list load event
    listservice.list.onListLoad = loadListItems;

    var getListItems = function (listid) {
        return this.list[listid];
    };


    var setObjList = function (list) {
        if (this.showList(list)) {
            this.list[list.id] = undefined;
        }
        else {
            this.loadList(list.id);
        }
    }
    var setObjListItem = function (item) {
        item.isselect = !item.isselect;
        infoservice.saveSelect(item);
    }
    var setNew = function (listid) {
        this.shownew[listid] = !this.shownew[listid];
        if (this.shownew[listid]) {
            this.newitem[listid] = itemdefault();
        }
    }
    var setEdit = function (item, listid) {
        var self = this;
        this.showedit[listid] = true;
        if (this.showedit[listid]) {
            this.showmouse[item.id] = false;
            this.edititem[listid] = item;
        }
    }
    var setMouse = function (typeid, set) {
        var self = this;
        if (self.showmouse[typeid] != set) {
            self.showmouse[typeid] = set;
        }
        return false;
    }

    var showObjList = function (listobj) {
        var id = listobj.id;
        if( this.list[id])
            return true;
        else
            return false;
    }
    var showObjListItem = function (item) {
        return item.isselect;
    }
    var showNew = function (listid) {
        return this.shownew[listid];
    }
    var showEdit = function (listid) {
        return this.showedit[listid];
    }
    var showMouse = function (typeid) {
        return this.showmouse[typeid];
    }


    var hideNew = function (listid) {
        this.shownew[listid] = false;
    }
    var hideEdit = function (listid) {
        this.showedit[listid] = false;
    }
    var hideMouse = function (listid) {
        this.showmouse[listid] = false;
    }

    var saveNew = function (listid) {
        var self = this;
        var newitem = self.newitem[listid];
        newitem.listid = listid;
        return $http.post("/API/ListItem/", newitem)
        .then(function (response) {
            var listitem = JSON.parse(response.data.Data);
            var newitem = self.newitem[listid];
            if (newitem.attrib) {
                listitem.attrib.attrib = newitem.attrib.attrib;
            }
            self.list[listid].push(listitem);
            infoservice.saveAttrib(listitem);
            self.newitem[listid] = itemdefault();
            return response.data;
        });
    }
    var saveEdit = function (listid) {
        var self = this;
        var item = this.edititem[listid];
        item.listid = listid;
        return $http.put("/API/ListItem/" + item.id, item)
        .then(function (response) {
            infoservice.saveAttrib(item);
            self.hideEdit(listid);
            return response.data;
        });
    }

    var keyNew = function (listid, ev) {
        if (ev.which === 13) {
            var additem = this.newitem[listid];
            this.saveNew(listid);
            var par = $(ev.currentTarget).parent();
            par.find("input").first().focus();
            ev.preventDefault();
        }
    }
    var keyEdit = function (listid, ev) {
        if (ev.which === 13) {
            var additem = this.edititem[listid];
            this.saveEdit(listid);
            var par = $(ev.currentTarget).parent();
            par.find("input").first().focus();
            ev.preventDefault();
        }
    }

    
    var listitem = {
        list: [], hasitem:[],
        shownew: [], itemdefault: itemdefault,
        newitem: [],
        showedit: [],
        edititem: [], editdefault: editdefault,
        showmouse: [],
        getList: getListItems,
        loadList: loadListItems,
        setObj: setObjListItem,
        setList: setObjList,
        showObj: showObjListItem,
        showList: showObjList,
        // edit Object
        setEdit: setEdit,
        showEdit: showEdit,
        hideEdit: hideEdit,
        saveEdit: saveEdit,
        setMouse: setMouse,
        showMouse: showMouse,
        // New Object
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNew,
        keyNew: keyNew,
    };

    return {
        listitem: listitem
    }
};

//listModule = angular.module("list",[]);
listModule.factory("listitemservice", listitemservice);
