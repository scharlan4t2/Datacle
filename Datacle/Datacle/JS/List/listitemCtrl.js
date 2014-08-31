
listModule.controller("ListItemController", function ($scope, listitemservice, listservice, infoservice) {
    $scope.dtList = listservice.list;
    $scope.dtListItem = listitemservice.listitem;

    var onError = function (data) {
        console.log(data);
    };


    //overrid set make list sortable
    $scope.setListItemObj = function (list) {
        $scope.dtList.setObj(list);
        if (list.isselect) {
            $scope.dtListItem.loadList(list.id);
            var sortlist = $(".sortListItem");
            var itemlist = sortlist.filter("[data-listid='" + list.id + "']");
            if (!itemlist.hasClass("ui-sortable")) {
                itemlist.sortable({
                    placeholder: "dragitem btn btn-success",
                    forcePlaceholderSize: true,
                    update: function (event, ui) {
                        setListItemOrder(event, ui, list.id);
                    }
                });
                itemlist.disableSelection();
            }
        }
        return false;
    };
    $scope.sortListItem = function () { return "sort"; }
    setListItemOrder = function (event, ui, listid) {
        var par = ui.item.parent();
        //var sort = 0;
        var order = par.sortable('toArray');
        //for(var idx =0;idx<order.length;idx++){
        //    var id = order[idx];
        //    var child = par.find("[data-sort='"+id+"']");
        //    var padding = "0000";
        //    var parsort = sort++;
        //    var padsort = padding.slice(parsort.toString().length) + parsort;
        //    child.data("sort", padsort);
        //}
        var listitems = $scope.dtListItem.list[listid];
        var attribs = [];
        for (var idx in listitems) {
            var listitem = listitems[idx];
            var padding = "0000";
            var parsort = order.indexOf(listitem.sort);
            order[parsort] = "";
            var padsort = padding.slice(parsort.toString().length) + parsort;
            if (listitem.sort != padsort){
                listitem.sort = padsort;
                listitem.attrib.attrib.sort = listitem.sort;
                attribs.push({
                    id: listitem.attrib.id,
                    attrib: JSON.stringify(listitem.attrib.attrib)
                });
            }
        }
      //  $scope.dtList.setObj(listid);
       // $scope.dtList.setObj(listid);
        infoservice.saveAttribs(attribs);
    };

    var onaddlistitemComplete = function (data) {
        var listitem = JSON.parse(data.Data);
        var itemdefault = $scope.dtListItem.itemdefault
        $scope.dtListItem.newitem[listitem.listid] = itemdefault();
        onchangelistitemComplete(data);
    }
    $scope.addlistitem = function (listid, additem) {
        additem.listid = listid;
        $scope.dtListItem.addListItem(additem)
            .then(onaddlistitemComplete, onError);
        return false;
    }

    var onchangelistitemComplete = function (data) {
        var listitem = JSON.parse(data.Data);
        $scope.dtListItem.getList(listitem.listid)
            .then(onlistitemComplete, onError);
    }
    $scope.savelistitem = function (listitem) {
        $scope.dtListItem.saveListItem(listitem)
            .then(onchangelistitemComplete, onError);
        return false;
    }
    $scope.deletelistitem = function (listitem) {
        $scope.dtListItem.deleteListItem(listitem)
            .then(onchangelistitemComplete, onError);
        return false;
    }




    $(function () {
    });

});
