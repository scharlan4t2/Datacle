
listModule.controller("ViewController", function ($scope, viewservice, listservice, infoservice) {
    $scope.dtViewType = viewservice.viewtype;
    $scope.dtView = viewservice.view;
    //$scope.dtViewItem = viewitemservice.viewitem;

    var onError = function (data) {
        console.log(data);
    };


    setViewTypeOrder = function (event, ui) {
        var par = ui.item.parent();
        var sort = 0;
        par.children().each(function () {
            var child = $(this);
            child.find("[data-sort]").data("sort", sort++);
        });
        var viewtypes = $scope.dtViewType.list;
        var attribs = [];
        for (var idx in viewtypes) {
            var viewtype = viewtypes[idx];
            var viewtypeid = viewtype.id;
            viewtypeDiv = par.find("[data-sort][value='" + viewtypeid + "']");
            viewtype.sort = viewtypeDiv.data("sort");
            var attrib = viewtype.attrib.attrib;
            if (!attrib) {
                viewtype.attrib.attrib = {};
            }
            viewtype.attrib.attrib.sort = viewtype.sort;
            attribs.push({
                id: viewtype.attrib.id,
                attrib: JSON.stringify(viewtype.attrib.attrib)
            });
        }
        infoservice.saveAttribs(attribs);
    };
    var setViewType = function () {
        var typeview = $("#sortViewType");
        typeview.sortable({
            placeholder: "dragitem btn btn-success",
            forcePlaceholderSize: true,
            update: function (event, ui) {
                setViewTypeOrder(event, ui);
            }
        });
        typeview.disableSelection();
    };
    setViewType();

    setViewOrder = function (event, ui) {
        var par = ui.item.parent();
        var sort = 0;
        par.children().each(function () {
            var child = $(this);
            child.find("[data-sort]").data("sort", sort++);
        });
        var views = $scope.dtView.list;
        var attribs = [];
        for (var idx in views) {
            var view = views[idx];
            var viewid = view.id;
            viewDiv = par.find("[data-sort][value='" + viewid + "']");
            view.sort = viewDiv.data("sort");
            var attrib = view.attrib.attrib;
            if (!attrib) {
                view.attrib.attrib = {};
            }
            view.attrib.attrib.sort = view.sort;
            attribs.push({
                id: view.attrib.id,
                attrib: JSON.stringify(view.attrib.attrib)
            });
        }
        infoservice.saveAttribs(attribs);
    };
    var setView = function () {
        var view = $("#sortView");
        view.sortable({
            placeholder: "dragitem btn btn-success",
            forcePlaceholderSize: true,
            update: function (event, ui) {
                setViewOrder(event, ui);
            }
        });
        view.disableSelection();
    };
    setView();
    //viewservice.getViews().then(onviewComplete, onError);

    //$scope.getView = function (model) {
    //    return model.getView();
    //}
    ////viewtype viewitem visible
    //$scope.isVisible = function (idx, model) {
    //    return model.visible[idx];
    //}
    ////viewtype viewitem visible
    //$scope.setVisible = function (idx, model) {
    //    model.visible[idx] = !model.visible[idx];
    //    model.view[idx].isselect = model.visible[idx];
    //    model.saveObj(model.view[idx]);
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
    //    //viewservice.saveType(newviewtype)
    //    //    .then(onsaveviewtypeComplete, onError);
    //    return false;
    //}

    ////new view types
    //$scope.setViewTypeVisible = function (viewtypeid) {
    //    $scope.viewtypevisible[viewtypeid] = !$scope.viewtypevisible[viewtypeid];
    //}
    //$scope.setNewViewTypeVisible = function () {
    //    $scope.newviewtypevisible = !$scope.newviewtypevisible;
    //}
    //$scope.setNewViewVisible = function () {
    //    $scope.newviewvisible = !$scope.newviewvisible;
    //};
    ////new views

    //get view types
    //var onsavetypeComplete = function (data) {
    //    $scope.viewtypes = JSON.parse(data.Data);
    //    $scope.dtViewType.getView()
    //        .then(onviewtypeComplete, onError);
    //    return false;
    //}
    //$scope.saveType = function (newviewtype) {
    //    $scope.dtViewType.saveObj(newviewtype)
    //        .then(onsavetypeComplete, onError);
    //    return false;
    //}
    // viewservice.getViewTypes().then(onviewtypeComplete, onError);
    ////view attrib
    //var onsaveattribComplete = function (data) {
    //    viewservice.getViewTypes().then(onviewtypeComplete, onError);
    //    return false;
    //};
    //$scope.saveAttrib = function (newviewtype) {
    //    viewservice.saveType(newviewtype)
    //        .then(onsaveviewtypeComplete, onError);
    //    return false;
    //}


    ////view
    //var onsaveviewComplete = function (data) {
    //    var viewdefault = $scope.dtView.viewdefault()
    //    $scope.dtView.newView = viewdefault();
    //    $scope.dtView.getViews().then(onviewComplete, onError);
    //    return false;
    //};
    //$scope.saveView = function (newview) {
    //    $scope.dtView.saveObj(newview).then(onsaveviewComplete, onError);
    //    return false;
    //}

    $(function () {
    });
});
