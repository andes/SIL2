/**
 * ============================================================
 * CKEditorWebForms
 * ============================================================
 *
 * Integra CKEditor con ASP.NET WebForms + Anthem.NET
 * evitando el RequestValidation (XSS validation).
 *
 * ------------------------------------------------------------
 * PROBLEMA
 * ------------------------------------------------------------
 *
 * ASP.NET WebForms detecta HTML enviado en Request.Form
 * y lanza:
 *
 * HttpRequestValidationException
 *
 * especialmente cuando un textarea contiene contenido HTML:
 *
 * <p>texto</p>
 *
 * CKEditor reemplaza un textarea HTML tradicional,
 * por lo tanto el contenido enriquecido produce
 * errores de validación en ASP.NET.
 *
 * Además Anthem.NET utiliza callbacks AJAX propios,
 * que no siempre disparan:
 *
 * - submit
 * - __doPostBack
 * - eventos jQuery
 *
 * ------------------------------------------------------------
 * SOLUCIÓN
 * ------------------------------------------------------------
 *
 * 1) El textarea NO debe tener:
 *
 *      runat="server"
 *      name=""
 *
 * 2) El contenido HTML se copia manualmente a un HiddenField.
 *
 * 3) El HiddenField viaja al servidor codificado con:
 *
 *      encodeURIComponent()
 *
 * 4) En el servidor:
 *
 *      Server.UrlDecode(hfHtml.Value)
 *
 * 5) El HTML debe sanitizarse antes de persistirlo.
 *
 * ------------------------------------------------------------
 * USO
 * ------------------------------------------------------------
 *
 * HTML:
 *
 *      <textarea id="editor1"></textarea>
 *
 *      <asp:HiddenField
 *          ID="hfHtml"
 *          runat="server" />
 *
 *
 * JS:
 *
 *      CKEditorWebForms.init(
 *          'editor1',
 *          '<%= hfHtml.ClientID %>'
 *      );
 *
 *
 * SERVER:
 *
 *      string html =
 *          Server.UrlDecode(hfHtml.Value);
 *
 * ============================================================
 */

(function (window) {

    window.CKEditorWebForms = {

        /**
            * Inicializa CKEditor y sincroniza el HTML
            * hacia un HiddenField seguro para WebForms.
            *
            * @param {string} editorId
            * ID del textarea usado por CKEditor.
            *
            * @param {string} hiddenFieldId
            * ID del HiddenField ASP.NET.
            *
            * @param {object} config
            * Configuración opcional de CKEditor.
        */

        init: function (editorId, hiddenFieldId, config) {

            config = config || {};

            CKEDITOR.config.versionCheck = false;

            // evitar doble instancia
            if (!CKEDITOR.instances[editorId]) {

                CKEDITOR.replace(editorId, config);
            }

            /**
            * Copia el HTML del editor
            * al HiddenField codificado.
            */

            function protegerCKEditor() {

                if (typeof CKEDITOR !== 'undefined') {

                    var editor = CKEDITOR.instances[editorId];

                    if (editor) {

                        var data = editor.getData();

                        var hf = document.getElementById(hiddenFieldId);

                        if (hf) {

                            hf.value = encodeURIComponent(data);
                            return hf.value;
                        }
                    }
                }
            }


            if (typeof Anthem_FireCallBackEvent === 'function') {

                var oldFire = Anthem_FireCallBackEvent;

                Anthem_FireCallBackEvent = function () {

                    protegerCKEditor();

                    return oldFire.apply(this, arguments);
                };
            }

            /**
            * Hook submit tradicional WebForms.
            */
            window.onload = function () {

                var forms = document.getElementsByTagName("form");

                if (forms.length > 0) {

                    var oldSubmit = forms[0].submit;

                    forms[0].submit = function () {

                        protegerCKEditor();

                        oldSubmit.call(forms[0]);
                    };
                }
            };


            /**
             * Hook global para Anthem.NET callbacks.
             */
            document.onclick = function () {

                protegerCKEditor();
            };


            // Exponer función manualmente
            window.protegerCKEditor = protegerCKEditor;
        }
    };

})(window);