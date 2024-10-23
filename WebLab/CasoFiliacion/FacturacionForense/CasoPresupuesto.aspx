<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CasoPresupuesto.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.CasoPresupuesto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2>Presupuestos del Caso</h2>
                        <h3>Caso: <asp:Label ID="lblCaso" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label>
                        </h3> 

                        </div>
       	<div class="panel-body">	
    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idPresupuesto"  CssClass="table table-bordered bs-table" 
                                    
                                    EmptyDataText="No se vincularon casos al presupuesto" OnRowDataBound="gvLista_RowDataBound">
        <Columns>
            <asp:BoundField DataField="idPresupuesto" HeaderText="Nro. Presupuesto" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
            <asp:TemplateField HeaderText="Habilitado">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server"  Checked='<%# Convert.ToBoolean(Eval("caso")) %>'/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle BackColor="#999999" />
    </asp:GridView>
               </div>
         	<div class="panel-footer">
    <br />
    <br />
               <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Width="100px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" />  
                      </div>
         </div>  
               </asp:Content>
