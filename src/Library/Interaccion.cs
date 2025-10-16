namespace Library;

using System;

public abstract class Interaccion
{
    public DateTime Fecha { get; set; }
    public string Tema { get; set; }
    public Nota NotaAdicional { get; set; }

    // Constructor
    protected Interaccion(DateTime fecha, string tema)
    {
        Fecha = fecha;
        Tema = tema;
    }
}