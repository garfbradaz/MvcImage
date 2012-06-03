$(document).ready(function () {
	function () {
	     $('#whats-new').show();
	});
});

$(function () {
setInterval(
	function () {
		$.ajaxSetup({ cache: false });
			$.getJSON("/Home/JSONWhatsNew", function (JSONData) {
				var div = $('<div>').attr({ id: "whats-new" }).appendTo('p.home-para');
				$('<ul>').attr({ id: "whats-new" }).appendTo('div#whats-new');

				$.each(JSONData, function (i, item) {


					var img = $('<img/>').attr("src", "/ImagePreview/GetImage/" + JSONData[i].UniqueKey);
					var anc = $('<a>').attr("href", "/Property/Details/" + JSONData[i].PropertyId)
								  .append(img);
					var lst = $('<li>').addClass("whats-new")
								   .append("<h3>" + JSONData[i].PropertyType + "</h3>")
								   .append('<br>Type: ' + JSONData[i].PropertyType)
								   .append('<br>Created: ' + JSONData[i].CreatedDate)
								   .append('<br>Area: ' + JSONData[i].Area + '<br>')
								   .append(anc)
								   .appendTo('ul#whats-new');

				});

		});
	},
	3000);
});