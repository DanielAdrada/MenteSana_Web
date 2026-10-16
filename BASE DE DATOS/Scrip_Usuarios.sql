-- DDL Usurios
-- CREATE – Registrar usuario
DELIMITER //

CREATE PROCEDURE proInsertUsuario(IN v_id VARCHAR(20), IN v_nombre_usuario VARCHAR(50), IN v_correo VARCHAR(100), IN v_contrasena TEXT, IN v_salt TEXT, IN v_rol VARCHAR(45))
BEGIN
    INSERT INTO tbl_usuarios
    (usu_id, usu_nombre_usuario, usu_correo, usu_contrasena, usu_salt, usu_rol)
    VALUES
    (v_id, v_nombre_usuario, v_correo, v_contrasena, v_salt, v_rol);
END//

DELIMITER ;

-- READ – Login (validación)
DELIMITER //

CREATE PROCEDURE proLoginUsuario(
    IN v_nombre_usuario VARCHAR(50),
    IN v_contrasena TEXT
)
BEGIN
    SELECT
        usu_id,
        usu_nombre_usuario,
        usu_rol
    FROM tbl_usuarios
    WHERE usu_nombre_usuario = v_nombre_usuario
      AND usu_contrasena = v_contrasena;
END//

DELIMITER ;

-- READ – Obtener usuario por ID
DELIMITER //
CREATE PROCEDURE proGetUsuarioById(IN v_id VARCHAR(20))
BEGIN
    SELECT usu_id, usu_nombre_usuario, usu_rol, usu_fecha_creacion
    FROM tbl_usuarios
    WHERE usu_id = v_id;
END//
DELIMITER ;

-- UPDATE – Cambiar contraseña
DELIMITER //
CREATE PROCEDURE proUpdateUsuarioPassword(IN v_id VARCHAR(20),IN v_contrasena TEXT,IN v_salt TEXT)
BEGIN
    UPDATE tbl_usuarios
    SET usu_contrasena = v_contrasena, usu_salt = v_salt
    WHERE usu_id = v_id;
END//
DELIMITER ;

-- DELETE – Eliminar usuario
DELIMITER //
CREATE PROCEDURE proDeleteUsuario(IN v_id VARCHAR(20))
BEGIN
    DELETE FROM tbl_usuarios
    WHERE usu_id = v_id;
END//
DELIMITER ;


