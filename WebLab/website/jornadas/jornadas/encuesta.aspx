<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="encuesta.aspx.cs" Inherits="WebLab.website.jornadas.encuesta" %>

<!DOCTYPE html>

<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
    <title>X Jornadas Bioquímicas Provinciales</title>
<link href="css/bootstrap.min.css" rel="stylesheet" />
<link href="css/style.css" rel="stylesheet" />
<link rel="stylesheet" href="css/font-awesome.min.css">
<script src="js/jquery.min.js"></script>
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
<link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900' rel='stylesheet' type='text/css'>
    <script src='https://www.google.com/recaptcha/api.js'></script>


   
</head>
<body>
<div class="header">
  <div class="container"><a class="navbar-brand" href="index.html"><h3>X Jornadas Bioquímicas Provinciales</h3></a>
            <div class="menu">
                <a class="toggleMenu" href="#"><img src="images/nav_icon.png" alt="" /> </a>
                <ul class="nav" id="nav">
                    <li class="current"><a href="index.html">Jornadas</a></li>
                    <li><a href="temario.html">Programa</a></li>
                    <li><a href="inscripcion.aspx">Inscripciones</a></li>                   
                    <li><a href="auspiciantes.html">Auspiciantes</a></li>
                    <li><a href="services.html">Expositores</a></li>
                    <!--<li><a href="index.html">Certificados</a></li>--> 
                    <div class="clear"></div>
                        
                </ul>
      <script type="text/javascript" src="js/responsive-nav.js"></script> 
    </div>
  </div>
</div>
<!--<div class="about">
  <div class="container">
    <section class="title-section">
      <h1 class="title-header">Contact Us</h1>
    </section>
  </div>
</div>-->
<div class="contact">
   
  <div class="container">
      <h2>Encuesta</h2>
      <%--<div class="row contact_top">
      <div class="col-md-4 contact_details">
        <h5>Dirección:</h5>
        <div class="contact_address">Gregorio Martínez 65, (8300) Neuquén - Argentina	</div>
      </div>
      
      <div class="col-md-4 contact_details">
        <h5>Email :</h5>
        <div class="contact_mail"> labcen@yahoo.com.ar</div>
      </div>
    </div>--%> 
  <div class="contact_bottom">    
        <h4>Esta encuesta es anónima. Nos sirve para evaluar distintos aspectos de estas jornadas y ser una guía para quienes las organizarán el año próximo.</h4>
      <br />
      <br />
        <form id="form1" runat="server" method="post">
    ¿Cómo calificarías el temario de estas jornadas?
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="temario" ErrorMessage="Dato requerido" ValidationGroup="0"></asp:RequiredFieldValidator>
<br>
            <asp:RadioButtonList ID="temario" runat="server">
                <asp:ListItem Value="Muy interesante">Muy interesante</asp:ListItem>
                <asp:ListItem Value="Interesante">Interesante</asp:ListItem>
                <asp:ListItem Value="Poco interesante">Poco interesante</asp:ListItem>
                <asp:ListItem Value="Nada interesante">Nada interesante</asp:ListItem>
            </asp:RadioButtonList>



<hr/>

¿Cómo fue el desarrollo de las jornadas?
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="desarrollo" ErrorMessage="Dato requerido" ValidationGroup="0"></asp:RequiredFieldValidator>
<br>
                 <asp:RadioButtonList ID="desarrollo" runat="server">
                <asp:ListItem Value="Se respetaron los tiempos y fueron amenas">Se respetaron los tiempos y fueron amenas</asp:ListItem>
                <asp:ListItem Value="No se respetaron los tiempos pero aun asi su desarrollo fue ameno">No se respetaron los tiempos pero aun asi su desarrollo fue ameno</asp:ListItem>
                <asp:ListItem Value="No se respetaron los tiempos y no fueron amenas">No se respetaron los tiempos y no fueron amenas</asp:ListItem>
                
            </asp:RadioButtonList>
 
              <hr />

