# MiniTienda - Archivos Corregidos

Este repositorio contiene los archivos modificados para resolver los problemas encontrados en el proyecto MiniTienda:

## Archivos corregidos

1. **CategoryData.cs**:
   - Modificado para mostrar los IDs reales de las categorías en lugar de usar índices secuenciales
   - Reemplazado el uso del procedimiento almacenado `spSelectCategory` por una consulta SQL directa que obtiene tanto el ID como la descripción

2. **UsersData.cs**:
   - Corregido el nombre del parámetro en el método `updateUsers()` para usar `p_idspInsertCategory` en lugar de `p_id`, coincidiendo con el procedimiento almacenado

3. **PruebaConexion.cs**:
   - Eliminada la ejecución de procedimientos almacenados que generaban errores debido a la sintaxis DELIMITER
   - Simplificado para solo verificar la conexión a la base de datos

## Problemas resueltos

1. Problema con actualización y eliminación de categorías (seleccionaba el ID incorrecto)
2. Error en `spUpdateUsers`: "Parameter 'p_idspInsertCategory' not found in the collection"
3. Errores DELIMITER al ejecutar el programa TestConexion

Estos archivos deben ser incorporados al repositorio principal.
