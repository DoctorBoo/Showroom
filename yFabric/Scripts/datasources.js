
var globals = (function ($) {
	var self = this;
	var verboseMode = true;

	var timeEditor = function (container, options) {
		$('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
				.appendTo(container)
				.kendoTimePicker({});
	}

	function dateTimeEditor(container, options) {
		$('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
				.appendTo(container)
				.kendoDateTimePicker({});
	}
	$(function () {
		
	})
	var writeTrace = function () {
		var max = arguments.length;
		var arg = arguments[0];
		if (verboseMode && max && console[arg] && typeof console[arg] === "function" && arg === 'trace') {
			console[arg]();
			return true;
		}
		return false;
	};


	var writeLog = function () {
		if (verboseMode && window.console) {
			for (var i = 0; i < arguments.length; i++) {
				var that = arguments[i];
				if (!writeTrace(that)) {
					console.log(that);
				}
			}
		}
	}
	return {
		consoleLog: writeLog,
		timeEditor: timeEditor
	}
}(jQuery));

var datasources = (function () {
	var viewModel = kendo.observable({
		autoCompleteValue: null,
		grades: [
			{ name: "A", value: "A" },
			{ name: "B", value: "B" },
			{ name: "C", value: "C" },
			{ name: "D", value: "D" },
			{ name: "E", value: "E" }
		]
	});
	var readDatasource = new kendo.data.DataSource({
			transport: {
				read: {
					url: "api/tables",
					data: function () {						
						return {
							take: 1000
						}
					},
					type: "get",
					dataType: "json"
				},
				update:function (e) {
					// batch is enabled
					//var updateItems = e.data.models;
					// batch is disabled
					var updatedItem = e.data;

					// save the updated item to the original datasource
					$.post({
						url: "api/tables/UpdateRestaurant",
						data: JSON.stringify({restaurant: updatedItem}),
						dataType: "json"
					});

					// on success
					e.success(function (d) {
						globals.consoleLog(d);
					});
					// on failure
					e.error("XHR response", "status code", "error message");
				},
			},			
			pageSize: 10,
			batch: true,
			refresh: true,
			//schema: {
			//	model: {
			//		id: "elts",
			//		fields: {
			//			_Id: { editable: false, nullable: false }
			//		}
			//	}
			//},
			schema: {
				parse: function (response) {
					var restaurants = [];
					for (var i = 0; i < response.length; i++) {
						var address = JSON.parse(response[i].elts[1].value);
						var restaurant = {
							id: response[i].elts[0].value
														.replace(new RegExp('ObjectId', 'g'), '') //
														.replace(new RegExp('\[()]', 'g'), '')
														.replace(new RegExp('\["]', 'g'), ''),
							name: JSON.parse(response[i].elts[5].value),
							location: address.street + ' ' + address.zipcode + ' ' + address.building
						};
						restaurant[response[i].elts[3].name] = JSON.parse(response[i].elts[3].value);
						restaurant[response[i].elts[4].name] = JSON.parse(response[i].elts[4].value
							.replace(new RegExp('ISODate', 'g'), '') //2014-07-18T00:00:00Z
							.replace(new RegExp('\[()]', 'g'), ''));
						restaurant['Time'] = new Date();
						restaurants.push(restaurant);
					}
					return restaurants;
				}
			}
	});
	var logContentTemplate = function (content) {
		globals.consoleLog(content);
	};
	var openDetails = function () {
		//$(this).show();
		$("#details").kendoWindow();
		var dialog = $("#dialog").data("kendoWindow");
		setTimeout(function () {
			dialog.close();
		}, 3000);
	};
	var closeDetails = function () {
		//$(this).show();
		$("#details").kendoWindow();
		var dialog = $("#dialog").data("kendoWindow");
		setTimeout(function () {
			dialog.close();
		}, 100);
	};
	$(function () {
		var rows = $('tr[role="row"] td[data-id]').closest('tr');
		rows.hover(
			function () {
				openDetails();
			}, function () {
				closeDetails();
			}
		);
		
		$(".grid2").kendoGrid({
			dataSource: readDatasource,
			pageable: true,
			refresh: true,
			sortable: true,			
			editable: {
				mode: "popup", //incell
				//template: kendo.template($("#popup-editor").html()),
				update:true
			},
			filterable: {
				mode: "row"
			},
			columns: [
			//{
			//	field: "id",
			//	title: "Id"
			//},
			{
				field: "name",
				
				title: "Name",
				attributes: {
					"class": "grades",
					"data-id": $("#dataTemplate").html()
				},
			},
			{
				field: "cuisine",
				title: "Cuisine"
			},
			//{
			//	field: "grades",
			//	title: "Grades",
			//	template: $("#listTemplate").html(),
			//	width: "300px"
			//},
			{
				field: "location",
				title: "Location"
			},
			{
				field: "Time",
				title: "Time",
				editor: globals.timeEditor,
				//template: $("#timeTemplate").html(),
				format: "{0: HH:mm:ss}"
			},
			{ command: "edit" }
			],
			
		})
		;
	//	$.ajax({ type: 'get', url: 'api/tables' }).done(function (d) {
	//		console.log(d); data = d;
	//	})
	})
	
	return {
		readDataSource: readDatasource,
		logContentTemplate: logContentTemplate,
		viewModel: viewModel,
		toString: function (ms) {
			var offset = 0;
			var d = new Date();
			var utc = ms - (d.getTimezoneOffset() * 60000);
			// create new Date object for different city
			// using supplied offset
			ms = new Date(utc + (3600000 * offset)).getTime();

			var x = ms / 1000;
			seconds = Math.round(x % 60);
			x /= 60;
			minutes = Math.round(x % 60);
			x /= 60;
			hours = Math.round( x % 24);
			var time = hours + ":" + (minutes < 10 ? "0" + minutes : minutes);//+ ":" + (seconds < 10 ? "0" + seconds : seconds)
			return time;
		}
	}
}())
