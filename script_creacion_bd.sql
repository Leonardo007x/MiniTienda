-- Script de creación de base de datos para MiniTienda
-- Autor: Sistema MiniTienda
-- Fecha: Mayo 2023

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS MiniTiendaDB;
USE MiniTiendaDB;

-- Insertar una Categoría
DELIMITER //
CREATE PROCEDURE spInsertCategory(IN p_description VARCHAR(45))
BEGIN
    INSERT INTO tbl_categorias(cat_descripcion) VALUES(p_description);
END//
DELIMITER ;

-- Seleccionar todas las Categorías
DELIMITER //
CREATE PROCEDURE spSelectCategory()
BEGIN
    SELECT cat_descripcion FROM tbl_categorias;
END//
DELIMITER ;

-- Actualizar una Categoría
DELIMITER //
CREATE PROCEDURE spUpdateCategory(IN p_id INT, IN p_description VARCHAR(45))
BEGIN
    UPDATE tbl_categorias
    SET cat_descripcion = p_description
    WHERE cat_id = p_id;
END//
DELIMITER ;

-- Borrar una Categoría
DELIMITER //
CREATE PROCEDURE spDeleteCategory(IN p_id INT)
BEGIN
    DELETE FROM tbl_categorias WHERE cat_id = p_id;
END//
DELIMITER ;

-- Buscar Categorías por letra inicial
DELIMITER //
CREATE PROCEDURE spSearchCategory(IN p_letter VARCHAR(10))
BEGIN
    SELECT cat_descripcion 
    FROM tbl_categorias
    WHERE cat_descripcion LIKE CONCAT(p_letter, '%');
END//
DELIMITER ;

-- Contar Categorías que comienzan con una letra
DELIMITER //
CREATE PROCEDURE spCountCategory(IN p_letter VARCHAR(10), OUT p_quantity INT)
BEGIN
    SELECT COUNT(cat_id) INTO p_quantity
    FROM tbl_categorias
    WHERE cat_descripcion LIKE CONCAT(p_letter, '%');
END//
DELIMITER ;

-- Insertar un producto
DELIMITER //
CREATE PROCEDURE spInsertProducts(
    IN p_code VARCHAR(45), 
    IN p_description VARCHAR(100), 
    IN p_quantity INT, 
    IN p_price DOUBLE, 
    IN p_fkcategory INT, 
    IN p_fkprovider INT
)
BEGIN
    INSERT INTO tbl_productos(
        pro_codigo, 
        pro_descripcion, 
        pro_cantidad, 
        pro_precio, 
        tbl_categorias_cat_id, 
        tbl_proveedores_prov_id
    )
    VALUES(
        p_code, 
        p_description, 
        p_quantity, 
        p_price, 
        p_fkcategory, 
        p_fkprovider
    );
END//
DELIMITER ;

-- Seleccionar todos los productos
DELIMITER //
CREATE PROCEDURE spSelectProducts()
BEGIN
    SELECT 
        pro_codigo, 
        pro_descripcion, 
        pro_cantidad, 
        pro_precio, 
        tbl_categorias_cat_id, 
        cat_descripcion, 
        tbl_proveedores_prov_id, 
        prov_nombre
    FROM tbl_productos
    INNER JOIN tbl_categorias 
        ON tbl_productos.tbl_categorias_cat_id = tbl_categorias.cat_id
    INNER JOIN tbl_proveedores 
        ON tbl_productos.tbl_proveedores_prov_id = tbl_proveedores.prov_id;
END//
DELIMITER ;

-- Actualizar un producto
DELIMITER //
CREATE PROCEDURE spUpdateProduct(
    IN p_id INT,
    IN p_code VARCHAR(45),
    IN p_description VARCHAR(100),
    IN p_quantity INT,
    IN p_price DOUBLE,
    IN p_fkcategory INT,
    IN p_fkprovider INT
)
BEGIN
    UPDATE tbl_productos
    SET 
        pro_codigo = p_code,
        pro_descripcion = p_description,
        pro_cantidad = p_quantity,
        pro_precio = p_price,
        tbl_categorias_cat_id = p_fkcategory,
        tbl_proveedores_prov_id = p_fkprovider
    WHERE pro_id = p_id;
END//
DELIMITER ;

-- Selecciona únicamente el ID y la descripción de los proveedores (para combobox o listas desplegables)
DELIMITER //
CREATE PROCEDURE spSelectProvidersDDL()
BEGIN
    SELECT prov_id, prov_nombre
    FROM tbl_proveedores;
END//
DELIMITER ;

-- Selecciona todos los atributos de los proveedores
DELIMITER //
CREATE PROCEDURE spSelectProviders()
BEGIN
    SELECT prov_id, prov_nit, prov_nombre
    FROM tbl_proveedores;
END//
DELIMITER ;

-- Inserta un nuevo proveedor
DELIMITER //
CREATE PROCEDURE spInsertProvider(
    IN p_nit VARCHAR(45),
    IN p_name VARCHAR(100)
)
BEGIN
    INSERT INTO tbl_proveedores(prov_nit, prov_nombre)
    VALUES(p_nit, p_name);
