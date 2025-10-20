using Library;

using System;

public class Reunion : Interaccion
{
    public string Lugar { get; set; }

    // Constructor
    public Reunion(DateTime fecha, string tema, string lugar)
        : base(fecha, tema)
    {
        Lugar = lugar;
    }
}