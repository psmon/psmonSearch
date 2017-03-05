$(function () {
    "use strict";
    var header = $('#header');
    var content = $('#content');
    var input = $('#input');
    var status = $('#status');
    var myName = "test";
    var author = null;
    var logged = false;
    var socket = atmosphere;
    var subSocket;
    var transport = 'websocket';

    var request = { url: 'ws://' + window.location.host +'/wsapi',
        contentType : "application/json",
        logLevel : 'debug',
        transport : transport ,
        enableProtocol : false,
        trackMessageLength : true,
        reconnectInterval : 5000,
        fallbackTransport: 'long-polling'
    };

    request.onOpen = function(response) {
        console.log('ttt');
        addChat("","형태소 분석기 테스트툴(사용 라이브리러:꼬꼬마-http://kkma.snu.ac.kr/documents/)");
        status.text('test:');
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
        //addMessage2("serverpush", message, 'blue');
        addChat("serverpush",message)

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
            var sendMsg = '{"text":"'+ msg + '","pid":"WordParserInfo","reqID":0}'
            subSocket.push( atmosphere.util.stringifyJSON( $.parseJSON(sendMsg)  ));
            $(this).val('');
        }
    });

    function addMessage(author, message, color, datetime) {
        content.append('<p><span style="color:' + color + '">' + author + '</span> &#64; ' +
        + (datetime.getHours() < 10 ? '0' + datetime.getHours() : datetime.getHours()) + ':'
        + (datetime.getMinutes() < 10 ? '0' + datetime.getMinutes() : datetime.getMinutes())
        + ': ' + message + '</p>');
    }

    function addMessage2(author, message, color) {
    content.append('<p><span style="color:' + color + '">' + author + '</span> &#64; '
        + ': ' + message + '</p>');
    }

    $( document ).ready(function() {
        $( "#btnSend" ).click(function() {
            sendChat();
        });

        $( "#inChatStr" ).keypress(function( event ) {
            if (event.which == 13 && !event.shiftKey) {
                event.preventDefault();
                sendChat();
            }
        });
    });

    var sendChat = function(){
        console.log('click');
        var chatText = $( "#inChatStr").val();
        addChat("ME",chatText);
        var sendMsg = '{"text":"'+ chatText + '","pid":"WordParserInfo","reqID":0}';
        subSocket.push( atmosphere.util.stringifyJSON( $.parseJSON(sendMsg)  ));

        $("#inChatStr").val('');
        $("#inChatStr").focus();
    }

    var addChat = function(nick,msg){
        var txtChatArea = $( "#txtChatArea" );
        txtChatArea.append( "<br/>" + nick + ">" + "<strong>"+msg+"</strong>" );
        txtChatArea.scrollTop(txtChatArea.scroll().height());
        //window.scrollTo(0,10000000);
    }

});