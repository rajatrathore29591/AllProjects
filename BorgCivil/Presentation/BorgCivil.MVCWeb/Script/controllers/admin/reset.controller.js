(function() {
    'use strict';

    /**
     * Reset Password & Change Password Controller
     * Created by: Kapil Chhabra (SIPL)
     * Created On: 29-07-2016
     */

    angular
        .module('borgcivil')
        .controller('RestPasswordCtrl', RestPasswordCtrl)
        .controller('ChangePasswordCtrl', ChangePasswordCtrl);

    //Inject required stuff as parameters to reset password controller function
    RestPasswordCtrl.$inject = ['$scope', 'UtilsFactory', '$location'];

    /**
     * @name RestPasswordCtrl
     */
    function RestPasswordCtrl($scope, UtilsFactory, $location){
        var vm = this;
        vm.create = create;
        
        //Form validation messages
        vm.validationMsg = {
            required: 'Required',
            invalid: 'Invalid Input',
            email:   'Invalid Email'
        }
        function create(isValid){
            if(isValid){
                $location.path('login')
            }
        }
    }
    

    //Inject required stuff as parameters to change password controller function
    ChangePasswordCtrl.$inject = ['$scope', 'UtilsFactory', '$location'];

    /**
     * @name ChangePasswordCtrl
     */
    function ChangePasswordCtrl($scope, UtilsFactory,$location){
        var vm = this;
        
        //Form validation messages
        vm.validationMsg = {
            required: 'Required',
            invalid: 'Invalid Input'
        }
        function create(isValid){
            if(isValid){
                $location.path('login')
            }
        }
    }

})();