// site.js

(function () {

	//var ele = $("#username");
	//ele.text("bla bla");

	//var main = $("#main");
	//main.on("mouseenter", function () {
	//	main.style = "background-color: #888;";
	//});

	//main.on("mouseleave", function () { 
	//	main.style = "";
	//});

	//var menuitems = $("ul.menu li a");
	//menuitems.on("click", function () {
	//	var me = $(this);
	//	alert(me.text());
	//});

	var $sidebarWrapper = $("#sidebar,#wrapper");

	$("#sidebarToggle").on("click", function () {
		$sidebarWrapper.toggleClass("hide-sidebar");
		if ($sidebarWrapper.hasClass("hide-sidebar")) {
			$(this).text("Show sidebar");
		} else {
			$(this).text("Hide sidebar");
		}
	});

})();