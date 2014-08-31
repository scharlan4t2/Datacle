var viewitemservice = function ($http, viewservice, infoservice) {
    var dtView = viewservice.view;
    var dtViewType = viewservice.viewtype;

    var itemdefault = function () { return { id: '', viewid: '', title: '', desc: '', attr: '' }; }
    var editdefault = function (item) {
        if (item) {
            return {
                id: item.id, viewid: item.viewid, title: item.title,
                desc: item.desc, attr: item.attr
            };
        }
        return {
            id: '', viewid: '', title: '',
            desc: '', attr: ''
        };
    }

    var loadViewItems = function (viewid) {
        return $http.get("/API/ViewItem/" + viewid)
        .then(function (response) {
            viewitem.hideNew(viewid);
            var viewitems = JSON.parse(response.data.Data);

            viewitem.list[viewid] = viewitems;
            for (var idx in viewitems) {
                var item = viewitems[idx];
                var attrib = item.attrib.attrib;
                var attr = infoservice.cleanAttrib(attrib);
                item.attrib.attrib = attr;
                item.sort = item.attrib.attrib.sort;
            }

        });
    };
    viewservice.view.onListLoad = loadViewItems;

    var getViewItems = function (viewid) {
        return this.list[viewid];
    };


    var setObjView = function (view) {
        if (this.showView(view)) {
            this.list[view.id] = undefined;
        }
        else {
            this.loadViewItems(view.id);
        }
    }
    var setObjViewItem = function (item) {
        item.isselect = !item.isselect;
        infoservice.saveSelect(item);
    }
    var setNew = function (viewid) {
        this.shownew[viewid] = !this.shownew[viewid];
        if (this.shownew[viewid]) {
            this.newitem[viewid] = itemdefault();
        }
    }
    var setEdit = function (item, viewid) {
        var self = this;
        this.showedit[viewid] = true;
        if (this.showedit[viewid]) {
            this.showmouse[item.id] = false;
            this.edititem[viewid] = item;
        }
    }
    var setMouse = function (typeid, set) {
        var self = this;
        if (self.showmouse[typeid] != set) {
            self.showmouse[typeid] = set;
        }
        return false;
    }

    var showObjView = function (viewobj) {
        var id = viewobj.id;
        if (this.list[id])
            return true;
        else
            return false;
    }
    var showObjViewItem = function (item) {
        return item.isselect;
    }
    var showNew = function (viewid) {
        return this.shownew[viewid];
    }
    var showEdit = function (viewid) {
        return this.showedit[viewid];
    }
    var showMouse = function (typeid) {
        return this.showmouse[typeid];
    }


    var hideNew = function (viewid) {
        this.shownew[viewid] = false;
    }
    var hideEdit = function (viewid) {
        this.showedit[viewid] = false;
    }
    var hideMouse = function (viewid) {
        this.showmouse[viewid] = false;
    }

    var saveNew = function (viewid) {
        var self = this;
        var newitem = self.newitem[viewid];
        newitem.viewid = viewid;
        return $http.post("/API/ViewItem/", newitem)
        .then(function (response) {
            var viewitem = JSON.parse(response.data.Data);
            var newitem = self.newitem[viewid];
            if (newitem.attrib) {
                viewitem.attrib.attrib = newitem.attrib.attrib;
            }
            self.list[viewid].push(viewitem);
            infoservice.saveAttrib(viewitem);
            self.newitem[viewid] = itemdefault();
            return response.data;
        });
    }
    var saveEdit = function (viewid) {
        var self = this;
        var item = this.edititem[viewid];
        item.viewid = viewid;
        return $http.put("/API/ViewItem/" + item.id, item)
        .then(function (response) {
            infoservice.saveAttrib(item);
            self.hideEdit(viewid);
            return response.data;
        });
    }

    var keyNew = function (viewid, ev) {
        if (ev.which === 13) {
            var additem = this.newitem[viewid];
            this.saveNew(viewid);
            var par = $(ev.currentTarget).parent();
            par.find("input").first().focus();
            ev.preventDefault();
        }
    }
    var keyEdit = function (viewid, ev) {
        if (ev.which === 13) {
            var additem = this.edititem[viewid];
            this.saveEdit(viewid);
            var par = $(ev.currentTarget).parent();
            par.find("input").first().focus();
            ev.preventDefault();
        }
    }


    var viewitem = {
        list: [], hasitem: [],
        shownew: [], itemdefault: itemdefault,
        newitem: [],
        showedit: [],
        edititem: [], editdefault: editdefault,
        showmouse: [],
        getView: getViewItems,
        loadView: loadViewItems,
        setObj: setObjViewItem,
        setView: setObjView,
        showObj: showObjViewItem,
        showView: showObjView,
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
        viewitem: viewitem
    }
};

//listModule = angular.module("list",[]);
listModule.factory("viewitemservice", viewitemservice);

