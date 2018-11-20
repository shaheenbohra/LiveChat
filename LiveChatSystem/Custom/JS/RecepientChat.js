$(document).ready(function () {
    $("#spUpload").click(function () {
        $("#divLoading").addClass("show")
        var logoFileName = $(".clsLogoFileName").val();
        var companyname = $(".clsCompanyName").val();
        var websiteprefernce = $(".clsWebsiteHeaderPref").val();
        var shorturl = $(".clsShortUrl").val();
        window.location.href = "/DocUpload?logoFileName="+logoFileName +"&companyName="+ companyname+"&websitePreference="+websiteprefernce+"&shortUrl="+shorturl;
    });
    $('#chats').scrollTop(99999);
    var maxLength = 160;
    $('#btn-input').keyup(function () {
        var length = $(this).val().length;
        length = maxLength - length;
        $('#chars').text('Characters remaining:' + length);
    });
    $("#divLoading").removeClass("show")
})


$("#btn-chat").click(function () {
    $("#divLoading").addClass("show")
    var serviceURL = '/User/SendMessageByUser';
    localStorage["clientId"] = $(".clsClientId").val();
    localStorage["recepientPhoneNumber"] = $(".clsRecipientPhoneNumber").val();
    localStorage["userid"] = $(".clsUserId").val();
   // localStorage["RecipientId"] = $(".clsRecipientId").val();
    //localStorage["shorturl"] = $(".clsShortUrl").val();
    var obj = {};
    obj.isSentByUser = false;
    obj.message = $(".clstxtMessage").val();
    obj.clientId = $(".clsClientId").val();
    obj.recepientName = $(".clsRecipientName").val();
    obj.recepientPhoneNumber = $(".clsRecipientPhoneNumber").val();
    obj.sentFromMobile = false;
    obj.loanNumber = $(".clsLoanNumber").val();
    obj.userPhoneNumber = $(".clsuserPhoneNumber").val();
    obj.userid = $(".clsUserId").val();
    obj.RecepientRole = $(".clsRecepientRole").val();
    obj.shortUrl = $(".clsShortUrl").val();
    obj.Email = $(".clsEmail").val();
    obj.loanNo = $(".clsLoanNo").val();

    //  obj.ResultRole = $(".clsResultRole").val();

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
            var html = "";
            html = '<div class="msg_container base_sent"><div class="col-sm-12 col-md-12 col-xs-12")"><div class="messages sent")" ><p>' +
                $(".clstxtMessage").val() +
                ' </p>  <small class="text-muted")">' + new Date() + '</small> <br /> </div ></div > </div > </div > ';
            $("#chats").append(html);
            $(".clstxtMessage").val('');
            $('#chats').scrollTop(99999);
            $("#divLoading").removeClass("show")

        }
        function errorFunc(err) {
            alert(err.responseText);
            $("#divLoading").removeClass("show")
        }
    
});