END//
DELIMITER ;

-- Actualiza un proveedor existente
DELIMITER //
CREATE PROCEDURE spUpdateProvider(
    IN p_id INT,
    IN p_nit VARCHAR(45),
    IN p_name VARCHAR(100)
)
BEGIN
    UPDATE tbl_proveedores
    SET prov_nit = p_nit,
        prov_nombre = p_name
    WHERE prov_id = p_id;
END//
DELIMITER ;

-- Selecciona todos los usuarios
DELIMITER //
CREATE PROCEDURE spSelectUsers()
BEGIN
    SELECT * 
    FROM tbl_usuarios;
END//
DELIMITER ;

-- Inserta un nuevo usuario
DELIMITER //
CREATE PROCEDURE spInsertUsers(
    IN p_mail VARCHAR(80),
    IN p_password TEXT,
    IN p_salt TEXT,
    IN p_state VARCHAR(15)
)
BEGIN
    INSERT INTO tbl_usuarios(usu_correo, usu_contrasena, usu_salt, usu_estado)
    VALUES(p_mail, p_password, p_salt, p_state);
END//
DELIMITER ;

-- Actualiza un usuario
DELIMITER //
CREATE PROCEDURE spUpdateUsers(
    IN p_idspInsertCategory INT,
    IN p_mail VARCHAR(80),
    IN p_password TEXT,
    IN p_salt TEXT,
    IN p_state VARCHAR(15)
)
BEGIN
=======
-- Crear tablas de la base de datos

