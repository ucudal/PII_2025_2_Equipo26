using Library;
using System;
using System.Collections.Generic;

// Esta clase implementa el patrón "Fachada".
// Es el único punto de contacto entre la interfaz de usuario (el bot)
// y toda la lógica de negocio (la biblioteca)
public class Fachada
{
    // --- Repositorios Internos ---
    // La Fachada "guarda" (agrega) instancias de todos los repositorios.
    // Son 'private' para que nadie fuera de esta clase pueda usarlos directamente.
    private RepoClientes _repoClientes = new RepoClientes();
    private RepoEtiquetas _repoEtiquetas = new RepoEtiquetas();
    private RepoUsuarios _repoUsuarios = new RepoUsuarios();
    private RepoVentas _repoVentas = new RepoVentas();
    // Un contador simple para asignar IDs a las ventas.
    private int _proximoIdVenta = 1;
    
    // --- Métodos de Clientes ---

    // Recibe los datos de un cliente, crea el objeto y lo pasa
    // al repositorio de clientes para que lo guarde.
    public void CrearCliente(string nombre, string apellido, string telefono, string correo, 
                           string genero, DateTime fechaNacimiento)
    {
        // (Pasa 0 como ID temporal, asumiendo que el Repo lo ajustará)
        var clienteTemporal = new Cliente(0, nombre, apellido, telefono, correo, 
                                        genero, fechaNacimiento);
        _repoClientes.Agregar(clienteTemporal);
    }
    
    // Pide al repositorio la lista completa de clientes.
    public List<Cliente> VerTodosLosClientes()
    {
        return _repoClientes.ObtenerTodos();
    }
    // Pasa los nuevos datos del cliente al repositorio para que los actualice.
    public void ModificarCliente(int id, string nombre, string apellido, string telefono, 
                               string correo, string genero, DateTime fechaNacimiento)
    {
        _repoClientes.Modificar(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);
    }

    // Pide al repositorio que filtre los clientes según un término de búsqueda.
    public List<Cliente> BuscarClientes(string termino)
    {
        return _repoClientes.BuscarPorTermino(termino);
    }

    // Le dice al repositorio que elimine un cliente por su ID.
    public void EliminarCliente(int id)
    {
        _repoClientes.Eliminar(id);
    }

    // --- Métodos de Coordinación ---

    // Este método es un buen ejemplo de Fachada: coordina dos repositorios.
    // Busca un cliente (en RepoClientes) y un vendedor (en RepoUsuarios)
    // y luego los asigna.
    public void AsignarClienteVendedor(int idCliente, int idNuevoVendedor)
    {
        // 1. Busca los objetos
        Cliente cliente = _repoClientes.Buscar(idCliente);
        Usuario nuevoVendedor = _repoUsuarios.Buscar(idNuevoVendedor);

        // 2. Aplica reglas de negocio (Validaciones)
        if (cliente == null || nuevoVendedor == null)
        {
            return; // Si uno no existe, no hace nada.
        }
        if (nuevoVendedor.Rol != RolUsuario.Vendedor)
        {
            return; // Solo se puede asignar a vendedores.
        }
        if (nuevoVendedor.Estado == EstadoUsuario.Suspendido)
        {
            return; // El vendedor no puede estar suspendido.
        }

        // 3. Ejecuta la acción en el objeto (Principio Experto)
        cliente.AsignarVendedor(nuevoVendedor);
    }

    // --- Métodos de Interacciones (Polimorfismo) ---

    // Busca al cliente y le agrega una nueva 'Llamada' a su historial.
    public void RegistrarLlamada(int idCliente, DateTime fecha, string tema, string tipoLlamada)
    {
        var cliente = _repoClientes.Buscar(idCliente);

        if (cliente != null)
        {
            // Crea el objeto específico (Llamada hereda de Interaccion)
            var nuevaLlamada = new Llamada(fecha, tema, tipoLlamada);
            cliente.Interacciones.Add(nuevaLlamada);
        }
    }

