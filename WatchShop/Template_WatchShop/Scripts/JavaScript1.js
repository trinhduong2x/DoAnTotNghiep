$('#btnSelectImage').on('click', function (e) {
    e.preventDefault();
    var finder = new CKFinder();
    finder.selectActionFunction = function (url) {
        $(('#m_image')).val(url);
    };
    finder.popup();
}
)
var ckEditor = CKEDITOR.replace('m_detail',
    { customConfig: '/ckeditor/config.js', });
var ckEditor = CKEDITOR.replace('basic-ckeditor',
    { customConfig: '/ckeditor/config.js', });
var ckEditor = CKEDITOR.replace('full-ckeditor',
    { customConfig: '/ckeditor/config.js', });