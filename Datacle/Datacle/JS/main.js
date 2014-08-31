require.config({
    paths: {
        jquery: "/Scripts/jquery-1.9.0.min",
        angular: "/Scripts/angular",
        userCtrl: "/User/userCtrl",
        userService: "/User/userService",
        listCtrl: "/List/listCtrl",
        listService: "/List/listService",
        viewCtrl: "/View/viewCtrl",
        viewService: "/View/viewService"

    },
    baseUrl: "/JS",
    shim: {
        'angular': { 'exports': 'angular' },
        'angularMocks': { deps: ['angular'], 'exports': 'angular.mock' },
        'index': { deps: ['angular'] }
}
});
require(['jquery', 'index'], function ($, index) {

    $(document).ready(function () {
    });
});
