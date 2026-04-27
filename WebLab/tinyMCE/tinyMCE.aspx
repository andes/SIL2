<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tinyMCE.aspx.cs" Inherits="WebLab.tinyMCE.tinyMCE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/tinymce/4.9.11/tinymce.min.js" type="text/javascript"></script>
    
    <script>
        tinymce.init({
            selector: '#mytextarea'
        });
    </script>
</head>
<body>
    <h1>TinyMCE v4</h1>
    <form method="post">
      <textarea id="mytextarea">Hello, World!</textarea>
    </form>
</body>
</html>
