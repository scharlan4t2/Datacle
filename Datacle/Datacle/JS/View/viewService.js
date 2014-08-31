var viewservice = function ($http, shareservice, infoservice) {
    var dtShare = shareservice.share;
    var dtShareList = shareservice.sharelist;

    // New Object
    var typedefault = function () { return { title: '', desc: '', type: 'view' }; }
    var viewdefault = function () { return { id: '', typeid: '', title: '', desc: '', attr: '' }; }

   //getUserType
    var loadListType = function () {
        $http.get("/API/Reference/ViewType", { type: 'view' })
        .then(function (response) {
            var viewtypes = JSON.parse(response.data.Data);
            viewtype.list = viewtypes;
            for (var idx in viewtypes) {
                var item = viewtypes[idx];
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
    var loadListView = function () {
        return $http.get("/API/View/")
        .then(function (response) {
            var viewlist = JSON.parse(response.data.Data);
            view.list = viewlist;
            for (var idx in viewlist) {
                var item = viewlist[idx];
                var attrib = item.attrib.attrib;
                var attr = infoservice.cleanAttrib(attrib);
                item.attrib.attrib = attr;
                if (attrib != "{}") {
                    item.sort = item.attrib.attrib.sort;
                }
                if (item.isselect) {
                    view.onListLoad(item.id);
                }
            }
        });
    }


    var getListType = function () {
        return viewtype.list;
    }
    var getListView = function () {
        return this.list;
    }


    var setObjType = function (viewtype) {
        viewtype.isselect = !viewtype.isselect;
        infoservice.saveSelect(viewtype);
    }

    var setObjView = function (view) {
        view.isselect = !view.isselect;
        infoservice.saveSelect(view);
    }

    var setEdit = function (view) {
        this.showedit = !this.showedit;
        if (this.showedit) {
            this.showmouse[view.id] = false;
            this.edititem = view;
        }
    }
    var setNew = function () {
        this.shownew = !this.shownew;
    }
    var setNewType = function () {
        this.shownew = !this.shownew;
    }
    var setMouse = function (typeid, set) {
        if (this.showmouse[typeid] != set) {
            this.showmouse[typeid] = set;
        }
        return false;
    }


    var showObjType = function (typeid) {
        for (var idx in this.list) {
            var objType = this.list[idx];
            if (objType.id == typeid) {
                return objType.isselect;
            }
        }
    }
    var showObjView = function (view) {
        var typevisible = viewtype.showObj(view.typeid);
        return view.isselect && typevisible;
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
        if (list.attrib.attrib.width)
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
        return $http.post("/API/Reference/ViewType/", this.newtype)
        .then(function (response) {
            var viewtype = JSON.parse(response.data.Data);
            var newtype = self.newtype;
            if (newtype.attrib) {
                viewtype.attrib.attrib = newtype.attrib.attrib;
            }
            self.list.push(viewtype);
            infoservice.saveAttrib(viewtype);
            self.newtype = typedefault();
            return response.data;
        });
    }
    var saveNewView = function () {
        var self = this;
        return $http.post("/API/View/", this.newview)
            .then(function (response) {
                var viewitem = JSON.parse(response.data.Data);
                var newview = self.newview;
                if (newview.attrib) {
                    viewitem.attrib.attrib = newview.attrib.attrib;
                }
                view.list.push(viewitem);
                infoservice.saveAttrib(viewitem);
                self.newview = viewdefault();
            });
    }
    var saveEditType = function () {
        var self = this;
        var viewtype = this.edititem;
        viewtype.type = "view";
        return $http.post("/API/Reference/ViewType/",viewtype)
        .then(function (response) {
            infoservice.saveAttrib(viewtype);
            self.hideEdit();
        });
    }
    var saveEditView = function (view) {
        var self = this;
        var view = this.edititem;
        return $http.post("/API/View/", view)
            .then(function (response) {
                infoservice.saveAttrib(view);
                self.hideEdit();
                return response.data;
            });
    }
    var delEditType = function () {
        var self = this;
        var typeitem = this.edititem;
        return $http.delete("/API/Reference/" + typeitem.id)
        .then(function (response) {
            var viewidx = viewtype.list.indexOf(typeitem);
            viewtype.list.splice(viewidx, 1);
            self.hideEdit();
            view.loadlist();
            return response.data;
        });
    }
    var delEditView = function () {
        var self = this;
        var viewitem = this.edititem;
        return $http.delete("/API/View/" + viewitem.id)
        .then(function (response) {
            var viewidx = view.list.indexOf(viewitem);
            view.list.splice(viewidx, 1);
            //removeShareList(viewitem.id);
            self.hideEdit();
            return response.data;
        });
    }
    var viewtype = {
        list: [],
        hastype: [],
        showobj: [],
        showmouse: [],
        getList: getListType,
        loadList: loadListType,
        setObj: setObjType,
        showObj: showObjType,
        setMouse: setMouse,
        showMouse: showMouse,
        // edit Object
        showedit: false,
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
        saveNew: saveNewType,
    };
    var view = {
        list: [],
        hasview: [],
        showobj: [],
        showmouse: [],
        getList: getListView,
        loadList: loadListView,
        onListLoad: onLoad,
        setObj: setObjView,
        showObj: showObjView,
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
        delEdit: delEditView,
        saveEdit: saveEditView,
        //New Object
        shownew: false,
        newview: viewdefault(),
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNewView
    };

    loadListType();
    loadListView();
    return {
        view: view,
        viewtype: viewtype,
    }
};

//viewModule = angular.module("view",[]);
listModule.factory("viewservice", viewservice);
