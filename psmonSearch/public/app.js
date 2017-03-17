

$(document).ready(function () {


    $('#btnRunCrawl').click(function () {
        var txtCmd = $('#txtCmd').val();
        //$(this).ajaxSubmit(options);
        var sendData = { command: txtCmd };

        $.ajax({
            type: 'post',
            url: '/api/cmd',
            data: JSON.stringify(sendData),
            processData: false,
            contentType: "application/json; charset=utf-8",            
            success: function (data) {
                $("ol").append("<li>" + data + "</li>");
            }
        });

    });
        
       
      
    
});



