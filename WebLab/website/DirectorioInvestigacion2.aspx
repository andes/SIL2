<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DirectorioInvestigacion2.aspx.cs" Inherits="WebLab.website.DirectorioInvestigacion2" %>

<!doctype html>
  <html lang="en">
    <head>
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>Laboratorio Central</title>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
        
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
        <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400;500;700&display=swap" rel="stylesheet">
        <link rel="shortcut icon" href="images/icolabo.ico">
        <link href="css/style.css" rel="stylesheet" type="text/css">
        <link href="css/tareas.css" rel="stylesheet" type="text/css">
        <link href="css/menu.css" rel="stylesheet" type="text/css">
        <link href="css/footer.css" rel="stylesheet" type="text/css">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
        <script src="js/slides.min.jquery.js"></script>

        <style type="text/css">
            .GridPager a, .GridPager span{
                display: block;
                height: 25px;
                width: 20px;
                text-align: center;
                text-decoration: none;
            }
        
            .GridPager a{
                background-color: #f5f5f5;
                color: #969696;
                border: 1px solid #969696;
            }
        
            .GridPager span{
                background-color: #509743;
                color: #000;
                border: 1px solid #509743;
            }
        </style>
    </head>
    <body>
        <div id="ventana-flotante">

        <!--<a class="cerrar" href="javascript:void(0);" >X</a>-->

            <div id="contenedor">
                <div class="contenido">
                    <a href="http://www.saludnqn.gob.ar/sips" target="_blank">RESULTADOS DE DERIVACIONES AQUI</a>
                </div>
            </div>
        </div>

       <div id="header-wrap">
        <header>
            <div class="neuquen">
                <div class="row">
                    <div class="col-sm-2 ">
                        <a href="../index.html" title="">
                            <img src="../website/images/logos-03.svg" id="logo-menu" />
                        </a>
                    </div>
                    <div class="col-sm-6"></div>

                    <div class="col-sm-2 ">
                        <a href="http://www.saludneuquen.gob.ar/" target="_blank">
                            <img src="../website/images/logos-04.svg" id="logo-neuquen" />
                           
                        </a>
                    </div>
                    <div class="col-sm-2">
                        <a href="http://www.saludneuquen.gob.ar/" target="_blank">
                            <img src="../website/images/logos-05.svg" id="logo-escudo" />
                        </a>
                    </div>
                </div>

            </div>





           


        </header>

    </div>
    <!-- end header wrap -->
    <nav class="navbar navbar-default" role="navigation">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
        </div>

        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav navbar-left">
                <li>
                    <a href="../index.html">Laboratorio Central</a>
                </li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Institucional</a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                        <a class="dropdown-item" href="../website/historia.html">El Laboratorio</a>
                        <a class="dropdown-item" href="../website/quienessomos_plantelprofesional.html">Quienes Somos</a>
                    </div>
                </li>
                <li><a href="../website/instalaciones.html">Tecnología e Innovación</a></li>
                <li><a href="../website/portalweb2.html">Resultados</a></li>
                <li><a href="../website/contacto.html">Dejanos tu consulta</a></li>
            </ul>
        </div>
    </nav>


        <form id="form1" runat="server" method="post">

            <div class="panel panel-success">
                <div style=" padding-top: 5px; padding-left: 10px; padding-bottom:25px; padding-right:10px;" class="form-inline">
                    <p>
                        <a href="investigacion.html" class="readmore">Investigación</a> 
                        <a href="DirectorioInvestigacion.aspx" class="readmore">Buscador</a>
                    </p>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridView1"  runat="server" AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5" GridLines="None" ShowHeader="False" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <ItemTemplate>
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <div class="panel-title"><strong><%# Eval("title") %></strong></div>                      
                                            </div>
                                            <div class="panel-body">
                                                <div style="margin-left:10px;margin-top:4px; margin-bottom:10px;font-style:italic; ">
                                                    <%# Eval("info") %>  
                                                </div>
                                                <div style="margin-left:10px;margin-top:1px; margin-bottom:1px; ">
                                                    <%# Eval("lugar") %>  
                                                </div>
                                                <div style="margin-left:10px;margin-top:1px; margin-bottom:1px; ">
                                                    <strong>Abstract</strong><br />	
                                                    <%# Eval("abstract") %>  
                                                </div>
                                            </div>
                                        </div>                                      
                                    </ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle CssClass="GridPager" Height="25px" ForeColor="Black" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>
        </form>
        <!-- end service-->

        <!-- Footer -->
       <section class="footer">
        <div class="container">
            <div class="row">

                <div class="col-md-4 col-sm-6">
                    <h3>Sitios de interés</h3>
                    <ul>
                        <li><a href="http://www.saludneuquen.gob.ar/" target="_blank">Ministerio de Salud de Neuquén</a></li>
                        <li><a href="https://www.argentina.gob.ar/salud/" target="_blank">Ministerio de Salud de la Nación</a></li>
                        <li><a href="https://www.argentina.gob.ar/salud/anlis"target="_blank">Instituto Malbrán</a></li>
                        <li><a href="https://www.argentina.gob.ar/salud/incucai" target="_blank">Cucai</a></li>
                    </ul>
                </div>

                <div class="col-xs-8 col-sm-4 col-md-4">
                    <h3>Nuestra Ubicación</h3>
                    <ul>
                        <li><a target="_blank" href="https://www.google.com.ar/maps/place/Gregorio+Mart%C3%ADnez+65,+Neuqu%C3%A9n/@-38.9542714,-68.0850468,17z/data=!3m1!4b1!4m5!3m4!1s0x960a33b019b82653:0x72282fe614342b5e!8m2!3d-38.9542714!4d-68.0828581"><img src="../website/images/map.png" alt="map" /></a></li>
                        <li>Gregorio Martínez 65 - (8300) - Neuquén</li>
                        <li>email: labcen@yahoo.com.ar</li>
                    </ul>

                </div>

                <div class="col-md-4 col-sm-6">
                    <h3>El Laboratorio</h3>
                    <ul>
                        <li><a href="../website/quienessomos_plantelprofesional.html">Quienes somos</a></li>
                        <li><a href="../website/contacto.html">Contáctenos</a></li>
                    </ul>
                </div>

                <div class="col-sm-2">
                    <p><b>Compartir en redes</b></p>
                    <a href="javascript:void(0);" onclick="window.open(&quot;http://www.facebook.com/sharer.php?u=http://laboratoriocentral.saludneuquen.gob.ar/index.html/&quot;,&quot;gplusshare&quot;,&quot;toolbar=0,status=0,width=548,height=325&quot;);" rel="nofollow" title="Compartir en Facebook">
                        <i class="fa fa-facebook-square fa-2x"></i>
                    </a>

                    <a href="javascript:void(0);" onclick="window.open(&quot;http://twitter.com/home?status=leyendo%20http://laboratoriocentral.saludneuquen.gob.ar/index.html/&quot;,&quot;gplusshare&quot;,&quot;toolbar=0,status=0,width=548,height=325&quot;);" rel="nofollow" title="Compartir en Twitter">
                        <i class="fa fa-twitter fa-2x"></i>
                    </a>

                    <!--<li class="list-inline-item"><a href="javascript:void(0);" onclick="window.open(&quot;https://www.instagram.com/?hl=es-la?url=http://laboratoriocentral.neuquen.gob.ar/index.html/&quot;,&quot;gplusshare&quot;,&quot;toolbar=0,status=0,width=548,height=325&quot;);" rel="nofollow" title="Compartir en Instrangram"><i class="fa fa-instagram"></i></a></li>-->

                    <a href="javascript:void(0);" onclick="window.open(&quot;https://plus.google.com/share?url=http://laboratoriocentral.saludneuquen.gob.ar/index.html/&quot;,&quot;gplusshare&quot;,&quot;toolbar=0,status=0,width=548,height=325&quot;);" rel="nofollow" title="Compartir en Google+">
                        <i class="fa fa-google-plus fa-2x"></i>
                    </a>
                </div>
            </div>
        </div>
    </section>


        <!-- Footer -->

    </body>

    
</html>