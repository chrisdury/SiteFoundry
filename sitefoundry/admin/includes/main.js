
function showNode(nodeID) {
	//$('nodeTemplateHolder').src = 'nodeDisplay.aspx?nodeID=' + nodeID;
	location.href = 'cms.aspx?nodeID=' + nodeID;
}


function hideElement(el) {
	if ($(el) != null) Element.hide(el);
}


function toggleTools() {
	Element.toggle('articleToolsTable');
}



function deleteConfirm(t) {
	return confirm('Are you sure that you want to delete '+t);
}


/* Window Openers */
function openWin(url,w,h,c) {
	l=screen.width/2-w/2;
	t=screen.height/2-h/2;
	if (typeof myWindow == "object") {
		myWindow.close();	
	}
	myWindow = window.open(url + '?' + c,'contentadminwindow','top='+t+',left='+l+',height='+h+',width='+w+',noresize,scrollbars=,status=yes');
}
function deleteWarning(item) {
	return confirm('Are you sure you want to delete ' + item + '\n\nThis operation is permanent and there is no undo.  Please be sure before clicking "yes" or "ok". \n');
}


var updateParent = true;


function closeEditWindow(skipRefresh) {
	if (!skipRefresh) {
		if (window.opener.document.forms[0].length > 1)
			window.opener.document.forms[0].submit();
		else
			window.opener.document.location.href = window.opener.document.location.href;
	}
	window.close();
}

function g(where) {
	location.href = where;
}



var currentMenuOpen;
function toggleNodeMenu(obj,id) {
	hideLBUtils();
	var o = document.getElementById("LB_MENU_" + id);
	currentMenuOpen = o;	
	if (o.style.visibility != 'visible') {
		o.style.visibility = 'visible';
		//alert(obj.offsetWidth);
		o.style.left = obj.offsetLeft + obj.offsetWidth;
		//o.style.left = 0;
		//o.style.marginLeft = 10;
		//o.style.top = obj.offsetTop;
	}
}
function hideLBUtils() {
	if (currentMenuOpen != null) currentMenuOpen.style.visibility = 'hidden';
}




function getObject(layerId) {
	if (document.all) {
		obj = document.all(layerId);
	} else if (document.layers) {
		obj = document.layers[layerId];
	} else if (document.getElementById) {
		obj = document.getElementById(layerId);
	}
	return obj;
}



// getCookie function
//   gets the menu cookie
function getCookie(cookieName) {
	var cookie;
	cookie = "" + document.cookie;
	var start = cookie.indexOf(cookieName);
	if (cookie == "" || start == -1) 
		return "";
	var end = cookie.indexOf(';',start);
	if (end == -1)
		end = cookie.length;
	return unescape(cookie.substring(start+cookieName.length + 1,end));
}

// setCookie function
//   sets the menu cookie
function setCookie(cookieName, value, expires) {
	cookieInfo = cookieName + "=" + escape(value) + ";path=/"
	document.cookie = cookieInfo;  
	return document.cookie;
}

// getExpirationDate function
//   gets the menu cookie from the browser
function getExpirationDate(days){
	today = new Date();
	today.setTime(Date.parse(today) + (days * 60 * 60 * 24 * 100));
	return  today.toUTCString();
}




/* Resources */

function selectCheckboxes(state,name) {
	var f = document.resources[name];
	for(i=0;i<f.length;i++) {
		switch(state) {
			case 'all':
				f[i].checked = true;			
			break;
			
			case 'none':
				f[i].checked = false;
			break;
			
			case 'invert':
				f[i].checked = (f[i].checked) ? false : true;
			break;
		}		
	}
}



/* Image & Document Library */

var activeTile = null;
var activeLink = null;
function setActiveTile(item) {
	if (activeTile != null)
		activeTile.className = 'imgTile normal';
	
	
	activeTile = item;
	activeTile.className = 'imgTile selected';		
}


function setActiveLink(item) {
	if (activeLink != null)
		activeLink.className = 'normal';
	
	activeLink = item;
	activeLink.className = 'selected';

}

var myWindow;
function openWindow(url,w,h) {
	l=screen.width/2-w/2;
	t=screen.height/2-h/2;
	if (typeof myWindow == "object") {
		myWindow.close();	
	}
	myWindow = window.open(url,'contentadminwindow','top='+t+',left='+l+',height='+h+',width='+w+',noresize,scrollbars=yes,status=yes');
}

function addNode() {
	url = 'helpers/nodeEdit.aspx?action=add';
	openWindow(url,600,450);
}

function editNode(id) {
	url = 'helpers/nodeEdit.aspx?action=edit&id='+id;
	openWindow(url,600,450);
}

function deleteNode(id) {
	url = 'helpers/nodeEdit.aspx?action=delete&id='+id;
	openWindow(url,600,450);
}


/* Assorted Utils */

function formatString() {
	targetString = arguments[0];
	if (arguments.length > 1) {
		for(i=1;i<arguments.length;i++) {
			targetString = targetString.split('{' + eval(i-1) + '}').join(arguments[i]);			
		}
	} else {
		alert("missing arguments!");
	}
	return targetString;
}











