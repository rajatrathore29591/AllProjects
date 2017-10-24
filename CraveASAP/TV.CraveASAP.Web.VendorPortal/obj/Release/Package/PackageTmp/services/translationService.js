'use strict';
app.service('translationService', ['$http', '$resource', function ($http, $resource) {
    this.getTranslation = function ($scope, $rootScope, callback) {
        
        //var languageFilePath = "http://cravevendor.techvalens.net/translations" + "/" + $scope.lang + '.json';
        var languageFilePath = "http://eddee.cloudapp.net/eddeevendor/translations" + "/" + $scope.lang + '.json';
        //var languageFilePath = "http://vendor.eddee.it/translations" + "/" + $scope.lang + '.json';
        //var languageFilePath = "http://localhost:25215/translations" + "/" + $scope.lang + '.json';
        $resource(languageFilePath).get(function (data) {
            $rootScope.translation = data;
            callback(data);
            //$rootScope.Pleaseverifyenddate = $rootScope.translation.Pleaseverifyenddate;
            //$rootScope.startDateandEndDatediffrenceshouldbelessthan7Days = $rootScope.translation.startDateandEndDatediffrenceshouldbelessthan7Days;



        });
       
    };
}]);