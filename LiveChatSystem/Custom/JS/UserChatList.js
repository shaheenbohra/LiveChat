$(document).ready(function () {
    var numItems = $('.alrtborder').length;
    $("#counter").text(numItems);


    $(".clsliMedia").click(function () {
       
        var isOpen = $(this).parent().find('.clsIsOpen').val();
        if (isOpen == "0") {
            $("#divLoading").addClass("show")
            var objdiv = $(this).parent().find('.clsloadChatScreen');
            $(this).parent().find('.clsloadChatScreen').css('display', '');
            $(this).parent().find('.clsIsOpen').val("1");
            $(this).parent().removeClass("alrtborder");
            var numItems = $('.alrtborder').length;
            $("#counter").text(numItems);
            var loanNumber = $(this).parent().find('.clsLoanNumber').val();
            var ClientId = $(this).parent().find('.clsClientId').val();
            var RecipientName = $(this).parent().find('.clsRecipientName').val();
            var RecipientPhoneNumber = $(this).parent().find('.clsRecipientPhoneNumber').val();
            var RecipientRole = $(this).parent().find('.clsRecipientRole').val();
            var Message = $(this).parent().find('.clsMessage').val();
            var RecipientId = $(this).parent().find('.clsRecipientId').val();
            var LoanNo = $(this).parent().find('.clsLoanNo').val();
            var ResultRole ="1";
            if ($.trim(Message) == "") {
                ResultRole = "0";
            }
            var MaskedPhoneNumber = $(this).parent().find('.clsMaskedPhoneNumber').val();
            var userid = $(this).parent().find('.clsUserId').val();
            var serviceUrl = "/User/_chatView";
            //  string clientId, string recepientName, string recepientPhoneNumber, int loanNumber, string userPhoneNumber, int RecepientRole)
            var obj = {};
            obj.clientId = ClientId;
            obj.recepientName = RecipientName;
            obj.recepientPhoneNumber = RecipientPhoneNumber;
            obj.loanNumber = loanNumber;
            obj.userPhoneNumber = MaskedPhoneNumber;
            obj.RecepientRole = RecipientRole;
            obj.userid = userid;
            obj.ResultRole = ResultRole;
            obj.RecipientId = RecipientId;
            obj.LoanNo = LoanNo;
            $.ajax({
                type: "POST",
                url: serviceUrl,
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: successFunc,
                error: errorFunc
            });
            function successFunc(data, status) {
                $(objdiv).html(data);
                //$(this).parent().find('.clsloadChatScreen').html(data);
               // $('.clsloadChatScreen').html(data);
                var myname = obj.recepientName.trim();
                myname = myname.replace(/ /g, "_");
                $('#chat-area.'+ myname).scrollTop(99999);
                var maxLength = 160;
                
                $("#txtMessage." + myname).keyup(function () {
                    var length = $(this).val().length;
                    var length = maxLength - length;
                    $(this).parent().children(".mychars").text('Characters remaining:' + length);
                });
                $("#divLoading").removeClass("show")
            }

            function errorFunc(err) {
                $("#divLoading").removeClass("show")
                alert(err.responseText);
            }
        }
        else {
            $(this).parent().find('.clsIsOpen').val("0");
            $(this).parent().find('.clsloadChatScreen').css('display', 'none');
        }
        //window.location.href = "UserChatScreen?clientId =" + ClientId + "&recepientName=" + RecipientName + "&recepientPhoneNumber=" + RecipientPhoneNumber + "&loanNumber=" + loanNumber + "&userPhoneNumber=" + MaskedPhoneNumber + "&RecepientRole=1";
           
    });

    $("#minim_chat_window").click(function () {
        $('.clsliMedia').parent().find('.clsIsOpen').val("0");
        $('.clsliMedia').parent().find('.clsloadChatScreen').css('display', 'none');
    });
    $("#divLoading").removeClass("show")
});

