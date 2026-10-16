CREATE TABLE tbl_perfil (
    per_id VARCHAR(20) COLLATE utf8mb4_unicode_ci NOT NULL,
    per_foto_ruta VARCHAR(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
    per_fecha_actualizacion TIMESTAMP 
        DEFAULT CURRENT_TIMESTAMP 
        ON UPDATE CURRENT_TIMESTAMP,
    usu_id VARCHAR(20) COLLATE utf8mb4_unicode_ci NOT NULL,

    PRIMARY KEY (per_id),
    UNIQUE KEY uk_perfil_usuario (usu_id),

    CONSTRAINT fk_perfil_usuario
        FOREIGN KEY (usu_id)
        REFERENCES tbl_usuarios (usu_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_unicode_ci;
-- DDL Perfil
-- CREATE – Crear perfil de usuario
DELIMITER //
CREATE PROCEDURE proInsertPerfil(
    IN v_per_id VARCHAR(20),
    IN v_usu_id VARCHAR(20)
)
BEGIN
    INSERT INTO tbl_perfil (per_id, usu_id)
    VALUES (v_per_id, v_usu_id);
END//
DELIMITER ;

-- READ – Obtener perfil por usuario
DELIMITER //
CREATE PROCEDURE proGetPerfilByUsuario(
    IN v_usu_id VARCHAR(20)
)
BEGIN
    SELECT per_id, per_foto_ruta, per_fecha_actualizacion
    FROM tbl_perfil
    WHERE usu_id = v_usu_id;
END//
DELIMITER ;

-- READ – Obtener perfil completo
DELIMITER //
CREATE PROCEDURE proGetPerfilCompleto(
    IN v_usu_id VARCHAR(20)
)
BEGIN
    SELECT 
        e.est_nombre,
        e.est_apellido,
        p.per_foto_ruta,
        p.per_fecha_actualizacion
    FROM tbl_usuarios u
    INNER JOIN tbl_estudiantes e ON u.usu_est_id = e.est_id
    LEFT JOIN tbl_perfil p ON u.usu_id = p.usu_id
    WHERE u.usu_id = v_usu_id;
END//
DELIMITER ;

-- UPDATE – Actualizar foto de perfil
DELIMITER //
CREATE PROCEDURE proUpdateFotoPerfil(
    IN v_usu_id VARCHAR(20),
    IN v_foto_ruta VARCHAR(255)
)
BEGIN
    UPDATE tbl_perfil
    SET per_foto_ruta = v_foto_ruta
    WHERE usu_id = v_usu_id;
END//
DELIMITER ;

-- DELETE – Eliminar perfil
DELIMITER //
CREATE PROCEDURE proDeletePerfil(
    IN v_usu_id VARCHAR(20)
)
BEGIN
    DELETE FROM tbl_perfil
    WHERE usu_id = v_usu_id;
END//
DELIMITER ;
