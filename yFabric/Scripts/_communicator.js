
var communicator = (function () {
    var self = this;
    try {
        
        var chat = $.connection.chatHub;
        var perf = $.connection.perfHub;
        chat.connection.stateChanged(connectionStateChanged);

        self.identity = null;

        $.connection.hub.logging = true;
        $.connection.hub.start();

        chat.client.newMessage = function (message) {
            model.addMessage(message);
        };
        chat.client.Identity = function (message) {
            communicator.identity = message;
        };

        perf.client.newCounters = function (counters) {
            model.addCounters(counters);
        }

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
                var deferred = $.Deferred();
                var message = $.isFunction(self.message) ? self.message() : self.message;

                chat.server.getIdentity().done(function () {

                    var id = communicator.identity || 'This noob';
                    message = id + ' says: ' + message;

                    if (connectionState !== 1) {
                        $.connection.hub.start().done(function () {
                            if (message) {
                                deferred.resolve(message);
                                sendMessage(message);
                            }
                        });
                    } else {
                        //deferred.resolve(message);
                        sendMessage(message);
                    }
                });

                return deferred;
            },
            addMessage: function (message) {               
                var self = this;

                self.messages.push(message);
                self.chatmessages.push(message);
                $('#chatmessages').append('<div>' + message + '<div/>')
                if (location.toString().indexOf('index.html') < 0) {
                    var iframe = $('iframe');
                    iframe.contents().find('#chats').remove();
                }
            }
        };
        var model = new Model();

        var sendMessage = function (message) {
            //var contextArgs = Array.prototype.slice.call(arguments);
            chat.server.sendMessage(message).done(function () {
                model.message('');
            });
        };

        //model.sendMessage().always(sendMessage);
    } catch (e) {
        console.log(e);
    }
    

    $(function () {

        try {
            ko.cleanNode($('#message')[0]);
            ko.applyBindings(model);

            //perf.startCounterCollection();
        } catch (e) {
            console.log(e);
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
    return {
        chatHub: chat,
        instanceModel: model,
        viewModel: Model,
        connectionState: connectionState,
        identity : identity 
    }
}());