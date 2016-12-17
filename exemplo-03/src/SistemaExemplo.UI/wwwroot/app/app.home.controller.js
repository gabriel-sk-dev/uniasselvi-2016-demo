(function () {
    'use strict';

    angular
        .module('app')
        .controller('appHome', appHomeController);

    appHomeController.$inject = ['$http', '$scope'];

    function appHomeController($http, $scope) {
        var vm = this;        
        vm.employee = {};
        vm.save = save;
        activate();

        function activate() {
            vm.employee = {
                "Name": "Gabriel",
                "CivilState": "Casado",
                "Salary": 4000
            }
        }

        function save() {
            console.log('Ok');
        }

        function conectaCom(equipamento) {
            //var promise = $http.post("http://localhost:5000/api/Equipamento/Conecta",
            //                            JSON.stringify(equipamento),
            //                            {
            //                                headers: { 'Content-Type': 'application/json' }
            //                            });
            //promise.then(
            //    function (response) {
            //        console.log(equipamento);
            //    },
            //    function (error) {
            //        console.log(error);
            //    });
        }        
    }
})();
