-- Script de creación de base de datos para MiniTienda
-- Autor: Sistema MiniTienda
-- Fecha: Mayo 2023

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS minitienda;

-- Usar la base de datos
USE minitienda;

-- Crear tabla de categorías
CREATE TABLE IF NOT EXISTS categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Procedimientos almacenados para operaciones CRUD

-- Procedimiento para mostrar todas las categorías
DELIMITER //
CREATE PROCEDURE sp_show_categories()
BEGIN
    SELECT id, name, description
    FROM categories
    ORDER BY name;
END //
DELIMITER ;

-- Procedimiento para guardar una nueva categoría
DELIMITER //
CREATE PROCEDURE sp_save_category(
    IN p_name VARCHAR(100),
    IN p_description TEXT
)
BEGIN
    INSERT INTO categories (name, description)
    VALUES (p_name, p_description);
    SELECT LAST_INSERT_ID() AS id;
END //
DELIMITER ;

-- Procedimiento para actualizar una categoría
DELIMITER //
CREATE PROCEDURE sp_update_category(
    IN p_id INT,
    IN p_name VARCHAR(100),
    IN p_description TEXT
)
BEGIN
    UPDATE categories
    SET name = p_name,
        description = p_description
    WHERE id = p_id;
    
    SELECT ROW_COUNT() AS rows_affected;
END //
DELIMITER ;

-- Procedimiento para eliminar una categoría
DELIMITER //
CREATE PROCEDURE sp_delete_category(
    IN p_id INT
)
BEGIN
    DELETE FROM categories
    WHERE id = p_id;
    
    SELECT ROW_COUNT() AS rows_affected;
END //
DELIMITER ; 