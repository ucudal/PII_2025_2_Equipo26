using Library;

/// <summary>
/// Representa una Etiqueta o "Tag" (ej: "VIP", "Nuevo", "Inactivo").
/// Sirve para clasificar o agrupar objetos <see cref="Cliente"/>.
/// </summary>
public class Etiqueta : IEntidad
{
    // --- Propiedades ---

    /// <summary>
    /// Obtiene el identificador numérico único para la etiqueta.
    /// </summary>
    /// <remarks>
    /// 'private set' significa que su valor solo se puede asignar en el constructor.
    /// </remarks>
    public int Id { get; private set; }
    
    /// <summary>
    /// Obtiene o establece el texto visible de la etiqueta (ej: "Cliente Frecuente").
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Etiqueta"/>.
    /// </summary>
    /// <param name="id">El ID único para la etiqueta.</param>
    /// <param name="nombre">El nombre de la etiqueta.</param>
    public Etiqueta(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
}