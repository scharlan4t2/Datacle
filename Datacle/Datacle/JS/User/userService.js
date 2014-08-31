var userservice = function ($http) {

    var userdefault = function () { return { id: '', typeid: '', title: '', desc: '' }; }
    var emaildefault = function () { return { email: ''}; }
    
    //getUserList
    var getUserList = function (userid) {
        return $http.get("/API/UserList")
        .then(function (response) {
            user.list = JSON.parse(response.data.Data);
        });
    }
    var saveUserList = function (userid) {
        return $http.post("/API/UserList/", user.newuser)
        .then(function (response) {
            var newuser = JSON.parse(response.data.Data);
            getUser();
            user.newuser = userdefault();
        });
    }
    //getUser
    var getUser = function () {
        return $http.get("/API/User")
        .then(function (response) {
            user.list = JSON.parse(response.data.Data);
        });
    }
    var saveUser = function () {
        return $http.post("/API/User/", user.newuser)
        .then(function (response) {
            var newuser = JSON.parse(response.data.Data);
            getUser();
            user.newuser = userdefault();
        });
    }
    //getUserType
    var getUserType = function () {
        $http.get("/API/Reference/UserType", { type: 'user' })
        .then(function (response) {
            usertype.list = JSON.parse(response.data.Data);
        });
    }
    var saveType = function () {
        return $http.post("/API/Reference/UserType/", usertype.newtype)
        .then(function (response) {
            var newusertype = JSON.parse(response.data.Data);
            getUserType();
            usertype.newtype = typedefault();
        });
    }
    var setNew = function () {
        this.shownew = !this.shownew;
        if (this.shownew) {
            this.newitem = typedefault();
        }
    }
    var showNew = function () {
        return this.shownew;
    }
    var hideNew = function () {
        this.shownew = false;
    }
    var saveNew = function () {
        return $http.post("/API/Reference/UserType/", usertype.newtype)
        .then(function (response) {
            var newusertype = JSON.parse(response.data.Data);
            getUserType();
            usertype.newtype = typedefault();
        });
    }
    var typedefault = function () { return { title: '', desc: '', type: 'user' }; }

    var usertype = {
        list: [],
        newtype: typedefault(),
        visible: [],
        shownew: false,
        // New Object
        setNew: setNew,
        showNew: showNew,
        hideNew: hideNew,
        saveNew: saveNew,
        getList: getUserType,
        saveObj: saveType
    };




    var getUserShares = function (userid) {
        return $http.get("/API/User/" + userid)
        .then(function (response) {
            return response.data;
        });
    }
    var saveUserList = function (userid, user) {
        return $http.post("/API/User/" + userid, user)
        .then(function (response) {
            return response.data;
        });
    }
    var user = {
        list: [], visible: [], newuser: userdefault(), adduser: emaildefault(), newvisible: false,
        getList: getUser, saveObj: saveUser
    };
    var userlist = {
        list: [],
        visible: [],
        newvisible: false,
        getList: getUserList,
        saveObj: saveUserList
    };

    return {
        usertype: usertype,
        user: user,
        userlist: userlist
    }
}

listModule.factory("userservice", userservice);
