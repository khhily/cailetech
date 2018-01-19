function initUploadify(obj, uploader) {
    $(obj).uploadify({
        buttonText: "选择文件"//按钮显示文字
            , buttonCursor: 'hand'// 按钮的鼠标图标
            , swf: '/scripts/uploadify/uploadify.swf'//必输入！flash.注意路径！！
            , fileObjName: 'context'//传递给后台程序的参数, 否则会接收不到！
            , uploader: uploader//后台处理程序. 注意路径！！
            , queueID: 'queue'//显示上传队列的容器
            , fileSizeLimit: "2MB"// 文件大小限制
            , fileTypeExts: '*.*' //'*.xls;*.xlsx;*.pdf;*.doc;*.docx;*.ppt;*.pptx;*.txt'// 扩展名
            , fileTypeDesc: '请选择 xls xlsx pdf doc docx ppt pptx txt 文件'// 文件说明
            , removeCompleted: true //上传后移除进度条
            , overrideEvents: ['onDialogClose', 'onUploadSuccess', 'onSelectError', 'onUploadError']//要重写的事件
            , onUploadSuccess: function (file, data, response) {
                //bootbox.alert("上传成功
                alert("上传成功");
                $("#imagespath").val(data).after('<img src="' + data + '" style="width: 150px;height: 100%" id="upload" />');
            }
            , onSelectError: function (file, errorCode, errorMsg) {
                var msgText = "上传失败\n";
                switch (errorCode) {
                    case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED:
                        //this.queueData.errorMsg = "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
                        msgText += "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
                        break;
                    case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                        msgText += "文件大小超过限制( " + this.settings.fileSizeLimit + " )";
                        break;
                    case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                        msgText += "文件大小为0";
                        break;
                    case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                        msgText += "文件格式不正确，仅限 " + this.settings.fileTypeExts;
                        break;
                    default:
                        msgText += "错误代码：" + errorCode + "\n" + errorMsg;
                }
                bootbox.alert(msgText);
            }
            , onUploadError: function (file, errorCode, errorMsg) {
                // Load the swfupload settings
                var settings = this.settings;

                // Set the error string
                var errorString = '上传失败';
                switch (errorCode) {
                    case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:
                        errorString = '服务器错误 (' + errorMsg + ')';
                        break;
                    case SWFUpload.UPLOAD_ERROR.MISSING_UPLOAD_URL:
                        errorString = 'Missing Upload URL';
                        break;
                    case SWFUpload.UPLOAD_ERROR.IO_ERROR:
                        errorString = 'IO Error';
                        break;
                    case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
                        errorString = 'Security Error';
                        break;
                    case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
                        alert('The upload limit has been reached (' + errorMsg + ').');
                        errorString = 'Exceeds Upload Limit';
                        break;
                    case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:
                        errorString = 'Failed';
                        break;
                    case SWFUpload.UPLOAD_ERROR.SPECIFIED_FILE_ID_NOT_FOUND:
                        break;
                    case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:
                        errorString = 'Validation Error';
                        break;
                    case SWFUpload.UPLOAD_ERROR.FILE_CANCELLED:
                        errorString = 'Cancelled';
                        this.queueData.queueSize -= file.size;
                        this.queueData.queueLength -= 1;
                        if (file.status == SWFUpload.FILE_STATUS.IN_PROGRESS || $.inArray(file.id, this.queueData.uploadQueue) >= 0) {
                            this.queueData.uploadSize -= file.size;
                        }
                        // Trigger the onCancel event
                        if (settings.onCancel) settings.onCancel.call(this, file);
                        delete this.queueData.files[file.id];
                        break;
                    case SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED:
                        errorString = 'Stopped';
                        break;
                }
                bootbox.alert(errorString);
            }
            , auto: true// 选择之后，自动开始上传
            , multi: true// 是否支持同时上传多个文件
            , queueSizeLimit: 5// 允许多文件上传的时候，同时上传文件的个数
    });
}