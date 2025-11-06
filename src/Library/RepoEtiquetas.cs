using Library;
using System.Collections.Generic;

/// <summary>
/// Implementa el patrón "Repositorio" (Repository).
/// Su única responsabilidad (SRP) es administrar la colección en memoria
/// de objetos <see cref="Etiqueta"/>.
/// </summary>
public class RepoEtiquetas
{
    // --- Campos Privados ---

    /// <summary>
    /// La lista interna donde se guardan todas las etiquetas del sistema.
    /// </summary>
    private List<Etiqueta> _etiquetas = new List<Etiqueta>();
    
    /// <summary>
    /// Un contador para asignar IDs únicos y automáticos a las nuevas etiquetas.
    /// </summary>
    private int _nextId = 1;
    
    // --- Métodos Públicos (Operaciones CRUD) ---

    /// <summary>
    /// Crea una nueva etiqueta con un nombre y la agrega a la lista (Create).
    /// </summary>
    /// <param name="nombre">El nombre de la nueva etiqueta.</param>
    public void Crear(string nombre)
    {
        // '++' después de '_nextId' usa el valor actual y LUEGO lo incrementa.
        var nuevaEtiqueta = new Etiqueta(_nextId++, nombre);
        
        _etiquetas.Add(nuevaEtiqueta);
    }

    /// <summary>
    /// Devuelve la lista completa de todas las etiquetas (Read All).
    /// </summary>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Etiqueta"/>.</returns>
    public List<Etiqueta> ObtenerTodas()
    {
        return _etiquetas;
    }

    /// <summary>
    /// Busca una etiqueta específica por su ID (Read).
    /// </summary>
    /// <param name="id">El ID de la etiqueta a buscar.</param>
    /// <returns>La <see cref="Etiqueta"/> encontrada, o <c>null</c> si no existe.</returns>
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

    /// <summary>
    /// Elimina una etiqueta de la lista (Delete).
    /// </summary>
    /// <param name="id">El ID de la etiqueta a eliminar.</param>
    public void Eliminar(int id)
    {
        var etiqueta = Buscar(id);
        
        if (etiqueta != null)
        {
            _etiquetas.Remove(etiqueta);
        }
    }
}