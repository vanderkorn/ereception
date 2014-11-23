var manageService = function ($http, $q) {

    var manageServiceFactory = {};

    var _becomeminister = function () {

        var deferredObject = $q.defer();

        $http.get('/Manage/BecomeMinister').
        success(function (data) {
            if (data == "True") {
                deferredObject.resolve({ success: true });
            } else {
                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

        return deferredObject.promise;

    };

    var _becomesecretary = function (receptionId) {

        var deferredObject = $q.defer();

        $http.post('/Manage/BecomeSecretary', { 'receptionId': receptionId }).
        success(function (data) {
            if (data == "True") {
                deferredObject.resolve({ success: true });
            } else {
                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

        return deferredObject.promise;

    };

    manageServiceFactory.becomeminister = _becomeminister;
    manageServiceFactory.becomesecretary = _becomesecretary;
    return manageServiceFactory;
};

manageService.$inject = ['$http', '$q'];