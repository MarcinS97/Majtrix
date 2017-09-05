<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="ArticleEdit.aspx.cs" Inherits="HRRcp.Portal.ArticleEdit" ValidateRequest="false" %>

<%--<%@ Register src="../Controls/Portal/cntArticleEdit.ascx" tagname="cntArticleEdit" tagprefix="uc1" %>--%>
<%@ Register Src="~/Portal/Controls/cntArticleEdit.ascx" TagName="cntArticleEdit" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script type="text/javascript" src="//cdn.tinymce.com/4/tinymce.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/tinymce.min.js"></script>
    <script type="text/javascript">
        tinymce.init(
            {
                selector: 'textarea',
                body_class: 'editor-content',
                height: 700,
                language: 'pl',
                content_css: "styles/editor.css",
                plugins: [
                  'advlist autolink lists link image charmap print preview anchor',
                  'searchreplace visualblocks code',
                  'insertdatetime media table contextmenu paste code emoticons textcolor colorpicker'
                ],
                paste_data_images: true,
                convert_urls: false,
                toolbar: 'insertfile undo redo | fontselect fontsizeselect forecolor backcolor | paste | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | imgupload | emoticons',
                setup: function (editor) {

                    editor.on('init', function (editor) {
                        editor.target.editorCommands.execCommand("fontName", false, "sans-serif");
                        editor.target.editorCommands.execCommand("fontSize", false, "14px");
                    });

                    editor.addButton('imgupload', {
                        text: 'Dodaj zdjęcie',
                        icon: false,
                        onclick: function () {
                            $('#uploader').trigger('click');
                            $("#uploader").on("change", function () {
                                var that = $(this);
                                var file = that.prop('files')[0];

                                var reader = new FileReader();

                                reader.addEventListener("load", function () {
                                    var base64 = reader.result;
                                    editor.insertContent("<img src='" + base64 + "' />");
                                }, false);

                                if (file) {
                                    var data = reader.readAsDataURL(file);
                                }
                            });
                        }
                    });
                }
            });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <uc1:cntArticleEdit ID="cntArticleEdit" runat="server" />
        <input id="uploader" type="file" class="hidden" />
    </div>
</asp:Content>
