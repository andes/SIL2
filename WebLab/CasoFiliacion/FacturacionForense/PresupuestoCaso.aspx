<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PresupuestoCaso.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.PresupuestoCaso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2>Casos del Presupuesto</h2>
                        <h3>Presupuesto: <asp:Label ID="lblPresupuesto" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label>
                        </h3> 

                        </div>
       	<div class="panel-body">	
    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idCasoFiliacion"  CssClass="table table-bordered bs-table" 
                                    
                                    EmptyDataText="No se vincularon casos al presupuesto" OnRowDataBound="gvLista_RowDataBound">
        <Columns>
            <asp:BoundField DataField="idCasoFiliacion" HeaderText="Nro. Caso" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
            <asp:BoundField DataField="factura" HeaderText="Factura" />
        </Columns>
        <HeaderStyle BackColor="#999999" />
    </asp:GridView>
               </div>
         	<div class="panel-footer">
               <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-primary" Width="100px"  Text="Regresar" 
         ValidationGroup="0" OnClick="btnRegresar_Click" />  
                      
                      </div>
         </div>  
               </asp:Content>
