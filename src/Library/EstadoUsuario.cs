using Library;

/// <summary>
/// Define los posibles estados de un <see cref="Usuario"/> en el sistema.
/// Una enumeración (enum) es un tipo de dato por valor que define un conjunto
/// de constantes con nombre.
/// </summary>
public enum EstadoUsuario
{
    /// <summary>
    /// El usuario está habilitado para usar el sistema.
    /// </summary>
    Activo,
    
    /// <summary>
    /// El usuario está bloqueado o deshabilitado temporalmente.
    /// </summary>
    Suspendido
}