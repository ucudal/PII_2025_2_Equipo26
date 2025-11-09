using Library;

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
    /// <remarks>
    /// No se puede cambiar después de que se crea el objeto (private set).
    /// </remarks>
    public int Id { get; set; }
    
    /// <summary>
    /// Obtiene o establece el nombre de login (ej: "jPerez").
    /// </summary>
    public string NombreUsuario { get; set; }
    
    /// <summary>
    /// Obtiene la contraseña del usuario.
    /// </summary>
    /// <remarks>
    /// Solo se puede cambiar usando <see cref="CambiarContrasena(string)"/>.
    /// </remarks>
    public string Contrasena { get; private set; } 
    
    /// <summary>
    /// Obtiene o establece el tipo de usuario (tomado del Enum <see cref="RolUsuario"/>).
    /// </summary>
    public RolUsuario Rol { get; set; }
    
    /// <summary>
    /// Obtiene el estado actual (tomado del Enum <see cref="EstadoUsuario"/>).
    /// </summary>
    /// <remarks>
    /// Solo se puede cambiar con <see cref="Activar()"/> o <see cref="Suspender()"/>.
    /// </remarks>
    public EstadoUsuario Estado { get; private set; } 

    // --- Constructor ---
    
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Usuario"/>.
    /// Por defecto, un usuario siempre se crea como <see cref="EstadoUsuario.Activo"/>.
    /// </summary>
    /// <param name="id">El ID único del usuario.</param>
    /// <param name="nombreUsuario">El nombre de login.</param>
    /// <param name="contrasena">La contraseña.</param>
    /// <param name="rol">El rol (Admin o Vendedor).</param>
    public Usuario(int id, string nombreUsuario, string contrasena, RolUsuario rol)
    {
        this.Id = id;
        this.NombreUsuario = nombreUsuario;
        this.Contrasena = contrasena; 
        this.Rol = rol;
        
        // Por defecto, un usuario siempre se crea como 'Activo'.
        this.Estado = EstadoUsuario.Activo; 
    }
    
    // --- Métodos (Comportamientos) ---

    /// <summary>
    /// Cambia el estado interno del usuario a <see cref="EstadoUsuario.Suspendido"/>.
    /// </summary>
    public void Suspender()
    {
        this.Estado = EstadoUsuario.Suspendido;
    }

    /// <summary>
    /// Cambia el estado interno del usuario a <see cref="EstadoUsuario.Activo"/>.
    /// </summary>
    public void Activar()
    {
        this.Estado = EstadoUsuario.Activo;
    }
    
    /// <summary>
    /// Permite actualizar la contraseña del usuario.
    /// </summary>
    /// <param name="nuevaContrasena">La nueva contraseña (sin encriptar).</param>
    public void CambiarContrasena(string nuevaContrasena)
    {
        // (Aquí se podría agregar lógica de validación,
        //  como "que no sea la misma de antes" o "que tenga 8 caracteres").
        this.Contrasena = nuevaContrasena;
    }
}