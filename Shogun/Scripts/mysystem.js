/// <reference path="jquery-1.8.2.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
var common = (function ($, w, document) {
	var self;
	var verboseMode = true;

	$(function () {

		init();
		self.writeLog('doc ready.');
	});


	var init = function () {
		self = common;
		$('.row').slick({
			dots: true,
			infinite: false,
			speed: 500,
			slidesToShow: 1,
			slidesToScroll: 1
		});

		$.ajaxSetup({
			error: function (x, e) {
				if (x.status == 0) {
					self.writeErrorLog('You were disconnected from the server.\n Please check your networkconnection and server online status.');
				} else if (x.status == 404) {
					self.writeErrorLog('Requested URL not found.');
				} else if (x.status == 500) {
					self.writeErrorLog('Internel Server Error.');
				} else if (e == 'parsererror') {
					self.writeLog('Warning.\nParsing JSON Request failed. *************');
				} else if (e == 'timeout') {
					self.writeErrorLog('Request Time out.');
				} else {
					self.writeErrorLog('Unknow Error:\t' + x.responseText);
				}
			}
		});

		$(document).ajaxStart(function () {
			self.writeLog("Triggered ajaxStart handler.");
			//
		});
		$(document).ajaxComplete(function (event, request, settings) {
			self.writeLog('Request Complete.');
			//
		});

		//websocket check
		if (checkSupported()) {
			connect();
			//
		}
	}

	function preventDefault(e) {
		e.preventDefault();
	}

	function failed(e) {
		console.log("Error occured.");
	}

	function canceled(e) {
		console.log("Canceled by user.");
	}

	function displayError(serverData, error) {
		var value = 'No result.....';
		if ('result' in serverData) value = serverData.result;
		$('#result').html(value + ' - ' + error);
	}

	/*
				WEBSOCKET
		*/
	var wsUri = 'ws://echo.websocket.org/';
	var webSocket;

	function writeOutput(message) {
		var output = $("#divOutput");
		output.html(output.html() + '<br />' + message);
	}

	function checkSupported() {
		if (window.WebSocket) {
			writeOutput('WebSockets supported!');
			return true;
		} else {
			writeOutput('WebSockets NOT supported');
			$('#btnSend').attr('disabled', 'disabled');
			return false;
		}
	}

	function connect() {
		webSocket = new WebSocket(wsUri);
		webSocket.onopen = function (evt) { onOpen(evt) };
		webSocket.onclose = function (evt) { onClose(evt) };
		webSocket.onmessage = function (evt) { onMessage(evt) };
		webSocket.onerror = function (evt) { onError(evt) };
	}

	function doSend() {

		if (webSocket.readyState != webSocket.OPEN) {
			writeOutput("NOT OPEN: " + $('#txtMessage').val());
			return;
		}

		var table = $('#txtMessage').val();
		//$.getJSON('/tree/list/query', table)
		//    .done(function (serverData) {
		//    writeOutput("query result:" + serverData['query-result']);
		//})
		//    .fail(function (serverData, error) {
		//    writeOutput("error result:" + serverData.responseText);
		//    writeOutput("error result:" + error);
		//});
		$.get("/tree/list/query", table)
						.done(function (data) {
							var output = $("#divOutput");
							output.html(data);
						})
						.fail(function (data, error) {
							writeOutput("error result:" + serverData.responseText);
							writeOutput("error result:" + error);
						});
		writeOutput("SENT: " + $('#txtMessage').val());
		webSocket.send($('#txtMessage').val());
	}
	function onOpen(evt) {
		writeOutput("CONNECTED");
	}
	function onClose(evt) {
		writeOutput("DISCONNECTED");
	}
	function onMessage(evt) {
		writeOutput('RESPONSE: ' + evt.data);
	}

	function onError(evt) {
		writeOutput('ERROR: ' + evt.data);
	}
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
		if (window.console && verboseMode) {
			for (var i = 0; i < arguments.length; i++) {
				var that = arguments[i];
				if (!writeTrace(that)) {
					console.log(that);
				}
			}
		}
	}
	var writeErrorLog = function () {
		if (window.console) {
			var stack = null;
			console.error("Error trace and info.");
			for (var i = 0; i < arguments.length; i++) {
				var that = arguments[i];
				if (typeof that === "object" &&
										Object.getPrototypeOf(that) instanceof Error) {
					console.info(that.stack);
					stack = that.stack;
				} else {
					console.info(that);
				}
			}
			if (!stack) { console.trace(); }
			throw new String('exception. Analyze error trace.');
		}
	}
	// a convenience function for parsing string namespaces and 
	// automatically generating nested namespaces
	var extend = function (ns, nsString) {
		var parts = nsString.split('.'),
				parent = ns,
				pl, i;

		if (parts[0] == "myApp") {
			parts = parts.slice(1);
		}

		pl = parts.length;
		for (i = 0; i < pl; i++) {
			//create a property if it doesnt exist
			if (typeof parent[parts[i]] == 'undefined') {
				parent[parts[i]] = {};
			}

			parent = parent[parts[i]];
		}

		return parent;
	}
	//CORS check
	var getTable = function (table) {
		var deferred = $.Deferred();
		var deploymentUrl ='http://haxe.azurewebsites.net/tree/list/query?table=' + table;
		var succes = function (data) {
			try {
				var mappingdata = data.d;
				self.writeLog('getTable', table, 'success:', data);
				deferred.resolve(data);

			} catch (e) {
				self.writeLog(e);
				deferred.rejectWith(this, [e, table]);
			};
		};
		var fail = function () {
			deferred.rejectWith(this, [table]);
		};

		$.ajax({
			type: "GET",
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			url: deploymentUrl
		}).then(succes, fail);

		return deferred.promise();
	}
	return {
		writeLog: writeLog,
		writeErrorLog: writeErrorLog,
		writeTrace: writeTrace,
		extend: extend,
		init: init,
		getTable: getTable
	};
}(jQuery, window, document))