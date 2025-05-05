// Importación de namespaces comunes de .NET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Data;
using MiniTienda.Data;
using System.Security.Cryptography;

namespace MiniTienda.Logic
{
    // Clase interna que actúa como puente entre la capa lógica (servicios) y la capa de datos.
    internal class CategoryLog
    {
        // Instancia privada de la clase de acceso a datos
        CategoryData objCat = new CategoryData();

        // Obtiene todas las categorías desde la base de datos
        public DataTable showCategories()
        {
            return objCat.ShowCategories();
        }

        // Inserta una nueva categoría. Devuelve un entero (posiblemente filas afectadas o ID generado)
        public int saveCategory(string _name, string _descripcion)
        {
            return objCat.SaveCategory(_name, _descripcion);
        }

        // Actualiza una categoría existente identificada por su ID
        public int updateCategory(int _id, string _name, string _descripcion)
        {
            return objCat.UpdateCategory(_id, _name, _descripcion);
        }

        // Elimina una categoría por ID
        public int deleteCategory(int _id)
        {
            return objCat.DeleteCategory(_id);
        }
    }
}
