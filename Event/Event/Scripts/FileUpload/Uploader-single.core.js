﻿function createMode() {
    $('.CreateLink').hide();
    Init_Upload();
}
function Init_Upload() {
    $('#CreateDialog input[name=UploadedFile]').change(function (evt) { singleFileSelected(evt); });
   
    $('#CreateDialog button[id=Submit_btn]').click(function () {
        UploadFile();
    });   
}

function singleFileSelected(evt) {
    //var selectedFile = evt.target.files can use this  or select input file element and access it's files object
    var selectedFile = ($("#UploadedFile"))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var FileSize = 0;
        var imageType = /image.*/;
        if (selectedFile.size > 1048576) {
            FileSize = Math.round(selectedFile.size * 100 / 1048576) / 100 + " MB";
        }
        else if (selectedFile.size > 1024) {
            FileSize = Math.round(selectedFile.size * 100 / 1024) / 100 + " KB";
        }
        else {
            FileSize = selectedFile.size + " Bytes";
        }
        
        if (selectedFile.type.match(imageType)) {
            var reader = new FileReader();
            reader.onload = function (e) {
                
                $("#Imagecontainer").empty();
                var dataURL = reader.result;
                var img = new Image()
                img.src = dataURL;
                img.className = "thumb";
                $("#Imagecontainer").append(img);
            };
            reader.readAsDataURL(selectedFile);
        }
        $("#FileName").text("Name : " + selectedFile.name);
        $("#FileType").text("type : " + selectedFile.type);
        $("#FileSize").text("Size : " + FileSize);
    }
}

function UploadFile() {
    // we can create form by passing the form to Constructor of formData obeject
    //or creating it manually using append function  but please note file file name should be same like the action Paramter
    //var dataString = new FormData();
    //dataString.append("UploadedFile", selectedFile);

    var form = $('#CreateDialog')[0];
    var dataString = new FormData(form);
    $.ajax({
        url: '/DMS_AttachFileStore/Create',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                //myXhr.upload.onprogress = progressHandlingFunction
                myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events
        success: successHandler,
        error: errorHandler,
        complete:completeHandler,
        // Form data
        data: dataString,
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });

}



