
var chatHub = (function () {
    
    try {
        $('#message').focus();

        var chat = chat = $.connection.chatHub;

        $.connection.hub.logging = true;

        chat.client.newMessage = function (message) {
            model.addMessage(message);
        };

        var Model = function () {
            var self = this;
            self.message = ko.observable("");
            self.messages = ko.observableArray();
        };

        Model.prototype = {
            sendMessage: function () {
                var self = this;
                chat.server.sendMessage(self.message());
                self.message = '';
            },
            addMessage: function (message) {
                var self = this;
                self.messages.push(message);
            }
        };

        
    } catch (e) {
        console.log(e);
    }
    var model = new Model();

    $(function () {
               

        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function (name, message) {
            // Html encode display name and message. 
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page. 
            $('#discussion').append('<li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };

        // Start the connection.
        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                // Call the Send method on the hub. 
                //chat.server.send('', $('#message').val());
                // Clear text box and reset focus for next comment. 
                $('#message').val('').focus();
            });
        });

        try {
            ko.cleanNode($('#message')[0]);
            ko.applyBindings(model);
        } catch (e) {
            console.log(e);
        }
    });

    return {
        instanceModel: model,
        viewModel: Model
    }
}());