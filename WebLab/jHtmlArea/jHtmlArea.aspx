<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jHtmlArea.aspx.cs" Inherits="WebLab.jHtmlArea.jHtmlArea" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src=""
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <textarea></textarea>
            <script>
                $(function () {
                    $("textarea").htmlarea();
                });
            </script>
        </div>
    </form>
</body>
</html>
