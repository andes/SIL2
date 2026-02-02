Cypress.Commands.add("login", () => {
    cy.visit("Logout.aspx");
    cy.title().should("not.be.empty");

   
    cy.get("input[id$='UserName']").type(Cypress.env("USER_NAME"));
    cy.get("input[id$='Password']").type(Cypress.env("USER_PASSWORD"));
 

    cy.get("input[id$='LoginButton']").click();
});

Cypress.Commands.add("seleccionarEfector", () => {
    cy.fixture("datos").then(u => {
        cy.get("select[id$='ddlEfector']").select(u.efector);
    });

    cy.get("input[id$='btnAceptar']").click();
});

Cypress.Commands.add("encabezadoProtocolo", () => {
    //Selecciono Origen
    cy.get("select[id$='ddlOrigen']").select("AMBULATORIO");
    //Selecciono Servicio ddlSectorServicio
    cy.get("select[id$='ddlSectorServicio']").select("1 - MEDICINA GENERAL");

    //Numero de Matricula de Especialista
    cy.get("input[id$='txtEspecialista']").type("0").type('{enter}');
});

Cypress.Commands.add("cargarDiagnostico", () => {
    //Selecciono el tab
    cy.get("#tabContainer")
        .contains("a", "Diagnósticos")
        .click();
    //Le cargo el diagnostico "Z00.0 - Examen médico general"
    cy.get("input[id$='txtCodigoDiagnostico']").type("Z00.0");
    cy.get("input[id$='btnBusquedaDiagnostico']").click();
    //Lo selecciono de la lista
    cy.get("select[id$='lstDiagnosticos']").select("11829");
    cy.get("input[id$='btnAgregarDiagnostico']").click();

});
