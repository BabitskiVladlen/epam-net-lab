var JsonTree = JsonTree || {};

JsonTree.json = null;

JsonTree.start =
	function()
	{
		if (JsonTree.json == null) return;
		var node = document.getElementById("node0");
		var body  = node.getElementsByClassName("node-body")[0];
		JsonTree.get(JsonTree.json, body);
		JsonTree.show();
	}

JsonTree.show =
	function()
	{
		var node = event.target.parentNode || event.srcElement.parentNode;
		JsonTree.showElement(node);
		var innerBodies = node.getElementsByClassName("node-body")[0].getElementsByClassName("node-body");
		for (var i = 0; i < innerBodies.length; ++i)
			innerBodies[i].style.display = "none";
	}

JsonTree.showElement =
	function(node)
	{
		node.onclick = function(e) { JsonTree.hide(); e.stopPropagation(); }
		var head = node.getElementsByClassName("node-head")[0];
		var sign = head.childNodes[0];
		sign.innerHTML = " – ";
		var body = node.getElementsByClassName("node-body")[0];
		body.style.display = "block";
	}

JsonTree.hide =
	function()
	{
		var node = event.target.parentNode || event.srcElement.parentNode;
		JsonTree.hideElement(node);
		innerNodes = node.getElementsByClassName("node");
		for (var i = 0; i < innerNodes.length; ++i)
			JsonTree.hideElement(innerNodes[i]);
	}

JsonTree.hideElement =
	function(node)
	{
		node.onclick = function(e) { JsonTree.show(); e.stopPropagation(); }
		var head = node.getElementsByClassName("node-head")[0];
		var sign = head.childNodes[0];
		sign.innerHTML = " + ";
		var body = node.getElementsByClassName("node-body")[0];
		body.style.display = "none";
	}

JsonTree.get =
	function(obj, parentNode)
	{
		if (obj == null || typeof obj == "undefined") return;
		var i = 0;
		for (var prop in obj)
		{
			var body = JsonTree.prepareNode(parentNode, prop);
			var p = obj[prop];

			// []
			if (p instanceof Array)
			{
				
				for (var i = 0; i < p.length; ++i)
				{
					if (typeof p[i] == "object")
					{
						JsonTree.get(p[i], JsonTree.prepareNode(body, "{ }"));
					}
					else
						body.appendChild(JsonTree.prepareValue(p[i]));	
				}	
			}

			// { }
			else if (typeof p == "object")
				JsonTree.get(p, body);

			// value
			else
				body.appendChild(JsonTree.prepareValue(p));
		}
	}

JsonTree.validate =
	function()
	{	
		var jsonText = document.getElementById("json-text").value;

		// get node and body
		var node = document.getElementById("node0");
		node.onclick = function(e) { JsonTree.start(); e.stopPropagation(); };
		var body  = node.getElementsByClassName("node-body")[0];
		body.innerHTML = "";

			// head
			var oldHead = node.getElementsByClassName("node-head")[0];
			node.removeChild(oldHead);
			var head = document.createElement("h2");
			head.className = "node-head";
			node.insertBefore(head, body);

		try
		{
			JsonTree.json = JSON.parse(jsonText);
		}
		catch (e)
		{ 
			head.style.color = "red";
			head.innerHTML = "Error";
			return;
		}
			// prepare head
			JsonTree.prepareHead(head, "{ }");
	}

JsonTree.prepareNode =
	function(parentNode, headValue)
	{
		// node
		var node = document.createElement("div");
		node.className = "node";
		node.onclick =  function(e) { JsonTree.show(); e.stopPropagation(); }
		parentNode.appendChild(node);

		//head
			var head = document.createElement("h2");
			head.className = "node-head";
			node.appendChild(head);
			JsonTree.prepareHead(head, headValue);

		// body
			var body = document.createElement("div");
			body.className = "node-body";
			node.appendChild(body);
			return body;
	}

JsonTree.prepareHead =
	function(head, innerText)
	{
		var sign = document.createElement("span");
		sign.className = "sign";
		sign.innerHTML = " + ";
		head.appendChild(sign);
		head.innerHTML += innerText;
	}

JsonTree.prepareValue =
	function(innerValue)
	{
		var val = document.createElement("div");
		val.className = "val";
		val.innerHTML = innerValue;
		return val;
	}
