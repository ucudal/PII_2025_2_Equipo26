using Library;
using System.Collections.Generic;

/// <summary>
/// Clase modelo simple (a veces llamada DTO - Data Transfer Object).
/// Su única responsabilidad es agrupar los diferentes datos que se
/// mostrarán en el panel o dashboard principal.
/// </summary>
public class ResumenDashboard
{
    /// <summary>
    /// Obtiene o establece el número total de clientes en el sistema.
    /// </summary>
    public int TotalClientes { get; set; }
    
    /// <summary>
    /// Obtiene o establece una lista con las interacciones más nuevas (ej: las últimas 5).
    /// </summary>
    public List<Interaccion> InteraccionesRecientes { get; set; }
    
    /// <summary>
    /// Obtiene o establece una lista de las reuniones que están agendadas para el futuro.
    /// </summary>
    public List<Reunion> ReunionesProximas { get; set; }
}