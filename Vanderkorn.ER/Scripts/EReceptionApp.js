var EReceptionApp = angular.module('EReceptionApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'SignalR']);

EReceptionApp.controller('IndexController', IndexController);
EReceptionApp.controller('LoginController', LoginController);
EReceptionApp.controller('RegisterController', RegisterController);
EReceptionApp.controller('ReceptionController', ReceptionController);
EReceptionApp.controller('ManageController', ManageController);

EReceptionApp.factory('authService', authService);
EReceptionApp.factory('authInterceptorService', authInterceptorService);
EReceptionApp.factory('manageService', manageService);

EReceptionApp.filter('getById', function() {
    return function(input, id) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (input[i].id == id) {
                return input[i];
            }
        }
        return null;
    }
});

var configFunction = function ($routeProvider) {
    $routeProvider.
        when('/reception', {
            templateUrl: '/Reception',
            controller: ReceptionController
        })
        .when('/manage', {
            templateUrl: '/Manage',
            controller: ManageController
        })
        .when('/login', {
            templateUrl: '/Account/Login',
            controller: LoginController
        })
        .when('/register', {
            templateUrl: '/Account/Register',
            controller: RegisterController
        });
}
configFunction.$inject = ['$routeProvider', '$httpProvider'];
EReceptionApp.config(configFunction);

EReceptionApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

EReceptionApp.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
