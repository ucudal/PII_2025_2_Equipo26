using Library;

/// <summary>
/// Representa una nota de texto simple.
/// Se usa (mediante Agregación) en <see cref="Interaccion"/> 
/// para añadir detalles extra.
/// </summary>
public class Nota
{
    // --- Propiedades ---
    
    /// <summary>
    /// Obtiene o establece el contenido textual de la nota.
    /// </summary>
    public string Texto { get; set; }

    // --- Constructor ---
    
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Nota"/>.
    /// </summary>
    /// <param name="texto">El contenido de la nota.</param>
    public Nota(string texto)
    {
        Texto = texto;
    }
}