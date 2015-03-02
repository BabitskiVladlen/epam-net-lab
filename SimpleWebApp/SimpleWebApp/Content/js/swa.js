var SimpleWebApp = SimpleWebApp || {};

SimpleWebApp.setSizesOfContainers =
function()
{
	var documentHeight = window.innerHeight ? window.innerHeight :
		(document.documentElement.clientHeight ? document.body.offsetHeight : document.documentElement.clientHeight);
	var headerHeight = documentHeight * 0.2;
	var footerHeight = documentHeight * 0.1;
	document.getElementById("header").style.height = headerHeight + "px";
	document.getElementById("footer").style.height = footerHeight + "px";
	document.getElementById("main").style.height = documentHeight - (footerHeight + headerHeight + 10) + "px";
}

SimpleWebApp.getXMLHttpRequest =
function()
{
	var request = false;
	if (window.XMLHttpRequest)
		request = new XMLHttpRequest();
	else if (window.ActiveXObject)
	{
		// IE
		try
		{
			request = new ActiveXObject("Microsoft.XMLHTTP");
		}
		catch (exc)
		{
			request = new ActiveXObject("Msxml2.XMLHTTP");
		}
	}
	return request;
}

SimpleWebApp.indexResponse =
function()
{
    var main = document.getElementById("main");
    main.innerHTML = "";
	var h = document.createElement("h2");
	main.appendChild(h);
	var info_link = document.createElement("a");
	info_link.innerHTML = "Your information";
	h.appendChild(info_link);
	var info = document.createElement("div");
	info.setAttribute("id", "info");
	main.appendChild(info);
	SimpleWebApp.prepareInfoLink(info_link);
	var logout = document.getElementById("log_");
	logout.innerHTML = "Logout";
	SimpleWebApp.prepareLogout();
}

SimpleWebApp.loginPageResponse =
function(request)
{
    document.getElementById("main").innerHTML = request.responseText;
    SimpleWebApp.prepareVlaidation();
    SimpleWebApp.prepareLoginButton();
}

SimpleWebApp.getLoginPage =
function()
{
	event.preventDefault();
	var request = SimpleWebApp.getXMLHttpRequest();
	if (!request) return;
	request.open("GET", "login.jhtm", true);
	request.onreadystatechange =
	function()
	{
		if ((request.readyState == 4) && (request.status == 206))
			SimpleWebApp.loginPageResponse(request);
		if (request.status == 404)
			SimpleWebApp.indexResponse();
	}
	request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
	request.send(null);
}

SimpleWebApp.validationResponse =
function(request)
{
	document.getElementById("validation").innerHTML = request.responseText;
}

SimpleWebApp.validate =
function ()
{
	var request = SimpleWebApp.getXMLHttpRequest();
	if (!request) return;
	var username = document.forms["login"].elements["username"].value;
	var params = "username=" + encodeURIComponent(username);
	request.open("POST", "validate.jhtm", true);
	request.onreadystatechange =
	function()
	{
		if ((request.readyState == 4) && (request.status == 200))
			    SimpleWebApp.validationResponse(request);
	}
	request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
	request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	request.send(params);
}

SimpleWebApp.loginFormResponse =
function(request)
{
	document.getElementById("main").innerHTML = request.responseText;
	SimpleWebApp.prepareVlaidation();
	SimpleWebApp.prepareLoginButton();
}

SimpleWebApp.login =
function()
{
	event.preventDefault();
	var request = SimpleWebApp.getXMLHttpRequest();
	if (!request) return;
	var username = document.forms["login"].elements["username"].value;
	var password = document.forms["login"].elements["password"].value;
	var params = "username=" + encodeURIComponent(username) + "&password=" + encodeURIComponent(password);
	request.open("POST", "login.jhtm", true);
	request.onreadystatechange =
		function()
		{
			if (request.readyState == 4)
			{
			    if (request.status == 401)
			        SimpleWebApp.loginFormResponse(request);
			    if ((request.status == 206) || (request.status == 404))
			        SimpleWebApp.indexResponse();
			}
		}
	request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.send(params);
}

