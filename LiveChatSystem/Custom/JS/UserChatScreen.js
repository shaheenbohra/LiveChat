
var showMessageTemplate = function (myname) {
    if ($('#ddlMsgTemplates .' + myname).val() == "0") {
        $("#txtMessage ." + myname).val('');
        return false;
    }
    else {
        var msg = $('#ddlMsgTemplates.' + myname).find("option:selected").text();

        $("#txtMessage." + myname).val(msg);
    }
}
//Send message from twilio using user chat screen
var sendMessage = function (current, clientId, recepientName, recepientPhoneNumber, loanNumber, userPhoneNumber, userid, RecepientRole, loanNo) {
    var sendByUser = 1;
    $('.clsBtnSend').trigger('click', [sendByUser]);//Trigger search button click event
    var serviceURL = '/User/SendMessageByUser';
    var msg = $(current).parent().parent().find('.clsmessage').val();
    var obj = {};
    obj.isSentByUser = true;
    obj.message = msg;
    obj.clientId = clientId;// $(".clsClientId").val();
    obj.recepientName = recepientName;// $(".clsRecipientName").val();
    obj.recepientPhoneNumber = recepientPhoneNumber;// $(".clsRecipientPhoneNumber").val();
    obj.sentFromMobile = false;
    obj.loanNumber = loanNumber;// $(".clsLoanNumber").val();
    obj.userPhoneNumber = userPhoneNumber;// $(".clsuserPhoneNumber").val();
    obj.userid = userid;//$(".clsUserId").val();
    obj.RecepientRole = RecepientRole;
    obj.connectionid = $(current).parent().parent().find(".clsConnectionid").val();
    obj.Email = "";
    obj.loanNo = loanNo;
  //  obj.ResultRole = $(".clsResultRole").val();

    if (msg != "") {
        $.ajax({
            type: "POST",
            url: serviceURL,
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data, status) {
            $(current).parents('li').find(".clsRecipientId").val(data[0]);
            $(current).parents('li').find(".clsMessage").val(data[2]);
            $(current).parents('li').find(".clsShortmsg").html(data[2].substring(0, 20));
            var myname = obj.recepientName.trim();
            myname = myname.replace(/ /g, "_");
            var html = "";
            html = '<li class="media msg_container base_sent")"><div class="media-body"><div class="messages sent")"><p>' + $('#txtMessage.' + myname).val() + ' </p><small class="text-muted">' + new Date() + '</small></div ></div ></li >'
            $('#' + myname).append(html);
            $("#txtMessage." + myname).val('');
            $('#chat-area.' + myname).scrollTop(99999);
        }

        function errorFunc(err) {
            alert(err.responseText);
        }
    }
    
}


var sendVcard = function (current, clientId, recepientName, recepientPhoneNumber, loanNumber, userPhoneNumber, userid, RecepientRole, loanNo) {
    var obj = {};

    obj.recepientPhoneNumber = recepientPhoneNumber;
    obj.userPhoneNumber = userPhoneNumber;// $(".clsuserPhoneNumber").val();
    obj.userid = userid;
    obj.clientId = clientId;

    var vcardurl = '/User/sendvcard';


    $.ajax({
        type: "POST",
        url: vcardurl,
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        alert("vcard sent");

    }

    function errorFunc(err) {
        alert(err.responseText);
    }
}
