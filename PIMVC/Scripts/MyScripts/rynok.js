function Rynok(options){
	this.options=options;
	this.url;
	this.urlProp;

	this.readOptions = function (options) {
		this.url = options.url;
		this.urlProp = options.urlProp;
	}

	this.init = function () {
		this.readOptions(this.options);
		var self = this;
		$("a.showChart").click(function (elem) {
			dateStart = $(elem.currentTarget).attr('dateStart');
			dateEnd = $(elem.currentTarget).attr('dateEnd');
			self.createObjectChart("#dlgChart", dateStart, dateEnd);
			$("#dlgChart").dialog("option", "title", dateStart + " - " + dateEnd);
			$("#dlgChart").dialog({
				width: 800,
				height: 500,
				title: dateStart + " - " + dateEnd,
				close: function () {
					$("#dlgChart").html("");
				}
			});
			
		});
	}

	this.createObjectChart = function (div, dateStart, dateEnd) {
		
		var html = '<object data="data:application/x-silverlight," type="application/x-silverlight-2" width="100%" height="98%">';
		html += '<param name="source" value="' + this.url + '" />';
		html += '<param name="background" value="White" />';
		html += '<param name="windowless" value="true" />';
		html += '<param name="autoUpgrade" value="false" />';
		html += '<param name="initParams" value="propFileName=' + this.urlProp + ',mode=visiblox,target=rynokChart,dateStart=' + dateStart + ',dateEnd=' + dateEnd + ',interval=1m,recorded=true" />';
		html += '<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">';
		html += '<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Получить сильверлайт" style="border-style: none" /></a>';
		html += '</object>'
		$(div).html(html);
	}

	
}