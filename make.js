// see examples below

function add(x, y) { return x + y; }

function mul(x, y) { return x * y; }

function make()
{
	var storage = [];
	if (arguments.length == 0) storage.push(0);
	for(var i = 0; i < arguments.length; ++i)
	{
		if (typeof(arguments[i]) == 'function')
		{
			var result = storage[0] || 0;
			for (var j = 1; j < storage.length; ++j)
				result = arguments[i](result, storage[j]);
			return result;
		}
		if(typeof(arguments[i]) != 'number') storage.push(0);
		else storage.push(arguments[i]);
	}
	
	var todo = function()
	{
		if (arguments.length == 0) storage.push(0);
		for(var i = 0; i < arguments.length; ++i)
		{
			if (typeof(arguments[i]) == 'function')
			{
				var result = storage[0];
				for (var j = 1; j < storage.length; ++j)
					result = arguments[i](result, storage[j]);
				return result;
			}
			if(typeof(arguments[i]) != 'number') storage.push(0);
			else storage.push(arguments[i]);
		}
		return todo;
	}


	todo.toString = function()
		{
			var result = "";
			for(var i = 0; i < storage.length; ++i)
				result += " " + storage[i];
			return result;
		}
	return todo;
}

// try it

alert(make("hello"));
alert(make(1, 5, 7));
alert(make(1, 5, 7, add));
alert(make()()()(2, 3)()(5, add));
alert(make(1,5,6)(2,1)(4,5)(mul));




