using System;

namespace MINIMAL_API.Models
{
    /// <summary>
    /// Representa um administrador no sistema.
    /// </summary>
    public class Administrador
    {
        /// <summary>
        /// Identificador único do administrador.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome de usuário do administrador.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Hash da senha do administrador.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Papel do administrador ("Admin" ou "Editor").
        /// </summary>
        public string Role { get; set; }
    }
}
