using Library;

/// <summary>
/// Define los roles que un <see cref="Usuario"/> puede tener en el sistema.
/// Una enumeración (enum) es un tipo de dato por valor que define un conjunto
/// de constantes con nombre.
/// </summary>
namespace Library
{


    public enum Rol
    {
        /// <summary>
        /// Un usuario que puede configurar el sistema, crear otros usuarios, etc.
        /// </summary>
        Administrador,

        /// <summary>
        /// Un usuario que interactúa con los clientes y registra ventas.
        /// </summary>
        Vendedor
    }
}