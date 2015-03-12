var ajs_task = ajs_task || { };

ajs_task.app.factory("GitHub", ["$resource",
  function($resource)
  {
  	return $resource("https://api.github.com/search/repositories");
  }]);

ajs_task.app.factory("GitHub", ["$resource",
  function($resource)
  {
  	return $resource("https://api.github.com/search/repositories");
  }]);