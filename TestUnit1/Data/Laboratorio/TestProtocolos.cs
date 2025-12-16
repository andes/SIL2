using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Data;

namespace TestProtocolos
{
    [TestClass()]
    [TestCategory("Integration")]
    public class TestProtocolos
    {
        /** PASOS
            🔹 Arrange – preparar
            🔹 Act – ejecutar
            🔹 Assert – verificar
         */

        [TestMethod]
        public void AppConfig_EstaSiendoLeido()
        {
            var valor = System.Configuration.ConfigurationManager.AppSettings["TEST_CONFIG"];

            
           // Assert.IsNotNull(valor, "No se encontró la sección TEST_CONFIG");
            Assert.AreEqual("OK", valor);

        }
        [TestMethod]
       
        public void Config_NHibernate_SeccionExiste()
        {
            var section = System.Configuration.ConfigurationManager
                .GetSection("nHibernate");
           
            Assert.IsNotNull(section, "La sección NHibernate no está registrada correctamente");
        }


        [TestMethod()]
        public void TestEstadoProtocolo()
        {
            /* Estado de protocolo
               0: no procesado --rojo
               1: en proceso - amarillo
               2: terminado - verde
               3: bloqueado - candadito
           */

            var protocolo = new Protocolo();
            int estadoInicial = 0;
            Assert.AreEqual(estadoInicial, protocolo.Estado);
        }

        //[TestMethod]
        //public void CrearProtocolo_DatosValidos_SeInicializaCorrectamente()
        //{
        //    // Arrange (preparación)
        //    var efector = new Efector { IdEfector = 1 };
        //    var sector = new SectorServicio { IdSectorServicio = 1 };
        //    var tipoServicio = new TipoServicio { IdTipoServicio = 1 };
        //    var paciente = new Paciente { IdPaciente = 1 };
        //    var usuario = new Usuario { IdUsuario = 1 };

        //    var fecha = DateTime.Today;

        //    // Act
        //    var protocolo = new Protocolo(
        //        idefector: efector,
        //        numero: 0,
        //        numerotiposervicio: 0,
        //        numerodiario: 0,
        //        prefijosector: "LAB",
        //        numerosector: 0,
        //        idsector: sector,
        //        idtiposervicio: tipoServicio,
        //        fecha: fecha,
        //        fechaorden: fecha,
        //        fecharetiro: fecha,
        //        idpaciente: paciente,
        //        idefectorsolicitante: efector,
        //        idespecialistasolicitante: 0,
        //        idobrasocial: new ObraSocial(),
        //        idorigen: new Origen(),
        //        idprioridad: new Prioridad(),
        //        estado: 0,
        //        edad: 30,
        //        unidadedad: 1,
        //        sexo: "F",
        //        embarazada: "N",
        //        alerta: false,
        //        observacionResultado: "",
        //        idusuarioregistro: usuario,
        //        fecharegistro: fecha,
        //        fechatomamuestra: fecha,
        //        idmuestra: 1,
        //        idconservacion: 1,
        //        descripcionProducto: "",
        //        nombreobrasocial: "",
        //        idcaracter: 0,
        //        especialista: "",
        //        matriculaespecialista: "",
        //        codos: 0,
        //        idcasosisa: 0,
        //        ipcarga: "127.0.0.1",
        //        impre: "",
        //        fechainiciosintomas: fecha,
        //        fechaultimocontacto: fecha,
        //        notificarresultado: true
        //    );

        //    // Assert (verificación)
        //    Assert.IsNotNull(protocolo);
        //    Assert.AreEqual(efector, protocolo.IdEfector);
        //    Assert.AreEqual(paciente, protocolo.IdPaciente);
        //    Assert.AreEqual("F", protocolo.Sexo);
        //    Assert.AreEqual(30, protocolo.Edad);
        //    Assert.IsFalse(protocolo.Baja);
        //    Assert.IsTrue(protocolo.Notificarresultado);
        //}

        //[TestMethod]
        //public void GenerarNumero_SiempreDevuelveNumeroMayorACero()
        //{
        //    // Arrange
        //    var protocolo = new Protocolo();

        //    // Act
        //    int numero = protocolo.GenerarNumero();

        //    // Assert
        //    Assert.IsTrue(numero > 0, "El número de protocolo debe ser mayor a cero");
        //}

        [TestMethod]
        [TestCategory("Integration")]
        public void EstadoPorDefecto_DebeSerNoProcesado()
        {
            var protocolo = new Protocolo(
                idefector: new Efector(),
                numero: 0,
                numerotiposervicio: 0,
                numerodiario: 0,
                prefijosector: "",
                numerosector: 0,
                idsector: new SectorServicio(),
                idtiposervicio: new TipoServicio(),
                fecha: DateTime.Today,
                fechaorden: DateTime.Today,
                fecharetiro: DateTime.Today,
                idpaciente: new Paciente(),
                idefectorsolicitante: new Efector(),
                idespecialistasolicitante: 0,
                idobrasocial: new ObraSocial(),
                idorigen: new Origen(),
                idprioridad: new Prioridad(),
                estado: 0,
                edad: 0,
                unidadedad: 0,
                sexo: "",
                embarazada: "",
                alerta: false,
                observacionResultado: "",
                idusuarioregistro: new Usuario(),
                fecharegistro: DateTime.Today,
                fechatomamuestra: DateTime.Today,
                idmuestra: 0,
                idconservacion: 0,
                descripcionProducto: "",
                nombreobrasocial: "",
                idcaracter: 0,
                especialista: "",
                matriculaespecialista: "",
                codos: 0,
                idcasosisa: 0,
                ipcarga: "",
                impre: "",
                fechainiciosintomas: DateTime.Today,
                fechaultimocontacto: DateTime.Today,
                notificarresultado: false
            );

            Assert.AreEqual(0, protocolo.Estado);
        }

    }
}