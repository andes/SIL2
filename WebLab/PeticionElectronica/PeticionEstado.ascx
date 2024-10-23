<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeticionEstado.ascx.cs" Inherits="WebLab.PeticionElectronica.PeticionEstado1" %>



<div  runat="server"  style="width:315px;height:300px;overflow:scroll;overflow-x:hidden;" 
             align="left" > 
             <asp:ImageButton ID="imgAgregarMensaje" runat="server"  
                 ImageUrl="~/App_Themes/default/images/actualizar.gif" 
                 onclick="imgAgregarMensaje_Click" ToolTip="Actualziar" />
             <br />
                                            <asp:DataList ID="DataList1" runat="server"
  onitemdatabound="DataList1_ItemDataBound"                                                 
width="300px" CellPadding="4" ForeColor="#333333" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
>
                                               
                                            
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                              
                                                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                               
                                            
<HeaderTemplate>
    Peticiones Pendientes
   </HeaderTemplate>



                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>                                                           
                                                            <td  class="myLabelIzquierda">                                                                
                                                              <b><%# DataBinder.Eval(Container.DataItem, "fechaHoraRegistro")%></b>    &nbsp;  &nbsp;  &nbsp;  &nbsp;
                                                                  <asp:HyperLink ID="hplProtocoloEdit" NavigateUrl=<%# DataBinder.Eval(Container.DataItem, "idPeticion")%>  runat="server"><b>Protocolo</b></asp:HyperLink>                                                        
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                        
                                                            <td >
                                                                
                                                           
                                                                <b> Numero:</b><p> <%# DataBinder.Eval(Container.DataItem, "idPeticion")%> </p>
                                                            </td>
                                                            
                                                        </tr>      
                                                        <tr>                                                        
                                                            <td >
                                                           <b>   De:</b><%# DataBinder.Eval(Container.DataItem, "sector")%></td>                                                             
                                                            
                                                        </tr>   
                                                        
                                                            <tr>                                                        
                                                            <td >
                                                           <b>   Paciente:</b><%# DataBinder.Eval(Container.DataItem, "paciente")%></td>                                                             
                                                            
                                                        </tr>                                                           
                                                        <tr>
                                                        
                                                            <td >
                                                                
                                                           
                                                              <b>  Solicitante:</b><b style="color: #FF0000; font-weight: bold"> <%# DataBinder.Eval(Container.DataItem, "solicitante")%> </b>
                                                            </td>
                                                            
                                                        </tr>
                                                                                                                                                     
                                                    </table>
                                                </ItemTemplate>

                                            </asp:DataList></div>