<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="PlacaEdit3.aspx.cs" Inherits="WebLab.Placas.PlacaEdit3" ValidateRequest="false" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    
<title>LABORATORIO</title> 

   
     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
         <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
<script type="text/javascript">
    function enterToTab(pEvent) {///solo para internet explorer
        if (window.event) // IE
        {
            if (window.event.keyCode == 13) {
                if (pEvent.srcElement.id.indexOf('txt') == 0) {
                    window.event.keyCode = 9;
                }
            }
        }

    }

    function Enter(field, event) {
        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            var i;
            for (i = 0; i < field.form.elements.length; i++)
                if (field == field.form.elements[i])
                    break;
            i = (i + 1) % field.form.elements.length;
            field.form.elements[i].focus();
            return false;
        }
        else
            return true;

    }
</script>
    <style type="text/css">

        /*Estilo de la tabla*/
        .myTabla, tr, td{
            border: 1px solid black;
            height:20px;
             border-style: solid; border-width: thin; line-height: normal; 
        }

        </style>  

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
      
     <div  style="width: 1600px" class="form-inline"   >
           <div class="panel panel-primary" runat="server" > 
                    <div class="panel-heading"> 
                        <h4 class="panel-title">PLACA SARS-COV2</h4>

                        </div>
                   <div class="panel-body"  >
                     <h3>    <asp:Label runat="server" Text="Label" id="lblEquipo"></asp:Label></h3>
                          
                         <h4>        <asp:Label runat="server" Text="lblNro" id="lblNro" ></asp:Label></h4>
                         
                       <h4>     Fecha:   <asp:Label runat="server" Text="lblFecha" id="lblFecha"></asp:Label></h4>
                          
                        
                        <h4>    Operadores:     
                       <asp:DropDownList ID="ddlOperador" runat="server">
                           
                       </asp:DropDownList>     
                       <asp:DropDownList ID="ddlOperador0" runat="server">
                           
                       </asp:DropDownList> <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Seleccione Operador 2" ControlToValidate="ddlOperador0" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="1"></asp:RangeValidator>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="Seleccione Operador 1" ControlToValidate="ddlOperador" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="1"></asp:RangeValidator>
                       </h4>
                          &nbsp;
                       <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                       <asp:Panel ID="pnlAlplex" runat="server" Visible="false">
                       

                           
                          
                       <table    class="myTabla"   cellpadding="2" cellspacing="3">
                           <tr>

                               <td style="height:100%;" align="center">
                                   <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                       
                                       <tr>
                                           <td  style="height:50px;"> </td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><asp:Label ID="Label1" runat="server" Text="A"  Width="25px"></asp:Label></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>B</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>C</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>D</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>E</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>F</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>G</strong></td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;"><strong>H</strong></td>
                                       </tr>
                                   </table>
                               </td>
                               <td  style="height:50px;" align="center">
   <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                       
       <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;1&nbsp;&nbsp;</strong></td>
                                       </tr>
                           <tr>
                               <td  style="height:50px; background-color: #C0C0C0;">
                                 &nbsp;&nbsp; NTC&nbsp;&nbsp;
                               </td>
                           </tr>
                             <tr>
                               <td  style="height:50px;">
                                  <input ID="txtB1" runat="server" class="form-control input-sm" tabindex="1"  onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>                                 
                           </tr>
      <tr>
          <td  style="height:50px;">
              <input ID="txtC1" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
          </td>
      </tr>

       <tr>
          <td  style="height:50px;">
              <input ID="txtD1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
              </td>
           </tr>
       <tr>
          <td  style="height:50px;">
             <input ID="txtE1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
              </td>
           </tr>
         <tr>
          <td  style="height:50px;">
             <input ID="txtF1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
               </td>
           </tr>
      <tr>
          <td  style="height:50px;">
            <input ID="txtG1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
          </td>
      </tr>
      <tr>
          <td  style="height:50px;">
          <input ID="txtH1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
          </td>
      </tr>

                       </table>
                               </td>
                               <td  style="height:50px;">
 <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                       
    <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;2&nbsp;&nbsp;</strong></td>
                                       </tr>
                           <tr>
                               <td  style="height:50px;" align="center">
                               <input ID="txtA2" runat="server" class="form-control input-sm" tabindex="0"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                              /></td>
                           </tr>
                             <tr>
                               <td  style="height:50px;" align="center">
                               <input ID="txtB2" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                           </tr>
    <tr>
        <td  style="height:50px;" align="center">
            <input ID="txtC2" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
        </td>
    </tr>
    <tr>
        <td  style="height:50px;" align="center">
            <input ID="txtD2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
        </td>
    </tr>

    <tr>
        <td  style="height:50px;" align="center">
           <input ID="txtE2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
        </td>
    </tr>
    <tr>
        <td  style="height:50px;" align="center">
          <input ID="txtF2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
        </td>
    </tr>

     <tr>
        <td  style="height:50px;" align="center">
           <input ID="txtG2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
            </td>
         </tr>
    <tr>
        <td  style="height:50px;" align="center">
           <input ID="txtH2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
        </td>
    </tr>
                       </table>

                               </td>

                               <td  style="height:50px;">
                                 <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;3&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;" align="center">
                                              <input ID="txtA3" runat="server" class="form-control input-sm" tabindex="2" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;" align="center">
                                            <input ID="txtB3" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;" align="center">
                                              <input ID="txtC3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>

                                        <tr>
                                           <td  style="height:50px;" align="center">
                                            <input ID="txtD3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;" align="center">
                                              <input ID="txtE3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;" align="center">
                                              <input ID="txtF3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;" align="center">
                                             <input ID="txtG3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                        <tr>
                                           <td  style="height:50px;" align="center">
                                              <input ID="txtH3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                           </td>
                                       </tr>
                                   </table>
                               </td>

                                 <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;4&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtA4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtB4" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtC4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtE4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtF4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                                <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtH4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                           </tr>
                                           </table>
                                     </td>

                                     <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;5&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtA5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB5" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtC5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtD5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                           <input ID="txtE5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtF5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtG5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtH5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                         </td>

                                    <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;" align="center"><strong>&nbsp;&nbsp;6&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtA6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB6" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtC6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtE6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtF6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtH6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                        </td>


                                  <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;7&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtA7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB7" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                            <input ID="txtC7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtE7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtF7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                           <input ID="txtG7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                            <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtH7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                      </td>

                                   <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;8&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtA8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB8" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtC8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtE8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtF8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtH8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                       </td>

                                 <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;9&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                            <input ID="txtA9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                              <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtB9" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtC9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtD9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtE9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                         <input ID="txtF9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                             <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtH9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>






                                           </table>
                                     </td>

                                 <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;10&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtA10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB10" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtC10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtE10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtF10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtH10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                     </td>

                                 <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;11&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtA11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB11" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtC11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtD11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtE11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtF11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtG11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                          <input ID="txtH11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                     </td>

                                    <td  style="height:50px;" align="center">
                                       <table style="border-style: solid; border-width: thin; line-height: normal;" cellpadding="2" cellspacing="3">
                        <tr>
                                           <td  style="height:50px;"><strong>&nbsp;&nbsp;12&nbsp;&nbsp;</strong></td>
                                       </tr>
                                       <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtA12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtB12" runat="server" class="form-control input-sm"   onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtC12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtD12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;"  
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                               <input ID="txtE12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtF12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                             <input ID="txtG12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           <tr>
                                           <td  style="height:50px;">
                                              <input ID="txtH12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                />
                                               </td>
                                           </tr>
                                           </table>
                                        </td>
                           </tr>
                       </table>
                     
                        </asp:Panel>

                           
                       <asp:Panel ID="pnlPromega" runat="server" Visible="true">
                       <table class="myTabla" cellpadding="2" cellspacing="3">
                           <tr>
                               <td colspan="2"> &nbsp;&nbsp;&nbsp;&nbsp;</td>
                               <td align="center" bgcolor="#CCCCCC"><h4>1</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>2</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>3</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>4</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>5</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>6</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>7</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>8</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>9</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>10</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>11</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>12</h4></td>                         

                           </tr>
                            <tr>
                               <td bgcolor="#CCCCCC"> &nbsp;&nbsp;</td>
                               <td bgcolor="#CCCCCC"> <h4>ID</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>NTC</h4></td>
                                 <td bgcolor="#CCCCCC"><input ID="txt_AB_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);"  onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                /></td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_AB_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_AB_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_AB_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_AB_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_AB_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_AB_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_AB_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_AB_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_AB_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_AB_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                                    

                           </tr>
                                <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h6>A</h6>
                                    </td>
                                      <td bgcolor="#CCCCCC"> <h6>N1</h6></td>
                                <td bgcolor="#CCCCCC"></td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             

                           </tr>
                    
                            <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6> B</h6></td>
                                  <td bgcolor="#CCCCCC"> <h6>RP</h6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             

                           </tr>
                            <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               
                               <td bgcolor="#CCCCCC"><h6>Res</h6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                 <td>&nbsp;</td> 
                           </tr>
                                 <tr>
                                     <td align="center" colspan="14">
                                        <br /></td>
                           </tr>
                                 <tr>
                                     <td align="center" bgcolor="#CCCCCC">
                                         <h4>&nbsp;</h4>
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <h4>ID</h4>
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                /></td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <input ID="txt_CD_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                     </td>
                           </tr>
                                 <tr>

                              <td align="center" bgcolor="#CCCCCC"  >
                                  <h6>C</h6>
                                     </td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                                     </td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                       <td>&nbsp;</td>             

                           </tr>

                           <tr>

                                <td align="center" bgcolor="#CCCCCC"  ><h6>D</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                                </td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                 <td>&nbsp;</td>             

                           </tr>
                               <tr>
                                   <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                   <td bgcolor="#CCCCCC">
                                       <h6>Res</h6>
                                   </td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                                   <td>&nbsp;</td>
                           </tr>
                               <tr>

                                <td align="center" colspan="14"  ><br /></td>
                               
                           </tr>
                            <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h4>&nbsp;</h4>
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <h4>ID</h4>
                                </td>
                                <td bgcolor="#CCCCCC">
                                   <input ID="txt_EF_1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                /></td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                 <td bgcolor="#CCCCCC">
                                    <input ID="txt_EF_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                           </tr>
                            <tr>

                              <td align="center" bgcolor="#CCCCCC"  ><h6>E</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                                </td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                 <td>&nbsp;</td>             

                           </tr>
                             <tr>

                               <td align="center" bgcolor="#CCCCCC"  ><h6>F</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                                 </td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                  <td>&nbsp;</td>             
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h6>Res</h6>
                               </td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                           </tr>
                           <tr>
                               <td align="center" colspan="14"><br /></td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h4>ID</h4>
                               </td>
                               <td bgcolor="#CCCCCC">
                                    <input ID="txt_GH_1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                /></td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_GH_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                               </td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6>G</h6></td>
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                               </td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6>H</h6></td>
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                               </td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h6>Res</h6>
                               </td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                           </tr>
                       </table> 

                           </asp:Panel>

                        <asp:Panel ID="pnlPromega2" runat="server" Visible="true">
                       <table class="myTabla" cellpadding="2" cellspacing="3">
                           <tr>
                               <td colspan="2"> &nbsp;&nbsp;&nbsp;&nbsp;</td>
                               <td align="center" bgcolor="#CCCCCC"><h4>1</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>2</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>3</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>4</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>5</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>6</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>7</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>8</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>9</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>10</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>11</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>12</h4></td>                         

                           </tr>
                            <tr>
                               <td bgcolor="#CCCCCC"> &nbsp;&nbsp;</td>
                               <td bgcolor="#CCCCCC"> <h4>ID</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4><asp:Label ID="Label2" runat="server" Text="NTC" Width="115px"></asp:Label></h4></td>
                                
                                 <td bgcolor="#CCCCCC"><input ID="txt_ABC_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);"  onblur="valNumero(this)"   type="text" style="font-size: large; width: 115px;" 
                                                /></td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_ABC_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_ABC_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_ABC_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_ABC_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_ABC_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_ABC_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                              <td bgcolor="#CCCCCC">
                                  <input ID="txt_ABC_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_ABC_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <input ID="txt_ABC_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <input ID="txt_ABC_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                </td>
                                                    

                           </tr>
                                <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h6>A</h6>
                                    </td>
                                      <td bgcolor="#CCCCCC"> <h6>N1</h6></td>
                                <td bgcolor="#CCCCCC"></td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             

                           </tr>
                    
                            <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6> B</h6></td>
                                  <td bgcolor="#CCCCCC"> <h6>N2</h6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                               <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             

                           </tr>
                            <tr>
                               <td align="center" bgcolor="#CCCCCC">c</td>
                               
                               <td bgcolor="#CCCCCC"><H6>RP</H6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                                <td>&nbsp;</td>
                              <td>&nbsp;</td>
                               <td>&nbsp;</td>
                               <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>             
                                <td>&nbsp;</td>             
                                 <td>&nbsp;</td> 
                           </tr>
                                 <tr>
                                     <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                     <td bgcolor="#CCCCCC">
                                         <h6>Res</h6>
                                     </td>
                                     <td bgcolor="#CCCCCC">&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                                     <td>&nbsp;</td>
                           </tr>
                                 <tr>
                                     <td align="center" colspan="14">
                                        <br /></td>
                           </tr>
                                 
                                 <tr>

                              <td align="center" bgcolor="#CCCCCC" colspan="2"  >
                                  <h6>D</h6>
                                    
                                     </td>
                               
                               <td> <h6>N1</h6>&nbsp;</td>
                              <td> <h6>N2</h6>&nbsp;</td>
                                <td> <h6>Rp</h6>&nbsp;</td>
                              <td>
                                  <h6>N1</h6>
                                  &nbsp;</td>
                                <td>
                                    <h6>N2</h6>
                                    &nbsp;</td>
                              <td>
                                  <h6>Rp</h6>
                                  &nbsp;</td>
                               <td>
                                   <h6>N1</h6>
                                   &nbsp;</td>
                               <td>
                                   <h6>N2</h6>
                                   &nbsp;</td>
                                <td>
                                    <h6>Rp</h6>
                                    &nbsp;</td>
                                <td>
                                    <h6>N1</h6>
                                    &nbsp;</td>             
                                <td>
                                    <h6>N2</h6>
                                    &nbsp;</td>             
                                       <td>
                                           <h6>Rp</h6>
                                           &nbsp;</td>             

                           </tr>
                           <tr>
                                     <td align="center" bgcolor="#CCCCCC" colspan="2">
                                        
                                         <h4>ID</h4>
                                     </td>
                                     <td bgcolor="#CCCCCC" colspan="3">
                                         <input ID="txt_D_123" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;"                                                 />
                                       </td>
                                     <td bgcolor="#CCCCCC" colspan="3">
                                         <input ID="txt_D_456" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                         &nbsp;&nbsp;
                                     </td>
                                     <td bgcolor="#CCCCCC" colspan="3">
                                         <input ID="txt_D_789" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                         &nbsp;&nbsp;
                                     </td>
                                     <td bgcolor="#CCCCCC" colspan="3">
                                         <input ID="txt_D_101112" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" onblur="valNumero(this)"  type="text" style="font-size: large; width: 115px;" 
                                                />
                                         &nbsp;&nbsp;
                                     </td>
                           </tr>
                        
                    
               
                               <tr>

                                <td align="center" colspan="14"  ><br /></td>
                               
                           </tr>
                           <tr>

                              <td align="center" bgcolor="#CCCCCC" colspan="2"  >
                                  <h6>E</h6>
                                    
                                     </td>
                               
                               <td> <h6>N1</h6>&nbsp;</td>
                              <td> <h6>N2</h6>&nbsp;</td>
                                <td> <h6>Rp</h6>&nbsp;</td>
                              <td>
                                  <h6>N1</h6>
                                  &nbsp;</td>
                                <td>
                                    <h6>N2</h6>
                                    &nbsp;</td>
                              <td>
                                  <h6>Rp</h6>
                                  &nbsp;</td>
                               <td>
                                   <h6>N1</h6>
                                   &nbsp;</td>
                               <td>
                                   <h6>N2</h6>
                                   &nbsp;</td>
                                <td>
                                    <h6>Rp</h6>
                                    &nbsp;</td>
                                <td>
                                    <h6>N1</h6>
                                    &nbsp;</td>             
                                <td>
                                    <h6>N2</h6>
                                    &nbsp;</td>             
                                       <td>
                                           <h6>Rp</h6>
                                           &nbsp;</td>             

                           </tr>
                       </table> 

                            <table cellpadding="2" cellspacing="3" class="myTabla">
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC" colspan="2">
                                        
                                        <h4>ID</h4>
                                        
                                    </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <input ID="txt_E_123" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                        &nbsp; </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <input ID="txt_E_456" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                        &nbsp;&nbsp; </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <input ID="txt_E_789" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                        &nbsp;&nbsp; </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_E_101112" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">&nbsp;</td>
                                    <td bgcolor="#CCCCCC">&nbsp;</td>
                                </tr>
                       
         
                 
                                <tr>
                                    <td align="center" colspan="14">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                    <td bgcolor="#CCCCCC">
                                        <h4>ID</h4>
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_1" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_2" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_3" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_4" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_5" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_6" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_7" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_8" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_9" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_10" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_11" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <input ID="txt_FGH_12" runat="server" class="form-control input-sm" onkeypress="javascript:return Enter(this, event);" type="text" style="font-size: large; width: 115px;" 
                                                />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">
                                        <h6>F</h6>
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <h6>N1</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">
                                        <h6>G</h6>
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                        <h6>N2</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">H</td>
                                    <td bgcolor="#CCCCCC">RP</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                    <td bgcolor="#CCCCCC">
                                        <h6>Res</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>

                           </asp:Panel>

                       </div>
                <div class="panel-footer">
                    <table width="70%" cellpadding="0" cellspacing="0">
                        <tr><td>
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar"  Width="150px" CssClass="btn btn-primary" ValidationGroup="0" OnClick="btnRegresar_Click"/>
                            </td>
                            <td align="right"> <asp:Button ID="btnGrabar" runat="server" Text="Guardar Parcial" OnClick="btnGrabar_Click"  Width="150px" CssClass="btn btn-primary" ValidationGroup="0" Visible="false"/>
                    <asp:Button ID="btnExportar" runat="server" Text="Exportar"  Width="150px" CssClass="btn btn-primary" Visible="false" OnClick="btnExportar_Click" />

                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="150px" CssClass="btn btn-primary" OnClick="btnImprimir_Click" />

                    <asp:Button ID="btnCerrar" runat="server" Text="Guardar y Cerrar" OnClick="btnCerrar_Click" Width="150px" CssClass="btn btn-primary" ValidationGroup="0" />
                      </td>
                        </tr>
                
                     </table>
                    </div>
               </div>
         </div>
     
</asp:Content>

   

