<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObraSocialSel.aspx.cs" Inherits="WebLab.Protocolos.ObraSocialSel" %>
<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="shortcut icon" href="App_Themes/default/images/favicon.ico"/>
    <link rel="stylesheet" href="App_Themes/default/bootstrap.min.css" />	
	<link rel="stylesheet" type="text/css" href="App_Themes/default/style.css" />
      <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
<script src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script src='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptMgr" runat="server">
                                                        </asp:ScriptManager>
                                                        <asp:UpdatePanel ID="upOSocial" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="tableOSociales" class="tabla3">
                                                                     <tr>
                                                                        <td  valign="top">Financiador / O.S:</td>
                                                                          </tr>
                                                                    <tr>
                                                                       
                                                                        <td>
                                                                            <div style="font-family: Verdana; position:relative; z-index:1;">
                                                                                <uc1:OSociales ID="OSociales" runat="server" TabIndex="10" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
         <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" AccessKey="s" CausesValidation="true"
                                          ToolTip="Alt+Shift+S: Guarda Protocolo"  onclick="btnGuardar_Click" CssClass="btn btn-primary" Width="80px" TabIndex="24"  />
    </div>
    </form>
</body>
</html>
