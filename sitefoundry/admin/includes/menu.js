var menuCookieName = "menucookie";

window.onload = function() { getMenuState(); }

window.onunload = function(){ saveMenuState();}


function showMenu(menuID) {
	window.setTimeout("toggleMenu('" + menuID + "')",1);
}

function toggleMenu(menuID) {
	obj = getObject(menuID);
	link = document.images["mo_" + menuID];
	//if (obj == null) return;
	if (obj.style.display=='') {
		obj.style.display='none';
		link.src = "images/closed.gif";
	} else {
		obj.style.display='';
		link.src = "images/open.gif";

	}
}


function saveMenuState() {
	var cookieData = new String();//"";
	var o = document.getElementsByTagName("ul");
	for(i=0;i<o.length;i++) {
		if (o[i].style.display == '' && o[i].className == 'menu') {
			//alert(o[i].id);
			cookieData += o[i].id + '|';
		}
	}
	cookieData = cookieData.substring(0,cookieData.length-1);
	//alert("write:" + cookieData);
	setCookie(menuCookieName,cookieData, getExpirationDate(1));
}

function getMenuState() {
	//alert("read: " + getCookie(menuCookieName));
	var menuItemsFromCookie = getCookie(menuCookieName).split('|');
	for (i = 0; i < menuItemsFromCookie.length; i++) {
		if (getObject(menuItemsFromCookie[i]) != null && menuItemsFromCookie[i] != 'menu_1') {
			showMenu(menuItemsFromCookie[i]);
		}
	}
	
}
