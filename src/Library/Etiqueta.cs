namespace Library;

public class Etiqueta
{
    public int Id { get; private set; }
    public string Nombre { get; set; }

    // Constructor
    public Etiqueta(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
}