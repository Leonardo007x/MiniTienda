/**
 * Proyecto MiniTienda - Prueba de Conexión a la Base de Datos
 * 
 * Este programa de prueba verifica la conexión a la base de datos MySQL
 * utilizando la clase PruebaConexion del proyecto MiniTienda.Data.
 * 
 * Autor: Leonardo
 * Fecha: 02/05/2025
 */

using System;
using MiniTienda.Data;

namespace TestConexion
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada para la aplicación de prueba de conexión
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal que ejecuta la prueba de conexión a la base de datos
        /// </summary>
        /// <param name="args">Argumentos de la línea de comandos (no utilizados)</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Probando conexión a la base de datos...");
            
            // Llamar al método que verifica la conexión a la base de datos
            bool resultado = PruebaConexion.VerificarConexion();
            
            // Mostrar mensaje según el resultado de la prueba
            if (resultado)
            {
                Console.WriteLine("La conexión a la base de datos funciona correctamente.");
            }
            else
            {
                Console.WriteLine("No se pudo conectar a la base de datos. Revisa la configuración en App.config.");
            }
            
            // Esperar a que el usuario presione una tecla antes de cerrar la aplicación
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
} 