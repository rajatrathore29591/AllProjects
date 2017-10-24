'use strict';

app.controller('masterController', ['$scope', '$rootScope', '$location', '$cookies', '$cookieStore', 'initData', function ($scope, $rootScope, $location,  $cookies, $cookieStore, initData) {

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/Login');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedUserName');
        //$location.path('/Main/Dashboard');
    }
   
    initData.initFunction();
    



}])
