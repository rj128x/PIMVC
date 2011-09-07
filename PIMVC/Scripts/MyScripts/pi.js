function PI(options){
	this.options=options;
	
	this.readOptions=function(options){
	}

	this.init = function () {
		this.readOptions(this.options);
		var self = this;

		$(".tabs").tabs({
			show: function (event, ui) {
				self.fullSize();
			}
		});
		$(".accordion").accordion({
			autoHeight: false,
			animated: !jQuery.browser.msie
		});
		$("div#mainMenu").hide();


		$(".treeTable").treeTable({
			clickableNodeNames: true,
			initialState: 'collapsed'
		});
		$("tr:.opened", $(".treeTable")).each(function () {
			$(this).expand();
		});

		$(".displayOpen").click(function (elem) {
			url = $(elem.currentTarget).attr('url');
			self.createObjectDisplay(url);
		})

		$(".txtDate").datepicker({
			dateFormat: 'dd.mm.yy',
			firstDay: 1,
			duration: "",
			isRTL: false,
			changeMonth: true,
			changeYear: true,
			showOn: 'button',
			buttonImage: '/images/calendar.gif',
			buttonImageOnly: true,
			yearRange: "-2:+1"
		});



		$("a.buttonOpen").button({
			icons: { primary: "ui-icon-folder-open" }
		});

		$("a#btnMenu").click(function () {
			$("div#mainMenu").toggle();
			len = $("div#mainMenu:visible").length;
			if (len > 0) {
				$("#embedObject").hide();
			} else {
				$("#embedObject").show();
			}			

		});

		$(".resizable").resizable({
			distance: 20
		});

		$("tr", $("table.tooltip")).each(function () {
			str = $(".header:eq(0)", this).text();
			$(this).easyTooltip({
				content: str
			});
		});

		$(window).resize(function () {
			self.fullSize();
		});

		self.fullSize();
	}

	this.createObjectDisplay = function (url) {
		this.fullSize();
		$("div#embedObject").html("");
		html = "<object ID= 'Pbd1' WIDTH = '100%' HEIGHT  = '100%' CLASSID = 'CLSID:4F26B906-2854-11D1-9597-00A0C931BFC8'>";
		html += "<param name = 'DisplayUrl' value = '" + url + "'/>";
		html += '<param name="windowless" value="true" />';
		html += "</object>";
		//html = "<h1>hello</h1>";
		$("div#embedObject").html(html);
	}

	this.fullSize = function () {
		var wh = parseInt($(window).height());
		if ($.browser.opera) {
			wh = $.browser.version > "9.5" ? document.documentElement["clientHeight"] : wh;
		}
		$("div.resizeFull").each(function (i, e) {
			offset = $(e).offset();
			pos = $(e).position()
			$(e).css("height", wh - offset.top - pos.top+10);
		});
		//$("div.resizeFull").css("height", wh - 95);
	}
	
}