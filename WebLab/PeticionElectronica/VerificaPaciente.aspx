<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerificaPaciente.aspx.cs" Inherits="WebLab.PeticionElectronica.VerificaPaciente" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
 

  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   

    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <br />   
     <div align="left" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h1 class="panel-title">Recepcion de peticiones</h1>
                        </div>
       
				<div class="panel-body">	
  <img src="../App_Themes/default/images/ico_alerta.png" />
         <br />  
        <asp:TextBox runat="server"   ID="lblMensaje" ReadOnly="true" TextMode="MultiLine" Rows="3" Width="600px" />
                    <br />
                    Desea cambiar los datos del Paciente por los informados en la peticion?
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar y Continuar" OnClick="btnActualizar_Click" />

                    <asp:Button ID="btnIgnorar" runat="server" Text="Ignorar y Continuar" OnClick="btnIgnorar_Click" />

                    </div>
       </div>
         </div>
    </asp:Content>