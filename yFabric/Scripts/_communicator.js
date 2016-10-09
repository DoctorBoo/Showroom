var $connectionperfHub = $.connection.perfHub;

var globals = (function (instance) {
	var self = this;
	
	$(function () {
		this.instance = instance;
	})
	return {
		
	}
}($connectionperfHub));

var PerfHub = (function () {
	var self = PerfHub;
	
	function PerfHub() {
		this.instance = null;//$.connection.perfHub;
	};
	//Do namespace injection elsewhere:
	//(function () {
	//	this.instance = $.connection.perfHub;;
	//}).apply(PerfHub);
	return PerfHub;
})();

var communicator = (function () {
    var self = this;
    try {
        
    	var chat = $.connection.chatHub || $.hubConnection().createHubProxy('ChatHub');
    	self.perfHub = {};//new PerfHub().instance;
		var perf = self.perfHub;
        chat.connection.stateChanged(connectionStateChanged);

        self.identity = null;
        self.hubStarted = null;

        $.connection.hub.logging = true;
        
        chat.on('newMessage',function (message) {
            model.addMessage(message);
        });
        chat.on('Identity', function (message) {
            communicator.identity = message;
        });        

        var ChartEntry = function (name) {
            var self = this;
            self.name = name;

            self.chart = new SmoothieChart({
                millisPerPixel: 50,
                labels: {
                    fontSize: 15
                }
            });
            self.timeSeries = new TimeSeries();
            self.chart.addTimeSeries(self.timeSeries, {
                lineWidth: 3,
                strokeStyle: '#00ff00'
            });
        };

        ChartEntry.prototype = {
            addValue: function (value) {
                var self = this;
                self.timeSeries.append(new Date().getTime(), value);
            },
            start: function () {
                var self = this;
                self.canvas = document.getElementById(self.name);
                self.chart.streamTo(self.canvas);
            }

        };

        var Model = function () {
            var self = this;
            self.message = ko.observable("");
            self.nick = ko.observable("");
            self.messages = ko.observableArray();
            self.chatmessages = ko.observableArray();
            self.counters = ko.observableArray();
        };
        Model.prototype = {

            addCounters: function(updatedCounters){
                var self = this;

                $.each(updatedCounters, function (index, updatedCounter) {
                    var entry = ko.utils.arrayFirst(self.counters(), function (counter) {
                        return counter.name === updatedCounter.name;
                    });

                    if (!entry) {
                        entry = new ChartEntry(updatedCounter.name);
                        self.counters.push(entry);
                        entry.start();
                    }

                    entry.addValue(updatedCounter.value);
                });
            },

            sendMessage: function () {
                var self = this;

                var message = $.isFunction(self.message) ? self.message() : self.message;

                chat.connection.start()
                    .done(function () {
                        chat.invoke('getIdentity').done(function () {
                            sendMessage(message);
                        });
                    });               
            },
            addMessage: function (message) {               
                var self = this;

                self.messages.unshift(message);
                self.chatmessages.unshift(message);
                $('#chatmessages').prepend('<div>' + message + '<div/>')
                if (location.toString().indexOf('index.html') < 0) {
                    var iframe = $('iframe');
                    iframe.contents().find('#chats').remove();
                }
            }
        };
        var model = new Model();

        var sendMessage = function (message) {
        	//var contextArgs = Array.prototype.slice.call(arguments);
        	var deferred = $.Deferred();

        	var doSend = function (message) {
        	    var model = this;
        	    chat.connection.start()
                   .done(function () {
                       chat.invoke('sendMessage',message).always(function () {
                           deferred.resolve();
                           model.message('');
                       });
                });
        	};

        	var id = communicator.identity || 'This noob';
        	message = id + ' says: ' + message;

        	if (communicator.connectionState !== 1) {
        	    $.connection.hub = $.hubConnection().createHubProxy('ChatHub').connection;
        		$.connection.hub.start().done(function () {
        			if (message) {
        				doSend.apply(model, [message]);
        			}
        		});
        	} else {
        		doSend.apply(model,[message]);
        	}
        	return deferred;
        };

        //model.sendMessage().always(sendMessage);
    } catch (e) {
        console.log(e);
    }
    
    /*
*  SIGNALR
*/
    try {
        var $insightHub = {};
        //var $insightHub = $.connection.insightHub;
        var connection = $.hubConnection();
        $insightHub = connection.createHubProxy('PerfHub');

        $insightHub.connection.stateChanged(connectionStateChanged);

        $insightHub.on('echo', function (message) {
            //globals.consoleLog(communicator.identity + ' echo :', message);
        });
        $insightHub.on('echoCallback', function (user, message) {
            globals.consoleLog(user + ' said :', message);
        });
        $insightHub.on('newCounters',function (counters) {
            model.addCounters(counters);
        });
           
    } catch (e) {
        globals.consoleLog(e);
    }
    $(function () {

        try {
            $.connection.hub.start();
            console.log('signalr started?');
        } catch (e) {
            console.log('Fallback to proxy signalr.');
        }
    });
    function connectionStateChanged(state) {        
        writeLog('SignalR state changed from: ' + stateConversion[state.oldState]
         + ' to: ' + stateConversion[state.newState]);
        connectionState = state.newState;
    }
    var stateConversion = { 0: 'connecting', 1: 'connected', 2: 'reconnecting', 4: 'disconnected' };
    var connectionState = {};
    var verboseMode = true;

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
    var startHub = function (alias) {
        alias = alias || 'PerfHub';
        var deferred = $.Deferred();
        try {
            self.hubStarted = true;
            var connection = $.hubConnection();
            var hub = connection.createHubProxy(alias);

            hub.connection.start()//{ transport: ['webSockets', 'longPolling'] }
                .done(function () {
                    globals.consoleLog('Hubstarted.');
                    globals.consoleLog("Connected, transport = " + hub.connection.transport.name);
                    deferred.resolve();
                })
                .fail(function () {
                    globals.consoleLog('Start hub failed.');
                    deferred.reject();
                });
        } catch (e) {
            globals.consoleLog(e);
        }
        return deferred;
    };
    var startPerf = function () {
        var perf = connection.createHubProxy('PerfHub');
        perf.connection.start()
            .done(function () {
                perf.on('newCounters', function (counters) {
                    model.addCounters(counters);
                });
                perf.invoke('startCounterCollection', function () {
                    globals.consoleLog('startCounterCollection');
                });
            });

    };
    return {
    	chatHub: chat,
    	perfHub: perf,
        instanceModel: model,
        viewModel: Model,
        connectionState: connectionState,
        identity: identity,
        sendMessage: sendMessage,
        startPerfHub: function () {
            return startHub('PerfHub');
        },
        startHub: startHub,
        startCounterCollection: function () {
            try {
                self.hubStarted = true;
                $insightHub.connection.start()//{ transport: ['webSockets', 'longPolling'] }
                    .done(function () {
                        globals.consoleLog('Hubstarted.');
                        globals.consoleLog("Connected, transport = " + $insightHub.connection.transport.name);
                        startPerf();
                    })
                    .fail(function () {
                        globals.consoleLog('Start hub failed.');
                    });
            } catch (e) {
                globals.consoleLog(e);
            }
        }
    }
}());

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
