function ActivateFileLoader() {
    $('#FileBoxLoader').css('display', 'block');
    $('#FileBoxLoader').css('display', 'block');
}

function DeActivateFileLoader() {
    $('#FileBoxLoader').css('display', 'none');
    $('#FileBoxLoader').css('display', 'none');
}
function UploadFile(event) {
    ActivateFileLoader();
    CreateNewAttachment(event);
}

function CreateNewAttachment(event) {
    $.ajax({
        url: '/Academics/Resources/CreateResourceAttachments',
        type: 'POST',
        data: { id: pid, type: type, FileType: $("#files").val().split('.').pop(), FileName: $('#files').val().split('\\').pop() },
        beforeSend: function () { },
        success: function (data, el) {
            if (data > 0) {
                debugger
                UploadFileToDropBox(event, data);
            }
            else {
                toastr.error("Failure! <br/> Attachment is not uploaded.");
                DeActivateFileLoader();
            }
        },
        error: function (el) {
            DisplayMessage('error', 'Oops ! Something went wrong please try again');
            $("#photosgrid").load("/Academics/Resources/GetAttachments" + pid);
            DeActivateFileLoader();
        },
        statusCode: null,
        onProgress: null,
        onComplete: null
    })
}

function UploadFileToDropBox(event, attachmentId) {
    //File Upload Code    
    toastr.info("File Uploading in Progress!");
    file = event.target.files[0];
    var TempName = $('#files').val().split('\\').pop();
    Filename = attachmentId + "_" + TempName;
    var UploadPath = UploadUrlForDropbox + "/" + Filename;
    var ConcatPath = '"' + UploadPath + '"';
    var DropboxApiArgs = '{"path":' + ConcatPath + ',' + '"mode": "add","autorename": true,"mute": false}';
    $.ajax({
        url: 'https://content.dropboxapi.com/2/files/upload',
        type: 'post',
        data: file,
        processData: false,
        contentType: 'application/octet-stream',
        headers: {
            "Authorization": dropboxToken,
            "Dropbox-API-Arg": DropboxApiArgs
        },

        success: function (data) {
            debugger;
            CreateThisFileShareAbleLink(UploadPath, data.id, attachmentId);
        },
        error: function (data) {
            toastr.error("Failure! <br/> Attachment is not uploaded.");
            DeActivateFileLoader();
        }
    })
}

function CreateThisFileShareAbleLink(Path, fileId, attachmentId) {
    var _data = {
        "path": Path,
        "settings": {
            "requested_visibility": "public",
            "audience": "public",
            "access": "viewer"
        }
    };
    $.ajax({
        url: 'https://api.dropboxapi.com/2/sharing/create_shared_link_with_settings',
        type: 'post',
        data: JSON.stringify(_data),
        headers: {
            "Authorization": dropboxToken,
            "Content-Type": "application/json"
        },
        success: function (data) {
            debugger
            var ShareAbleurl = data.url;
            var DownloadableShareableLink = ShareAbleurl.slice(0, -1) + '1';
            SaveDownloadAbleLinkToDatabase(DownloadableShareableLink, fileId, attachmentId);
        },
        error: function (data) {
            toastr.error("Failure! <br/> Attachment is not uploaded.");
            DeActivateFileLoader();
        }
    })

}

function SaveDownloadAbleLinkToDatabase(DownloadableLink, fileId, attachmentId) {
    $.ajax({
        url: '/Academics/Resources/SaveDropboxAttachmentsToDatabase',
        type: 'POST',
        data: { FileId: fileId, Link: DownloadableLink, AttachmentId: attachmentId },
        beforeSend: function () { },
        success: function (data, el) {
            if (data == true) {
                debugger
                toastr.success("File Uploaded Successfully!");
            }
            else {
                toastr.error("Failure! <br/> Attachment is not uploaded.");
            }
            $("#photosgrid").load('/Academics/Resources/GetAttachments/' + pid);
            DeActivateFileLoader();
        },
        error: function (el) {
            DisplayMessage('error', 'Oops ! Something went wrong please try again');
            $("#photosgrid").load("/Academics/Resources/GetAttachments" + pid);
            DeActivateFileLoader();
        },
        statusCode: null,
        onProgress: null,
        onComplete: null
    })
}