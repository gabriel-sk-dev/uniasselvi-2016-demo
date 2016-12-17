(function () {
    'use strict';

    angular.module('app', [        
        'ui.router',
        'ngMaterial',        
    ]);

    angular.module('app')
        .config(appConfig)
    appConfig.$inject = ['$stateProvider', '$qProvider'];
    function appConfig($stateProvider, $qProvider) {
        $qProvider.errorOnUnhandledRejections(false);

        var homeState = {
            name: 'home',
            url: '/home',
            controllerAs: 'vm',
            controller: 'appHome',
            templateUrl: 'app/app.home.html' 
        }
        $stateProvider.state(homeState);
    }

    angular.module('app')
        .run(appRun)
    appRun.$inject = ['$state'];
    function appRun($state) {
        $state.go('home');
    }

})();