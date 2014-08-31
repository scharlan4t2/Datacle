/// <reference path="shareCtrl.js" />

listModule.controller("UserController", function ($scope, userservice) {
    $scope.dtUserType = userservice.usertype;
    $scope.dtUser = userservice.user;
    $scope.dtUserList = userservice.userlist;

    userservice.user.getList();
    userservice.usertype.getList();

    //listtype listitem visible
    $scope.isVisible = function (idx, model) {
        return model.visible[idx];
    }
    $scope.isNewVisible = function (model) {
        return model.newvisible;
    }
    $scope.setVisible = function (idx, model) {
        model.visible[idx] = !model.visible[idx];
        return false;
    }
    $scope.setNewVisible = function (model) {
        model.newvisible = !model.newvisible;
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
