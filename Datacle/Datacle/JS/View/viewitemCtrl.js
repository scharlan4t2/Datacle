listModule.controller("ViewItemController", function ($scope, listservice, listitemservice, viewservice, viewitemservice, viewlistservice) {
    $scope.dtView = viewservice.view;
    $scope.dtViewItem = viewitemservice.viewitem;
    $scope.dtList = listservice.list;
    $scope.dtListItem = listitemservice.listitem;
    $scope.dtViewList = viewlistservice.viewlist;
    $scope.dtViewConn = viewlistservice.viewconn;


    var onError = function (data) {
        console.log(data);
    };


    //overrid set make view sortable
    $scope.setViewListItemObj = function (list) {

    }
        //overrid set make view sortable
    $scope.setViewItemObj = function (view) {
        $scope.dtView.setObj(view);
        if (view.isselect) {
            $scope.dtViewItem.loadList(view.id);
            var sortview = $(".sortViewItem");
            var itemview = sortview.filter("[data-viewid='" + view.id + "']");
            if (!itemview.hasClass("ui-sortable")) {
                itemview.sortable({
                    placeholder: "dragitem btn btn-success",
                    forcePlaceholderSize: true,
                    update: function (event, ui) {
                        setViewItemOrder(event, ui, view.id);
                    }
                });
                itemview.disableSelection();
            }
        }
        return false;
    };

    setViewItemOrder = function (event, ui, viewid) {
        var par = ui.item.parent();
        var sort = 0;
        par.children().each(function () {
            var child = $(this);
            child.find("[data-sort]").data("sort", sort++);
        });
        var viewitems = $scope.dtViewItem.view[viewid];
        var attribs = [];
        for (var idx in viewitems) {
            var viewitem = viewitems[idx];
            var viewitemid = viewitem.id;
            viewitemDiv = par.find("[data-sort][value='" + viewitemid + "']");
            viewitem.sort = viewitemDiv.data("sort");
            viewitem.attrib.attrib.sort = viewitem.sort;
            attribs.push({
                id: viewitem.attrib.id,
                attrib: JSON.stringify(viewitem.attrib.attrib)
            });
        }
        infoservice.saveAttribs(attribs);
    };

    var onaddviewitemComplete = function (data) {
        var viewitem = JSON.parse(data.Data);
        var itemdefault = $scope.dtViewItem.itemdefault
        $scope.dtViewItem.newitem[viewitem.viewid] = itemdefault();
        onchangeviewitemComplete(data);
    }
    $scope.addviewitem = function (viewid, additem) {
        additem.viewid = viewid;
        $scope.dtViewItem.addViewItem(additem)
            .then(onaddviewitemComplete, onError);
        return false;
    }

    var onchangeviewitemComplete = function (data) {
        var viewitem = JSON.parse(data.Data);
        $scope.dtViewItem.getView(viewitem.viewid)
            .then(onviewitemComplete, onError);
    }
    $scope.saveviewitem = function (viewitem) {
        $scope.dtViewItem.saveViewItem(viewitem)
            .then(onchangeviewitemComplete, onError);
        return false;
    }
    $scope.deleteviewitem = function (viewitem) {
        $scope.dtViewItem.deleteViewItem(viewitem)
            .then(onchangeviewitemComplete, onError);
        return false;
    }




    $(function () {
    });

});
