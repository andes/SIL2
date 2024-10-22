<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProfesionalList.aspx.cs" Inherits="WebLab.Profesionales.ProfesionalList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

   
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">           

    	 <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">MEDICOS SOLICITANTES</h3>
                        </div>

				<div class="panel-body">
	   
	  

	

                                           <div  style="width:100%;height:450pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered bs-table" 
            CellPadding="1" DataKeyNames="idProfesional" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="8pt" Width="97%" 
                        EmptyDataText="No hay areas creadas" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="8pt" />
            <Columns>                
           
                
                 <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="numeroDocumento" HeaderText="Documento" />
                        
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
              
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        </div>
              

                    </div>
                   	<div class="panel-footer">

                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="100px"  />
                               </div>
       </div>
            

</div>	

   

</asp:Content>