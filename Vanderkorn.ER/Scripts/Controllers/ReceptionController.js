var ReceptionController = function ($scope, $http, Hub, $filter) {
    
    var hub = new Hub('ReceptionHub', {

        //client side methods
        listeners: {
            'updateAppeal': function (data) {
                console.log(data);
                console.log(data.id);

                var found = $filter('getById')($scope.appeals, data.id);
                console.log(found);
                if (!found) {
                    $scope.appeals.push(data);
                    console.log("added item");
                    $scope.$apply();
                } else {
                    console.log("update item");
                    found.isExecuted = data.isExecuted;
                    found.decisionType = data.decisionType;
                    found.appealType = data.appealType;
                    found.modifyDate = data.modifyDate;
                    found.comment = data.comment;
                    $scope.$apply();
                }
            }
        },

        //handle connection error
        errorHandler: function (error) {
            console.error(error);
        },

        //specify a non default root
        rootPath: '/signalr'

    });
    //      hub.connect();

    $scope.title = "loading question...";
    $scope.appeals = [];
    $scope.working = false;
    $scope.master = {};
    $scope.appealtypes = [
         { "name": 'Звонок', 'id': 0 },
        { "name": 'Посетитель', 'id': 1 }
    ];
    $scope.decisionTypes = [
         { "name": 'Не выставлен', 'id': 0 },
         { "name": 'Принять', 'id': 1 },
         { "name": 'Отказать', 'id': 2 },
         { "name": 'Перезвонить', 'id': 3 }
    ];
    $scope.execute = function (isExecuted, appealId) {

        console.log(appealId);
        console.log(isExecuted);
        $http.put('/api/appeals/execute/' + appealId, { 'isExecuted': isExecuted }).success(function (data, status, headers, config) {

        }).error(function (data, status, headers, config) {
            console.log("error");
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });
    }
    $scope.decide = function (itemId, comment, appealId) {
        console.log(itemId);
        console.log(comment);
        console.log(appealId);
        $http.put('/api/appeals/decide/' + appealId, { 'comment': comment, 'decisionType': itemId }).success(function (data, status, headers, config) {

        }).error(function (data, status, headers, config) {
            console.log("error");
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });
    }



    $scope.create = function (newappeal) {

        console.log(newappeal.description);
        console.log(newappeal.appealtype);
        $scope.master = angular.copy(newappeal);
        $http.post('/api/appeals', { 'description': newappeal.description, 'appealType': newappeal.appealtype }).success(function (data, status, headers, config) {
            $scope.appeals.push(data);
        }).error(function (data, status, headers, config) {
            console.log("error");
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    };

    $scope.reset = function () {
        $scope.newappeal = angular.copy($scope.master);
    };

    $scope.reset();

    $scope.getAppeals = function () {
        $scope.appeals = [];
        $http.get("/api/appeals").success(function (data, status, headers, config) {
            $scope.appeals = data;
        }).error(function (data, status, headers, config) {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });
    };

}

ReceptionController.$inject = ['$scope', '$http', 'Hub', '$filter'];