SimpleWebApp.jsonResponse =
function(request)
{
    var jsonObj = JSON.parse(request.responseText);
    var info = document.getElementById("info");
    info.innerHTML = "";
    var ul = document.createElement("ul");
    ul.setAttribute("id", "info-list");
    info.appendChild(ul);
    var li = document.createElement("li");
    li.setAttribute("id", "info-list-item1");
    li.innerHTML = jsonObj.name;
    ul.appendChild(li);
    li = document.createElement("li");
    li.setAttribute("id", "info-list-item2");
    li.innerHTML = jsonObj.surname.surname1 + " " + jsonObj.surname.surname2;
    ul.appendChild(li);
    li = document.createElement("li");
    li.setAttribute("id", "info-list-item2");
    for (var i = 0; i < jsonObj.phones.length; ++i) {
        li.innerHTML += jsonObj.phones[i];
        if (i != jsonObj.phones.length - 1) li.innerHTML += ", ";
    }
    ul.appendChild(li);
}

SimpleWebApp.notFoundResponse =
function(request)
{
    document.getElementById("main").innerHTML = request.responseText;
    var login = document.getElementById("log_");
    login.innerHTML = "Login";
    SimpleWebApp.prepareLogin();
}

SimpleWebApp.getInfo =
function()
{
	event.preventDefault();
	var request = SimpleWebApp.getXMLHttpRequest();
	if (!request) return;
	request.open("GET", "info.jhtm", true);
	request.onreadystatechange =
		function()
		{
			if (request.readyState == 4)
			{
			    if (request.status == 206)
			        SimpleWebApp.jsonResponse(request);
			    if (request.status == 404)
			        SimpleWebApp.notFoundResponse(request);
			}
		}
	request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
	request.send(null);
}

SimpleWebApp.logoutResponse =
function()
{
    var main = document.getElementById("main");
    main.innerHTML = "";
    var h = document.createElement("h2");
    h.innerHTML = "Hello World!";
    main.appendChild(h);
    var login = document.getElementById("log_");
    login.innerHTML = "Login";
    SimpleWebApp.prepareLogin();
}

SimpleWebApp.logout =
function()
{
    event.preventDefault();
    var request = SimpleWebApp.getXMLHttpRequest();
    if (!request) return;
    request.open("GET", "logout.jhtm", true);
    request.onreadystatechange =
		function()
		{
			if (request.readyState == 4)
			{
			    if ((request.status == 206) || (request.status == 404))
                    SimpleWebApp.logoutResponse();
			}
		}
    request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    request.send(null);
}

SimpleWebApp.onReady =
function(isAuthentcicated)
{
    SimpleWebApp.setSizesOfContainers();
    window.onresize = SimpleWebApp.setSizesOfContainers;

    var log_ = document.getElementById("log_");
    if (isAuthentcicated) {
        SimpleWebApp.prepareLogout();
        SimpleWebApp.prepareInfoLink(document.getElementById("info_link"));
    }
    else
        SimpleWebApp.prepareLogin();

    SimpleWebApp.prepareVlaidation();
    SimpleWebApp.prepareLoginButton();
}

SimpleWebApp.prepareLogin =
function ()
{
    var login = document.getElementById("log_");
    login.setAttribute("href", "login.jhtm");
    login.onclick = function(e) { SimpleWebApp.getLoginPage(); }
}

SimpleWebApp.prepareLogout =
function()
{
    var logout = document.getElementById("log_");
    log_.setAttribute("href", "logout.jhtm");
    log_.onclick = function(e) { SimpleWebApp.logout(); }
}

SimpleWebApp.prepareInfoLink =
function(info_link)
{
    info_link.setAttribute("href", "info.jhtm");
    info_link.onclick = function(e) { SimpleWebApp.getInfo(); }
}

SimpleWebApp.prepareVlaidation =
function()
{
    var username = document.getElementById("username");
    if (username != null)
        username.onchange = function(e) { SimpleWebApp.validate() }
}

SimpleWebApp.prepareLoginButton =
function()
{
    var login_button = document.getElementById("login_button");
    if (login_button == null) return;
    login_button.setAttribute("href", "login.jhtm");
    if (login_button != null)
        login_button.onclick = function(e) { SimpleWebApp.login(); }
}