using Library;

using System.Collections.Generic;

public class RepoEtiquetas
{
    private List<Etiqueta> _etiquetas = new List<Etiqueta>();
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