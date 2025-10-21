using Library;

// Esta clase representa a un usuario del sistema (Admin o Vendedor).
public class Usuario
{
    // --- Propiedades ---

    // ID numérico único. 'private set' significa que no se puede
    // cambiar después de que se crea el objeto.
    public int Id { get; private set; }
    
    // Nombre de login (ej: "jPerez").
    public string NombreUsuario { get; set; }
    
    // Contraseña. 'private set' significa que solo se puede cambiar
    // usando un método de esta clase (ej: CambiarContrasena).
    public string Contrasena { get; private set; } 
    
    // El tipo de usuario (tomado del Enum 'RolUsuario').
    public RolUsuario Rol { get; set; }
    
    // El estado actual (tomado del Enum 'EstadoUsuario').
    // 'private set' para que solo se cambie con Activar/Suspender.
    public EstadoUsuario Estado { get; private set; } 

    // --- Constructor ---
    // Es la "receta" para crear un nuevo objeto Usuario.
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

    // Método público que cambia el estado interno a 'Suspendido'.
    public void Suspender()
    {
        this.Estado = EstadoUsuario.Suspendido;
    }

    // Método público que cambia el estado interno a 'Activo'.
    public void Activar()
    {
        this.Estado = EstadoUsuario.Activo;
    }
    
    // Método público que permite actualizar la contraseña.
    public void CambiarContrasena(string nuevaContrasena)
    {
        // (Aquí se podría agregar lógica de validación,
        //  como "que no sea la misma de antes" o "que tenga 8 caracteres").
        this.Contrasena = nuevaContrasena;
    }
}
