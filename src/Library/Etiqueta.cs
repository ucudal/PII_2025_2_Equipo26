using Library;

// Esta clase representa una Etiqueta o "Tag".
// Sirve para clasificar o agrupar clientes (ej: "VIP", "Nuevo", "Inactivo").
public class Etiqueta
{
    // --- Propiedades ---

    // Identificador numérico único para la etiqueta.
    // 'private set' significa que su valor solo se puede poner en el constructor.
    public int Id { get; private set; }
    // El texto visible de la etiqueta (ej: "Cliente Frecuente").
    public string Nombre { get; set; }

    // Constructor
    // Es la "receta" para crear una nueva etiqueta.
    // Pide un ID y un Nombre para crear el objeto.
    public Etiqueta(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
}
