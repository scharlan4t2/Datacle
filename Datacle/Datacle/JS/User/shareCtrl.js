listModule.controller("ShareController", function ($scope, shareservice) {
    $scope.dtShare = shareservice.share;
    $scope.dtShareList = shareservice.sharelist;
    $scope.dtSharedList = shareservice.sharedlist;

    shareservice.share.getList();
    
    $scope.isSelect = function (model) {
        return model.isselect;
    }
    $scope.isShareVisible = function (idx, model) {
        return model.sharevisible[idx];
    }
    $scope.classSelect = function (model) {
        return 'btn-' + (model.isselect ? 'info' : 'success');
    }
    $scope.setVisible = function (idx, model) {
        model.visible[idx] = !model.visible[idx];
        return false;
    }
    $scope.getList = function (model) {
        return model.list;
    }
    $scope.saveObj = function (model) {
        model.saveObj;
        return false;
    }
});
