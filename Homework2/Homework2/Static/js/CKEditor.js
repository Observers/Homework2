$(document).ready(function () {
    // CKEDITORs 
    var myeditor = ClassicEditor.create(document.querySelector('#editor'))
        .then
        (editor => { editorinstance = editor; })
        .catch(error => {
            console.error(error);
        });
});


function encode() {
    const editorData = editorinstance.getData()
    const encodedHtml = encodeURIComponent(editorData)
    alert(encodedHtml)
    var data = {
        encodedHtml: encodedHtml,
    }
    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/CKEditor/Save',
        success: function (data) {
                window.location.replace(data.newUrl)
        }
    })
}