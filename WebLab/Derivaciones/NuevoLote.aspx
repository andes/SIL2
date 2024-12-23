<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoLote.aspx.cs" Inherits="WebLab.Derivaciones.NuevoLote" MasterPageFile="~/Site1.Master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />
    <script type="text/javascript" src="../script/jquery.min.js"></script>
    <script type="text/javascript" src="../script/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>

</asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    &nbsp;
<div class="" style="width: 600px; padding-left: 50px" >
    <div class="panel panel-default" style="text-align: center">
       
        <div class="panel-heading">
            <h3 class="panel-title" style="align-items">
                <asp:Label ID="lblTitulo" runat="server" Text="Nuevo Lote generado"></asp:Label>
            </h3>
        </div>
        

            
        <div class="panel panel-body">
            <div class="row">
                    <div class=" col-lg-12  "> <h4><asp:Label ID="lblLote" Text="Observaciones" runat="server"/></h4>   <br />  </div>
            </div>
            
            <div class="row">
                    <div class="col-lg-12">
                    <img alt="" src="../App_Themes/default/images/pdf.jpg" />&nbsp;
                        <asp:LinkButton ID="lnkPDF" runat="server" CssClass="myLittleLink" OnClick="lnkPDF_Click">Descargar PDF</asp:LinkButton><br />
                </div>
            </div>
            <div class="row">
                <br />
                <div class="col-lg-12">
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink"
                    NavigateUrl="~/Derivaciones/Derivados2.aspx?tipo=informe">Regresar</asp:HyperLink>
                </div>
                
            </div>
  
        </div>
                                     
    </div>
</div>
       
</asp:Content>
