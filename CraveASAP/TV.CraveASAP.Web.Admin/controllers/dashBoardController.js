'use strict';
app.controller('dashBoardController', ['$scope', '$rootScope', '$cookies', '$cookieStore', '$location', 'initData', function ($scope, $rootScope, $cookies, $cookieStore, $location, initData) {


   
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
            //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $rootScope.active = 'dashboard';
    $rootScope.isLogged = 1;

    //$rootScope.logout = function () {
    //    alert();
    //    $('#leftMenu').css({ 'display': 'none' });
    //    $('#topMenu').css({ 'display': 'none' });
    //    $cookieStore.remove('loggedUserName');
    //    $rootScope.UserName = $cookieStore.get('loggedUserName');
    //    $location.path('/');
    //}

}]);
