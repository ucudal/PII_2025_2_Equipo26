using Library; // Este 'using' es para IEntidad, asumiendo que también está en Library.

namespace Library
{
    /// <summary>
    /// Representa a un usuario del sistema (Admin o Vendedor).
    /// Es el "Experto" (Expert) en su propia información y estado.
    /// </summary>
    public class Usuario : IEntidad
    {
        // --- Propiedades ---

        /// <summary>
        /// Obtiene el ID numérico único del usuario.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Obtiene o establece el nombre de login (ej: "jPerez").
        /// </summary>
        public string NombreUsuario { get; set; }
        
        // --- PROPIEDAD 'Contrasena' ELIMINADA ---
        
        /// <summary>
        /// Obtiene o establece el tipo de usuario (tomado del Enum <see cref="Rol"/>).
        /// </summary>
        public Rol Rol { get; set; }
        
        /// <summary>
        /// Obtiene el estado actual (tomado del Enum <see cref="Estado"/>).
        /// </summary>
        public Estado Estado { get; private set; } 

        // --- Constructor ---
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Usuario"/>.
        /// Por defecto, un usuario siempre se crea como <see cref="Estado.Activo"/>.
        /// </summary>
        /// <param name="id">El ID único del usuario.</param>
        /// <param name="nombreUsuario">El nombre de login.</param>
        /// <param name="rol">El rol (Admin o Vendedor).</param>
        // --- PARÁMETRO 'contrasena' ELIMINADO ---
        public Usuario(int id, string nombreUsuario, Rol rol)
        {
            this.Id = id;
            this.NombreUsuario = nombreUsuario;
            this.Rol = rol;
            
            this.Estado = Estado.Activo; 
        }
        
        // --- Métodos (Comportamientos) ---

        /// <summary>
        /// Cambia el estado interno del usuario a <see cref="Estado.Suspendido"/>.
        /// </summary>
        public void Suspender()
        {
            this.Estado = Estado.Suspendido;
        }

        /// <summary>
        /// Cambia el estado interno del usuario a <see cref="Estado.Activo"/>.
        /// </summary>
        public void Activar()
        {
            this.Estado = Estado.Activo;
        }
        
        // --- MÉTODO 'CambiarContrasena' ELIMINADO ---
    }
} // <-- AÑADE ESTA LÍNEA