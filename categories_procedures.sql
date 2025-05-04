/**
 * Proyecto MiniTienda - Procedimientos Almacenados para Categorías
 * 
 * Este script crea los procedimientos almacenados necesarios para realizar
 * operaciones CRUD (Create, Read, Update, Delete) en la tabla tbl_categoria.
 * 
 * Autor: Leonardo
 * Fecha: Octubre 2023
 */

-- Seleccionar la base de datos a utilizar
USE minitiendadb;

/**
 * Procedimiento para mostrar todas las categorías
 * 
 * Este procedimiento recupera todas las categorías de la tabla tbl_categoria
 * ordenadas por descripción.
 * 
 * Devuelve:
 *   - id: Identificador único de la categoría (cat_id)
 *   - description: Descripción de la categoría (cat_descripcion)
 */
DROP PROCEDURE IF EXISTS sp_show_categories;
DELIMITER //
CREATE PROCEDURE sp_show_categories()
BEGIN
    SELECT cat_id AS id, cat_descripcion AS description
    FROM tbl_categoria
    ORDER BY cat_descripcion;
END //
DELIMITER ;

/**
 * Procedimiento para guardar una nueva categoría
 * 
 * Este procedimiento inserta un nuevo registro en la tabla tbl_categoria
 * y devuelve el ID generado automáticamente.
 * 
 * Parámetros:
 *   - p_name: Nombre de la categoría (no utilizado en esta versión)
 *   - p_description: Descripción de la categoría
 * 
 * Devuelve:
 *   - id: ID de la categoría recién creada (LAST_INSERT_ID)
 */
DROP PROCEDURE IF EXISTS sp_save_category;
DELIMITER //
CREATE PROCEDURE sp_save_category(
    IN p_name VARCHAR(45),
    IN p_description VARCHAR(45)
)
BEGIN
    -- Insertar nueva categoría
    INSERT INTO tbl_categoria (cat_descripcion)
    VALUES (p_description);
    
    -- Devolver el ID insertado
    SELECT LAST_INSERT_ID() AS id;
END //
DELIMITER ;

/**
 * Procedimiento para actualizar una categoría existente
 * 
 * Este procedimiento actualiza la descripción de una categoría existente
 * basado en su ID y devuelve el número de filas afectadas.
 * 
 * Parámetros:
 *   - p_id: ID de la categoría a actualizar
 *   - p_name: Nuevo nombre (no utilizado en esta versión)
 *   - p_description: Nueva descripción
 * 
 * Devuelve:
 *   - affected_rows: Número de filas afectadas (0 si no se encontró la categoría)
 */
DROP PROCEDURE IF EXISTS sp_update_category;
DELIMITER //
CREATE PROCEDURE sp_update_category(
    IN p_id INT,
    IN p_name VARCHAR(45),
    IN p_description VARCHAR(45)
)
BEGIN
    -- Actualizar la categoría
    UPDATE tbl_categoria
    SET cat_descripcion = p_description
    WHERE cat_id = p_id;
    
    -- Devolver el número de filas afectadas
    SELECT ROW_COUNT() AS affected_rows;
END //
DELIMITER ;

/**
 * Procedimiento para eliminar una categoría
 * 
 * Este procedimiento elimina una categoría de la tabla tbl_categoria
 * basado en su ID y devuelve el número de filas afectadas.
 * 
 * Parámetros:
 *   - p_id: ID de la categoría a eliminar
 * 
 * Devuelve:
 *   - affected_rows: Número de filas afectadas (0 si no se encontró la categoría)
 * 
 * Nota: Este procedimiento no verifica dependencias. Si existen registros
 * en otras tablas que hacen referencia a esta categoría, la eliminación
 * podría fallar debido a restricciones de integridad referencial.
 */
DROP PROCEDURE IF EXISTS sp_delete_category;
DELIMITER //
CREATE PROCEDURE sp_delete_category(
    IN p_id INT
)
BEGIN
    -- Eliminar la categoría
    DELETE FROM tbl_categoria
    WHERE cat_id = p_id;
    
    -- Devolver el número de filas afectadas
    SELECT ROW_COUNT() AS affected_rows;
END //
DELIMITER ; 