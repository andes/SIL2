<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Derivacion.aspx.cs" Inherits="WebLab.Protocolos.Derivacion" MasterPageFile="../Site1.Master" %>


<%@ Register src="ProtocoloList.ascx" tagname="ProtocoloList" tagprefix="uc1" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

  

  
    <style type="text/css">
        .style1
        {
            height: 28px;
        }
        .style2
        {
            width: 16px;
        }
    </style>

  

  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <br />   &nbsp;

        <div align="left" style="width:800px;" class="form-inline" >
 <table  width="800px" align="center" >
					<tr>
		<td  rowspan="6">
    
            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
    
        </td>
		<td  rowspan="6"  > &nbsp;</td>
        <td  rowspan="6"  style="vertical-align: top" >
               <div class="panel panel-primary">
                    <div class="panel-heading"> Recepción de Derivación
  </div>
                    <div class="panel-body">
        <table>
        

					<tr>
		<td class="myLabelIzquierda">Efector Derivante:</td><td class="style1">
            <asp:DropDownList ID="ddlEfector" runat="server" class="form-control">
            </asp:DropDownList> <asp:RangeValidator ID="rvEfector" runat="server" 
                                ControlToValidate="ddlEfector" ErrorMessage="Efector Derivante" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
        </td>
                                            </tr>

                                            			<tr>
		<td class="myLabelIzquierda">Nro. Protocolo:</td><td ><asp:TextBox ID="txtNumeroProtocolo" runat="server" 
                                                                class="form-control" Width="100px" TabIndex="1"></asp:TextBox>
                                                            
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                ControlToValidate="txtNumeroProtocolo" ErrorMessage="Nro. Protocolo" 
                                                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                 <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click" ValidationGroup="0"   Width="90px" >
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>                                           
                                                            </td>
                                            </tr>

                                            			<tr>
		<td   colspan="2">&nbsp;</td>
                                            </tr>

                                            			<tr>
		<td  ></td><td class="style7">
                 
                                                
<%--            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  CssClass="myButton" Height="25px" Width="100px"
                                                                onclick="btnBuscar_Click" ValidationGroup="0" TabIndex="2" />--%>
                                                            
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                                HeaderText="De completar los datos requeridos:" ShowMessageBox="True" 
                                                                ValidationGroup="0" />
                                                            
                                                            </td>
                                            </tr>

                                            			<tr>
		<td   colspan="2" style="vertical-align: bottom">    
        <hr />
        <div class="alert alert-info">Importante: 
            Para que este proceso capture correctamente las prácticas derivadas
            deberá asegurarse tener el mismo código de práctica el derivante y el receptor.</div>
        
            </td>
        </tr>
        </table>

                        </div>
                   </div>
        </td>
		
                                            </tr>
                                            </table>
</div>
		    
 </asp:Content>