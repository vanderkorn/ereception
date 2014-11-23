var authInterceptorService = function ($q, $injector,$location) {
    return {
        //response: function (response) {
        //    if (response.status === 401) {
        //        console.log("Response 401");
        //    }
        //    return response || $q.when(response);
        //},
        responseError: function (rejection) {
            if (rejection.status === 401) {
                console.log("Response Error 401", rejection);
                var authService = $injector.get('authService');
               // authService.logOut();
                $location.path('/login').search('returnUrl', $location.path());
            }
            return $q.reject(rejection);
        }
    }
};

authInterceptorService.$inject = ['$q', '$injector', '$location'];