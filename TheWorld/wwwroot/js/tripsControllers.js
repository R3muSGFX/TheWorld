// tripsController.js

(function () {
	"use strict";

	// getting the module
	angular.module("app-trips")
		.controller("tripsController", tripsController);

	function tripsController($http) {
		var vm = this;
		vm.trips = [];
		vm.newTrip = {};
		vm.errorMessage = "";
		vm.isBusy = true;

		$http
			.get("/api/trips")
			.then(function (response) {
				// success
				angular.copy(response.data, vm.trips);
			}, function (error) {
				// error
				vm.errorMessage = "Error retrieving data. Error: " + error;
			})
			.finally(function () {
				vm.isBusy = false;
			});

		vm.addTrip = function () {
			vm.isBusy = true;
			vm.errorMessage = "";

			$http.post("/api/trips", vm.newTrip)
				.then(function (response) {
					// success
					vm.trips.push(response.data);
					vm.newTrip = {};
				}, function (error) {
					// error
					vm.errorMessage = "Failed to save the new trip. Error: " + error;
				})
				.finally(function () {
					vm.isBusy = false;
				});
		};	
	}
})();