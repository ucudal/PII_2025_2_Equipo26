using Library;
using System.Collections.Generic;

// Esta clase es un modelo simple, no hace nada por sí misma.
// Solo sirve para "agrupar" los diferentes datos que se
// mostrarán en el panel o dashboard principal.
public class ResumenDashboard
{
    // Guarda el número total de clientes en el sistema.
    public int TotalClientes { get; set; }
    
    // Una lista con las interacciones más nuevas (ej: las últimas 5).
    public List<Interaccion> InteraccionesRecientes { get; set; }
    
    // Una lista de las reuniones que están agendadas para el futuro.
    public List<Reunion> ReunionesProximas { get; set; }
}
}
