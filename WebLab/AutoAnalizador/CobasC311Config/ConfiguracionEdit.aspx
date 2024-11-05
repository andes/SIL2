<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.CobasC311Config.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .style1 {
            width: 618px;
        }

        .style4 {
            width: 132px;
        }

        .style5 {
            width: 476px;
        }
    </style>

</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading"> 
    <h3 class="panel-title">Configuración SIL - EQUIPO Cobas C311</h3>
                        </div>

				<div class="panel-body">

        <table align="center" width="600" class="style1">
          
            <tr>
                <td class="style4">Area :</td>
                <td class="style5">
                    <anthem:DropDownList ID="ddlArea" runat="server" class="form-control input-sm"
                        AutoCallBack="True" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                    </anthem:DropDownList>

                    

                </td>

            </tr>
              <tr>
                <td class="style4"> Análisis del LIS:</td>
                <td class="style5">
                    <anthem:DropDownList ID="ddlItem" runat="server" class="form-control input-sm">
                    </anthem:DropDownList>

                    <asp:RangeValidator ID="RangeValidator1" runat="server"
                        ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999"
                        MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                    </td>
                    </tr>
            <tr>
                <td class="style4">ID en Equipo:</td>
                <td class="style5">
                    <anthem:DropDownList ID="ddlItemEquipo" runat="server" Width="150px" class="form-control input-sm">
                       
                    </anthem:DropDownList>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                            runat="server" ControlToValidate ="ddlItemEquipo"
                            ErrorMessage="*" 
                            InitialValue="0" ValidationGroup="0">
                     </asp:RequiredFieldValidator>
                </td>
            </tr>
            &nbsp;
            <tr>
                <td class="style4">Tipo de muestra:</td>
                <td class="style5">
                    <asp:DropDownList ID="ddlTipoMuestra" runat="server" Width="150px" class="form-control input-sm">
                        <asp:ListItem Selected="True" Text="Seleccione" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Suero/Plasma" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Orina" Value="2"></asp:ListItem>
                        <asp:ListItem Text="CSF" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Suprnt" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Otros" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                     
                    <asp:RequiredFieldValidator ID="rfvMuestra" 
                            runat="server" ControlToValidate ="ddlTipoMuestra"
                            ErrorMessage="*" 
                            InitialValue="0" ValidationGroup="0">
                     </asp:RequiredFieldValidator>

               </td>
            </tr>
            &nbsp;
           <tr>
                <td class="style4">Prefijo especial:</td>
                <td class="style5">
                    <asp:DropDownList ID="ddlPrefijo" runat="server" Width="150px" class="form-control input-sm">
                        <asp:ListItem Selected="True" Value="">Seleccione</asp:ListItem>
                        <asp:ListItem Text="ORI" Value="ORI"></asp:ListItem>
                        <asp:ListItem Text="PTO" Value="PTO"></asp:ListItem>
                        <asp:ListItem Text="PP" Value="PP"></asp:ListItem>
                        <asp:ListItem Text="GLI" Value="GLI"></asp:ListItem>
                        <asp:ListItem Text="LCR" Value="LCR"></asp:ListItem>
                        </asp:DropDownList>
              
                    &nbsp;(Sólo para casos como Glucemia Post ingesta, Glucemia Post Almuerzo, etc.)</td>

            </tr>

            <tr>
                <td class="style4">&nbsp;</td>
                <td class="style5">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                        OnClick="btnGuardar_Click2" Width="100px"  CssClass="btn btn-primary" ValidationGroup="0" />
                </td>

            </tr>
            <tr>
                <td class="myLabelIzquierda" colspan="2">
                    <asp:Label ID="lblMensajeValidacion" runat="server"
                        ForeColor="#FF3300"></asp:Label>
                </td>

            </tr>
            <tr>
                <td class="myLabelIzquierda" colspan="2">
                    <hr />
                </td>

            </tr>
            <tr>
                <td class="myLabelIzquierda" colspan="2">

                    <div style="border: 1px solid #999999; height: 500px; width: 610px; overflow: scroll;">
                        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px"
                            DataKeyNames="id" OnRowCommand="gvLista_RowCommand"  CssClass="table table-bordered bs-table"
                            OnRowDataBound="gvLista_RowDataBound"
                            EmptyDataText="No se configuraron prácticas para el equipo">
                            <Columns>
                                <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="prefijo" HeaderText="Prefijo" />
                                <asp:BoundField DataField="ItemCobas" HeaderText="ID en Equipo" />
                                <asp:BoundField DataField="idItemSil" HeaderText="ID en Sil" />
                                <asp:BoundField DataField="tipoMuestra" HeaderText="Tipo de Muestra" />
                                <asp:TemplateField HeaderText="Habilitado">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkStatus" runat="server"
                                            AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"
                                            Checked='<%# Convert.ToBoolean(Eval("Habilitado")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar"
                                            ImageUrl="../../App_Themes/default/images/eliminar.jpg"
                                            OnClientClick="return PreguntoEliminar();" />
                                    </ItemTemplate>
                                    <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                </asp:TemplateField>
                            </Columns>

                            <HeaderStyle BackColor="#999999" />
                        </asp:GridView>
                        <br />
                    </div>

                </td>

            </tr>
        </table>

        <br />

    </div>


    <script language="javascript" type="text/javascript">


        function PreguntoEliminar() {
            if (confirm('¿Está seguro de eliminar?'))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>
