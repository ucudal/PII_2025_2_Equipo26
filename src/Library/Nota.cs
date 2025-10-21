using Library;

// Esta clase representa una nota de texto simple.
// Se usa para agregar detalles extra a una 'Interaccion'.
public class Nota
{
    // --- Propiedades ---
    
    // El contenido de la nota.
    public string Texto { get; set; }

    // --- Constructor ---
    // La "receta" para crear una Nota nueva.
    // Obliga a que siempre le pases el texto al crearla.
    public Nota(string texto)
    {
        Texto = texto;
    }
}

