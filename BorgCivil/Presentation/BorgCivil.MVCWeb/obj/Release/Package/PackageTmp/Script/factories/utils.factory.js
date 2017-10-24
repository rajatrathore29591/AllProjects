(function() {
    'use strict';
    /**
     * Client Factory
     * Created by: Kapil Chhabra (SIPL)
     * Created On: 21-07-2016
     */

    //Inject required modules to factory method
    UtilsFactory.$inject = ['CONST', '$http', 'NotificationFactory', 'SweetAlertFactory', 'notify', 'NgTableParams'];

    /**
     * @name UtilsFactory
     * @desc Contains all notification methods to be used in whole application
     * @param notify
     * @constructor
     */
    function UtilsFactory(CONST, $http, NotificationFactory, SweetAlertFactory, notify, NgTableParams)
    {
        var bcaUtilities = {
            confirmBox : function (title, message, callback) {
                return SweetAlertFactory.swal({
                        title: title,
                        text: message,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Ok",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true },

                function (isConfirm) {
                    callback(isConfirm);
                });
            },

            oops : function (title, message) {
                return SweetAlertFactory.swal({
                    title: title,
                    text: message,
                    showCancelButton: false,
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
            },

            range : function(min, max, step){
                step = step || 1;
                var input = [];
                for (var i = min; i <= max; i += step) input.push(i);
                return input;
            },

            capitalizeFirstLetter : function(string){
                return string.charAt(0).toUpperCase() + string.slice(1);
            },
            /* jshint ignore:start */
            createNgTable : function(data) {
                return new NgTableParams({}, { dataset: data});
            },

            bcaTableOptions : function (dataset) {
                var per_page = 10;
                if(arguments.length > 1){
                    per_page = arguments[1];
                }
                var initialParams = {
                    count: per_page // initial page size
                };
                var initialSettings = {
                    // page size buttons (right set of buttons in demo)
                    counts: [10,20,30,50],
                    // determines the pager buttons (left set of buttons in demo)
                    paginationMaxBlocks: 5,
                    paginationMinBlocks: 2,
                    dataset: dataset
                };
                return new NgTableParams(initialParams, initialSettings);
            },

            bcaGroupTableOptions : function (dataset, group) {
                var per_page = 10;
                if(arguments.length > 2){
                    per_page = arguments[2];
                }
                var initialParams = {
                    count: per_page, // initial page size
                    group: group
                };
                var initialSettings = {
                    // page size buttons (right set of buttons in demo)
                    counts: [10,20,30,50],
                    // determines the pager buttons (left set of buttons in demo)
                    paginationMaxBlocks: 5,
                    paginationMinBlocks: 2,
                    dataset: dataset
                };
                return new NgTableParams(initialParams, initialSettings);
            },
              /* jshint ignore:end */
            uniqueArraybyKey : function (collection, keyname) {
                var output = [], 
                          keys = [];

                      angular.forEach(collection, function(item) {
                          var key = item[keyname];
                          if(keys.indexOf(key) === -1) {
                              keys.push(key);
                              output.push(item);
                          }
                      });
                return output;
            },

            serverPagination: function(url,attr){
                var per_page = 10;
                if(arguments.length > 2){
                    per_page = arguments[2];
                }
                return new NgTableParams({
                    page: 1,
                    count: per_page
                }, {
                    counts: [10,20,30,50],
                    getData: function($defer, params) {
                        var param =  {pageNumber:params.page() - 1, limit: params.count(), sorting: params.sorting() };
                        angular.extend(param, attr);
                        $http.post(CONST.CONFIG.API_URL + url, param)
                            .success(function(response) {
                                params.total(response.result.total);
                                $defer.resolve(response.result.ezData);
                            });
                    }
                })
            }

        }

        return bcaUtilities;
    }

    //Inject required modules to factory method
    NotificationFactory.$inject = ['CONST', 'toaster', '$http'];

    /**
     * @name NotificationFactory
     * @desc Contains all notification methods to be used in whole application
     * @param notify
     * @constructor
     */
    function NotificationFactory(CONST, toaster, $http)
    {
        var bcaNotification = {
            /**
             * @name success
             * @desc Show success message
             * @param message
             */
            success: function (message) {
                console.log('message'+message)
                toaster.success('Success', message);
            },

            /**
             * @name error
             * @desc Show common error message
             */
            error : function(err) {
                if(!err || err == ''){
                    err = CONST.MSG.COMMON_ERROR;
                }
                toaster.error('Failed', err);
            },

            /**
             * @name warning
             * @desc Show warning message
             * @param message
             */
            warning : function(message) {
                toaster.clear();
                toaster.pop('warning', 'Info', message);
            },

            /**
             * @name waiting
             * @desc Show custom waiting message
             * @param message
             */
            waiting : function(message) {
                toaster.pop('wait', '', null, null);
            },

            /**
             * @name requestWaiting
             * @desc Show request waiting message
             * @param message
             */
            requestWaiting : function() {
                toaster.pop('wait', CONST.MSG.WAITING_REQUEST, null, null);
            },

            /**
             * @name dataRetrievalWaiting
             * @desc Show data retrieval waiting message
             * @param message
             */
            dataRetrievalWaiting : function() {
                toaster.pop('wait', CONST.MSG.WAITING_DATA_RETRIEVAL, null, null);
            },

            /**
             * for clear all toaster messages
             */
            clear : function() {
                toaster.clear();
            }
        };

        return bcaNotification;
    }

   
//Inject required modules to factory method
SweetAlertFactory.$inject = ['$timeout', '$window'];

/**
 * @name SweetAlertFactory
 * @desc Contains all common methods to be used for sweet alert
 * @param notify
 * @constructor
 */
function SweetAlertFactory($timeout, $window) {
    var swal = $window.swal;
    return {
        swal: function (arg1, arg2, arg3) {
            $timeout(function () {
                if (typeof(arg2) === 'function') {
                    swal(arg1, function (isConfirm) {
                        $timeout(function () {
                            arg2(isConfirm);
                        });
                    }, arg3);
                } else {
                    swal(arg1, arg2, arg3);
                }
            }, 200);
        },
        success: function (title, message) {
            $timeout(function () {
                swal(title, message, 'success');
            }, 200);
        },
        error: function (title, message) {
            $timeout(function () {
                swal(title, message, 'error');
            }, 200);
        },
        warning: function (title, message) {
            $timeout(function () {
                swal(title, message, 'warning');
            }, 200);
        },
        info: function (title, message) {
            $timeout(function () {
                swal(title, message, 'info');
            }, 200);
        }

    };
};

    /**
     * Associate factory with BCA module
     */
    angular
        .module('borgcivil')
        .factory('UtilsFactory', UtilsFactory)
        .factory('NotificationFactory', NotificationFactory)
        .factory('SweetAlertFactory', SweetAlertFactory);
})();