var offsetfromcursorX = 12;
var offsetfromcursorY = 10;
var offsetdivfrompointerX = 10;
var offsetdivfrompointerY = 13;

document.write('<div id="dhtmltooltip"></div>');
document.write('<img id="dhtmlpointer" src="style/images/tooltiparrow.gif">');

var ie = document.all;
var ns6 = document.getElementById && ! document.all;
var enabletip = false;
var forimg = false;
var  tipobj = document.getElementById("dhtmltooltip");
var pointerobj = document.getElementById("dhtmlpointer");
function ietruebody() {
	return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body;
}

String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

function showtip(thetext, thewidth, thecolor) {
	if (ns6 || ie) {
		if (typeof thewidth != "undefined")
			tipobj.style.width = thewidth + "px";
		if (typeof thecolor != "undefined" && thecolor != "")
			tipobj.style.backgroundColor = thecolor;
		thetext = thetext.trim();
		var arr = thetext.split(" ");
		for(i=0;i<arr.length;i++)
			if(arr[i].length>=40)
				thetext=thetext.replace(arr[i],arr[i].substr(0,40)+"...");
		tipobj.innerHTML = thetext;		
		enabletip = true;
		return false;
	}
}
function showtipimg(thetext, thewidth, thecolor) {
	if (ns6 || ie) {
		tipobj.style.width = "auto";
		tipobj.style.opacity = 1;
		if (typeof thecolor != "undefined" && thecolor != "")
			tipobj.style.backgroundColor = thecolor;
		thetext = thetext.trim();
		
		tipobj.innerHTML = thetext;		
		enabletip = true;
		forimg = true;
		return false;
	}
}
function positiontipimg(e) {
	if (enabletip) {
		alert('a');
		var nondefaultpos = false;
		var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
		var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
		
		var winwidth = ie && ! window.opera ? ietruebody().clientWidth : window.innerWidth - 20;
		var winheight = ie && ! window.opera ? ietruebody().clientHeight : window.innerHeight - 20;

		var rightedge = ie && ! window.opera ? winwidth - event.clientX - offsetfromcursorX : winwidth - e.clientX - offsetfromcursorX;
		var bottomedge = ie && ! window.opera ? winheight - event.clientY - offsetfromcursorY : winheight - e.clientY - offsetfromcursorY;

		var leftedge = (offsetfromcursorX < 0) ? offsetfromcursorX * (- 1) : - 1000;

		if (rightedge < tipobj.offsetWidth) {
			tipobj.style.left = curX - tipobj.offsetWidth + "px";
			nondefaultpos = true;
		}
		else if (curX < leftedge)
			tipobj.style.left = "5px";
		else {
			tipobj.style.left = curX + offsetfromcursorX - offsetdivfrompointerX + "px";
			pointerobj.style.left = curX + offsetfromcursorX + "px";
		}

		if (bottomedge < tipobj.offsetHeight) {
			tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px";
			nondefaultpos = true;
		}
		else {
			tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px";
			pointerobj.style.top = curY + offsetfromcursorY + "px";
		}

		tipobj.style.visibility = "visible";

		if (! nondefaultpos)
			pointerobj.style.visibility = "visible";
		else
			pointerobj.style.visibility = "hidden";
	}
}
function positiontip(e) {
	if (enabletip) {
		var nondefaultpos = false;
		var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
		var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
		
		var winwidth = ie && ! window.opera ? ietruebody().clientWidth : window.innerWidth - 20;
		var winheight = ie && ! window.opera ? ietruebody().clientHeight : window.innerHeight - 20;

		var rightedge = ie && ! window.opera ? winwidth - event.clientX - offsetfromcursorX : winwidth - e.clientX - offsetfromcursorX;
		var bottomedge = ie && ! window.opera ? winheight - event.clientY - offsetfromcursorY : winheight - e.clientY - offsetfromcursorY;

		var leftedge = (offsetfromcursorX < 0) ? offsetfromcursorX * (- 1) : - 1000;
		
		
			if (rightedge < tipobj.offsetWidth) {
				tipobj.style.left = curX - tipobj.offsetWidth + "px";
				nondefaultpos = true;
			}
			else if (curX < leftedge)
				tipobj.style.left = "5px";
			else {
				tipobj.style.left = curX + offsetfromcursorX - offsetdivfrompointerX + "px";
				pointerobj.style.left = curX + offsetfromcursorX + "px";
			}
		if(!forimg){
			if (bottomedge < tipobj.offsetHeight) {
				tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px";
				nondefaultpos = true;
			}
			else {
				tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px";
				pointerobj.style.top = curY + offsetfromcursorY + "px";
			}
		}
		else{
			if (bottomedge < tipobj.offsetHeight/2.5) {
				tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px";
				nondefaultpos = true;
			}
			else {
				tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px";
				pointerobj.style.top = curY + offsetfromcursorY + "px";
			}
		}
		tipobj.style.visibility = "visible";

		if (! nondefaultpos)
			pointerobj.style.visibility = "visible";
		else
			pointerobj.style.visibility = "hidden";
	}
}

function hidetip() {
	if (ns6 || ie) {
		enabletip = false;
		tipobj.style.visibility = "hidden";
		pointerobj.style.visibility = "hidden";
		tipobj.style.left = "-1000px";
		tipobj.style.backgroundColor = '';
		tipobj.style.width = '';
	}
}
document.onmousemove = positiontip;
