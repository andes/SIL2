<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebLab.ckeditor.WebForm1" %>

<!DOCTYPE html>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <title>CKEditor 4</title>
        <!-- Make sure the path to CKEditor is correct. -->
        <script src="./Basic/ckeditor.js" type="text/javascript"></script>
    </head>
    <body>
        <form>
            <h1>Ckeditor Basic</h1>
            <textarea name="editor1" id="editor1" rows="10" cols="80">
               
            </textarea>
            <script>
                // Replace the <textarea id="editor1"> with a CKEditor 4
                // instance, using default configuration.
                CKEDITOR.config.versionCheck = false;
                CKEDITOR.replace('editor1');
              
            </script>
        </form>
    </body>
</html>