¿Cuál fue el aporte de las charlas a tu tarea diaria?
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="aporte" ErrorMessage="Dato requerido" ValidationGroup="0"></asp:RequiredFieldValidator>
            <br />
                 <asp:RadioButtonList ID="aporte" runat="server">
                <asp:ListItem Value="Me permitieron pensar en mejoras o en reorganizar mi tarea">Me permitieron pensar en mejoras o en reorganizar mi tarea</asp:ListItem>
                <asp:ListItem Value="Me permitieron actualizarme en temas referentes a mi tarea">Me permitieron actualizarme en temas referentes a mi tarea</asp:ListItem>
                <asp:ListItem Value="Me permitieron conocer el funcionamiento de redes provinciales y otros aspectos de mi tarea que desconocía">Me permitieron conocer el funcionamiento de redes provinciales y otros aspectos de mi tarea que desconocía</asp:ListItem>
                     <asp:ListItem Value="No hubo aportes relevantes para mi tarea diaria">No hubo aportes relevantes para mi tarea diaria</asp:ListItem>
                     <asp:ListItem Value="Las charlas no fueron acordes a mi tarea diaria">Las charlas no fueron acordes a mi tarea diaria</asp:ListItem>
                
            </asp:RadioButtonList>
 
       

      <hr/>

¿Cuáles fueron las charlas que más te gustaron y por qué?
<br>

&nbsp;<asp:TextBox ID="txtCharlasBuenas" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:TextBox>

      <hr/>

¿Cuáles fueron las charlas que menos te gustaron y por qué?
            <br />
            <asp:TextBox ID="txtCharlasMalas" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:TextBox>
<br>
    <hr/>

¿Cuál considera que es el objetivo de las Jornadas Bioquímicas?
<br>

&nbsp;<asp:TextBox ID="txtObjetivos" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:TextBox>
 <hr/>

¿Estaría dispuesto a abonar una entrada para repartir el costo de las Jornadas entre los concurrentes?
<br>

&nbsp;   <asp:RadioButtonList ID="rdbCostos" runat="server">
                <asp:ListItem Value="SI">SI</asp:ListItem>
                <asp:ListItem Value="NO">NO</asp:ListItem>
              
                
            </asp:RadioButtonList>
&nbsp;<hr/>


Dejanos alguna sugerencia para las próximas jornadas
<br>

            <asp:TextBox ID="txtSugerencia" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:TextBox>
&nbsp;<hr/>

              <div class="g-recaptcha" data-sitekey="6LfUBE4UAAAAACNddOssX1gprgJJX8g7CnWVTHim"></div>
                &nbsp;<asp:Button ID="Button1" runat="server" Text="Enviar" CssClass="btn btn-success" ValidationGroup="0" OnClick="Button1_Click1"/>
               </form>
    </div>
   
  </div>
</div>
   <div class="footer">
            <div class="footer_bottom">
             
              
              <div class="follow-us">
                
                  <a class="fa fa-facebook social-icon" href="http://www.facebook.com/sharer.php?u=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Facebook"></a>
               

                  <a class="fa fa-twitter social-icon" href="http://twitter.com/home?status=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank"  rel="nofollow" title="Compartir en Twitter"></a>
                  <a class="fa fa-linkedin social-icon" href="http://www.linkedin.com/shareArticle?url=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Linkedin"></a>
                  <a class="fa fa-google-plus social-icon" href="https://plus.google.com/share?url=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Google+"></a>
                
                      
</div>
                <div class="copy">
                    <p>Copyright &copy; 2018 <a target="_blank" href="http://laboratoriocentral.neuquen.gob.ar" rel="nofollow">Laboratorio Central</a></p>
                </div>
                
              
</div>
            <a href="http://www.saludneuquen.gov.ar/" target="_blank">
                <img src="../images/logo_ministerio.png" />
            </a>
          
</div>
     <script type="text/javascript">
        // Get your number elements
        var numberElements = document.getElementsByClassName("number");
        // Loop through each one
        for (var i = 0; i < numberElements.length; i++) {
            // Get your current element
            numberElements[i].type = 'number';
        }
</script>
<script src="js/bootstrap.min.js"></script> 
<script src="js/jquery.flexslider.js"></script>
</body>
</html>
