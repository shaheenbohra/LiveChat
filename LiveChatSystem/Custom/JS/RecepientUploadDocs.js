var fileData = new FormData();
var fileObject = [];
$(document).ready(function () { $("#divLoading").removeClass("show") })
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

$("#upldbtn").click(function () {

    $("#FileUpload1").click();
});
$(".btnsave").click(function () {

    if (!painter.isEditing()) {
        var base64data = '';
       
        var blob = painter.getBlob();
        var reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = function () {

            base64data = reader.result;


            const type = base64data.split(';')[0].split('/')[1];
            var stringLength = base64data.length - 'data:image/png;base64,'.length;
            var filename = $("#txtFileName").val();
            var sizeInBytes = 4 * Math.ceil((stringLength / 3)) * 0.5624896334383812;
            newRowContent = '<tr><td style="word-wrap: break-word;max-width:20px;">' + filename + '</td><td>' + type + '</td><td>' + bytesToSize(sizeInBytes) + '</td><td><i class="fa fa-trash" onclick="removeFile(this)"></i></td></tr>';
            $(newRowContent).appendTo($("#tblDocs"));
            filename = $("#ipt-downloadName").val();

            var object = {};
            object.fileName = filename;
            object.fileContent = base64data;
            object.fileSize = bytesToSize(sizeInBytes);
            object.fileType = type;
            fileObject.push(object);

        }
        // var file = new Blob([blob], { type: 'image/jpeg' });
        // saveAs(blob, 'image.jpeg');

        //saveData(blob, "a.jpeg");
    }

});

var removeFile = function (id) {
    var rowId = $(id).closest('td').parent()[0].sectionRowIndex;
    fileObject.splice(rowId, 1);
    $(id).parent().parent().remove();
    return false;
}

function getBase64(file, fileName, type, length) {
    var object = {};
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        object.fileName = fileName;
        object.fileContent = reader.result;
        object.fileSize = length;
        object.fileType = type;
        fileObject.push(object);
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}
var showCamera = function () {
    $("#camhdn").css("display", "");
    painter.showVideo();
}

$(document).ready(function () {
 
    $('input:file').change(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            // Create FormData object  
          

            // Looping over all files and add it to FormData object  
            var newRowContent = '';
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);

                getBase64(files[i], files[i].name, files[i].type, bytesToSize(files[i].size));
                newRowContent = '<tr><td style="word-wrap: break-word;max-width:20px;">' + files[i].name + '</td><td>' + files[i].type + '</td><td>' + bytesToSize(files[i].size) + '</td><td><i class="fa fa-trash" onclick="removeFile(this)"></i></td></tr>';

            }
            $(newRowContent).appendTo($("#tblDocs"));

        } else {
            alert("FormData is not supported.");
        }
    });
});
$("#btnSubmit").click(function () {
   
    if (fileObject.length == 0) {
        alert("Select a file to upload or take a picture.");
    }
    else {
        $("#divLoading").addClass("show")
        var shorturl = $('.clsShortUrl').val();
        $.ajax({
            url: '/Recipient/SaveFile',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                $("#divLoading").removeClass("show")
                alert("Documents uploaded successfully!");
               window.location.href = "/Chat?shorturl=" + shorturl;
            },
            error: function (err) {
                alert("Error in uploading the documents. Please try again.");
                $("#divLoading").removeClass("show")
            }
        });  
        
    }
});
function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
};
