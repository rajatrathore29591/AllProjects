(function () {
    'use strict';

    /**
     * Associate booking factory, booking  info factory with BCA module
     */
    angular
		.module('borgcivil')
		.factory('BCAFactory', BCAFactory)

    //Inject required modules to factory method
    BCAFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name BCAFactory
     * @desc common dropdown data
     * @returns {{}}
     * @constructor
     */
    function BCAFactory($http, $q, CONST, NotificationFactory) {

        var mjbObj = {

            customerList: [],
            siteList: [],
            workTypeList: [],
            fleetTypeList: [],
            fleetRegistrationList: [],
            driverList: [],
            attachementList: [],
            bookingFleetDetail: [],
            bookingFleetHistoryDetail: [],
            employmentCategoryList: [],
            statusLookupList: [],
            getLicenseClassList: [],
            countries: [],
            states: [],
            roles: [],
            subContractors: [],
            
            /**
           * @name getCustomersDDL
           * @desc Retrieve MJB listing from db
           * @returns {*}
           */
            getCustomersDDL: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Customer/GetCustomers')
                    .then(function (response) {

                        if (response.status == 200) {
                            mjbObj.customerList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
          * @name getSitesByCustomerIdDDL
          * @desc Retrieve MJB listing from db
          * @returns {*}
          */
            getSitesByCustomerIdDDL: function (customerId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/GetSitesByCustomerId/' + customerId)
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.siteList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
          * @name getWorkTypesDDL
          * @desc Retrieve MJB listing from db
          * @returns {*}
          */
            getWorkTypesDDL: function () {
                return $http.get(CONST.CONFIG.API_URL + 'WorkType/GetWorkTypes')
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.workTypeList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
         * @name getFleetTypesDDL
         * @desc Retrieve MJB listing from db
         * @returns {*}
         */
            getFleetTypesDDL: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetFleetTypes')
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.fleetTypeList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
           * @name getFleetRegistrationsByFleetTypeIdDDL
           * @desc Retrieve MJB listing from db
           * @returns {*}
           */
            getFleetRegistrationsByFleetTypeIdDDL: function (fleetTypeId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetFleetRegistrationsByFleetTypeId/' + fleetTypeId)
                    .then(function (response) {
                        
                        if (response.status == 200) {
                            mjbObj.fleetRegistrationList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
            * @name getDriversByFleetRegistrationIdDDL
            * @desc Retrieve MJB listing from db
            * @returns {*}
            */
            getDriversByFleetRegistrationIdDDL: function (fleetRegistrationId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetDriversByFleetRegistrationId/' + fleetRegistrationId)
                    .then(function (response) {
                        
                        if (response.status == 200) {
                            mjbObj.driverList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
             * @name getAttachmentsChk
             * @desc Retrieve MJB listing from db
             * @returns {*}
             */
            getAttachmentsChk: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetAllAttachment')
                    .then(function (response) {

                        if (response.status == 200) {
                            mjbObj.attachementList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name getBookingFleetDetail
              * @desc Retrieve Booking detail from db
              * @returns {*}
              */
            getBookingFleetDetail: function (bookingFleetId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetBookingFleetDetail/' + bookingFleetId)
                    .then(function (response) {

                        if (response.status == 200) {
                            mjbObj.bookingFleetDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name addBookingFleet
              * @desc add booking fleet data to db
              * @returns {response message}
              */
            addBookingFleet: function (bookingFleetData) {

                return $http.post(CONST.CONFIG.API_URL + 'Booking/AddBookingFleet', bookingFleetData, {
                })
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            ////check respons data result.
                            if (response.data.result) { ////case of success/no error.
                                NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                            }
                            else {////case of success/with error.
                                NotificationFactory.error(CONST.MSG.COMMON_ERROR);
                            }
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            /**
            * @name updateBookingFleet
            * @desc add booking from db
            * @returns {response message}
            */
            updateBookingFleet: function (bookingData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateBookingFleet', bookingData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            /**
            * @name deleteBookingFleet
            * @desc Retrieve Booking detail from db
            * @returns {*}
            */
            deleteBookingFleet: function (bookingFleetId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/DeleteBookingFleet/' + bookingFleetId)
                    .then(function (response) {

                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
           * @name UpdateBookingFleetStatus
           * @desc Change booking fleet status 
           * @returns {*}
           */
            updateBookingFleetStatus: function (bookingFleetObj) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateBookingFleetStatus', bookingFleetObj, {})
                  .then(function (response) {

                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                      } else {
                          NotificationFactory.error(response.data);
                      }

                  }, function (response) {
                      return $q.reject(response.data);
                      NotificationFactory.error(false);
                  });
            },

            /**
              * @name GetBookingFleetHistory
              * @desc Get fleet history of BookingFleetId 
              * @returns {*}
              */
            getBookingFleetHistory: function (bookingFleetId, bookingId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllFleetHistory/' + bookingFleetId + '/' + bookingId)
                    .then(function (response) {

                        if (response.status == 200) {
                            mjbObj.bookingFleetHistoryDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
               * @name getEmploymentCategory
               * @desc Retrieve EmployementCategory listing from db
               * @returns {*}
               */
            getEmploymentCategory: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetEmploymentCategory')
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.employmentCategoryList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name getStatusLookup
              * @desc Retrieve StatusLookup listing from db
              * @returns {*}
              */
            getStatusLookup: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetStatusLookup')
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.statusLookupList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
             * @name getLicenseClass
             * @desc Retrieve LicenseClass listing from db
             * @returns {*}
             */
            getLicenseClass: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetLicenseClass')
                    .then(function (response) {
                        if (response.status == 200) {
                            mjbObj.getLicenseClassList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name getAllCountry
              * @desc Retrieve All Countries from db
              * @returns {*}
              */
            getAllCountry: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllCountry')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        mjbObj.countries = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return mjbObj.countries;
            },

            /**
             * @name getAllState
             * @desc Retrieve All states from db
             * @returns {*}
             */
            getAllState: function (CountryId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllState/' + CountryId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        mjbObj.states = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return mjbObj.states;
            },

            /**
             * @name getAllRoles
             * @desc Retrieve users listing from db
             * @returns {*}
             */
            getAllRoles: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllRoles')
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            mjbObj.roles = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });

                return mjbObj.roles;
            },

            /**
            * @name  getSubcontractor
            * @desc Retrieve subContractor listing from db
            * @returns {*}
            */
            getSubcontractor: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetSubcontractor')
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            mjbObj.subContractors = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });

                return mjbObj.subContractors;
            }
        }
        return mjbObj;
    }
})();