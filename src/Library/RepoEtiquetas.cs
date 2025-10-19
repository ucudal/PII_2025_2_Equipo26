namespace Library;

using System.Collections.Generic;

/// <summary>
/// Repositorio encargado de gestionar todas las etiquetas del sistema.
/// Sigue el principio Expert, ya que es el experto en almacenar y manejar etiquetas.
/// </summary>
public class RepoEtiquetas
{
    private List<Etiqueta> _etiquetas = new();
    private int _nextId = 1;
    
    public void Crear(string nombre)
    {
        
        var nuevaEtiqueta = new Etiqueta(_nextId++, nombre);
        _etiquetas.Add(nuevaEtiqueta);
    }


    public List<Etiqueta> ObtenerTodas()
    {
        return _etiquetas;
    }


    public Etiqueta Buscar(int id)
    {
        foreach (var etiqueta in _etiquetas)
        {
            if (etiqueta.Id == id)
            {
                return etiqueta;
            }
        }
        return null;
    }

    public void Eliminar(int id)
    {
        var etiqueta = Buscar(id);
        if (etiqueta != null)
        {
            _etiquetas.Remove(etiqueta);
        }
    }
}