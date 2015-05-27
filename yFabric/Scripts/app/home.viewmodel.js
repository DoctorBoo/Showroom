function HomeViewModel(app, dataModel) {
    var self = this;

    self.myHometown = ko.observable("");
    self.message = ko.observable("");
    self.nick = ko.observable("");
    self.messages = ko.observableArray();
    self.chatmessages = ko.observableArray();
    self.counters = ko.observableArray();
	
    var chat = communicator.chatHub;
    chat.client.newMessage = function (message) {
    	self.addMessage(message);
    };
    chat.client.Identity = function (message) {
    	communicator.identity = message;
    };
    self.sendMessage = function () {
    	var self = this;
    	var deferred = $.Deferred();
    	var message = $.isFunction(self.message) ? self.message() : self.message;
		
    	chat.server.getIdentity().done(function () {
    		communicator.sendMessage.apply(self, [message]).always(function (resolvedwith) {
    			self.message('');
    		});
    	});

    	return deferred;
    };
    self.addMessage = function (message) {
    	var self = this;

    	self.messages.unshift(message);
    }
	
    Sammy(function () {
        this.get('#home', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            $.ajax({
                method: 'get',
                url: app.dataModel.userInfoUrl,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.myHometown('Your Hometown is : ' + data.hometown);
                }
            });
        });
        this.get('/', function () { this.app.runRoute('get', '#home') });
    });

    
    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});

$(function () {

})