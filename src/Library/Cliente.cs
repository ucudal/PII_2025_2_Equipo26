using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// Representa a un cliente en el sistema.
/// Esta clase es un "Experto en Información" (Patrón Expert) sobre los datos
/// personales y el historial de un cliente.
/// </summary>
namespace Library
{
    public class Cliente : IEntidad, IBuscable
    {
        // --- Propiedades del Cliente ---

        /// <summary>
        /// Obtiene el identificador numérico único del cliente.
        /// </summary>
        /// <remarks>
        /// El 'private set' asegura que el ID solo se pueda asignar en el constructor.
        /// </remarks>

        // --- CORRECCIÓN ---
        // Se cambia 'private set' por 'set' para que el RepoBase pueda asignar el ID.
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del cliente.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el apellido del cliente.
        /// </summary>
        public string Apellido { get; set; }

        /// <summary>
        /// Obtiene o establece el teléfono de contacto del cliente.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del cliente.
        /// </summary>
        public string Correo { get; set; }

        /// <summary>
        /// Obtiene o establece el género del cliente.
        /// </summary>
        public GeneroCliente Genero { get; set; }
        

        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del cliente.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        // --- Relaciones y Listas (Agregación) ---

        private List<Interaccion> _interacciones = new List<Interaccion>();
        private List<Etiqueta> _etiquetas = new List<Etiqueta>();
        private List<Venta> _ventas = new List<Venta>();

        /// <summary>
        /// Obtiene la lista de interacciones (llamadas, reuniones, etc.)
        /// asociadas a este cliente.
        /// </summary>
        public IReadOnlyList<Interaccion> Interacciones
        {
            get { return _interacciones.AsReadOnly(); }
        }

        /// <summary>
        /// Obtiene la lista de Etiquetas para clasificar al cliente.
        /// </summary>
        public IReadOnlyList<Etiqueta> Etiquetas
        {
            get { return _etiquetas.AsReadOnly(); }
        }

        /// <summary>
        /// Obtiene el historial de ventas cerradas con este cliente.
        /// </summary>
        public IReadOnlyList<Venta> Ventas
        {
            get { return _ventas.AsReadOnly(); }
        }

        /// <summary>
        /// Obtiene el usuario (vendedor) que está a cargo de este cliente.
        /// </summary>
        public Usuario VendedorAsignado { get; private set; }

        // --- Constructor ---

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Cliente"/>.
        /// </summary>
        /// <param name="id">El ID único del cliente.</param>
        /// <param name="nombre">El nombre del cliente.</param>
        /// <param name="apellido">El apellido del cliente.</param>
        /// <param name="telefono">El teléfono del cliente.</param>
        /// <param name="correo">El correo electrónico del cliente.</param>
        /// <param name="genero">El género del cliente.</param>
        /// <param name="fechaNacimiento">La fecha de nacimiento del cliente.</param>
        public Cliente(string nombre, string apellido, string telefono, string correo, string generoTexto,
            DateTime fechaNacimiento)
        {
            if (nombre == null || nombre == "") throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            if (apellido == null || apellido == "") throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            if (telefono == null || telefono == "") throw new ArgumentException("El teléfono no puede ser nulo o vacío.", nameof(telefono));
            if (correo == null || correo == "") throw new ArgumentException("El correo no puede ser nulo o vacío.", nameof(correo));
            if (generoTexto == null || generoTexto == "") throw new ArgumentException("El género no puede ser nulo o vacío.", nameof(generoTexto));
            
            GeneroCliente generoEnum;

            // Enum.TryParse con el parámetro 'ignoreCase' (true) es compatible con C# 6.
            if (!Enum.TryParse(generoTexto, true, out generoEnum))
            {
                // Si la conversión falla, se lanza una excepción con un mensaje útil.
                throw new ArgumentException($"El valor '{generoTexto}' no es un género válido. Use: {String.Join(", ", Enum.GetNames(typeof(GeneroCliente)))}", nameof(generoTexto));
            }
            
            // Ya NO se asigna el 'Id' aquí.
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Telefono = telefono;
            this.Correo = correo;
            this.Genero = generoEnum;
            this.FechaNacimiento = fechaNacimiento;
        }

        // --- Métodos ---

