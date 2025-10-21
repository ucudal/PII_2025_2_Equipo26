using Library;

using System.Collections.Generic;

public class RepoUsuarios
{
    private List<Usuario> _usuarios = new List<Usuario>();
    private int _nextId = 1;

    public Usuario Agregar(string nombreUsuario, string contrasena, RolUsuario rol)
    {
        var nuevoUsuario = new Usuario(_nextId++, nombreUsuario, contrasena, rol);
        _usuarios.Add(nuevoUsuario);
        return nuevoUsuario;
    }

    public Usuario Buscar(int id)
    {
        foreach (var usuario in _usuarios)
        {
            if (usuario.Id == id)
            {
                return usuario;
            }
        }
        return null;
    }

    public void Suspender(int id)
    {
        var usuario = Buscar(id);
        if (usuario != null)
        {
            usuario.Suspender(); 
        }
    }
    public void Activar(int id)
    {
        var usuario = Buscar(id);
        if (usuario != null)
        {
            usuario.Activar();
        }
    }
    public void Eliminar(int id)
    {
        var usuario = Buscar(id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
        }
    }
    
    public List<Usuario> ObtenerTodos()
    {
        return _usuarios;
    }
}