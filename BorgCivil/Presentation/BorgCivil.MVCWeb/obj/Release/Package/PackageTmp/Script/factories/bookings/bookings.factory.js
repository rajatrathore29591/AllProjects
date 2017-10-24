(function () {
    'use strict';

    /**
     * Associate booking factory, booking  info factory with BCA module
     */
    angular
		.module('borgcivil')
		.factory('WorkAllocationFactory', WorkAllocationFactory)
        .factory('BookingListFactory', BookingListFactory)
        .factory('ManageJobBookingFactory', ManageJobBookingFactory)
        .factory('ManageBookingSiteFactory', ManageBookingSiteFactory)
        .factory('ManageDocketFactory', ManageDocketFactory)

    //Inject required modules to factory method
    WorkAllocationFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name WorkAllocationFactory
     * @desc WorkAllocation data
     * @returns {{waStatus: Array, bookingInfo: object}}
     * @constructor
     */
    function WorkAllocationFactory($http, $q, CONST, NotificationFactory) {

        var waObj = {

            bookings: [],
            waStatus: [],
            bookingInfo: [],
            bookingDetail: [],

            /**
             * @name getListing
             * @desc Retrieve bookings listing from db
             * @returns {*}
             */
            getListing: function () {
                //return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllWorkAllocationByTitle/' + 2)
                //    .then(function(response) {
                //        if (response.status == 200 && typeof response.data === 'object') {
                //        	waObj.bookings = response.data; 
                //        }else {
                //            NotificationFactory.error(response.data);
                //        }

                //    }, function(response) {
                //        return $q.reject(response.data);
                //        NotificationFactory.error(false);
                //    });

                return waObj.bookings = [
                {
                    number: "43802",
                    fleet_booking_date_time: "22/02/2017 08:00 AM",
                    fleet_number_description: "Fleet Number: AF18JL Description: HILU05C",
                    customer_name: "2010 Debtors",
                    driver_name: "Paul Cavanagh",
                    site_details: "Austral, Australia",
                    dockets: "3",
                    start_date_time: "11/02/2017 12:00 PM",
                    end_date: "16/02/2017"
                },
                {
                    number: "43812",
                    fleet_booking_date_time: "19/02/2017 08:00AM",
                    fleet_number_description: "Fleet Number: 8ORG Description: T90499A",
                    customer_name: "Absolute Civil Australia Pty Ltd",
                    driver_name: "Marcus Maumber",
                    site_details: "MT Druitt, Australia	",
                    dockets: "3",
                    start_date_time: "13/02/2017 12:00 PM",
                    end_date: "19/02/2017"
                },
                {
                    number: "43701",
                    fleet_booking_date_time: "20/02/2017 08:00AM",
                    fleet_number_description: "Fleet Number: 8RGCVL Description: CMHR",
                    customer_name: "Ages Build Pty Ltd",
                    driver_name: "Phil Hilton",
                    site_details: "Active Earthworks	",
                    dockets: "2",
                    start_date_time: "20/02/2017 11:00 AM",
                    end_date: "25/02/2017"
                }
                ];
            },

            /**
           * @name getListingByStatus
           * @desc Retrieve WA listing from db
           * @returns {*}
           */
            getListingByStatus: function (status) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllWorkAllocationByStatus/' + status)
                    .then(function (response) {

                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response))
                            waObj.waStatus = response;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });


            },

            getBookingInfo: function () {
                /*return $http.get(CONST.CONFIG.API_URL + 'customers')
                    .then(function(response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            waObj.bookingInfo = response.data[0]; 
                        }else {
                            NotificationFactory.error(response.data);
                        }

                    }, function(response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });*/

                return waObj.bookingInfo = {
                    department: "TCEB",
                    name: 'tesco Boys Europe',
                    contact_person: "Mike",
                    designation: 'Director',
                    email: 'tesco@hotmail.com',
                    contact_no: '+44 20 8962 1234'
                }
            },

            getWorkAllocationByBookingId: function (BookingId, status) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetWorkAllocationByBookingId/' + BookingId + '/' + status)
                   .then(function (response) {
                       if (response.status == 200) {
                           //console.log("asdasdg" + JSON.stringify(response))
                           waObj.bookingDetail = response.data;
                       } else {
                           NotificationFactory.error(response.data);
                       }
                   }, function (response) {
                       return $q.reject(response.data);
                       NotificationFactory.error(false);
                   });
            }


        }

        return waObj;
    }

    //BookingsListDetailsFactory
    //Inject required modules to factory method
    BookingListFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name BookingListFactory
     * @desc Booking data
     * @returns {{}}
     * @constructor
     */
    function BookingListFactory($http, $q, CONST, NotificationFactory) {

        var blObj = {

            bookingList: [],

            /**
         * @name GetAllBookingByDateRange
         * @desc Retrieve Booking list from db
         * @returns {*}
         */
            getAllBookingByDateRange: function (fromDate, toDate) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllBookingByDateRange/' + fromDate + '/' + toDate)
                    .then(function (response) {

                        if (response.status == 200) {
                            blObj.bookingList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
            * @name DeleteBooking by bookingId
            * @desc deleting booking record from db
            * @returns {*}
            */
            deleteBooking: function (bookingId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/DeleteBooking/' + bookingId)
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
        * @name updateBookingStatus by bookingId
        * @desc updating booking status listing from db
        * @returns {*}
        */
            updateBookingStatus: function (statusData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateBookingStatus', statusData)
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
            * @name sendAttachment
            * @desc updating booking status listing from db
            * @returns {*}
            */
            sendAttachment: function (data) {
                return $http.post(CONST.CONFIG.BASE_URL + 'EmailAttachment/SendAttachment', data)
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
        }

        return blObj;
    }

    //Inject required modules to factory method
    ManageJobBookingFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name ManageJobBookingFactory
     * @desc JobBooking data
     * @returns {{}}
     * @constructor
     */
    function ManageJobBookingFactory($http, $q, CONST, NotificationFactory) {

        var mjbObj = {

            bookingDetail: [],
            bookingFleetDetail: [],

            /**
        * @name getBookingDetail
        * @desc Retrieve Booking detail from db
        * @returns {*}
        */
            getBookingDetail: function (bookingId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetBookingDetail/' + bookingId)
                    .then(function (response) {

                        if (response.status == 200) {
                            mjbObj.bookingDetail = response.data;
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
                            //console.log("From Factory Response", response);
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            ///**
            //* @name addBooking
            //* @desc add booking from db
            //* @returns {response message}
            //*/
            addBooking: function (bookingData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/AddBooking', bookingData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            ///**
            //* @name addBookingFleet
            //* @desc add booking fleet data to db
            //* @returns {response message}
            //*/
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

            ///**
            //* @name updateBookingFleet
            //* @desc add booking from db
            //* @returns {response message}
            //*/
            updateBookingFleet: function (bookingData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateBookingFleet', bookingData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
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

        }

        return mjbObj;
    }

    //Inject required modules to factory method
    ManageBookingSiteFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name ManageBookingSiteFactory
     * @desc Site Booking data 
     * @returns {{siteDetail: object, contactDetail: object}}
     * @constructor
     */
    function ManageBookingSiteFactory($http, $q, CONST, NotificationFactory) {

        var mbsObj = {

            siteDetail: [],
            contactDetail: [],

            /**
          * @name getBookingSiteDetail
          * @desc Retrieve WA listing from db
          * @returns {*}
          */
            getBookingSiteDetail: function (bookingId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetBookingSiteDetail/' + bookingId)
                    .then(function (response) {

                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data))
                            mbsObj.siteDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
            * @name addBookingSiteDetail
            * @desc add booking from db
            * @returns {response message}
            */
            addBookingSiteDetail: function (bookingSiteData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/AddBookingSiteDetail', bookingSiteData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            /**
              * @name getContactPersons
              * @desc Retrieve WA listing from db
              * @returns {*}
              */
            getContactPersons: function (gateId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetContactPersons/' + gateId)
                    .then(function (response) {
                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data));
                            mbsObj.contactDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
            * @name AddGateContactPerson
            * @desc add Contact from db
            * @returns {response message}
            */
            addGateContactPerson: function (contactData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/AddGateContactPerson', contactData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            /**
            * @name AddGateContactPerson
            * @desc add Contact from db
            * @returns {response message}
            */
            updateGateContactPerson: function (contactData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateGateContactPerson', contactData, {
                })
                .then(function (response) {
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }

                }, function (response) {
                    //return $q.reject(response.data);
                    NotificationFactory.error("Error: " + response.statusText);
                });
            }
        }
        return mbsObj;
    }

    //Inject required modules to factory method
    ManageDocketFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
     * @name ManageDocketFactory
     * @desc Docket data
     * @returns {{waStatus: Array, bookingInfo: object}}
     * @constructor
     */
    function ManageDocketFactory($http, $q, CONST, NotificationFactory) {
        var mdfObj = {

            docketDetail: [],
            docketList: [],
            docketCheckList: [],

            /**
              * @name getBookedFleetRegistration
              * @desc Retrieve Fleet Registration Numbers from db
              * @returns {*}
              */
            getBookedFleetRegistration: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetBookedFleetRegistration/')
                    .then(function (response) {
                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data))
                            mdfObj.docketDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },
            /**
             * @name addDocket
             * @desc Add new Docket into db
             * @returns {*}
             */
            addDocket: function (docketData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/AddDocket', docketData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },
            /**
            * @name updateDocket
            * @desc Add new Docket into db
            * @returns {*}
            */
            updateDocket: function (docketData) {
                return $http.post(CONST.CONFIG.API_URL + 'Booking/UpdateDocket', docketData, {
                })
                    .then(function (response) {
                        if (response.status == 200) {
                            NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                        } else {
                            NotificationFactory.error(response.data.Message);
                        }

                    }, function (response) {
                        //return $q.reject(response.data);
                        NotificationFactory.error("Error: " + response.statusText);
                    });
            },

            /**
             * @name GetAllDocketByBookingFleetId
             * @desc Retrieve Fleet Registration Numbers from db
             * @returns {*}
             */
            GetAllDocketByBookingFleetId: function (BookingFleetId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllDocketByBookingFleetId/' + BookingFleetId)
                    .then(function (response) {
                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data))
                            mdfObj.docketList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name getDocketDetail
              * @desc Retrieve Fleet Registration Numbers from db
              * @returns {*}
              */
            getDocketDetail: function (DocketId) {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetDocketDetail/' + DocketId)
                    .then(function (response) {
                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data))
                            mdfObj.docketDetail = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },

            /**
              * @name getAllDocketCheckboxList
              * @desc Retrieve Fleet Registration Numbers from db
              * @returns {*}
              */
            getAllDocketCheckboxList: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Booking/GetAllDocketCheckboxList/')
                    .then(function (response) {
                        if (response.status == 200) {
                            //console.log("asdasdg" + JSON.stringify(response.data))
                            mdfObj.docketCheckList = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });
            },
        }

        return mdfObj;
    }

})();