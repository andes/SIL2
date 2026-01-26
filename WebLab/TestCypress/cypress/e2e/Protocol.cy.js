describe("Prueba básica", () => {

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
    });
});

