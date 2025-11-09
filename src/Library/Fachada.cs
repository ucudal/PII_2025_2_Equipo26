using Library;
using System;
using System.Collections.Generic;
/// <summary>
/// Implementa el patrón "Fachada" (Facade).
/// Es el único punto de entrada unificado para todas las operaciones de la lógica
/// de negocio (la biblioteca o "core").
/// Oculta la complejidad interna de los repositorios y la coordinación entre ellos.
/// </summary>
public class Fachada
{
    // --- Repositorios Internos (Composición) ---
    // La Fachada "se compone de" repositorios.
    
    /// <summary>
    /// Repositorio para la gestión de clientes.
    /// </summary>
    private RepoClientes _repoClientes = new RepoClientes();
    
    /// <summary>
    /// Repositorio para la gestión de etiquetas.
    /// </summary>
    private RepoEtiquetas _repoEtiquetas = new RepoEtiquetas();
    
    /// <summary>
    /// Repositorio para la gestión de usuarios.
    /// </summary>
    private RepoUsuarios _repoUsuarios = new RepoUsuarios();
    
    /// <summary>
    /// Repositorio para la gestión de ventas generales.
    /// </summary>
    private RepoVentas _repoVentas = new RepoVentas();

    /// <summary>
    /// Contador simple para asignar IDs a las ventas.
    /// </summary>
    private int _proximoIdVenta = 1;
    
    // --- Métodos de Clientes ---

    /// <summary>
    /// Crea un nuevo cliente y lo persiste en el repositorio.
    /// </summary>
    /// <param name="nombre">Nombre del cliente.</param>
    /// <param name="apellido">Apellido del cliente.</param>
    /// <param name="telefono">Teléfono del cliente.</param>
    /// <param name="correo">Correo del cliente.</param>
    /// <param name="genero">Género del cliente.</param>
    /// <param name="fechaNacimiento">Fecha de nacimiento del cliente.</param>
    public Cliente CrearCliente(string nombre, string apellido, string email, string telefono, string genero, DateTime fechaNacimiento)
{
    // La Fachada delega la creación al Repositorio (Creator)
    return this._repoClientes.CrearCliente(nombre, apellido, email, telefono, genero, fechaNacimiento);
}   
    
    /// <summary>
    /// Obtiene una lista de todos los clientes registrados.
    /// </summary>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Cliente"/>.</returns>
    // src/Library/Fachada.cs
// src/Library/Fachada.cs

/// <summary>
/// Obtiene una lista de solo lectura de todos los clientes registrados.
/// </summary>
/// <returns>Una <see cref="IReadOnlyList{T}"/> de <see cref="Cliente"/>.</returns>
public IReadOnlyList<Cliente> VerTodosLosClientes() // <-- CAMBIA List POR IReadOnlyList
{
    return this._repoClientes.ObtenerTodos(); // <-- Ahora los tipos coinciden
}
    /// <summary>
    /// Modifica los datos de un cliente existente.
    /// </summary>
    /// <param name="id">El ID del cliente a modificar.</param>
    /// <param name="nombre">El nuevo nombre.</param>
    /// <param name="apellido">El nuevo apellido.</param>
    /// <param name="telefono">El nuevo teléfono.</param>
    /// <param name="correo">El nuevo correo.</param>
    /// <param name="genero">El nuevo género.</param>
    /// <param name="fechaNacimiento">La nueva fecha de nacimiento.</param>
    public void ModificarCliente(int id, string nombre, string apellido, string telefono, 
                               string correo, string genero, DateTime fechaNacimiento)
    {
        _repoClientes.Modificar(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);
    }

    /// <summary>
    /// Busca clientes cuyo nombre, apellido, teléfono, correo o género contengan el término.
    /// </summary>
    /// <param name="termino">El texto a buscar (ignora mayúsculas/minúsculas).</param>
    /// <returns>Una lista de clientes que coinciden con la búsqueda.</returns>
    public List<Cliente> BuscarClientes(string termino)
    {
        return _repoClientes.BuscarPorTermino(termino);
    }

