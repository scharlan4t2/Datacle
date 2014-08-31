var usermodel = function (userservice) {
    var newuser = { id: '', title: '', desc: '' };
    var newtype = { title: '', desc: '', type: 'type' };
    var usertype = { list: [], newtype: newtype, visible: [], newvisible: false };
    var user = { list: [], visible: [], newuser: newuser, newvisible: false };
    var userlist = { list: [], visible: [], newvisible: false };

    /*
    var onError = function (data) {
        console.log(data);
    };

    usertype.__proto__.onloadlist = function (data) {
        list = JSON.parse(data.Data);
    }
    usertype.__proto__.loadlist = function () {
        userservice.getUserTypes().then(this.onloadlist, onError);
    }
    user.__proto__.onloadlist = function (data) {
        list = JSON.parse(data.Data);
    }
    user.__proto__.loadlist = function () {
        userservice.getUsers().then(this.onloadlist, onError);
    }
    userlist.__proto__.onloadlist = function (data) {
        list = JSON.parse(data.Data);
    }
    userlist.__proto__.loadlist = function () {
        userservice.getUserLists().then(this.onloadlist, onError);
    }

    var getList = function (model) {
        if (model.list.length == 0)
            model.loadlist();
        return model.list;
    }
    var getUserList = function (idx) {
        if (userlist.list[idx].length == 0)
            loaduserlist(idx);
        return userlist.list[idx];
    }


    usertype.__proto__.resetNew = function () {
        this.newtype = newtype;
        this.newvisible = false;
    };


    user.__proto__.onSaveList = function (data) {
        userservice.getUsers().then(onlisttypeComplete, onError);
        return false;
    };
    usertype.__proto__.saveList = function (userid) {
        userservice.saveUserList(userid, this.list[idx])
            .then(this.onSaveNew, onError);
    };




    //get user shares
    var onusershareComplete = function (data) {
        userlist = JSON.parse(data.Data);
    };
    //get user list
    var onuserComplete = function (data) {
        user.list = JSON.parse(data.Data);
        var idx;
        for (idx in user.list) {
            var user = user.list[idx]
            //if is selected user
            if (user.isselect) {
                userservice.getUserShares(user.id)
                    .then(onusershareComplete, onError);
            }
        }
    };
    */

    return {
        //        getList: getList ,
        usertype: usertype,
        user: user,
        userlist: userlist
    };
}


listModule.factory("usermodel", usermodel);