-- Tabla de categorías
CREATE TABLE IF NOT EXISTS tbl_categorias (
    cat_id INT AUTO_INCREMENT PRIMARY KEY,
    cat_descripcion VARCHAR(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de proveedores
CREATE TABLE IF NOT EXISTS tbl_proveedores (
    prov_id INT AUTO_INCREMENT PRIMARY KEY,
    prov_nit VARCHAR(45) NOT NULL,
    prov_nombre VARCHAR(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de usuarios
CREATE TABLE IF NOT EXISTS tbl_usuarios (
    usu_id INT AUTO_INCREMENT PRIMARY KEY,
    usu_correo VARCHAR(80) NOT NULL UNIQUE,
    usu_contrasena TEXT NOT NULL,
    usu_salt TEXT NOT NULL,
    usu_estado VARCHAR(15) DEFAULT 'activo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de productos
CREATE TABLE IF NOT EXISTS tbl_productos (
    pro_id INT AUTO_INCREMENT PRIMARY KEY,
    pro_codigo VARCHAR(45) NOT NULL UNIQUE,
    pro_descripcion VARCHAR(100) NOT NULL,
    pro_cantidad INT NOT NULL DEFAULT 0,
    pro_precio DOUBLE NOT NULL,
    tbl_categorias_cat_id INT NOT NULL,
    tbl_proveedores_prov_id INT NOT NULL,
    FOREIGN KEY (tbl_categorias_cat_id) REFERENCES tbl_categorias(cat_id),
    FOREIGN KEY (tbl_proveedores_prov_id) REFERENCES tbl_proveedores(prov_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- PROCEDIMIENTOS ALMACENADOS --

-- Insertar una Categoría
DELIMITER //
CREATE PROCEDURE spInsertCategory(IN p_description VARCHAR(45))
BEGIN
    INSERT INTO tbl_categorias(cat_descripcion) VALUES(p_description);
END//
DELIMITER ;

-- Seleccionar todas las Categorías
DELIMITER //
CREATE PROCEDURE spSelectCategory()
BEGIN
    SELECT cat_descripcion FROM tbl_categorias;
END//
DELIMITER ;

-- Actualizar una Categoría
DELIMITER //
CREATE PROCEDURE spUpdateCategory(IN p_id INT, IN p_description VARCHAR(45))
BEGIN
    UPDATE tbl_categorias
    SET cat_descripcion = p_description
    WHERE cat_id = p_id;
END//
DELIMITER ;

-- Borrar una Categoría
DELIMITER //
CREATE PROCEDURE spDeleteCategory(IN p_id INT)
BEGIN
    DELETE FROM tbl_categorias WHERE cat_id = p_id;
END//
DELIMITER ;

-- Buscar Categorías por letra inicial
DELIMITER //
CREATE PROCEDURE spSearchCategory(IN p_letter VARCHAR(10))
BEGIN
    SELECT cat_descripcion 
    FROM tbl_categorias
    WHERE cat_descripcion LIKE CONCAT(p_letter, '%');
END//
DELIMITER ;

-- Contar Categorías que comienzan con una letra
DELIMITER //
CREATE PROCEDURE spCountCategory(IN p_letter VARCHAR(10), OUT p_quantity INT)
BEGIN
    SELECT COUNT(cat_id) INTO p_quantity
    FROM tbl_categorias
    WHERE cat_descripcion LIKE CONCAT(p_letter, '%');
END//
DELIMITER ;

-- Insertar un producto
DELIMITER //
CREATE PROCEDURE spInsertProducts(
    IN p_code VARCHAR(45), 
    IN p_description VARCHAR(100), 
    IN p_quantity INT, 
    IN p_price DOUBLE, 
    IN p_fkcategory INT, 
    IN p_fkprovider INT
)
BEGIN
    INSERT INTO tbl_productos(
        pro_codigo, 
        pro_descripcion, 
        pro_cantidad, 
        pro_precio, 
        tbl_categorias_cat_id, 
        tbl_proveedores_prov_id
    )
    VALUES(
        p_code, 
        p_description, 
        p_quantity, 
        p_price, 
        p_fkcategory, 
        p_fkprovider
    );
END//
DELIMITER ;

-- Seleccionar todos los productos
DELIMITER //
CREATE PROCEDURE spSelectProducts()
BEGIN
    SELECT 
        pro_codigo, 
        pro_descripcion, 
        pro_cantidad, 
        pro_precio, 
        tbl_categorias_cat_id, 
        cat_descripcion, 
        tbl_proveedores_prov_id, 
        prov_nombre
    FROM tbl_productos
    INNER JOIN tbl_categorias 
        ON tbl_productos.tbl_categorias_cat_id = tbl_categorias.cat_id
    INNER JOIN tbl_proveedores 
        ON tbl_productos.tbl_proveedores_prov_id = tbl_proveedores.prov_id;
END//
DELIMITER ;

-- Actualizar un producto
DELIMITER //
CREATE PROCEDURE spUpdateProduct(
    IN p_id INT,
    IN p_code VARCHAR(45),
    IN p_description VARCHAR(100),
    IN p_quantity INT,
    IN p_price DOUBLE,
    IN p_fkcategory INT,
    IN p_fkprovider INT
)
BEGIN
    UPDATE tbl_productos
    SET 
        pro_codigo = p_code,
        pro_descripcion = p_description,
        pro_cantidad = p_quantity,
        pro_precio = p_price,
        tbl_categorias_cat_id = p_fkcategory,
        tbl_proveedores_prov_id = p_fkprovider
    WHERE pro_id = p_id;
END//
DELIMITER ;

-- Selecciona únicamente el ID y la descripción de los proveedores (para combobox o listas desplegables)
DELIMITER //
CREATE PROCEDURE spSelectProvidersDDL()
BEGIN
    SELECT prov_id, prov_nombre
    FROM tbl_proveedores;
END//
DELIMITER ;

-- Selecciona todos los atributos de los proveedores
DELIMITER //
CREATE PROCEDURE spSelectProviders()
BEGIN
    SELECT prov_id, prov_nit, prov_nombre
    FROM tbl_proveedores;
END//
DELIMITER ;

-- Inserta un nuevo proveedor
DELIMITER //
CREATE PROCEDURE spInsertProvider(
    IN p_nit VARCHAR(45),
    IN p_name VARCHAR(100)
)
BEGIN
    INSERT INTO tbl_proveedores(prov_nit, prov_nombre)
    VALUES(p_nit, p_name);
END//
DELIMITER ;

-- Actualiza un proveedor existente
DELIMITER //
CREATE PROCEDURE spUpdateProvider(
    IN p_id INT,
    IN p_nit VARCHAR(45),
    IN p_name VARCHAR(100)
)
BEGIN
    UPDATE tbl_proveedores
    SET prov_nit = p_nit,
        prov_nombre = p_name
    WHERE prov_id = p_id;
END//
DELIMITER ;

-- Elimina un proveedor existente
DELIMITER //
CREATE PROCEDURE spDeleteProvider(
    IN p_id INT
)
BEGIN
    DELETE FROM tbl_proveedores
    WHERE prov_id = p_id;
END//
DELIMITER ;

-- Selecciona todos los usuarios
DELIMITER //
CREATE PROCEDURE spSelectUsers()
BEGIN
    SELECT * 
    FROM tbl_usuarios;
END//
DELIMITER ;

-- Inserta un nuevo usuario
DELIMITER //
CREATE PROCEDURE spInsertUsers(
    IN p_mail VARCHAR(80),
    IN p_password TEXT,
    IN p_salt TEXT,
    IN p_state VARCHAR(15)
)
BEGIN
    INSERT INTO tbl_usuarios(usu_correo, usu_contrasena, usu_salt, usu_estado)
    VALUES(p_mail, p_password, p_salt, p_state);
END//
DELIMITER ;

-- Actualiza un usuario
DELIMITER //
CREATE PROCEDURE spUpdateUsers(
    IN p_idspInsertCategory INT,
    IN p_mail VARCHAR(80),
    IN p_password TEXT,
    IN p_salt TEXT,
    IN p_state VARCHAR(15)
)
BEGIN
    UPDATE tbl_usuarios
    SET usu_correo = p_mail,
        usu_contrasena = p_password,
        usu_salt = p_salt,
        usu_estado = p_state
    WHERE usu_id = p_idspInsertCategory;
END//

DELIMITER ;