        /// <summary>
        /// Asigna un nuevo vendedor a este cliente.
        /// Aplica el patrón Expert: Cliente conoce sus datos y relaciones.
        /// La validación de reglas de negocio complejas (como roles) puede delegarse,
        /// pero el Cliente mantiene la integridad de su estado.
        /// </summary>
        /// <param name="nuevoVendedor">El objeto Usuario que será el nuevo vendedor.</param>
        public void AsignarVendedor(Usuario nuevoVendedor)
        {
            if (nuevoVendedor == null)
                throw new ArgumentNullException(nameof(nuevoVendedor), "El vendedor no puede ser nulo.");

            bool esVendedor = false;

            foreach (Rol r in nuevoVendedor.Roles)
            {
                if (r == Rol.Vendedor)
                {
                    esVendedor = true;
                    break;
                }
            }
            if (nuevoVendedor.Estado == Estado.Suspendido)
            {
                throw new InvalidOperationException($"El vendedor '{nuevoVendedor.NombreUsuario}' está suspendido y no puede ser asignado.");
            }
            if (!esVendedor)
            {
                throw new InvalidOperationException("El usuario asignado no tiene el rol de Vendedor.");
            }
            this.VendedorAsignado = nuevoVendedor;
        }

        /// <summary>
        /// Agrega una nueva venta al historial del cliente.
        /// </summary>
        /// <param name="venta">La venta completada.</param>
        public void AgregarVenta(Venta venta)
        {
            if (venta == null)
            {
                throw new ArgumentNullException(nameof(venta), "La venta no puede ser nula.");
            }
            this._ventas.Add(venta);
        }

        /// <summary>
        /// Agrega una interacción al cliente.
        /// </summary>
        /// <param name="interaccion">La interacción a agregar.</param>
        public void AgregarInteraccion(Interaccion interaccion)
        {
            if (interaccion == null)
            {
                throw new ArgumentNullException(nameof(interaccion), "La interacción no puede ser nula.");
            }
            this._interacciones.Add(interaccion);
        }

        /// <summary>
        /// Agrega una etiqueta al cliente.
        /// </summary>
        /// <param name="etiqueta">La etiqueta a agregar.</param>
        public void AgregarEtiqueta(Etiqueta etiqueta)
        {
            if (etiqueta == null)
            {
                throw new ArgumentNullException(nameof(etiqueta), "La etiqueta no puede ser nula.");
            }
            if (!this._etiquetas.Contains(etiqueta))
            {
                this._etiquetas.Add(etiqueta);
            }
        }

        /// <summary>
        /// Quita una etiqueta del cliente.
        /// </summary>
        /// <param name="etiqueta">La etiqueta a quitar.</param>
        public void QuitarEtiqueta(Etiqueta etiqueta)
        {
            if (etiqueta != null)
            {
                this._etiquetas.Remove(etiqueta);
            }
        }

        /// <summary>
        /// Verifica si el cliente coincide con un término de búsqueda.
        /// Busca en Nombre, Apellido, Teléfono y Correo.
        /// </summary>
        /// <param name="termino">El término a buscar.</param>
        /// <returns>True si coincide, False en caso contrario.</returns>
        public bool Coincide(string termino)
        {
            if (termino == null || termino == "")
            {
                return false;
            }

            string busqueda = termino.ToLower();

            if (this.Nombre != null && this.Nombre != "" && this.Nombre.ToLower().Contains(busqueda))
            {
                return true;
            }
            if (this.Apellido != null && this.Apellido != "" && this.Apellido.ToLower().Contains(busqueda))
            {
                return true;
            }
            if (this.Telefono != null && this.Telefono != "" && this.Telefono.Contains(busqueda))
            {
                return true;
            }
            if (this.Correo != null && this.Correo != "" && this.Correo.ToLower().Contains(busqueda))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Obtiene la fecha de la interacción más reciente registrada para este cliente.
        /// Aplica el patrón Expert: El cliente es responsable de calcular la fecha a partir
        /// de su colección de interacciones (Agregación).
        /// </summary>
        /// <returns>La fecha y hora de la última interacción, o DateTime.MinValue si no hay interacciones.</returns>
        public DateTime ObtenerFechaUltimaInteraccion()
        {
            // Verificamos si la colección está vacía.
            if (this._interacciones.Count == 0)
            {
                // Si no hay interacciones, devolvemos la fecha mínima para que la Fachada
                // pueda interpretarlo como "nunca tuvo interacción".
                return DateTime.MinValue; 
            }

            DateTime fechaMasReciente = DateTime.MinValue;
            
            // Recorremos la colección IReadOnlyList<Interaccion> (propiedad Interacciones)
            // o el campo privado _interacciones.
            foreach (var interaccion in this._interacciones)
            {
                // La Interacción debe tener una propiedad pública 'Fecha'.
                if (interaccion.Fecha > fechaMasReciente)
                {
                    fechaMasReciente = interaccion.Fecha;
                }
            }
            
            return fechaMasReciente;
        }


        /// <summary>
        /// Verifica que la venta este en rango del cliente
        /// </summary>
        /// <returns>Verdadero o Falso</returns>
        public bool TieneVentaEnRango(double montoInicio, double montoFin)
        {
            foreach (Venta venta in this.Ventas)
            {
                if (venta.Importe >= montoInicio && venta.Importe <= montoFin)
                {
                    return true; 
                }
            }
            return false; 
        }
    }
}
