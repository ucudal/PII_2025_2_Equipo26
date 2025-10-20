
using Library;

using System;

public abstract class Interaccion
{
    public DateTime Fecha { get; set; }
    public string Tema { get; set; }
    public Nota NotaAdicional { get; set; }

    public Interaccion(DateTime fecha, string tema)
    {
        this.Fecha = fecha;
        this.Tema = tema;
    }
}