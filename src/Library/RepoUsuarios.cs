using Library;

using System.Collections.Generic;

// Esta clase es un "Repositorio" (Patrón Repository).
// Su única responsabilidad es guardar y administrar la lista de Usuarios
// (Vendedores y Administradores).
public class RepoUsuarios
{
    // --- Campos Privados ---

    // La lista interna donde se guardan los objetos Usuario.
    private List<Usuario> _usuarios = new List<Usuario>();
    
    // Un contador para asignar IDs únicos y automáticos.
    private int _nextId = 1;

    // --- Métodos Públicos (Operaciones CRUD) ---

    // Agrega un nuevo usuario a la lista (Create).
    public Usuario Agregar(string nombreUsuario, string contrasena, RolUsuario rol)
    {
        // 1. Crea el nuevo objeto Usuario usando el contador de ID.
        var nuevoUsuario = new Usuario(_nextId++, nombreUsuario, contrasena, rol);
        
        // 2. Lo añade a la lista interna.
        _usuarios.Add(nuevoUsuario);
        
        // 3. Devuelve el usuario recién creado (por si se necesita su ID).
        return nuevoUsuario;
    }

    // Busca un usuario por su ID (Read).
    public Usuario Buscar(int id)
    {
        // Recorre la lista de usuarios.
        foreach (var usuario in _usuarios)
        {
            // Si encuentra el ID, lo devuelve.
            if (usuario.Id == id)
            {
                return usuario;
            }
        }
        // Si no lo encuentra, devuelve null.
        return null;
    }

    // Cambia el estado de un usuario a 'Suspendido'.
    public void Suspender(int id)
    {
        // 1. Busca al usuario.
        var usuario = Buscar(id);
        
        // 2. Si existe...
        if (usuario != null)
        {
            // 3. ...le "pide" al objeto Usuario que se suspenda a sí mismo.
            //    (Esto es delegación de responsabilidad).
            usuario.Suspender(); 
        }
    }
    
    // Cambia el estado de un usuario a 'Activo'.
    public void Activar(int id)
    {
        // 1. Busca al usuario.
        var usuario = Buscar(id);

        // 2. Si existe...
        if (usuario != null)
        {
            // 3. ...le "pide" al objeto Usuario que se active a sí mismo.
            usuario.Activar();
        }
    }

    // Elimina un usuario de la lista (Delete).
    public void Eliminar(int id)
    {
        // 1. Busca al usuario.
        var usuario = Buscar(id);
        
        // 2. Si existe...
        if (usuario != null)
        {
            // 3. ...lo quita de la lista.
            _usuarios.Remove(usuario);
        }
    }
    
    // Devuelve la lista completa de todos los usuarios (Read All).
    public List<Usuario> ObtenerTodos()
    {
        // Devuelve la referencia a la lista interna.
        return _usuarios;
    }
}
