var IndexController = function ($scope, $location, authService) {
    $scope.models = {
        helloAngular: 'Электронная приёмная'
    };
    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;
}

// The $inject property of every controller (and pretty much every other type of object in Angular) needs to be a string array equal to the controllers arguments, only as strings
IndexController.$inject = ['$scope','$location', 'authService'];