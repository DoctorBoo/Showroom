
(function () {

   // Declare a proxy to reference the hub. 

    //$.connection.hub.start();
    
    // Get the user name and store it to prepend to messages.
    //$('#displayname').val(prompt('Enter your name:', ''));

    // Set initial focus to message input box.  


    $(function () {
        try {
            $('#message').focus();

            var Model = function () {
                var self = this;
                self.message = ko.observable("");
                self.messages = ko.observableArray();
            };

            Model.prototype = {
                sendMessage: function () {
                    var self = this;
                    chat.server.send(self.message);
                    self.message = '';
                },
                addMessage: function () {
                    var self = this;
                    self.messages.push(message);
                }
            };

            var model = new Model();
            ko.applyBindings(model);
        } catch (e) {
            console.log(e);
        }
        
        var chat = $.connection.chatHub;
        $.connection.hub.logging = true;

        chat.client.newMessage = function (message) {
            model.addMessage(message);
        };
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
                chat.server.send('', $('#message').val());
                // Clear text box and reset focus for next comment. 
                $('#message').val('').focus();
            });
        });

    });

}());