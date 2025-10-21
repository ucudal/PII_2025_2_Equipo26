using Library;

using System.Collections.Generic;

// Esta clase es un "Repositorio" (Patrón Repository).
// Su única responsabilidad es guardar y administrar la lista de Etiquetas.
public class RepoEtiquetas
{
    // --- Campos Privados ---

    // La lista interna donde se guardan todas las etiquetas del sistema.
    private List<Etiqueta> _etiquetas = new List<Etiqueta>();
    
    // Un contador para asignar IDs únicos y automáticos a las nuevas etiquetas.
    private int _nextId = 1;
    
    // --- Métodos Públicos (Operaciones CRUD) ---

    // Crea una nueva etiqueta con un nombre y la agrega a la lista (Create).
    public void Crear(string nombre)
    {
        // 1. Crea el nuevo objeto Etiqueta, usando el contador de ID.
        //    '++' después de '_nextId' usa el valor actual y LUEGO lo incrementa.
        var nuevaEtiqueta = new Etiqueta(_nextId++, nombre);
        
        // 2. Agrega la etiqueta a la lista interna.
        _etiquetas.Add(nuevaEtiqueta);
    }

    // Devuelve la lista completa de todas las etiquetas (Read All).
    public List<Etiqueta> ObtenerTodas()
    {
        // Devuelve la referencia a la lista interna.
        return _etiquetas;
    }

    // Busca una etiqueta específica por su ID (Read).
    public Etiqueta Buscar(int id)
    {
        // Recorre la lista una por una.
        foreach (var etiqueta in _etiquetas)
        {
            // Si encuentra una con el ID buscado...
            if (etiqueta.Id == id)
            {
                // ...la devuelve.
                return etiqueta;
            }
        }
        // Si el 'foreach' termina y no encontró nada, devuelve null.
        return null;
    }

    // Elimina una etiqueta de la lista (Delete).
    public void Eliminar(int id)
    {
        // 1. Usa el método 'Buscar' de esta misma clase para encontrar la etiqueta.
        var etiqueta = Buscar(id);
        
        // 2. Si la encontró (no es null)...
        if (etiqueta != null)
        {
            // 3. ...la quita de la lista.
            _etiquetas.Remove(etiqueta);
        }
    }
}
