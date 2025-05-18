/**
 * Proyecto MiniTienda - Capa de Modelo
 * 
 * Clase que representa la entidad Usuario en el sistema.
 * Contiene las propiedades básicas para la autenticación y gestión de usuarios.
 * 
 * Autor: Leonardo
 * Fecha: 15/05/2025
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTienda.Model
{
    /// <summary>
    /// Clase que representa un usuario en el sistema.
    /// Contiene las propiedades necesarias para la autenticación y gestión de permisos.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Correo electrónico del usuario. Utilizado como identificador único.
        /// </summary>
        public string Correo { get; set; }

        /// <summary>
        /// Contraseña cifrada del usuario.
        /// </summary>
        public string Contrasena { get; set; }

        /// <summary>
        /// Salt criptográfica utilizada para el cifrado de la contraseña.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Estado del usuario (Activo, Inactivo, Bloqueado, etc.).
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Tipo o rol del usuario en el sistema (Admin, Usuario, etc.).
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Constructor con parámetros para inicializar todas las propiedades.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario</param>
        /// <param name="contrasena">Contraseña del usuario (ya cifrada)</param>
        /// <param name="salt">Salt criptográfica utilizada para el cifrado</param>
        /// <param name="estado">Estado del usuario en el sistema</param>
        public User(string correo, string contrasena, string salt, string estado)
        {
            this.Correo = correo;
            this.Contrasena = contrasena;
            this.Salt = salt;
            this.State = estado;
        }

        /// <summary>
        /// Constructor con ID y tipo/rol para inicializar todas las propiedades.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="correo">Correo electrónico del usuario</param>
        /// <param name="contrasena">Contraseña del usuario (ya cifrada)</param>
        /// <param name="salt">Salt criptográfica utilizada para el cifrado</param>
        /// <param name="estado">Estado del usuario en el sistema</param>
        /// <param name="tipo">Tipo o rol del usuario</param>
        public User(int id, string correo, string contrasena, string salt, string estado, string tipo)
        {
            this.Id = id;
            this.Correo = correo;
            this.Contrasena = contrasena;
            this.Salt = salt;
            this.State = estado;
            this.Tipo = tipo;
        }
    }
} 