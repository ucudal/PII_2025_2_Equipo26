using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class FachadaTest
    {
        private Fachada _fachada;
        private IRepoClientes repoClientes;
        private IRepoEtiquetas repoEtiquetas;
        private IRepoUsuarios repoUsuarios;
        private IRepoVentas repoVentas;

        [SetUp]
        public void SetUp()
        {
            this.repoClientes = new RepoClientes();
            this.repoEtiquetas = new RepoEtiquetas();
            this.repoUsuarios = new RepoUsuarios();
            this.repoVentas = new RepoVentas();
            this._fachada = new Fachada(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);
        }

        // --- GESTIÓN DE CLIENTES ---

        [Test]
        public void TestCrearCliente()
        {
            string nombre = "Juan";
            this._fachada.CrearCliente(nombre, "Perez", "099123456", "juan@perez.com", "Masculino", new DateTime(1990, 5, 15));
            var clientes = this._fachada.VerTodosLosClientes();
            
            Assert.AreEqual(1, clientes.Count);
            Assert.AreEqual(nombre, clientes[0].Nombre);
            Assert.AreEqual(1, clientes[0].Id);
        }

        [Test]
        public void TestCrearCliente_VerificaDatosDeContacto()
        {
            string telefono = "098111222";
            string correo = "maria.contact@mail.com";
            this._fachada.CrearCliente("Maria", "Lopez", telefono, correo, "Femenino", DateTime.Now);

            var cliente = this._fachada.VerTodosLosClientes()[0];
            Assert.AreEqual(telefono, cliente.Telefono);
            Assert.AreEqual(correo, cliente.Correo);
        }

        [Test]
        public void TestModificarCliente()
        {
            this._fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "Masculino", DateTime.Now);
            var cliente = this._fachada.VerTodosLosClientes()[0];
            
            this._fachada.ModificarCliente(cliente.Id, "Juan Modificado", cliente.Apellido, cliente.Telefono, "nuevo@mail.com", "Femenino", DateTime.Now); 

            var modificado = this._fachada.BuscarCliente(cliente.Id);
            Assert.AreEqual("Juan Modificado", modificado.Nombre);
            Assert.AreEqual("nuevo@mail.com", modificado.Correo);
        }

        [Test]
        public void TestModificarCliente_ActualizacionTotal()
        {
            this._fachada.CrearCliente("Original", "Viejo", "111", "old@mail.com", "Masculino", DateTime.MinValue);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            string nuevoNom = "Nuevo";
            string nuevoApe = "Apellido";
            string nuevoTel = "222";
            string nuevoMail = "new@mail.com";
            string nuevoGen = "Femenino";
            DateTime nuevaFecha = new DateTime(2000, 1, 1);

            this._fachada.ModificarCliente(id, nuevoNom, nuevoApe, nuevoTel, nuevoMail, nuevoGen, nuevaFecha);

            var cliente = this._fachada.BuscarCliente(id);
            Assert.AreEqual(nuevoNom, cliente.Nombre);
            Assert.AreEqual(nuevoApe, cliente.Apellido);
            Assert.AreEqual(nuevoTel, cliente.Telefono);
            Assert.AreEqual(nuevoMail, cliente.Correo);
            Assert.AreEqual(GeneroCliente.Femenino, cliente.Genero);
            Assert.AreEqual(nuevaFecha, cliente.FechaNacimiento);
        }

        [Test]
        public void TestEliminarCliente()
        {
            this._fachada.CrearCliente("Juan", "Perez", "123", "j@p.com", "M", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;
            
            this._fachada.EliminarCliente(id);
            
            Assert.AreEqual(0, this._fachada.VerTodosLosClientes().Count);
            Assert.IsNull(this._fachada.BuscarCliente(id));
        }

        [Test]
        public void TestEliminarCliente_BorradoSelectivo()
        {
            this._fachada.CrearCliente("A", "Borrar", "1", "1@1.com", "M", DateTime.Now);
            this._fachada.CrearCliente("B", "Quedar", "2", "2@2.com", "F", DateTime.Now);
            
            var lista = this._fachada.VerTodosLosClientes();
            int idBorrar = lista[0].Id;
            int idQuedar = lista[1].Id;

            this._fachada.EliminarCliente(idBorrar);

            Assert.IsNull(this._fachada.BuscarCliente(idBorrar));
            Assert.IsNotNull(this._fachada.BuscarCliente(idQuedar));
            Assert.AreEqual(1, this._fachada.VerTodosLosClientes().Count);
        }

        [Test]
        public void TestRegistrarDatosAdicionalesCliente()
        {
            this._fachada.CrearCliente("Ana", "Gomez", "123", "a@g.com", "NoEspecificado", DateTime.MinValue);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            DateTime nuevaFecha = new DateTime(1995, 12, 25);
            this._fachada.RegistrarDatosAdicionalesCliente(id, "Femenino", nuevaFecha);

            var cliente = this._fachada.BuscarCliente(id);
            Assert.AreEqual(GeneroCliente.Femenino, cliente.Genero);
            Assert.AreEqual(nuevaFecha, cliente.FechaNacimiento);
        }

        // --- BÚSQUEDA ---

        [Test]
        public void TestBuscarClientes()
        {
            this._fachada.CrearCliente("Juan", "Perez", "111", "j@m.com", "M", DateTime.Now);
            this._fachada.CrearCliente("Juana", "Diaz", "222", "jd@m.com", "F", DateTime.Now);

            var resultados = this._fachada.BuscarClientes("Juan");
            
            Assert.AreEqual(2, resultados.Count);
            
            bool estaJuan = false;
            bool estaJuana = false;
            foreach(var c in resultados) {
                if (c.Nombre == "Juan") estaJuan = true;
                if (c.Nombre == "Juana") estaJuana = true;
            }
            Assert.IsTrue(estaJuan);
            Assert.IsTrue(estaJuana);
        }

        [Test]
        public void TestBuscarClientes_PorApellidoTelefonoCorreo()
        {
            this._fachada.CrearCliente("Luke", "Skywalker", "999", "luke@force.com", "M", DateTime.Now);

            // Apellido
            var res1 = this._fachada.BuscarClientes("Skywalker");
            Assert.AreEqual(1, res1.Count);
            Assert.AreEqual("Luke", res1[0].Nombre);

            // Teléfono
            var res2 = this._fachada.BuscarClientes("999");
            Assert.AreEqual(1, res2.Count);
            Assert.AreEqual("Luke", res2[0].Nombre);

            // Correo
            var res3 = this._fachada.BuscarClientes("luke@force.com");
            Assert.AreEqual(1, res3.Count);
            Assert.AreEqual("Luke", res3[0].Nombre);
        }

        [Test]
        public void TestVerTodosLosClientes()
        {
            this._fachada.CrearCliente("A", "A", "1", "a", "M", DateTime.Now);
            this._fachada.CrearCliente("B", "B", "2", "b", "F", DateTime.Now);
            
            var cartera = this._fachada.VerTodosLosClientes();
            Assert.AreEqual(2, cartera.Count);
        }

        // --- INTERACCIONES (Llamadas, Reuniones, Mensajes, Correos, Notas) ---

        [Test]
        public void TestRegistrarLlamada()
        {
            this._fachada.CrearCliente("Lau", "Call", "123", "l@c.com", "F", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            DateTime fecha = new DateTime(2023, 11, 15);
            this._fachada.RegistrarLlamada(id, fecha, "Venta", "entrante");

            var interacciones = this._fachada.VerInteraccionesCliente(id);
            Assert.AreEqual(1, interacciones.Count);
            Assert.IsInstanceOf<Llamada>(interacciones[0]);
            
            Llamada llamada = (Llamada)interacciones[0];
            Assert.AreEqual(fecha, llamada.Fecha);
            Assert.AreEqual("Venta", llamada.Tema);
            Assert.AreEqual("entrante", llamada.TipoLlamada);
        }

        [Test]
        public void TestRegistrarReunion()
        {
            this._fachada.CrearCliente("Sof", "Meet", "456", "s@m.com", "F", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            this._fachada.RegistrarReunion(id, DateTime.Now, "Negocio", "Oficina");

            var interacciones = this._fachada.VerInteraccionesCliente(id);
            Assert.IsInstanceOf<Reunion>(interacciones[0]);
            
            Reunion reunion = (Reunion)interacciones[0];
            Assert.AreEqual("Oficina", reunion.Lugar);
        }

        [Test]
        public void TestRegistrarMensaje()
        {
            this._fachada.CrearCliente("Luc", "Msg", "789", "l@m.com", "M", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            this._fachada.RegistrarMensaje(id, DateTime.Now, "Duda", "789", "Vendedor");

            var interacciones = this._fachada.VerInteraccionesCliente(id);
            Assert.IsInstanceOf<Mensaje>(interacciones[0]);
            
            Mensaje mensaje = (Mensaje)interacciones[0];
            Assert.AreEqual("789", mensaje.Remitente);
            Assert.AreEqual("Vendedor", mensaje.Destinatario);
        }

        [Test]
        public void TestRegistrarCorreo()
        {
            this._fachada.CrearCliente("Dan", "Mail", "000", "d@m.com", "F", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            this._fachada.RegistrarCorreo(id, DateTime.Now, "Info", "ventas@crm.com", "d@m.com", "Presupuesto");

            var interacciones = this._fachada.VerInteraccionesCliente(id);
            Assert.IsInstanceOf<Correo>(interacciones[0]);
            
            Correo correo = (Correo)interacciones[0];
            Assert.AreEqual("Presupuesto", correo.Asunto);
        }

        [Test]
        public void TestAgregarNotaAInteraccion()
        {
            this._fachada.CrearCliente("Car", "Note", "111", "c@n.com", "M", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;
            
            this._fachada.RegistrarLlamada(id, DateTime.Now, "Hola", "saliente");
            
            // Verificamos antes
            Assert.IsNull(this._fachada.VerInteraccionesCliente(id)[0].NotaAdicional);

            this._fachada.AgregarNotaAInteraccion(id, 0, "Nota importante");

            // Verificamos después
            var interaccion = this._fachada.VerInteraccionesCliente(id)[0];
            Assert.IsNotNull(interaccion.NotaAdicional);
            Assert.AreEqual("Nota importante", interaccion.NotaAdicional.Texto);
        }

        [Test]
        public void TestRegistrarCotizacion()
        {
            this._fachada.CrearCliente("Empresa", "SA", "222", "e@sa.com", "Otro", DateTime.Now);
            var id = this._fachada.VerTodosLosClientes()[0].Id;

            double monto = 5000;
            this._fachada.RegistrarCotizacion(id, "Servidores", monto, DateTime.Now);

            var interacciones = this._fachada.VerInteraccionesCliente(id);
            // Buscamos la última interacción que sea Cotización
            Cotizacion coti = null;
            // Recorremos a la inversa para hallar la última
            for(int i = interacciones.Count - 1; i >= 0; i--)
            {
                if (interacciones[i] is Cotizacion)
                {
                    coti = (Cotizacion)interacciones[i];
                    break;
                }
            }

            Assert.IsNotNull(coti);
            Assert.AreEqual(monto, coti.Monto);
            Assert.AreEqual("Servidores", coti.Tema);
        }

        // --- REPORTES Y LÓGICA DE NEGOCIO ---

        [Test]
        public void TestObtenerClientesInactivos()
        {
            // Activo (hace 5 días)
            this._fachada.CrearCliente("Activo", "X", "1", "a", "M", DateTime.Now);
            int idActivo = this._fachada.VerTodosLosClientes()[0].Id; // indice 0
            this._fachada.RegistrarLlamada(idActivo, DateTime.Now.AddDays(-5), "Reciente", "entrante");

            // Inactivo (hace 20 días)
            this._fachada.CrearCliente("Inactivo", "Y", "2", "b", "M", DateTime.Now);
            int idInactivo = this._fachada.VerTodosLosClientes()[1].Id; // indice 1
            this._fachada.RegistrarLlamada(idInactivo, DateTime.Now.AddDays(-20), "Vieja", "saliente");

            int diasLimite = 15;
            var listaInactivos = this._fachada.ObtenerClientesInactivos(diasLimite);

            Assert.AreEqual(1, listaInactivos.Count);
            Assert.AreEqual("Inactivo", listaInactivos[0].Nombre);
        }

        [Test]
        public void TestCalcularTotalVentas()
        {
            this._fachada.CrearCliente("Cliente", "Ventas", "0", "v@v.com", "M", DateTime.Now);
            int id = this._fachada.VerTodosLosClientes()[0].Id;

            // Fechas
            DateTime fechaDentro1 = new DateTime(2025, 2, 5);
            DateTime fechaDentro2 = new DateTime(2025, 2, 20);
            DateTime fechaFuera = new DateTime(2025, 3, 10);

            this._fachada.RegistrarVenta(id, "P1", 100, fechaDentro1);
            this._fachada.RegistrarVenta(id, "P2", 200, fechaDentro2);
            this._fachada.RegistrarVenta(id, "P3", 500, fechaFuera);

            DateTime inicio = new DateTime(2025, 2, 1);
            DateTime fin = new DateTime(2025, 2, 28);

            float total = this._fachada.CalcularTotalVentas(inicio, fin);
            
            // 100 + 200 = 300 (la de 500 está fuera)
            Assert.AreEqual(300, total);
        }

        [Test]
        public void TestObtenerResumenDashboard()
        {
            this._fachada.CrearCliente("C1", "A", "1", "1", "M", DateTime.Now);
            this._fachada.CrearCliente("C2", "B", "2", "2", "F", DateTime.Now);
            int id1 = this._fachada.VerTodosLosClientes()[0].Id;
            int id2 = this._fachada.VerTodosLosClientes()[1].Id;

            // Pasada (ayer)
            this._fachada.RegistrarLlamada(id1, DateTime.Now.AddDays(-1), "Llamada Ayer", "entrante");
            // Futura (mañana)
            this._fachada.RegistrarReunion(id2, DateTime.Now.AddDays(1), "Reunion Mañana", "Oficina");

            var resumen = this._fachada.ObtenerResumenDashboard();

            Assert.AreEqual(2, resumen.TotalClientes);
            
            // Validar interacción reciente
            Assert.AreEqual(1, resumen.InteraccionesRecientes.Count);
            Assert.AreEqual("Llamada Ayer", resumen.InteraccionesRecientes[0].Tema);

            // Validar reunión próxima
            Assert.AreEqual(1, resumen.ReunionesProximas.Count);
            Assert.AreEqual("Reunion Mañana", resumen.ReunionesProximas[0].Tema);
        }

        [Test]
        public void TestAsignarClienteVendedor()
        {
            this._fachada.CrearCliente("Cliente", "Test", "0", "c@t.com", "M", DateTime.Now);
            int idCliente = this._fachada.VerTodosLosClientes()[0].Id;

            this._fachada.CrearUsuario("Vendedor1", Rol.Vendedor);
            this._fachada.CrearUsuario("Admin1", Rol.Administrador);
            this._fachada.CrearUsuario("Suspendido", Rol.Vendedor);
            
            // Buscamos IDs de usuarios manualmente (sin LINQ First)
            int idVendedor = 0;
            int idAdmin = 0;
            int idSuspendido = 0;

            foreach(var u in this._fachada.VerTodosLosUsuarios()) {
                if(u.NombreUsuario == "Vendedor1") idVendedor = u.Id;
                if(u.NombreUsuario == "Admin1") idAdmin = u.Id;
                if(u.NombreUsuario == "Suspendido") idSuspendido = u.Id;
            }

            this._fachada.SuspenderUsuario(idSuspendido);

            // 1. Asignación correcta
            this._fachada.AsignarClienteVendedor(idCliente, idVendedor);
            Assert.IsNotNull(this._fachada.BuscarCliente(idCliente).VendedorAsignado);
            Assert.AreEqual(idVendedor, this._fachada.BuscarCliente(idCliente).VendedorAsignado.Id);

            // 2. Falla Admin (Excepción)
            Assert.Throws<InvalidOperationException>(() => {
                this._fachada.AsignarClienteVendedor(idCliente, idAdmin);
            });

            // 3. Falla Suspendido (Excepción)
            Assert.Throws<InvalidOperationException>(() => {
                this._fachada.AsignarClienteVendedor(idCliente, idSuspendido);
            });
        }

        [Test]
        public void TestBuscarVentasProducto()
        {
            this._fachada.CrearCliente("Cliente", "Test", "0", "c@t.com");
            this._fachada.RegistrarVenta(1, "tostadora", 500, DateTime.Now);
            
        }
        
    }
}