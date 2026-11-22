describe("Altas de Protocolos", () => {

    it("Alta de protocolo [Laboratorio]", () => {

        cy.login();
        cy.seleccionarEfector();

        //Creo un protocolo sin turno
        cy.get("[id$='mnuPrincipal']")
            .contains("Recepcion")
            .trigger("mouseover");

        cy.get("[id$='mnuPrincipal']")
            .contains("Pacientes sin turno")
            .click();
        //Pongo el DNI del paciente para buscarlo y crear el protocolo
        cy.fixture("datos").then(u => {
            cy.get("input[id$='txtDni']").type(u.pacienteDNI);
        });
        //Elijo el sexo del paciente
        cy.get("select[id$='ddlSexo']").select("Femenino");

        cy.get("input[id$='btnBuscar']").click();

        //De la busqueda elijo el primero
        cy.get("[id$='gvLista']")
            .find("input[type='submit'][value='Protocolo']")
            .first()
            .click();

        //Pantalla de protocolo NUEVO
        //Ingreso determinaciones manualmente
        cy.fixture("determinaciones").then(d => {

            d.codigos.forEach((valor, index) => {

                cy.get("[id$='tabla']")
                    .find(`input[id$='Codigo_${index}']`)
                    .should("be.visible")
                    .clear()
                    .type(valor)
                    .type('{enter}');
            });

        });

        cy.encabezadoProtocolo();
        cy.cargarDiagnostico();

        //"LABORATORIO CENTRAL"
        cy.fixture("datos").then(x => {
            if (x.efector == "LABORATORIO CENTRAL") {
                //Caracter SISA:
                cy.get("select[id$='ddlCaracter']").select(x.caracterSisa);
            }
        });

        cy.get('body').click(0, 0);
        cy.wait(1000); 
        // Interceptar el guardado para obligar a Cypress a esperar la respuesta
        cy.intercept('POST', '**/ProtocoloEdit2.aspx*').as('guardadoFinal');

        cy.get("input[id$='btnGuardar']").click({ force: true });
        // Esperar a que el servidor responda positivamente
        cy.wait('@guardadoFinal');
    });

    it("Alta de protocolo [Microbiologia]", () => {

        cy.login();
        cy.seleccionarEfector();

        //Creo un protocolo sin turno
        cy.get("[id$='mnuPrincipal']")
            .contains("Microbiología")
            .trigger("mouseover");

        cy.get("[id$='mnuPrincipal']")
            .contains("Recepción de Muestras")
            .click();

        

        //Pongo el DNI del paciente para buscarlo y crear el protocolo
        cy.fixture("datos").then(u => {
            cy.get("input[id$='txtDni']").type(u.pacienteDNI);
        });
        //Elijo el sexo del paciente
        cy.get("select[id$='ddlSexo']").select("Femenino");

        cy.get("input[id$='btnBuscar']").click();

        //De la busqueda elijo el primero
        cy.get("[id$='gvLista']")
            .find("input[type='submit'][value='Protocolo']")
            .first()
            .click();

        //Pantalla de protocolo NUEVO
        //Ingreso determinaciones manualmente
        cy.fixture("determinaciones").then(d => {

            d.micro.forEach((valor, index) => {

                cy.get("[id$='tabla']")
                    .find(`input[id$='Codigo_${index}']`)
                    .should("be.visible")
                    .clear()
                    .type(valor)
                    .type('{enter}');
            });

        });

        cy.encabezadoProtocolo();
        cy.cargarDiagnostico();

        //Tipo de Muestra
        cy.get("input[id$='txtCodigoMuestra']").type("cm").type('{enter}');
      
        //"LABORATORIO CENTRAL"
        cy.fixture("datos").then(x => {
            if (x.efector == "LABORATORIO CENTRAL") {
                //Caracter SISA:
                cy.get("select[id$='ddlCaracter']").select(x.caracterSisa);
                //Nro. Hisopado
                cy.get("input[id$='txtNumeroOrigen2']").type(x.numeroHisopado);
                
            }
        });

        //Tiempo de espera para el anthem
        cy.get('body').click(0, 0);
        cy.wait(1000);
        // 2. Interceptar el guardado para obligar a Cypress a esperar la respuesta
        cy.intercept('POST', '**/ProtocoloEdit2.aspx*').as('guardadoFinal');

        cy.get("input[id$='btnGuardar']").click({ force: true });
        // Esperar a que el servidor responda positivamente
        cy.wait('@guardadoFinal');
    });

    it("Alta de protocolo [No Paciente]", () => {

        cy.login();
        cy.seleccionarEfector();


        //Creo un protocolo sin turno
        cy.get("[id$='mnuPrincipal']")
            .contains("Muestras de No Pacientes")
            .trigger("mouseover");

        cy.get("[id$='mnuPrincipal']").contains("Recepcion de Muestras").click();


        //Pantalla de protocolo NUEVO
      
        cy.get("[id$='txtCodigoMuestra']").type("agua");//.type('{enter}');  //Tipo de Muestra

        cy.get("[id$='txtDescripcionProducto']").type("Test"); //Descripcion

        cy.get("select[id$='ddlConservacion']").select("Agua");//Conservacion
         
        cy.get("select[id$='ddlSectorServicio']").select("1 - MEDICINA GENERAL"); //Origen


        //Ingreso determinaciones manualmente
        cy.fixture("determinaciones").then(d => {

            d.codigos.forEach((valor, index) => {

                cy.get("[id$='tabla']")
                    .find(`input[id$='Codigo_${index}']`)
                    .should("be.visible")
                    .clear()
                    .type(valor)
                    .type('{enter}');
            });

        });

        
        //Guardo 
        cy.get("input[id$='btnGuardar']").click();

        cy.location('pathname').should('include', '/Protocolos/ProtocoloMensaje.aspx');

    });
});

