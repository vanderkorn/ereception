var ManageController = function ($scope, $routeParams, $location, manageService) {

    $scope.failure = false;
   
    $scope.becomeminister = function () {
        var result = manageService.becomeminister();
        result.then(function (result) {
            if (result.success) {
                $location.path('/reception');
            } else {
                $scope.failure = true;
            }
        });
    }

    $scope.becomesecretary = function () {
        var result = manageService.becomesecretary($scope.reception);
        result.then(function (result) {
            if (result.success) {
                $location.path('/reception');
            } else {
                $scope.failure = true;
            }
        });
    }
}

ManageController.$inject = ['$scope', '$routeParams', '$location', 'manageService'];