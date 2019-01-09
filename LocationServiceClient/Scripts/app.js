/// <reference path="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.0/angular.js" />
var locationApp = angular.module('locationApp', ['AdalAngular']);

locationApp.config(['$httpProvider', 'adalAuthenticationServiceProvider', function ($httpProvider, adalProvider) {
    var endpoints = {
        "http://localhost:44300": " https://tektaniuminc.onmicrosoft.com/LocationSvcTest"
    };

    adalProvider.init({
        instance: 'https://login.microsoftonline.com/',
        tenant: 'tektaniuminc.onmicrosoft.com',
        clientId: '59f85b55-5869-498e-84b6-1470b2bc18a8',
        endpoints: endpoints
    }, $httpProvider);
}]);

var locationController = locationApp.controller("locationController", [
    '$scope', '$http', 'adalAuthenticationService',
    function ($scope, $http, adalService) {
        $scope.getLocation = function () {
            $http.get("https://localhost:44300/api/location?cityName=dc").success(function (location) {
                $scope.city = location;
            });
        };

        $scope.login = function () {
            adalService.login();
        };

        $scope.logout = function () {
            adalService.logOut();
        };
    }]);