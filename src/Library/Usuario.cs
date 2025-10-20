using Library;

public class Usuario
{
    public int Id { get; private set; }
    public string NombreUsuario { get; set; }
    public string Contrasena { get; private set; } 
    public RolUsuario Rol { get; set; }
    public EstadoUsuario Estado { get; private set; } 

    // Constructor
    public Usuario(int id, string nombreUsuario, string contrasena, RolUsuario rol)
    {
        this.Id = id;
        this.NombreUsuario = nombreUsuario;
        this.Contrasena = contrasena; 
        this.Rol = rol;
        this.Estado = EstadoUsuario.Activo; 
    }
    
    public void Suspender()
    {
        this.Estado = EstadoUsuario.Suspendido;
    }

    public void Activar()
    {
        this.Estado = EstadoUsuario.Activo;
    }
    
    public void CambiarContrasena(string nuevaContrasena)
    {
        this.Contrasena = nuevaContrasena;
    }
}