    // Busca al cliente y le agrega una nueva 'Reunion'.
    public void RegistrarReunion(int idCliente, DateTime fecha, string tema, string lugar)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevaReunion = new Reunion(fecha, tema, lugar);
            cliente.Interacciones.Add(nuevaReunion);
        }
    }

    // Busca al cliente y le agrega un nuevo 'Mensaje'.
    public void RegistrarMensaje(int idCliente, DateTime fecha, string tema, string remitente, string destinatario)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoMensaje = new Mensaje(fecha, tema, remitente, destinatario);
            cliente.Interacciones.Add(nuevoMensaje);
        }
    }

    // Busca al cliente y le agrega un nuevo 'Correo'.
    public void RegistrarCorreo(int idCliente, DateTime fecha, string tema, string remitente, string destinatario, string asunto)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoCorreo = new Correo(fecha, tema, remitente, destinatario, asunto);
            cliente.Interacciones.Add(nuevoCorreo);
        }
    }

    // Busca un cliente y devuelve su historial completo de interacciones.
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            return cliente.Interacciones;
        }
        return new List<Interaccion>(); // Devuelve lista vacía si no encuentra al cliente.
    }

    // Esto es una *sobrecarga* del método anterior (mismo nombre, distintos parámetros).
    // Filtra las interacciones por tipo (ej: "llamada", "reunion").
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente, string tipoDeInteraccion)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente == null)
        {
            return new List<Interaccion>();
        }

        var interaccionesFiltradas = new List<Interaccion>();
        string tipoBuscado = tipoDeInteraccion.ToLower(); // Normaliza a minúsculas

        foreach (var interaccion in cliente.Interacciones)
        {
            bool coincide = false;
            // Usa 'is' para chequear el tipo de objeto (Polimorfismo)
            if (tipoBuscado == "llamada" && interaccion is Llamada)
            {
                coincide = true;
            }
            else if (tipoBuscado == "reunion" && interaccion is Reunion)
            {
                coincide = true;
            }
            else if (tipoBuscado == "mensaje" && interaccion is Mensaje)
            {
                coincide = true;
            }
            else if (tipoBuscado == "correo" && interaccion is Correo)
            {
                coincide = true;
            }

            if (coincide)
            {
                interaccionesFiltradas.Add(interaccion);
            }
        }

        return interaccionesFiltradas;
    }

    // Otra *sobrecarga*. Primero filtra por tipo (reusando la función anterior)
    // y luego filtra por fecha (solo las más nuevas que 'fechaDesde').
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente, string tipoDeInteraccion, DateTime fechaDesde)
    {
        // 1. Llama a la función anterior
        var interaccionesFiltradasPorTipo = VerInteraccionesDeCliente(idCliente, tipoDeInteraccion);
        var resultadoFinal = new List<Interaccion>();

        // 2. Aplica el segundo filtro
        foreach (var interaccion in interaccionesFiltradasPorTipo)
        {
            if (interaccion.Fecha >= fechaDesde)
            {
                resultadoFinal.Add(interaccion);
            }
        }

        return resultadoFinal;
    }

    // Busca una interacción específica por su índice (posición en la lista)
    // y le asigna una nueva nota.
    public void AgregarNotaAInteraccion(int idCliente, int indiceInteraccion, string textoNota)
    {
        var cliente = _repoClientes.Buscar(idCliente);

        if (cliente != null)
        {
            // Valida que el índice esté dentro de los límites de la lista
            if (indiceInteraccion >= 0 && indiceInteraccion < cliente.Interacciones.Count)
            {
                var interaccionSeleccionada = cliente.Interacciones[indiceInteraccion];
                interaccionSeleccionada.NotaAdicional = new Nota(textoNota);
            }
        }
    }

    // --- Métodos de Etiquetas ---

    // Delega la creación de la etiqueta al repo correspondiente.
    public void CrearEtiqueta(string nombre)
    {
        _repoEtiquetas.Crear(nombre);
    }

    // Devuelve todas las etiquetas que existen en el sistema.
    public List<Etiqueta> VerTodasLasEtiquetas()
    {
        return _repoEtiquetas.ObtenerTodas();
    }

    // Elimina una etiqueta del sistema central.
    public void EliminarEtiqueta(int idEtiqueta)
    {
        _repoEtiquetas.Eliminar(idEtiqueta);
    }

    // Asocia una etiqueta existente a un cliente existente.
    public void AgregarEtiquetaACliente(int idCliente, int idEtiqueta)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        var etiqueta = _repoEtiquetas.Buscar(idEtiqueta);

        if (cliente != null && etiqueta != null)
        {
            // Evita agregar la misma etiqueta dos veces
            if (!cliente.Etiquetas.Contains(etiqueta))
            {
                cliente.Etiquetas.Add(etiqueta);
            }
        }
    }

    // Quita la asociación entre un cliente y una etiqueta.
    public void QuitarEtiquetaDeCliente(int idCliente, int idEtiqueta)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        var etiqueta = _repoEtiquetas.Buscar(idEtiqueta);

        if (cliente != null && etiqueta != null)
        {
            cliente.Etiquetas.Remove(etiqueta);
        }
    }

    // --- Métodos de Usuarios ---
    
    // Agrega un nuevo usuario (Vendedor o Admin) al sistema.
    public void CrearUsuario(string nombreUsuario, string contrasena, RolUsuario rol)
    {
        _repoUsuarios.Agregar(nombreUsuario, contrasena, rol);
    }

    // Cambia el estado del usuario a Suspendido.
    public void SuspenderUsuario(int idUsuario)
    {
        _repoUsuarios.Suspender(idUsuario);
    }
    // Cambia el estado del usuario a Activo.
    public void ActivarUsuario(int idUsuario)
    {
        _repoUsuarios.Activar(idUsuario);
    }
    // Elimina un usuario del sistema.
    public void EliminarUsuario(int idUsuario)
    {
        _repoUsuarios.Eliminar(idUsuario);
    }
    // Busca y devuelve un usuario por su ID.
    public Usuario BuscarUsuario(int idUsuario)
    {
        return _repoUsuarios.Buscar(idUsuario);
    }
    // Devuelve la lista completa de usuarios.
    public List<Usuario> VerTodosLosUsuarios()
    {
        return _repoUsuarios.ObtenerTodos();
    }

    // --- Métodos de Ventas y Reportes ---
    
    // (Esta parece ser una venta general, no asignada a un cliente específico).
    public void RegistrarVenta(string producto, float importe, DateTime fecha)
    {
        _repoVentas.Agregar(producto, importe, fecha);
    }

    // *Sobrecarga* que SÍ asigna la venta a un cliente.
    public void RegistrarVenta(int clienteId, string producto, float monto)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId);

        if (clienteEncontrado != null)
        {
            // Crea la venta usando el contador de ID y la fecha actual
            Venta nuevaVenta = new Venta(_proximoIdVenta++, producto, monto, DateTime.Now);
            // La agrega al historial del cliente
            clienteEncontrado.Ventas.Add(nuevaVenta); 
            
        }
    }

    // Calcula el total de dinero vendido en un rango de fechas.
    public float CalcularTotalVentas(DateTime fechaInicio, DateTime fechaFin)
    {
        var todasLasVentas = _repoVentas.ObtenerTodas();
        float total = 0;

        foreach (var venta in todasLasVentas)
        {
            // Filtra por fecha
            if (venta.Fecha >= fechaInicio && venta.Fecha <= fechaFin)
            {
                total += venta.Importe;
            }
        }
        
        return total;
    }

    // Registra una Cotización (que es un tipo de Interaccion) para un cliente.
    public void RegistrarCotizacion(int clienteId, string tema, double monto, string detalle)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId); 

        if (clienteEncontrado != null)
        {
            Cotizacion nuevaCotizacion = new Cotizacion(tema, monto, detalle);
            clienteEncontrado.Interacciones.Add(nuevaCotizacion); 
        }
    }

    // Busca clientes que no han tenido contacto en 'diasSinInteraccion' días.
    public List<Cliente> ObtenerClientesInactivos(int diasSinInteraccion)
    {
        List<Cliente> clientesInactivos = new List<Cliente>();
        // Calcula la fecha límite (ej: hoy - 30 días)
        DateTime fechaLimite = DateTime.Now.AddDays(-diasSinInteraccion);

        foreach (var cliente in _repoClientes.ObtenerTodos())
        {
            // Si no tiene interacciones, está inactivo.
            if (cliente.Interacciones.Count == 0)
            {
                clientesInactivos.Add(cliente);
                continue; // Salta al siguiente cliente
            }

            // Busca la fecha de la última interacción
            DateTime fechaMasReciente = DateTime.MinValue;
            foreach (var interaccion in cliente.Interacciones)
            {
                if (interaccion.Fecha > fechaMasReciente)
                {
                    fechaMasReciente = interaccion.Fecha;
                }
            }

            // Si la última interacción es más vieja que la fecha límite, está inactivo.
            if (fechaMasReciente < fechaLimite)
            {
                clientesInactivos.Add(cliente);
            }
        }

        return clientesInactivos;
    }

    // Un atajo simple para buscar un cliente (delegga al repo).
    public Cliente BuscarCliente(int clienteId)
    {
        return _repoClientes.Buscar(clienteId);
    }

    // Reporte: Busca clientes cuya última interacción fue una llamada "Recibida".
    // (Asume que una llamada recibida requiere una acción o respuesta).
    public List<Cliente> ObtenerClientesSinRespuesta()
    {
        List<Cliente> clientesSinRespuesta = new List<Cliente>();

        foreach (var cliente in _repoClientes.ObtenerTodos())
        {
            if (cliente.Interacciones.Count == 0)
            {
                continue; // Si no hay interacciones, no aplica.
            }

            // Busca la última interacción (la más reciente)
            Interaccion ultimaInteraccion = null;
            DateTime fechaMasReciente = DateTime.MinValue;

            foreach (var interaccion in cliente.Interacciones)
            {
                if (interaccion.Fecha > fechaMasReciente)
                {
                    fechaMasReciente = interaccion.Fecha;
                    ultimaInteraccion = interaccion;
                }
            }

            // Chequea si esa última interacción fue una Llamada
            if (ultimaInteraccion is Llamada)
            {
                // 'as' convierte el tipo de forma segura (da null si no puede)
                Llamada ultimaLlamada = ultimaInteraccion as Llamada;
                if (ultimaLlamada.TipoLlamada == "Recibida")
                {
                    clientesSinRespuesta.Add(cliente);
                }
            }
        }

        return clientesSinRespuesta;
    }
    
    // --- Dashboard ---
    
    // Recopila varias estadísticas para mostrar un resumen general.
    public ResumenDashboard ObtenerResumenDashboard()
    {
        var todosLosClientes = _repoClientes.ObtenerTodos();
        // 1. Total de clientes
        int totalClientes = todosLosClientes.Count;

        // 2. Busca todas las interacciones de todos los clientes
        List<Interaccion> todasLasInteracciones = new List<Interaccion>();
        foreach (Cliente cliente in todosLosClientes)
        {
            foreach (Interaccion interaccion in cliente.Interacciones)
            {
                todasLasInteracciones.Add(interaccion);
                /*
                foreach (Interaccion interaccion in cliente.Interacciones)
                {
                    todasLasInteracciones.Add(interaccion);
                }
                */
            }
        }

        // Ordena de más nueva a más vieja
        todasLasInteracciones.Sort((i1, i2) => i2.Fecha.CompareTo(i1.Fecha));

        // Se queda solo con las 5 primeras (las más recientes)
        List<Interaccion> interaccionesRecientes = new List<Interaccion>();
        int contadorMaximo = 5;
        
        for (int i = 0; i < todasLasInteracciones.Count; i++)
        {
            if (i >= contadorMaximo)
            {
                break; // Para de agregar si ya tiene 5
            }
            interaccionesRecientes.Add(todasLasInteracciones[i]);
        }

        // 3. Busca reuniones que todavía no pasaron
        List<Reunion> reunionesProximas = new List<Reunion>();
        DateTime ahora = DateTime.Now;

        foreach (Interaccion interaccion in todasLasInteracciones) // Reusa la lista total
        {
            if (interaccion is Reunion) // Si es una reunión
            {
                if (interaccion.Fecha > ahora) // Y es en el futuro
                {
                    reunionesProximas.Add((Reunion)interaccion);
                }
            }
        }

        // Ordena las reuniones de más cercana a más lejana
        reunionesProximas.Sort((r1, r2) => r1.Fecha.CompareTo(r2.Fecha));

        // 4. Arma el objeto de resumen y lo devuelve
        var resumen = new ResumenDashboard
        {
            TotalClientes = totalClientes,
            InteraccionesRecientes = interaccionesRecientes,
            ReunionesProximas = reunionesProximas
        };

        return resumen;
    }
}
