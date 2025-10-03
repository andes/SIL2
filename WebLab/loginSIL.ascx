<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="loginSIL.ascx.cs" Inherits="WebLab.loginSIL" %>


 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

 
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
   
 


<asp:Login ID="Login1" runat="server" 
        DisplayRememberMe="False" LoginButtonText="Acceder" 
        onauthenticate="Login1_Authenticate" 
        PasswordRequiredErrorMessage="Contraseña es requerida" 
        TitleText="Nueva autenticación de usuario" UserNameLabelText="Usuario:" 
        UserNameRequiredErrorMessage="Usuario es requerido" 
        FailureText=""            >
    <LayoutTemplate>
       <div class="container" >
    <div class="row">
      	<div class="d-flex justify-content-center h-100">
            <div class="row">
                <div class="col-lg-6 col-md-8 mx-auto">

                    <!-- form card login -->
                    <div class="card rounded shadow shadow-sm">
                        <div class="card header" >
                      
                        </div>
                         <div class="card-body">
                                   <div class="form-group">
    <label for="ejemplo_password_1">Usuario</label>

                                <asp:TextBox ID="UserName" runat="server" class="form-control input-sm" placeholder="usuario"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Usuario es requerido" ToolTip="Usuario es requerido" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                                       </div>
                             
                                 <div class="form-group">
    <label for="ejemplo_password_1">Contraseña</label>
    
                                                             <asp:TextBox ID="Password" runat="server"  class="form-control input-sm" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Contraseña es requerida" 
                                    ToolTip="Contraseña es requerida" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
  </div>
                             </div>
        
            </div>
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            
                                <asp:Button ID="LoginButton" runat="server"  CommandName="Login" 
                               class="btn btn-primary" Width="120px" Text="Acceder" 
                                    ValidationGroup="ctl00$Login1" />
                    </div>
                </div>
            </div>
        </div>
       </div>                    
    </LayoutTemplate>
    </asp:Login>

<!-- Terminos y condiciones de uso-->
<div class="modal fade" tabindex="-1" role="dialog" id="modalTerminosCondiciones">
  <div class="modal-dialog modal-lg" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Condiciones de Uso - Sistema Informático de Laboratorios</h4>
      </div>
      <div class="modal-body">
         <section>
    <article>
      <p>
        El <strong>Ministerio de Salud de la Provincia de Neuquén</strong>, en el contexto del <strong>Sistema Informático de Laboratorios</strong>, se encuentra comprometido y comparte la responsabilidad de resguardar los derechos a la intimidad y confidencialidad de la información de los pacientes.
      </p>

      <p>
        Cualquier integrante de los equipos de salud que, en función de hacer posible el proceso de asistencia sanitaria a dichas personas, tenga acceso a esta información almacenada en el Sistema Informático de Laboratorios, debe cumplir con la normativa jurídica que protege estos derechos:
      </p>

      <h4>Leyes Nacionales:</h4>
      <ol>
        <li>Artículos 18, 19 y 43 de la Constitución Nacional (derechos a la intimidad y habeas data).</li>
        <li>Artículo 52 del Código Civil y Comercial Nacional (derecho a la intimidad).</li>
        <li>Artículo 11 de la Ley Nacional 17.132 de Ejercicio de la Medicina, Odontología y actividades de colaboración con ambas disciplinas (secreto profesional).</li>
        <li>Artículo 2, incisos c y d, de la Ley Nacional 26.529 de Derechos del Paciente en su Relación con los Profesionales e Instituciones de la Salud (derechos a la intimidad y confidencialidad).</li>
        <li>Artículos 8 y 10 de la Ley Nacional 25.326 de Protección de Datos Personales.</li>
      </ol>

      <h4>Leyes Provinciales:</h4>
      <ol>
        <li>Ley 578 de Ejercicio de la medicina, odontología y actividades de colaboración.</li>
        <li>Artículo 8 de la Ley 2611, de derechos y obligaciones de los pacientes y usuarios de los servicios de salud públicos y privados de la Provincia (derecho a la confidencialidad y a la intimidad).</li>
        <li>Ley 2399 de Protección de Datos Personales.</li>
      </ol>

      <p>
        Ante incumplimiento de estas obligaciones debe saber que puede tener consecuencias legales. Asimismo, desde el Ministerio de Salud se procederá a:
      </p>

      <ul>
        <li>Iniciar sumario administrativo por falta grave.</li>
        <li>Suspender en forma automática los permisos de acceso al Sistema Informático de Laboratorios.</li>
        <li>En caso de ser profesional, se informará al comité de ética deontológico correspondiente.</li>
      </ul>

      <asp:CheckBox runat="server" ID="cb_aceptaTerminos" Text="Acepto las condiciones de uso del Sistema Informático de Laboratorios" onchange="habilitarBotonAceptar();" />
        
    </article>
  </section>
      </div>
      <div class="modal-footer">
          <div class="col-md-4">
              <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
          </div>
        
          <div class="col-md-4">
              <asp:Button runat="server" Enabled="false" ID="btn_aceptarTerminosCondiciones" CssClass="btn btn-success" Text="Aceptar" OnClick="btn_aceptarTerminosCondiciones_Click"  UseSubmitBehavior="true"  CausesValidation="false" />
          </div>
        
      </div>
    </div>
  </div>
</div><!-- /.modal -->

<script type="text/javascript">

    document.getElementById('<%= cb_aceptaTerminos.ClientID %>').checked = false;

    function habilitarBotonAceptar() {
        check = document.getElementById('<%= cb_aceptaTerminos.ClientID %>');
        btnAceptar = document.getElementById('<%= btn_aceptarTerminosCondiciones.ClientID %>');
        if (check.checked) {
            btnAceptar.disabled  = false;
        } else {
            btnAceptar.disabled = true;
        }
       
    }

</script>