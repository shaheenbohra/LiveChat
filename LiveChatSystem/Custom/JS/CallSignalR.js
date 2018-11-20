
$(function () {
    // Declare a proxy to reference the hub.
    var notifications = $.connection.chatHub;

    //debugger;
    // Create a function that the hub can call to broadcast messages.
    notifications.client.updateMessages = function (userid,clientid ,recipientPhone, recipientId) {
        getAllMessages(userid, clientid, recipientPhone, recipientId)
    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        //getAllMessages();
    }).fail(function (e) {
        alert(e);
    });
});

function getAllMessages(userid, clientid, recipientPhone, recipientId) {
    //   var tbl = $('#trial');
    var obj = {};
    obj.clientId = clientid;
    obj.recepientPhoneNumber = recipientPhone;
    obj.userid = userid;
    obj.recipientId = recipientId;
    // obj.shorturl = localStorage["shorturl"];
    $.ajax({
        url: '/User/GetMessages',
        contentType: 'application/json ; charset:utf-8',
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(obj),
        success: successFunc,
        error: errorFunc


    });
    function successFunc(result) {
        var myname = result[0].RecipientName.trim();
        myname = myname.replace(/ /g, "_");
        var html = '<li class="media msg_container base_recieve")"><div class="media-body"><div class="messages recieved")"><p>' + result[0].Message + ' </p><small class="text-muted">' + new Date() + '</small></div ></div ></li >'
        $('#' + myname).append(html);
        var shortmsg = result[0].Message.slice(0, 20);
        $('#sneak_msg.' + myname).text(shortmsg);
       $('#chat-area.' + myname).scrollTop(99999);
        var isOpen = $('.clsliMedia.' + myname).parent().find('.clsIsOpen').val();
        if (isOpen == "0") {
            $('.clsliMedia.' + myname).parent().addClass("alrtborder");
           
            var numItems = $('.alrtborder').length;
            $("#counter").text(numItems);

        }
        else {
            console.log('here');
        }

        // tbl.empty().append(result);

    }

    function errorFunc(err) {
        // alert(err.responseText);
    }
}
