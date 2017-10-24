'use strict';
app.controller('indexController', ['$scope', '$rootScope', 'indexService', 'initLoader', function ($scope, $rootScope, indexService, initLoader) {
    $scope.banner = "Go";
    alert()
    indexService.GetAllBanners($scope);
    $scope.sendMail = function () {
        alert()
        indexService.saveSubscribeUsers($scope);
    }
}])

app.controller('indexControllerVideo', ['$scope', 'indexServiceVideo', '$sce', function ($scope, indexServiceVideo, $sce) {
    {
        indexServiceVideo.GetAllVideoWebApp($scope, function (success, data) {
            if (success) {

                $scope.sp_data = JSON.parse(data.data1);
               
            }
            else {
                console.log("failed", data);

            }
        });

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }
        //$scope.video1 = { src: "http://www.youtube.com/embed/Lx7ycjC8qjE" };
        //$scope.video2 = { src: "http://www.youtube.com/embed/Lx7ycjC8qjE" };
    }
    //alert()
    //$("a").removeClass("active");
    //$rootScope.active = "indexControllerVideo";

}])


app.controller('indexControllerMapLocation', ['$scope', 'indexMapLocationService', function ($scope, indexMapLocationService) {
    {
        indexMapLocationService.GetMapLocation($scope);
    }

}])

app.service('indexService', ['$http', '$rootScope', 'initLoader', function ($http, $rootScope, initLoader) {

    this.saveSubscribeUsers = function ($scope) {
        alert()
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateSubscribedUsers",
            data: { email: $scope.email, isSubscribe: true }
        }).success(function (data) {
            console.log("da"+data)
        }).error(function (data) {
        });;
    };

    this.GetAllBanners = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllBanners 1,flash,English"
        }).success(function (data) {
            $rootScope.landingBanner = data;
            var temp = []
            for (var i = 0; i < data.length; i++) {
                //temp[i] = { image: "http://svc.eddee.it/Pictures/" + data[i].imageURL, title: data[i].imageURL, thumb: "http://svc.eddee.it/Pictures/" + data[i].imageURL, url: data[i].reference };
                temp[i] = { image: $scope.config.api + "/Pictures/" + data[i].imageURL, title: data[i].imageURL, thumb: $scope.config.api + "/Pictures/" + data[i].imageURL, url: data[i].reference };

            }
            initLoader.testLader(temp)

        }).error(function (data) {
            console.log(data);
        });;
    };
}]);

app.service('indexServiceVideo', ['$http', function ($http) {

    this.GetAllVideoWebApp = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVideoWebApp"
        }).success(function (data) {
            $scope.landingVideo = data;
            //  alert(JSON.stringify($scope.landingVideo));
            $scope.UserLink = $scope.config.api + "/Video/" + $scope.landingVideo[0].contentLink.trim();
            $scope.BusinessLink = $scope.config.api + "/Video/" + $scope.landingVideo[1].contentLink.trim();

            $scope.userVideo = '<video id="app_tour_video" class="video-js vjs-default-skin  embed-responsive-item" controls preload="none" poster=""  data-setup="{}"  width="auto" height="auto">' +
           '<source src="' + $scope.UserLink + '" type="video/mp4" /> <p class="vjs-no-js"> To view this video please enable JavaScript, and consider upgrading to a web browser that' +
           '<a ng-if="landingVideo.contentName=="Business video"" href="landingVideo.contentLink" target="_blank">supports HTML5 video</a> </p> </video>';
            $("#UserVideo").append($scope.userVideo);


            $scope.businessVideo = '<video id="app_tour_video2" class="video-js vjs-default-skin  embed-responsive-item" controls preload="none" poster=""  data-setup="{}"  width="auto" height="auto">' +
           '<source src="' + $scope.BusinessLink + '" type="video/mp4" /> <p class="vjs-no-js"> To view this video please enable JavaScript, and consider upgrading to a web browser that' +
           '<a ng-if="landingVideo.contentName=="Business video"" href="landingVideo.contentLink" target="_blank">supports HTML5 video</a> </p> </video>';
            $("#BusinessVideo").append($scope.businessVideo);

            videojs("#app_tour_video").ready(function () {
                "use strict";
                var tour_player = videojs("#app_tour_video");
                tour_player.width($(window).width());
                tour_player.height(600);
            });
            videojs("#app_tour_video2").ready(function () {
                "use strict";
                var tour_player = videojs("#app_tour_video2");
                tour_player.width($(window).width());
                tour_player.height(600);
            });
            $("#app_tour_video").css('height', 'auto');
            $("#app_tour_video2").css('height', 'auto');
        }).error(function (data) {
        });;
    };

}]);

var apps = angular.module('plunker', ['ngSanitize']);

apps.controller('MainCtrl', function ($scope, $sce) {
    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    $scope.movie = { src: "http://www.youtube.com/embed/Lx7ycjC8qjE", title: "Egghead.io AngularJS Binding" };
});
