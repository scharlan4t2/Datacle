listModule.controller("ListController", function ($scope, listservice, listitemservice, infoservice) {
    $scope.dtListType = listservice.listtype;
    $scope.dtList = listservice.list;
    $scope.dtListItem = listitemservice.listitem;

    var onError = function (data) {
        console.log(data);
    };

    //set list type sort
    var setListTypeOrder = function (event, ui) {
        var par = ui.item.parent();
        var sort = 0;
        par.children().each(function () {
            var child = $(this);
            child.find("[data-sort]").data("sort", sort++);
        });
        var listtypes = $scope.dtListType.list;
        var attribs = [];
        for (var idx in listtypes) {
            var listtype = listtypes[idx];
            var listtypeid = listtype.id;
            listtypeDiv = par.find("[data-sort][value='" + listtypeid + "']");
            listtype.sort = listtypeDiv.data("sort");
            var attrib = listtype.attrib.attrib;
            if (!attrib) {
                listtype.attrib.attrib = {};
            }
            listtype.attrib.attrib.sort = listtype.sort;
            attribs.push({
                id: listtype.attrib.id,
                attrib: JSON.stringify(listtype.attrib.attrib)
            });
        }
        infoservice.saveAttribs(attribs);
    };
    var setListType = function () {
        var typelist = $("#sortListType");
        typelist.sortable({
            placeholder: "dragitem btn btn-success",
            forcePlaceholderSize:true,
            update: function (event, ui) {
                setListTypeOrder(event, ui);
            }
        });
        typelist.disableSelection();
    };
    setListType();

    //set list sort
    setListOrder = function (event, ui) {
        var par = ui.item.parent();
        var sort = 0;
        par.children().each(function () {
            var child = $(this);
            child.find("[data-sort]").data("sort", sort++);
        });
        var lists = $scope.dtList.list;
        var attribs = [];
        for (var idx in lists) {
            var list = lists[idx];
            var listid = list.id;
            listDiv = par.find("[data-sort][value='" + listid + "']");
            list.sort = listDiv.data("sort");
            var attrib = list.attrib.attrib;
            if (!attrib ) {
                list.attrib.attrib = {};
            }
            list.attrib.attrib.sort = list.sort;
            attribs.push({
                id: list.attrib.id,
                attrib: JSON.stringify(list.attrib.attrib)
            });
        }
        infoservice.saveAttribs(attribs);
    };
    var setList = function () {
        var list = $("#sortList");
        list.sortable({
            placeholder: "dragitem btn btn-success",
            forcePlaceholderSize: true,
            update: function (event, ui) {
                setListOrder(event, ui);
            }
        });
        list.disableSelection();

    };
    setList();
    
    //$scope.getList = function (model) {
    //    return model.getList();
    //}
    ////listtype listitem visible
    //$scope.isVisible = function (idx, model) {
    //    return model.visible[idx];
    //}
    ////listtype listitem visible
    //$scope.setVisible = function (idx, model) {
    //    model.visible[idx] = !model.visible[idx];
    //    model.list[idx].isselect = model.visible[idx];
    //    model.saveObj(model.list[idx]);
    //}
    //$scope.setEditVisible = function (idx, model) {
    //    model.editvisible[idx] = !model.editvisible[idx];
    //}
    //$scope.setNewVisible = function (model) {
    //    model.newvisible = !model.newvisible;
    //}
    //$scope.hideNewVisible = function (model) {
    //    model.newvisible = false;
    //}

    //$scope.saveObjModel = function (model) {
    //    model.saveModel();
    //    //listservice.saveType(newlisttype)
    //    //    .then(onsavelisttypeComplete, onError);
    //    return false;
    //}

    //new lists

    ////list attrib
    //var onsaveattribComplete = function (data) {
    //    listservice.getListTypes().then(onlisttypeComplete, onError);
    //    return false;
    //};
    //$scope.saveAttrib = function (newlisttype) {
    //    listservice.saveType(newlisttype)
    //        .then(onsavelisttypeComplete, onError);
    //    return false;
    //}


    ////list
    //var onsavelistComplete = function (data) {
    //    var listdefault = $scope.dtList.listdefault()
    //    $scope.dtList.newList = listdefault();
    //    $scope.dtList.getLists().then(onlistComplete, onError);
    //    return false;
    //};
    //$scope.saveList = function (newlist) {
    //    $scope.dtList.saveObj(newlist).then(onsavelistComplete, onError);
    //    return false;
    //}

    $(function () {
    });

});
