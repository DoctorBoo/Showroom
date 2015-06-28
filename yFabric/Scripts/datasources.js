
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
				//type: "odata",
				read: {
				    url: "api/tables",
				    //url: function(options) {
				    //    return kendo.format("odata/Restaurants", options);
				    //},
					data: function () {						
						return {
							take: 100
						}
					},
					type: "get",
					dataType: "json",
					contentType: "application/json; charset=utf-8"
				},
				update: {
					url: "api/tables/updaterestaurant",
					//data:JSON.stringify({ model: model}),
					type: "post",
					dataType: "json",
					contentType: "application/json;charset=utf-8"// "application/x-www-form-urlencoded;  charset=UTF-8" //"application/octet-stream"// "application/json; charset=utf-8"
				},
				parameterMap: function (options, operation) {
					var paramMap = kendo.data.transports.odata.parameterMap(options);

					// note that you may need to merge that postData with the options send from the DataSource
					globals.consoleLog(options);
					
					if (operation !== "read" && options.models) {
					    //return options.models;
					    return kendo.stringify(options.models);
						return kendo.stringify({ "restaurant": kendo.stringify(options.models[0]) });// { models: kendo.stringify(options.models) }; //return kendo.stringify(model);
					}
					return options;//JSON.stringify(options)
				}  
			},
			error: function (e) {
				/* the e event argument will represent the following object:
		
				{
					errorThrown: "custom error",
					errors: ["foo", "bar"]
					sender: {... the Kendo UI DataSource instance ...}
					status: "customerror"
					xhr: null
				}
		
				*/
				globals.consoleLog("Errors: ",e);
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
			//autoSync: true,
			schema: {
				model: {
					id: "id",
					fields: {
						id: { editable: false, nullable: true },
						Name: { validation: { required: false } },
						Time: { validation: { required: false } },
						location: { validation: { required: false } },
						cuisine: { validation: { required: false } },
						Graded: { validation: { required: false } }
					}
				}
                //,
				//parse: function (response) {
				//	var restaurants = [];
				//    try {
				//        for (var i = 0; i < response.length; i++) {
				//            var address = JSON.parse(response[i].elts[1].value);
				//            var restaurant = {
				//                id: response[i].elts[0].value
                //                                            .replace(new RegExp('ObjectId', 'g'), '') //
                //                                            .replace(new RegExp('\[()]', 'g'), '')
                //                                            .replace(new RegExp('\["]', 'g'), ''),
				//                name: JSON.parse(response[i].elts[5].value),
				//                location: address.street + ' ' + address.zipcode + ' ' + address.building
				//            };
				//            restaurant[response[i].elts[3].name] = JSON.parse(response[i].elts[3].value);
				//            restaurant[response[i].elts[4].name] = JSON.parse(response[i].elts[4].value
                //                .replace(new RegExp('ISODate', 'g'), '') //2014-07-18T00:00:00Z
                //                .replace(new RegExp('\[()]', 'g'), ''));
				//            restaurant['Time'] = new Date();
				//            restaurants.push(restaurant);
				//        }
				//    } catch (e) {

				//    }
				//	return restaurants;
				//}
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
			editable: "incell",
			filterable: {
				mode: "row"
			},
			toolbar: ["create", "save"],
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
			{
				command: [{
					name: "edit",
					text: { edit: "Custom edit", cancel: "Custom cancel", update: "Custom update" }
				}]
			}
			],
			
		});
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
