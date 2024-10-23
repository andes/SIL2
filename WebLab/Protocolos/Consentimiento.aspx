<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Consentimiento.aspx.cs" Inherits="WebLab.Protocolos.Consentimiento"  MasterPageFile="~/Site1.Master"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

   <style>
          .thumb {
            height: 300px;
            border: 1px solid #000;
            margin: 10px 5px 0 0;
          }
        </style>

    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script src='<%=ResolveUrl("~/Protocolos/Webcam_Plugin/jquery.webcam.js") %>' type="text/javascript"></script>
<script type="text/javascript">
    var pageUrl = '<%=ResolveUrl("~/Protocolos/Consentimiento.aspx") %>'; 
   

$(function () {
    jQuery("#webcam").webcam({
        width: 320,
        height: 240,
        mode: "save",
        swffile: '<%=ResolveUrl("~/Protocolos/Webcam_Plugin/jscam.swf") %>', 
        debug: function (type, status) {
            $('#camStatus').append(type + ": " + status + '<br /><br />');
        },
        onSave: function (data) {
            $.ajax({
                type: "POST",
                url: pageUrl + "/GetCapturedImage",            
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("[id*=imgCapture]").css("visibility", "visible");
                    $("[id*=imgCapture]").attr("src", r.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            }); 
        },
        onCapture: function () {
          

            webcam.save(pageUrl);
        }
    });
});
function Capture() {
    webcam.capture();
    return false;
}

 
   

        
   </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
<%-- <object id="MyInkControl" height="100" width="100"
classid="CoDe_InkControl.dll#
CoDe_InkControl.InkontheWeb.InkWebControl"
VIEWASTEXT>
</object>--%>
        <div align="left" style="width: 800px" class="form-inline"  >
            <br />
        <div class="panel panel-default" >
                    <div class="panel-heading">
                      <h3>Caso de Filiacion <asp:Label ID="lblCasoNro" runat="server" 
                    
        ></asp:Label></h3>
                          <br />

                           <h2>Paso 3. Consentimiento Informado</h2>
                        </div>

				<div class="panel-body">	
                      


                    <table>
                        <tr>
                            <td>

                                <asp:Label ID="lblAlertaProtocolo" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="#CC3300" ></asp:Label>
                              
                            </td>
                         
                            <td rowspan="10" align="right">
  

                            </td>

                        </tr>

                        <tr>
                            <td><div class="form-group">
  <h4>  <label for="ejemplo_password_1">Apellido:</label> <asp:Label ID="lblApellido" runat="server" 
                    
        ></asp:Label></h4><input id="hdnProtocolo" runat="server" type="hidden" /></div></td>

                        </tr>

                        <tr>
                            <td> <h4>  <label for="ejemplo_password_1">Nombres:</label> <asp:Label ID="lblNombre" runat="server" 
                    
        ></asp:Label></h4></td>
                        </tr>

                        <tr>
                            <td> 
   
 
  <h4>  <label for="ejemplo_password_1">Fecha de Nacimiento:</label> <asp:Label ID="lblFechaNacimiento" runat="server" 
                    
        ></asp:Label></h4>
                            </td>
                        </tr>

                        <tr>
                            <td> 
  <h4>  <label for="ejemplo_password_1">Sexo:</label> <asp:Label ID="lblSexo" runat="server" 
                    
        ></asp:Label>
                    </h4>
                            </td>
                        </tr>


                         <tr>
                            <td> 
                             <h4>  <label for="ejemplo_password_1">Lugar:</label> 
                                <asp:TextBox ID="txtLugar" runat="server"></asp:TextBox></h4>
                            </td>
                        </tr>

                        <tr>
                            <td> 
  <h4>  <label for="ejemplo_password_1">Fecha:</label> 
      <input id="txtFecha" runat="server"  style="width: 100px" tabindex="3" type="text" /></h4>
                            </td>
                        </tr>

                        <tr>
                            <td> 
                               <table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="center">
            <u>WebCam</u>
        </td>
        <td>
        </td>
        <td align="center">
            <u>Foto tomada</u>
        </td>
    </tr>
    <tr>
        <td>
            <div id="webcam">
            </div>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:Image ID="imgCapture" runat="server" Style=" width: 320px;
                height: 240px" />
            <img src=".." id="img1" alt="" style="width:320px" />
        </td>
    </tr>
</table>
<br />
<asp:Button ID="btnCapture" CssClass="btn btn-info" Text="Tomar Foto" Width="100px" runat="server" OnClientClick="return Capture();" />
<br />
<span id="camStatus"></span></td>
                        </tr>

                       

                        <tr>
                            <td> 
                                <div id="divInputLoad">
                                <div id="divFileUpload">
                   <h4>   <label for="ejemplo_password_1"> Foto:</label>
                       <asp:FileUpload ID="trepadorFoto" runat="server"   class="form-control input-sm" onchange="readFile(this)"  />   </h4>
                                                    <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" runat="server"
                        ControlToValidate="trepadorFoto"
                        ErrorMessage="el archivo de la foto debe ser formato jpg " Font-Bold="True"
                        Font-Size="Medium"
                        ValidationExpression="(.*?)\.(jpg|JPG)$" ValidationGroup="0" Enabled="False"></asp:RegularExpressionValidator>

                                    </div>
                       <br />
                                <div id="file-preview-zone">
                                    </div>
            &nbsp;<%--<asp:FileUpload ID="trepadorHuella" runat="server"  class="form-control input-sm" onchange="showimagepreview(this)" />--%><div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
                                    </div>
                      
                            </td>
                        </tr>

                       

                        <tr>
                            <td> 
                                &nbsp;</td>
                        </tr>
                    </table>
   
 
                  

    </div>

            <div class="panel-footer">
<div class="form-group" id="datos" runat="server">
                      
                     
                     &nbsp;<asp:Button ID="btnRegresar" runat="server" Text="Regresar"  CssClass="btn btn-info"    Width="141px" OnClick="btnRegresar_Click"/>

                           <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="subir_Click" Visible="false"    Width="250px" ValidationGroup="0"   >  Generar Consentimiento en WORD</asp:Linkbutton>

                        &nbsp;<asp:LinkButton ID="lnkBuscar0" runat="server" CssClass="btn btn-info" OnClick="lnkBuscar0_Click"    Width="250px" ValidationGroup="0"   >  Generar Consentimiento</asp:Linkbutton>

                        <asp:LinkButton ID="lnkProtocolo" runat="server" CssClass="btn btn-info"    Width="200px" OnClick="lnkProtocolo_Click"   >  Generar Protocolo</asp:Linkbutton>

                        <br />
                        
                         
    
                     

                    </div>
            </div>
            </div>
          
    </div>
   
    <script type="text/javascript">
        function readFile(input) {
            
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var filePreview = document.createElement('img');
                    filePreview.id = 'file-preview';
                    //e.target.result contents the base64 data from the image uploaded
                    filePreview.src = e.target.result;
                    console.log(e.target.result);

                    var previewZone = document.getElementById('file-preview-zone');
                    previewZone.appendChild(filePreview);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        var fileUpload = document.getElementById('trepadorFoto');
        fileUpload.onchange = function (e) {
            readFile(e.srcElement);
        }
      </script>
</asp:Content>