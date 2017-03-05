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
        content.html($('<pre>', { text: 'Atmosphere connected using ' + response.transport + '\r\n' +
            '형태소 분석기 테스트툴' + '\r\n' +
            '채팅에 입력되는 한글문장의 형태소를 실시간 분석합니다.' + '\r\n' +
            '사용라이브러리 : 꼬꼬마 (http://kkma.snu.ac.kr/documents/)' + '\r\n'
        }));
        input.removeAttr('disabled').focus();
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
        addMessage2("serverpush", message, 'blue');

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

});