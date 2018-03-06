// site.js

(function () {

	var $sidebarWrapper = $("#sidebar,#wrapper");
	var $icon = $("#sidebarToggle i.fa");

	$("#sidebarToggle").on("click", function () {
		$sidebarWrapper.toggleClass("hide-sidebar");
		if ($sidebarWrapper.hasClass("hide-sidebar")) {
			$icon.removeClass("fa-angle-left");
			$icon.addClass("fa-angle-right");
		} else {
			$icon.addClass("fa-angle-left");
			$icon.removeClass("fa-angle-right");
		}
	});
})();