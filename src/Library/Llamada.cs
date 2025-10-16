namespace Library;

using System;

public class Llamada : Interaccion
{
    public string TipoLlamada { get; set; }

    // Constructor
    public Llamada(DateTime fecha, string tema, string tipoLlamada)
        : base(fecha, tema)
    {
        TipoLlamada = tipoLlamada;
    }
}