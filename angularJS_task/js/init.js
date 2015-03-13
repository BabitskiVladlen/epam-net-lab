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
		$scope.repos = [];
		$scope.order = $scope.repos.language || "";
		var flag = true;
		$scope.ordHdlr =
			function()
			{
				flag ? $scope.order = "-language" : $scope.order = "language";
				flag = !flag;
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

