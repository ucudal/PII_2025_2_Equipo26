using System;
using System.Collections.Generic;

namespace Library
{

    /// <summary>
    /// Representa a un cliente en el sistema.
    /// Esta clase es un "Experto en Información" (Patrón Expert) sobre los datos
    /// personales y el historial de un cliente.
    /// </summary>
    public class Cliente : IEntidad
    {
        // --- Propiedades del Cliente ---

        /// <summary>
        /// Obtiene el identificador numérico único del cliente.
        /// </summary>
        /// <remarks>
        /// El 'private set' asegura que el ID solo se pueda asignar en el constructor.
        /// </remarks>
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
        public string Genero { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del cliente.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        // --- Relaciones y Listas (Agregación) ---

        /// <summary>
        /// Obtiene o establece la lista de interacciones (llamadas, reuniones, etc.)
        /// asociadas a este cliente.
        /// </summary>
        /// <remarks>
        /// Esto es un ejemplo de agregación: el Cliente "tiene" Interacciones.
        /// </remarks>
        public List<Interaccion> Interacciones { get; set; } = new List<Interaccion>();

        /// <summary>
        /// Obtiene o establece la lista de Etiquetas para clasificar al cliente.
        /// </summary>
        public List<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();

        /// <summary>
        /// Obtiene el historial de ventas cerradas con este cliente.
        /// </summary>
        public List<Venta> Ventas { get; private set; } = new List<Venta>();

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
        public Cliente(int id, string nombre, string apellido, string telefono,
            string correo, string genero, DateTime fechaNacimiento)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Telefono = telefono;
            this.Correo = correo;
            this.Genero = genero;
            this.FechaNacimiento = fechaNacimiento;
        }

        // --- Métodos ---

        /// <summary>
        /// Asigna un nuevo vendedor a este cliente.
        /// La lógica de validación (rol y estado) se delega a la <see cref="Fachada"/>,
        /// pero el Cliente (Experto) es quien finalmente realiza la asignación.
        /// </summary>
        /// <param name="nuevoVendedor">El objeto Usuario que será el nuevo vendedor.</param>
        public void AsignarVendedor(Usuario nuevoVendedor)
        {
            // La Fachada ya validó que el vendedor es válido (no nulo, rol Vendedor, estado Activo)
            if (nuevoVendedor != null && nuevoVendedor.Rol == RolUsuario.Vendedor)
            {
                this.VendedorAsignado = nuevoVendedor;
            }
        }

        /// <summary>
        /// Agrega una nueva venta al historial del cliente.
        /// </summary>
        /// <param name="venta">La venta completada.</param>
        public void AgregarVenta(Venta venta)
        {
            this.Ventas.Add(venta);
        }
    }
}