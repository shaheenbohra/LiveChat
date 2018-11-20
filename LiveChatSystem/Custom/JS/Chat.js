
//var userName = "";
//var sessionVal = '';
$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    debugger;

    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    };
   
    // Set initial focus to message input box.
    $('.clstxtMessage').focus();
    $('.clstxtMessage').keypress(function (e) {
        if (e.which == 13) {//Enter key pressed
            $('.clsBtnSend').trigger('click');//Trigger search button click event
        }
    });
    $("#smile").click(function () {

    });
    // Start the connection.
    $.connection.hub.start().done(function () {
        $(document).on('click', '.clsBtnSend', function (event, sendByUser)
        //$('.clsBtnSend').on('click' , function ()
        {

            // Call the Send method on the hub.
            var connId = $("#connId").val();
            var frndConnId = $(".clsConnectionid").val();
            var finalConnId = frndConnId == "" ? $.connection.hub.id : frndConnId;
            $(".clsConnectionid").val($.connection.hub.id);

            chat.server.send($('#displayname').val(), $('.clstxtMessage').val(), finalConnId);
            $("#connId").val($.connection.hub.id);
            if (frndConnId == "") {
                alert($.connection.hub.id);
              //  swal("You connection Id", $.connection.hub.id, "success");
            }
            // Clear text box and reset focus for next comment.
            var html = '';
            if (sendByUser == 1) {
                html = '<li class="media msg_container base_sent")"><div class="media-body"><div class="messages sent")"><p>' + $('.clstxtMessage').val() + ' </p><small class="text-muted">' + new Date() + '</small></div ></div ></li >'
                $('li.msg_container:last').append(html);
            }
            else {
                html = '<div class="msg_container base_sent")">< div class="row"><div class="col-sm-12 col-md-12 col-xs-12"><p> <small class="text-muted hidden"></small> <small class="text-muted"></small></p ><div class="messages sent" ><p>' + $('.clstxtMessage').val() +'</p><small class="text-muted">'+ new Date()+'</small><br /></div ></div ></div ></div >';
                $('div.msg_container:last').append(html);
            }
           
            $('.clstxtMessage').focus();
        });
    });


});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}


