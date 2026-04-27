<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ckeditor.aspx.cs" Inherits="WebLab.ckeditor.ckeditor" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <title>CKEditor 4</title>
        <!-- Make sure the path to CKEditor is correct. -->
        <script src="./Standard/ckeditor.js" type="text/javascript"></script>
    </head>
    <body>
        <form>
            <h1>Ckeditor Standar</h1>
            <textarea name="editor1" id="editor1" rows="10" cols="80">
               
            </textarea>
            <script>
                // Replace the <textarea id="editor1"> with a CKEditor 4
                // instance, using default configuration.
                CKEDITOR.config.versionCheck = false;
                //Los navegadores modernos bloquean los botones de copiar,pegar y cortar
                CKEDITOR.replace('editor1', {
                    removeButtons: 'Paste,PasteText,PasteFromWord'
                });
              
            </script>
        </form>
    </body>
</html>

