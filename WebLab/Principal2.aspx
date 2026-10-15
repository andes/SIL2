<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Principal.aspx.cs"
    Inherits="WebLab.Principal"
    MasterPageFile="~/Site1.Master" %>

<%@ Register Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajx" %>

<%@ Register Src="~/PeticionList.ascx"
    TagPrefix="uc1"
    TagName="PeticionList" %>

<%@ Register Src="~/seguimientoCovid.ascx"
    TagPrefix="uc1"
    TagName="seguimientoCovid" %>


<asp:Content ID="content1"
    ContentPlaceHolderID="head"
    runat="server">

    <link rel="stylesheet"
        href="bootstrap/3.3.7/bootstrap.min.css" />

    <script src="bootstrap/3.3.7/jquery.min.js"></script>
    <script src="bootstrap/3.3.7/bootstrap.min.js"></script>

    <link rel="shortcut icon"
        href="website/website/images/icolabo.ico" />

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />


    <style type="text/css">

        .principal-container {
            padding: 15px 20px 25px 20px;
            max-width: 1400px;
        }


        /* =====================================================
           SESION
           ===================================================== */

        .sesion-contenedor {
            text-align: right;
            margin-bottom: 15px;
        }

        .panel-sesion {
            display: inline-block;
            background: #f8f8f8;
            border: 1px solid #dddddd;
            border-radius: 5px;
            padding: 6px 10px;
        }

        .panel-sesion-titulo {
            color: #666666;
            font-size: 12px;
            font-weight: bold;
            margin-right: 8px;
        }

        .panel-sesion .btn {
            margin-left: 3px;
        }


        /* =====================================================
           BOTONES COMPACTOS
           ===================================================== */

        .btn-compacto {
            width: auto !important;
            min-width: 0 !important;
            height: auto !important;
            display: inline-block !important;
            padding: 4px 10px !important;
            font-size: 12px !important;
            line-height: 18px !important;
        }


        /* =====================================================
           ALERTA
           ===================================================== */

        .principal-alerta {
            margin-bottom: 15px;
        }

        .principal-alerta .alert {
            margin-bottom: 0;
            padding: 10px 15px;
        }


        /* =====================================================
           TARJETAS
           ===================================================== */

        .acceso-card {
            background: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 6px;
            padding: 17px;
            margin-bottom: 20px;
            min-height: 195px;
            position: relative;

            -webkit-transition: all .2s ease;
            transition: all .2s ease;
        }

        .acceso-card:hover {
            border-color: #bbbbbb;
            box-shadow: 0 3px 10px rgba(0,0,0,.10);
        }

        .acceso-card h3 {
            font-size: 18px;
            font-weight: 600;
            color: #444444;
            margin-top: 5px;
            margin-bottom: 10px;
        }

        .acceso-card p {
            font-size: 13px;
            color: #777777;
            min-height: 38px;
        }

        .acceso-icono {
            font-size: 30px;
            margin-bottom: 5px;
        }


        /* TURNOS */

        .card-turnos {
            border-top: 4px solid #5cb85c;
        }

        .card-turnos .acceso-icono {
            color: #5cb85c;
        }


        /* RECEPCION */

        .card-recepcion {
            border-top: 4px solid #5bc0de;
        }

        .card-recepcion .acceso-icono {
            color: #5bc0de;
        }


        /* DOCUMENTOS */

        .card-documentos {
            border-top: 4px solid #337ab7;
        }

        .card-documentos .acceso-icono {
            color: #337ab7;
        }


        /* CAPACITACION */

        .card-capacitacion {
            border-top: 4px solid #f0ad4e;
        }

        .card-capacitacion .acceso-icono {
            color: #f0ad4e;
        }


        /* =====================================================
           RECEPCION
           ===================================================== */

        .proximo-protocolo {
            background: #f7f7f7;
            border-radius: 4px;
            padding: 6px 8px;
            margin: 7px 0;
            font-size: 11px;
        }

        .numero-protocolo {
            color: #cc3300;
            font-size: 14px;
            font-weight: bold;
        }


        /* =====================================================
           PANELES INFERIORES
           ===================================================== */

        .panel-principal {
            background: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 6px;
            padding: 15px;
            margin-bottom: 20px;
        }

        .panel-principal-titulo {
            margin: 0 0 12px 0;
            padding-bottom: 9px;
            border-bottom: 1px solid #eeeeee;
            font-size: 16px;
            font-weight: 600;
            color: #444444;
        }

        .panel-principal-titulo i {
            color: #337ab7;
            margin-right: 5px;
        }


        /* =====================================================
           MENSAJES
           ===================================================== */

        .mensajeria-contenedor {
            max-height: 400px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .mensaje-item {
            padding: 7px;
        }

        .mensaje-fecha {
            font-size: 11px;
            color: #777777;
        }

        .mensaje-separador {
            margin: 5px 0;
        }


        /* =====================================================
           RESPONSIVE
           ===================================================== */

        @media (max-width: 767px) {

            .principal-container {
                padding: 10px;
            }

            .sesion-contenedor {
                text-align: left;
            }

            .panel-sesion {
                display: block;
            }

            .panel-sesion-titulo {
                display: block;
                margin-bottom: 5px;
            }

            .panel-sesion .btn {
                margin-left: 0;
                margin-right: 3px;
                margin-bottom: 3px;
            }

            .acceso-card {
                min-height: auto;
            }
        }

    </style>

</asp:Content>


<asp:Content ID="content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">


    <ajx:ToolkitScriptManager
        ID="ToolkitScriptManager1"
        runat="server"
        EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </ajx:ToolkitScriptManager>


    <div class="principal-container">


        <!-- =====================================================
             SESION
             ===================================================== -->

        <div class="row">

            <div class="col-sm-12 sesion-contenedor">

                <div class="panel-sesion">

                    <span class="panel-sesion-titulo">

                        <i class="fa fa-user"></i>
                        Sesión

                    </span>


                    <asp:LinkButton
                        ID="lnkCambioEfector"
                        runat="server"
                        PostBackUrl="~/LoginEfector.aspx"
                        CssClass="btn btn-default btn-compacto">

                        <i class="fa fa-exchange"></i>
                        Cambiar Efector

                    </asp:LinkButton>


                    <a href="login.aspx"
                        class="btn btn-default btn-compacto">

                        <i class="fa fa-sign-out"></i>
                        Cambiar Usuario

                    </a>

                </div>

            </div>

        </div>



        <!-- =====================================================
             ALERTA
             ===================================================== -->

        <div class="row principal-alerta"
            ID="divAlerta"
            runat="server">

            <div class="col-sm-12">

                <div class="alert alert-danger">

                    <strong>

                        <i class="fa fa-exclamation-triangle"></i>
                        ALERTA

                    </strong>

                    &nbsp;&nbsp;

                    <asp:Label
                        ID="lblAlerta"
                        runat="server"
                        Text="">
                    </asp:Label>

                </div>

            </div>

        </div>



        <!-- =====================================================
             ACCESOS PRINCIPALES
             ===================================================== -->

        <div class="row">


            <!-- =================================================
                 TURNOS
                 ================================================= -->

            <div class="col-sm-3"
                ID="pnlTurno"
                runat="server">

                <div class="acceso-card card-turnos">

                    <div class="acceso-icono">
                        <i class="fa fa-calendar"></i>
                    </div>

                    <h3>
                        Turnos
                    </h3>

                    <p>
                        Generación y administración de turnos programados.
                    </p>


                    <a href="Turnos/TurnoList.aspx?tipo=generacion"
                        target="_parent"
                        class="btn btn-success btn-compacto">

                        <i class="fa fa-calendar"></i>
                        Turnos

                    </a>

                </div>

            </div>



            <!-- =================================================
                 RECEPCION DE MUESTRAS
                 ================================================= -->

            <div class="col-sm-3"
                ID="pnlRecepcion"
                runat="server">

                <div class="acceso-card card-recepcion">

                    <div class="acceso-icono">
                        <i class="fa fa-flask"></i>
                    </div>

                    <h3>
                        Recepción de Muestras
                    </h3>

                    <p>
                        Generación y recepción de protocolos.
                    </p>


                    <div class="proximo-protocolo">

                        <asp:Label
                            ID="lblProximoProtocolo"
                            runat="server"
                            Text="Próximo Número de Protocolo Disponible:"
                            Font-Bold="True">
                        </asp:Label>

                        <br />

                        <asp:Label
                            ID="lblProximoProtocolo1"
                            runat="server"
                            CssClass="numero-protocolo"
                            Text="">
                        </asp:Label>


                        <asp:LinkButton
                            ID="lnkUltimoNumeroSector"
                            runat="server"
                            OnClick="lnkUltimoNumeroSector_Click"
                            Visible="False">

                            Ver

                        </asp:LinkButton>

                    </div>


                    <asp:Panel
                        ID="pnlProtocolo"
                        runat="server"
                        Width="100%">

                        <a href="Protocolos/Default2.aspx?idServicio=3&idUrgencia=0"
                            target="_parent">

                            Microbiología

                        </a>

                    </asp:Panel>


                    <asp:Panel
                        ID="pnlTurnoRecepcion"
                        runat="server">

                        <a href="Turnos/TurnoList.aspx?tipo=recepcion"
                            target="_parent">

                            Pacientes con turnos

                        </a>

                    </asp:Panel>

                </div>

            </div>



            <!-- =================================================
                 DOCUMENTOS RED DE LABORATORIOS
                 ================================================= -->

            <div class="col-sm-3"
                ID="pnlDocumentosRDL"
                runat="server">

                <div class="acceso-card card-documentos">

                    <div class="acceso-icono">
                        <i class="fa fa-file-text-o"></i>
                    </div>

                    <h3>
                        Documentos de la Red de Laboratorios
                    </h3>

                    <p>
                        Acceso a documentos, instructivos e información
                        de la Red de Laboratorios.
                    </p>


                    <asp:Button
                        ID="btnDocumentosRDL"
                        runat="server"
                        Text="Ver documentos"
                        CssClass="btn btn-primary btn-compacto"
                         />

                </div>

            </div>



            <!-- =================================================
                 CAPACITACION
                 ================================================= -->

            <div class="col-sm-3">

                <div class="acceso-card card-capacitacion">

                    <div class="acceso-icono">
                        <i class="fa fa-graduation-cap"></i>
                    </div>

                    <h3>
                        Capacitación
                    </h3>

                    <p>
                        Material de capacitación y ayuda para el uso
                        del sistema.
                    </p>


                    <asp:LinkButton
                        ID="LinkButton1"
                        runat="server"
                        PostBackUrl="~/Capacita/Capacita.aspx"
                        CssClass="btn btn-warning btn-compacto">

                        <i class="fa fa-book"></i>
                        Ingresar

                    </asp:LinkButton>

                </div>

            </div>

        </div>



        <!-- =====================================================
             INFORMACION INFERIOR
             ===================================================== -->

        <div class="row">


            <!-- =================================================
                 SEGUIMIENTO
                 ================================================= -->

            <div class="col-sm-6"
                ID="pnlSeguimiento"
                runat="server">

                <div class="panel-principal">

                    <h4 class="panel-principal-titulo">

                        <i class="fa fa-line-chart"></i>
                        Seguimiento

                    </h4>


                    <uc1:seguimientoCovid
                        runat="server"
                        ID="seguimientoCovid" />

                </div>

            </div>



            <!-- =================================================
                 MENSAJES INTERNOS
                 ================================================= -->

            <div class="col-sm-6"
                ID="mensajeria"
                runat="server">

                <div class="panel-principal">

                    <h4 class="panel-principal-titulo">

                        <i class="fa fa-comments-o"></i>
                        Mensajes Internos


                        <asp:ImageButton
                            ID="imgAgregarMensaje"
                            runat="server"
                            ImageUrl="~/App_Themes/default/images/svn_added.png"
                            OnClick="imgAgregarMensaje_Click"
                            ToolTip="Agregar Mensaje"
                            Style="float:right;" />

                    </h4>


                    <div class="mensajeria-contenedor">

                        <asp:DataList
                            ID="DataList1"
                            runat="server"
                            OnItemDataBound="DataList1_ItemDataBound"
                            Width="100%"
                            CellPadding="4"
                            ForeColor="#333333"
                            BorderColor="#CCCCCC"
                            BorderStyle="Solid"
                            BorderWidth="1px">


                            <ItemStyle
                                BackColor="#F7F6F3"
                                ForeColor="#333333" />


                            <SelectedItemStyle
                                BackColor="#E2DED6"
                                Font-Bold="True"
                                ForeColor="#333333" />


                            <ItemTemplate>

                                <div class="mensaje-item">


                                    <div class="mensaje-fecha">

                                        <b>

                                            <%# DataBinder.Eval(
                                                Container.DataItem,
                                                "fechaHoraRegistro") %>

                                        </b>


                                        <asp:HyperLink
                                            ID="hplMensajeEdit"
                                            NavigateUrl='<%# DataBinder.Eval(
                                                Container.DataItem,
                                                "idMensaje") %>'
                                            runat="server"
                                            Style="float:right;">

                                            Eliminar

                                        </asp:HyperLink>

                                    </div>


                                    <hr class="mensaje-separador" />


                                    <div>

                                        <b>De:</b>

                                        <%# DataBinder.Eval(
                                            Container.DataItem,
                                            "remitente") %>

                                    </div>


                                    <div>

                                        <b>Para:</b>

                                        <span style="color:#cc3300;font-weight:bold;">

                                            <%# DataBinder.Eval(
                                                Container.DataItem,
                                                "destinatario") %>

                                        </span>

                                    </div>


                                    <div style="margin-top:5px;">

                                        <b>Mensaje:</b>

                                        <p>

                                            <%# DataBinder.Eval(
                                                Container.DataItem,
                                                "mensaje") %>

                                        </p>

                                    </div>


                                </div>

                            </ItemTemplate>

                        </asp:DataList>

                    </div>

                </div>

            </div>

        </div>



        <!-- =====================================================
             PETICIONES
             ===================================================== -->

        <div class="row">

            <div class="col-sm-12">

                <uc1:PeticionList
                    runat="server"
                    ID="PeticionList1" />

            </div>

        </div>



        <!-- =====================================================
             CONTROLES ANTIGUOS DE PROTOCOLOS
             
             Se mantienen para no romper Principal.aspx.cs,
             pero nunca se muestran en pantalla.
             ===================================================== -->

        <div ID="Div1"
            runat="server"
            style="display:none;">

            <asp:Button
                ID="btnProtocoloEfector"
                runat="server"
                Text="Protocolos Efector"
                OnClick="btnProtocoloEfector_Click" />

            <asp:GridView
                ID="gvProtocolosxEfector"
                runat="server">
            </asp:GridView>

            <asp:Button
                ID="btnSISA"
                runat="server"
                Text="Protocolos SISA"
                OnClick="btnSISA_Click" />

            <asp:GridView
                ID="gvProtocolosxSISA"
                runat="server">
            </asp:GridView>

        </div>


        <!-- =====================================================
             pnlNuevoUsuario
             
             Se mantiene oculto para compatibilidad si el
             code-behind todavía referencia este control.
             ===================================================== -->

        <div ID="pnlNuevoUsuario"
            runat="server"
            style="display:none;">
        </div>


    </div>

</asp:Content>