/**
 * Proyecto MiniTienda - Capa Lógica de Negocio
 * 
 * Implementación de la lógica de negocio para la gestión de usuarios.
 * Esta clase proporciona métodos para operaciones CRUD, validando los datos
 * antes de enviarlos a la capa de datos.
 * 
 * Autor: Leonardo
 * Fecha: 04/05/2025        
 */

using System;
using System.Data;
using MiniTienda.Data;

namespace MiniTienda.Logic
{
    /// <summary>
    /// Clase para gestionar la lógica de negocio relacionada con usuarios.
    /// Implementa validaciones y se comunica con la capa de datos.
    /// </summary>
    public class UsersLog
    {
        /// <summary>
        /// Instancia de la clase UsersData que proporciona acceso a los datos de usuarios
        /// </summary>
        UsersData objUse = new UsersData();

        /// <summary>
        /// Obtiene todos los usuarios desde la capa de datos.
        /// </summary>
        /// <returns>DataSet con la información de los usuarios</returns>
        public DataSet showUsers()
        {
            return objUse.showUsers();
        }

        /// <summary>
        /// Guarda un nuevo usuario en el sistema después de validar los datos.
        /// </summary>
        /// <param name="_mail">Correo electrónico del usuario</param>
        /// <param name="_password">Contraseña del usuario</param>
        /// <param name="_salt">Salt para la contraseña</param>
        /// <param name="_state">Estado del usuario</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool saveUsers(string _mail, string _password, string _salt, string _state)
        {
            // Validación de datos
            if (string.IsNullOrEmpty(_mail))
                return false;
            
            if (string.IsNullOrEmpty(_password))
                return false;

            if (string.IsNullOrEmpty(_salt))
                return false;

            if (string.IsNullOrEmpty(_state))
                return false;
                
            return objUse.saveUsers(_mail, _password, _salt, _state);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente después de validar los datos.
        /// </summary>
        /// <param name="_id">ID del usuario a actualizar</param>
        /// <param name="_mail">Nuevo correo electrónico</param>
        /// <param name="_password">Nueva contraseña</param>
        /// <param name="_salt">Nuevo salt para la contraseña</param>
        /// <param name="_state">Nuevo estado del usuario</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario o si los datos no son válidos</returns>
        public bool updateUsers(int _id, string _mail, string _password, string _salt, string _state)
        {
            // Validación de datos
            if (_id <= 0)
                return false;
                
            if (string.IsNullOrEmpty(_mail))
                return false;
            
            if (string.IsNullOrEmpty(_password))
                return false;

            if (string.IsNullOrEmpty(_salt))
                return false;

            if (string.IsNullOrEmpty(_state))
                return false;
                
            return objUse.updateUsers(_id, _mail, _password, _salt, _state);
        }
    }
} 