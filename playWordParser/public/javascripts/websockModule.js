$(function () {
    "use strict";
    var header = $('#header');
    var content = $('#content');
    var input = $('#input');
    var status = $('#status');
    var myName = false;
    var author = null;
    var logged = false;
    var socket = atmosphere;
    var subSocket;
    var transport = 'websocket';

    var request = { url: 'ws://' + window.location.host +'/wsapi',
    contentType : "application/json",
    logLevel : 'debug',
    transport : transport ,
    enableProtocol : true,
    trackMessageLength : true,
    reconnectInterval : 5000,
    fallbackTransport: 'long-polling'};

    request.onOpen = function(response) {
        content.html($('<pre>', { text: 'Atmosphere connected using ' + response.transport + '\r\n' +
            'Test Sample' + '\r\n' +
            '{ "operation": "AUTHENTICATE",   "values": ["xxxxxxxxxxxx"] }' + '\r\n' +
            '{ "operation": "SUBSCRIBE",   "values":   ["poker.tournaments.sng","poker.tournaments.scheduled"] }' + '\r\n' +
            '{ "operation": "SUBSCRIBE",   "values": ["poker.tournaments.registered"] }' + '\r\n'
        }));
        input.removeAttr('disabled').focus();
        status.text('Choose name:');
        transport = response.transport;
    };

    request.onReopen = function(response) {
        input.removeAttr('disabled').focus();
        content.html($('<p>', { text: 'Atmosphere re-connected using ' + response.transport }));
    };
    <!-- For demonstration of how you can customize the fallbackTransport using the onTransportFailure function -->
    request.onTransportFailure = function(errorMsg, request) {
        atmosphere.util.info(errorMsg);
        if (window.EventSource) {
        request.fallbackTransport = "sse";
        }
        header.html($('<h3>', { text: 'LobbyAPI WebSocket TestPage, Default transport is WebSocket, fallback is ' + request.fallbackTransport }));
    };
    request.onMessage = function (response) {
        var message = response.responseBody;
        try {
            var json = atmosphere.util.parseJSON(message);
            console.log(json);
        } catch (e) {
            console.log('This doesn\'t look like a valid JSON: ', message);
            return;
        }
        input.removeAttr('disabled').focus();
        if (!logged && myName) {
            logged = true;
            status.text(myName + ': ').css('color', 'blue');
        } else {
            var me = json.author == author;
            var date = typeof(json.time) == 'string' ? parseInt(json.time) : json.time;
            if(json.author){
                addMessage(json.author, json.message, me ? 'blue' : 'black', new Date(date));
            }else{
                addMessage2("serverpush", message, 'blue', parseInt(json.timestamp) );
        }
    }
    };
        request.onClose = function(response) {
            content.html($('<p>', { text: 'Client closed the connection after a timeout' }));
            subSocket.push(atmosphere.util.stringifyJSON({ author: author, message: 'disconnecting' }));
            input.attr('disabled', 'disabled');
        };
        request.onError = function(response) {
            content.html($('<p>', { text: 'Sorry, but there\'s some problem with your '
            + 'socket or the server is down' }));
            logged = false;
        };
        request.onReconnect = function(request, response) {
        //content.html($('<p>', { text: 'Connection lost, trying to reconnect. Trying to reconnect ' + request.reconnectInterval}));
        //input.attr('disabled', 'disabled');
        };
        subSocket = socket.subscribe(request);
        input.keydown(function(e) {
            if (e.keyCode === 13) {
                var msg = $(this).val();
            // First message is always the author's name
            if (author == null) {
                author = msg;
        }

        /*
        if(msg=="1"){
            subSocket.push(atmosphere.util.stringifyJSON({ "operation": "SUBSCRIBE", "values": ["poker.tournaments.sng"] }));
        }else if(msg=="2"){
            subSocket.push(atmosphere.util.stringifyJSON({ operation: "SUBSCRIBE", values: ["poker.tournaments.scheduled"] }));
        }else if(msg=="3"){
            subSocket.push(atmosphere.util.stringifyJSON({ operation: "SUBSCRIBE", values: "poker.tournaments.scheduled" }));
        }else{
            subSocket.push(atmosphere.util.stringifyJSON({ author: author, message: msg }));
        }*/


        var sendMsg = '{"text":"'+ msg + '","pid":"WordParserInfo","reqID":0}'


        subSocket.push( atmosphere.util.stringifyJSON( $.parseJSON(sendMsg)  ));

        $(this).val('');
        //input.attr('disabled', 'disabled');
        if (myName === false) {
        myName = msg;
        }


        }
    });

    function addMessage(author, message, color, datetime) {
        content.append('<p><span style="color:' + color + '">' + author + '</span> &#64; ' +
        + (datetime.getHours() < 10 ? '0' + datetime.getHours() : datetime.getHours()) + ':'
        + (datetime.getMinutes() < 10 ? '0' + datetime.getMinutes() : datetime.getMinutes())
        + ': ' + message + '</p>');
    }

    function addMessage2(author, message, color, datetime) {
    content.append('<p><span style="color:' + color + '">' + author + '</span> &#64; ' +
        + datetime
        + ': ' + message + '</p>');
    }

});