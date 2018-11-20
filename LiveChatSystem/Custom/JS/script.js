var isMobileSafari = (/iPhone/i.test(navigator.platform) || /iPod/i.test(navigator.platform) || /iPad/i.test(navigator.userAgent)) && !!navigator.appVersion.match(/(?:Version\/)([\w\._]+)/);
if (isMobileSafari) {
	/* In safari at ios, 
	 * when open this page by '_blank' mode,
	 * and run the script in every pages which this page can link to, 
	 * can disable ios safari swipe back and forward.
	 */
    window.history.replaceState(null, null, "#");
}

$("#imgShowMdl").on("touchmove", function (ev) {
    ev.preventDefault();
    ev.stopPropagation();
});

var painter = new MBC("<license Key>");
painter.onStartLoading = function () { $("#grayFog").show(); };
painter.onFinishLoading = function () {

    $("#grayFog").hide();

    $("#dialog").css("display", "");
    $("#dialog").dialog();
    var blob = painter.getBlob();
    var reader = new FileReader();
    reader.readAsDataURL(blob);
    reader.onloadend = function () {
        base64data = reader.result;
        console.log(base64data);
    }
    // var byteLength = parseInt((base64data).replace(/=/g, "").length * 0.75);

};
painter.onNumChange = function (curIndex, length) {
    $("#pageNum").html((curIndex + 1) + "/" + length);
};
var painterDOM = painter.getHtmlElement();
painterDOM.style.width = '100%';
painterDOM.style.height = '100%';
painterDOM.style.backgroundColor = 'rgba(0,0,0,0.3)';
$("#imgShowMdl").append(painterDOM);
$(window).resize(function () {
    painter.updateUIOnResize(true);
});

$("#grayFog").hide();
$('#btnClose').click(function () {
    $('#dialog').dialog('close');
    return false;
});
//var saveData = (function () {
//    var a = document.createElement("a");
//    document.body.appendChild(a);
//    a.style = "display: none";
//    return function (data, fileName) {
//        var json = JSON.stringify(data),
//            blob = new Blob([json], { type: "octet/stream" }),
//            url = window.URL.createObjectURL(blob);
//        a.href = url;
//        a.download = fileName;
//        a.click();
//        window.URL.revokeObjectURL(url);
//    };
//}());

/* how to get image blob */
$(".btnsave").click(function () {
    
    if (!painter.isEditing()) {
        var base64data = '';
        var fileObject = {};
        var blob = painter.getBlob();
        var reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = function () {
          
            base64data = reader.result;
           

            const type = base64data.split(';')[0].split('/')[1];
            var stringLength = base64data.length - 'data:image/png;base64,'.length;
            var filename = $("#txtFileName").val();
            var sizeInBytes = 4 * Math.ceil((stringLength / 3)) * 0.5624896334383812;
            newRowContent = '<tr><td>' + filename + '</td><td>' + type + '</td><td>' + sizeInBytes + '</td><td><i class="fa fa-trash" onclick="removeFile(this)"></i></td></tr>';
            $(newRowContent).appendTo($("#tblDocs"));
            var filename = $("#ipt-downloadName").val();
           
            var object = {};
            object.fileName = filename;
            object.fileContent = base64data;
            object.fileSize = sizeInBytes;
            object.fileType = type;
            fileObject.push(object);
          
        }
        // var file = new Blob([blob], { type: 'image/jpeg' });
        // saveAs(blob, 'image.jpeg');

        //saveData(blob, "a.jpeg");
    }

});