    /// <summary>
    /// Elimina un cliente del sistema usando su ID.
    /// </summary>
    /// <param name="id">El ID del cliente a eliminar.</param>
    public void EliminarCliente(int id)
    {
        _repoClientes.Eliminar(id);
    }

    // --- Métodos de Coordinación ---

    /// <summary>
    /// Asigna un Vendedor (Usuario) a un Cliente.
    /// Este método coordina múltiples repositorios y aplica lógica de negocio
    /// (validaciones de rol y estado).
    /// </summary>
    /// <param name="idCliente">El ID del cliente que recibirá la asignación.</param>
    /// <param name="idNuevoVendedor">El ID del usuario (Vendedor) a asignar.</param>
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

    /// <summary>
    /// Registra una nueva llamada en el historial de un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="fecha">Fecha de la llamada.</param>
    /// <param name="tema">Tema de la llamada.</param>
    /// <param name="tipoLlamada">Tipo ("Entrante", "Saliente", etc.).</param>
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

    /// <summary>
    /// Registra una nueva reunión en el historial de un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="fecha">Fecha de la reunión.</param>
    /// <param name="tema">Tema de la reunión.</param>
    /// <param name="lugar">Lugar de la reunión (físico o virtual).</param>
    public void RegistrarReunion(int idCliente, DateTime fecha, string tema, string lugar)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevaReunion = new Reunion(fecha, tema, lugar);
            cliente.Interacciones.Add(nuevaReunion);
        }
    }

    /// <summary>
    /// Registra un nuevo mensaje (SMS, chat) en el historial de un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="fecha">Fecha del mensaje.</param>
    /// <param name="tema">Tema del mensaje.</param>
    /// <param name="remitente">Quién envió el mensaje.</param>
    /// <param name="destinatario">Quién recibió el mensaje.</param>
    public void RegistrarMensaje(int idCliente, DateTime fecha, string tema, string remitente, string destinatario)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoMensaje = new Mensaje(fecha, tema, remitente, destinatario);
            cliente.Interacciones.Add(nuevoMensaje);
        }
    }

    /// <summary>
    /// Registra un nuevo correo en el historial de un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="fecha">Fecha del correo.</param>
    /// <param name="tema">Tema del correo.</param>
    /// <param name="remitente">Remitente del correo.</param>
    /// <param name="destinatario">Destinatario del correo.</param>
    /// <param name="asunto">Asunto del correo.</param>
    public void RegistrarCorreo(int idCliente, DateTime fecha, string tema, string remitente, string destinatario, string asunto)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoCorreo = new Correo(fecha, tema, remitente, destinatario, asunto);
            cliente.Interacciones.Add(nuevoCorreo);
        }
    }

    /// <summary>
    /// Obtiene el historial completo de interacciones de un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente a consultar.</param>
    /// <returns>Una lista de <see cref="Interaccion"/>.</returns>
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            return cliente.Interacciones;
        }
        return new List<Interaccion>(); // Devuelve lista vacía si no encuentra al cliente.
    }

    /// <summary>
    /// (Sobrecarga) Obtiene las interacciones de un cliente, filtradas por tipo.
    /// </summary>
    /// <param name="idCliente">ID del cliente a consultar.</param>
    /// <param name="tipoDeInteraccion">El tipo a filtrar (ej: "llamada", "reunion").</param>
    /// <returns>Una lista filtrada de <see cref="Interaccion"/>.</returns>
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

    /// <summary>
    /// (Sobrecarga) Obtiene las interacciones, filtradas por tipo y fecha.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="tipoDeInteraccion">El tipo a filtrar.</param>
    /// <param name="fechaDesde">La fecha de inicio (inclusive) para el filtro.</param>
    /// <returns>Una lista doblemente filtrada de <see cref="Interaccion"/>.</returns>
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente, string tipoDeInteraccion, DateTime fechaDesde)
    {
        // 1. Llama a la función anterior (reúso de código)
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

    /// <summary>
    /// Agrega una nota de texto a una interacción específica, identificada por su índice.
    /// </summary>
    /// <param name="idCliente">ID del cliente dueño de la interacción.</param>
    /// <param name="indiceInteraccion">Posición (índice) de la interacción en la lista del cliente.</param>
    /// <param name="textoNota">El texto a agregar.</param>
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

    /// <summary>
    /// Crea una nueva etiqueta global en el sistema.
    /// </summary>
    /// <param name="nombre">El nombre de la etiqueta (ej: "VIP").</param>
    public void CrearEtiqueta(string nombre)
    {
        _repoEtiquetas.Crear(nombre);
    }

    /// <summary>
    /// Obtiene todas las etiquetas disponibles en el sistema.
    /// </summary>
    /// <returns>Una lista de <see cref="Etiqueta"/>.</returns>
    public List<Etiqueta> VerTodasLasEtiquetas()
    {
        return _repoEtiquetas.ObtenerTodas();
    }

    /// <summary>
    /// Elimina una etiqueta del repositorio global de etiquetas.
    /// </summary>
    /// <param name="idEtiqueta">El ID de la etiqueta a eliminar.</param>
    public void EliminarEtiqueta(int idEtiqueta)
    {
        _repoEtiquetas.Eliminar(idEtiqueta);
    }

    /// <summary>
    /// Asocia una etiqueta existente a un cliente.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="idEtiqueta">ID de la etiqueta.</param>
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

    /// <summary>
    /// Quita la asociación entre un cliente y una etiqueta.
    /// </summary>
    /// <param name="idCliente">ID del cliente.</param>
    /// <param name="idEtiqueta">ID de la etiqueta.</param>
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
    
    /// <summary>
    /// Crea un nuevo usuario (Vendedor o Administrador) en el sistema.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de login (ej: "jPerez").</param>
    /// <param name="contrasena">Contraseña del usuario.</param>
    /// <param name="rol">El rol (<see cref="RolUsuario.Administrador"/> o <see cref="RolUsuario.Vendedor"/>).</param>
    public void CrearUsuario(string nombreUsuario, string contrasena, RolUsuario rol)
    {
        _repoUsuarios.Agregar(nombreUsuario, contrasena, rol);
    }

    /// <summary>
    /// Cambia el estado de un usuario a <see cref="EstadoUsuario.Suspendido"/>.
    /// </summary>
    /// <param name="idUsuario">ID del usuario a suspender.</param>
    public void SuspenderUsuario(int idUsuario)
    {
        _repoUsuarios.Suspender(idUsuario);
    }
    
    /// <summary>
    /// Cambia el estado de un usuario a <see cref="EstadoUsuario.Activo"/>.
    /// </summary>
    /// <param name="idUsuario">ID del usuario a activar.</param>
    public void ActivarUsuario(int idUsuario)
    {
        _repoUsuarios.Activar(idUsuario);
    }
    
    /// <summary>
    /// Elimina un usuario del sistema.
    /// </summary>
    /// <param name="idUsuario">ID del usuario a eliminar.</param>
    public void EliminarUsuario(int idUsuario)
    {
        _repoUsuarios.Eliminar(idUsuario);
    }
    
    /// <summary>
    /// Busca y devuelve un usuario por su ID.
    /// </summary>
    /// <param name="idUsuario">ID del usuario a buscar.</param>
    /// <returns>El <see cref="Usuario"/> encontrado, o <c>null</c> si no existe.</returns>
    public Usuario BuscarUsuario(int idUsuario)
    {
        return _repoUsuarios.Buscar(idUsuario);
    }
    
    /// <summary>
    /// Devuelve la lista completa de usuarios.
    /// </summary>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Usuario"/>.</returns>
    public List<Usuario> VerTodosLosUsuarios()
    {
        return _repoUsuarios.ObtenerTodos();
    }

    // --- Métodos de Ventas y Reportes ---
    
    /// <summary>
    /// Registra una venta general (no asignada a un cliente específico).
    /// </summary>
    /// <param name="producto">Descripción del producto/servicio.</param>
    /// <param name="importe">Monto de la venta.</param>
    /// <param name="fecha">Fecha de la venta.</param>
    public void RegistrarVenta(string producto, float importe, DateTime fecha)
    {
        _repoVentas.Agregar(producto, importe, fecha);
    }

    /// <summary>
    /// (Sobrecarga) Registra una venta y la asigna a un cliente específico.
    /// </summary>
    /// <param name="clienteId">ID del cliente que realizó la compra.</param>
    /// <param name="producto">Descripción del producto/servicio.</param>
    /// <param name="monto">Monto de la venta.</param>
    public void RegistrarVenta(int clienteId, string producto, float monto)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId);

        if (clienteEncontrado != null)
        {
            // Crea la venta usando el contador de ID y la fecha actual
            Venta nuevaVenta = new Venta(_proximoIdVenta++, producto, monto, DateTime.Now);
            // La agrega al historial del cliente (Principio Experto)
            clienteEncontrado.Ventas.Add(nuevaVenta); 
        }
    }

    /// <summary>
    /// Calcula el monto total de ventas generales (no de clientes) 
    /// dentro de un rango de fechas.
    /// </summary>
    /// <param name="fechaInicio">Fecha de inicio del período.</param>
    /// <param name="fechaFin">Fecha de fin del período.</param>
    /// <returns>La suma de los importes de las ventas en ese rango.</returns>
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

    /// <summary>
    /// Registra una <see cref="Cotizacion"/> (que es una <see cref="Interaccion"/>) 
    /// en el historial de un cliente.
    /// </summary>
    /// <param name="clienteId">ID del cliente.</param>
    /// <param name="tema">Tema de la cotización.</param>
    /// <param name="monto">Monto cotizado.</param>
    /// <param name="detalle">Detalle de la cotización.</param>
    public void RegistrarCotizacion(int clienteId, string tema, double monto, string detalle)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId); 

        if (clienteEncontrado != null)
        {
            Cotizacion nuevaCotizacion = new Cotizacion(tema, monto, detalle);
            clienteEncontrado.Interacciones.Add(nuevaCotizacion); 
        }
    }

    /// <summary>
    /// Obtiene una lista de clientes que no han tenido interacciones recientes.
    /// </summary>
    /// <param name="diasSinInteraccion">El número de días hacia atrás para considerar "inactivo".</param>
    /// <returns>Una lista de clientes inactivos.</returns>
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

    /// <summary>
    /// Busca un cliente por su ID (Atajo para el repositorio).
    /// </summary>
    /// <param name="clienteId">ID del cliente a buscar.</param>
    /// <returns>El <see cref="Cliente"/> encontrado, o <c>null</c>.</returns>
    public Cliente BuscarCliente(int clienteId)
    {
        return _repoClientes.Buscar(clienteId);
    }

    /// <summary>
    /// Reporte: Busca clientes cuya última interacción fue una llamada "Recibida".
    /// (Asume que una llamada recibida requiere una acción o respuesta).
    /// </summary>
    /// <returns>Una lista de clientes que esperan una respuesta.</returns>
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
            
            // Si la última interacción no es nula y es una Llamada...
            if (ultimaInteraccion != null && ultimaInteraccion is Llamada)
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
    
    /// <summary>
    /// Recopila varias estadísticas para mostrar un resumen general del sistema.
    /// </summary>
    /// <returns>Un objeto <see cref="ResumenDashboard"/> con los datos compilados.</returns>
    public ResumenDashboard ObtenerResumenDashboard()
    {
        var todosLosClientes = _repoClientes.ObtenerTodos();
        // 1. Total de clientes
        int totalClientes = todosLosClientes.Count;

        // 2. Busca todas las interacciones de todos los clientes
        List<Interaccion> todasLasInteracciones = new List<Interaccion>();
        foreach (Cliente cliente in todosLosClientes)
        {
            // Agrega todas las interacciones de este cliente a la lista grande
            todasLasInteracciones.AddRange(cliente.Interacciones);
        }

        // Ordena de más nueva a más vieja (descendente)
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
                    // Hacemos 'casting' explícito porque sabemos que es una Reunion
                    reunionesProximas.Add((Reunion)interaccion);
                }
            }
        }

        // Ordena las reuniones de más cercana a más lejana (ascendente)
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