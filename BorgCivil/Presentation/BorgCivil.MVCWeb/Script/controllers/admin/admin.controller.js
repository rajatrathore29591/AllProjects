(function() {
    'use strict';

    /**
     * Login Controller
     * Created by: Sonali Nagar (SIPL)
     * Created On: 04-05-2017
     */

    angular
        .module('borgcivil')
        .controller('LoginCtrl', LoginCtrl);

    //Inject required stuff as parameters to factories controller function
    LoginCtrl.$inject = ['$scope', '$state', 'UtilsFactory', 'LoginFactory', '$cookieStore', '$rootScope', 'CONST', '$location'];

    /**
     * @name LoginCtrl
     * @param $scope
     * @param $state
     * @param UtilsFactory
     * @param LoginFactory 
     * @param $cookieStore   
     */
    function LoginCtrl($scope, $state, UtilsFactory, LoginFactory, $cookieStore, $rootScope, CONST, $location) {
    	var vm = this;    	
    	vm.response = true;
    	vm.emailId = '';
    	vm.remember = false;
    	vm.attributes = {
    	    'Email': '',
    	    'Password': ''
    	};
    	vm.attributes = $cookieStore.get('user_cookies');
    	vm.passAttribute = {
            'Email':''
    	}

        //method    	
    	vm.loginSubmit = loginSubmit;
    	vm.passwordSubmit = passwordSubmit;
          
        /**
          * @name loginSubmit
          * @desc Login by user credentials
          * @returns {*}
          */
    	function loginSubmit(form) {
    	    console.log(vm.remember);    	   
            LoginFactory
                .userLogin(vm.attributes)
                .then(function () {               
                    if (LoginFactory.loginObject) {
                        localStorage.setItem("ACCESS_TOKEN", LoginFactory.loginObject.access_token);
                        localStorage.setItem("UserId", LoginFactory.loginObject.LoginUserId);
                        localStorage.setItem("Image", LoginFactory.loginObject.ImageUrl);
                        $rootScope.LoginUserId = localStorage["UserId"];
                        $rootScope.Image = CONST.CONFIG.IMG_URL + localStorage["Image"];
                        console.log('LoginFactory.loginObject.access_token', LoginFactory.loginObject.access_token);
                        if (vm.remember) {

                            //Set Cookies if remember is Enabled
                            $cookieStore.put('user_cookies', vm.attributes);
                            
                        } else {

                            //Set Cookies if remember is Disabled
                            $cookieStore.remove('user_cookies');
                            console.log("REMOVE", $cookieStore.get('user_cookies'));

                        }
                        $state.go('booking.BookingList');
                    } else {
                    
                        vm.response = LoginFactory.loginObject;
                        return false;
                    }
                
                });
    	}

        /**
         * @name initLoginId
         * @desc initialize image on manage profile
         * @returns {*}
         */
    	vm.initLoginId = function () {

    	    $rootScope.Image = CONST.CONFIG.IMG_URL + localStorage["Image"];
    	    $rootScope.LoginUserId = localStorage["UserId"];
    	    //$rootScope.LoginUserName = localStorage["LOGIN_USER_NAME"];
    	    console.log("$rootScope.imageUrl" + $rootScope.imageUrl);
    	}

        /**
         * @name logOut
         * @desc logOut method destroying local storage
         * @returns {*}
         */
    	vm.logOut = function () {

            // deleting all local storage variables
    	    delete localStorage["ACCESS_TOKEN"];
    	    delete localStorage["UserId"];
    	    delete localStorage["Image"];
    	    window.localStorage.clear();
    	    localStorage.removeItem('ACCESS_TOKEN');
    	    localStorage.removeItem('UserId');
    	    localStorage.removeItem('Image');

    	    $location.path('admin/Login');
    	}

        /**
          * @name passwordSubmit
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function passwordSubmit(form) {
            console.log(vm.passAttribute.Email, form);
            if (form && vm.passAttribute.Email) {
                LoginFactory
                .forgotPassword(vm.passAttribute)
                .then(function () {
                $state.go('admin.Login')});
            }
           
        }
    }
    

})();