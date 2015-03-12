

var ajs_task = ajs_task || { };
ajs_task.app = angular.module("ajs_task", ["ngResource"]);

ajs_task.app.controller("SearchCtrl", ["$scope", "GitHub",
	function SearchCtrl($scope, GitHub)
	{
		$scope.query = "";
		$scope.rSize = 1000;
		$scope.rStars = 0;
		$scope.rForks = 0;
		$scope.limit = 10;
		$scope.order = $scope.repos ? $scope.repos.language : "";

		var isAsc = true;
		$scope.asc =
			function()
			{
				isAsc ? $scope.order = -"language" : $scope.order = "language";
				isAsc = !isAsc;
			};

		$scope.executeSearch =
			function executeSearch()
			{
				GitHub.get({ q: $scope.query + " size:<=" + $scope.rSize
					+ " stars:>=" +  $scope.rStars + " forks:>=" + $scope.rForks },
					function(data)
					{
						$scope.repos = data.items;
					});	
			};
	}]);

