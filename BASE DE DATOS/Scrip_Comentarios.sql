-- DDL Comentarios
-- CREATE – Insertar comentario
DELIMITER //
CREATE PROCEDURE proInsertComentario(
    IN v_usu_id VARCHAR(20),
    IN v_contenido TEXT,
    IN v_positivo BOOLEAN
)
BEGIN
    INSERT INTO tbl_comentarios (com_usu_id, com_contenido, com_positivo)   
    VALUES (v_usu_id, v_contenido, v_positivo);
END//
DELIMITER ;

-- READ – Listar comentarios
DELIMITER //
CREATE PROCEDURE proGetComentarios()
BEGIN
    SELECT 
        c.com_id,
        u.usu_nombre_usuario,
        e.est_nombre,
        e.est_apellido,
        p.per_foto_ruta,
        c.com_contenido,
        c.com_fecha,
        c.com_respuesta,
    c.com_respuesta_fecha
    FROM tbl_comentarios c
    INNER JOIN tbl_usuarios u
        ON u.usu_id = c.com_usu_id
    INNER JOIN tbl_estudiantes e
        ON e.est_id = u.usu_id
    LEFT JOIN tbl_perfil p
        ON p.usu_id = u.usu_id
    WHERE c.com_activo = 1
    ORDER BY c.com_fecha DESC;
END//
DELIMITER ;

-- READ – Comentarios positivos 
DELIMITER //
CREATE PROCEDURE proGetComentariosPositivos()
BEGIN
    SELECT 
        u.usu_nombre_usuario,
        c.com_contenido,
        c.com_fecha
    FROM tbl_comentarios c
    INNER JOIN tbl_usuarios u ON u.usu_id = c.com_usu_id
    WHERE c.com_activo = TRUE
      AND c.com_positivo = TRUE
    ORDER BY c.com_fecha DESC
    LIMIT 5;
END//
DELIMITER ;

-- UPDATE – Desactivar comentario
DELIMITER //
CREATE PROCEDURE proDesactivarComentario(IN v_com_id INT)
BEGIN
    UPDATE tbl_comentarios
    SET com_activo = FALSE
    WHERE com_id = v_com_id;
END//
DELIMITER ;


-- UPDATE – Responder comentario
DELIMITER //
CREATE PROCEDURE proResponderComentario(
    IN v_com_id INT,
    IN v_respuesta TEXT
)
BEGIN
    UPDATE tbl_comentarios
    SET com_respuesta = v_respuesta,
        com_respuesta_fecha = CURRENT_TIMESTAMP
    WHERE com_id = v_com_id;
END//
DELIMITER ;


-- Activar comentario
DELIMITER //
CREATE PROCEDURE proActivarComentario(IN v_com_id INT)
BEGIN
    UPDATE tbl_comentarios
    SET com_activo = TRUE
    WHERE com_id = v_com_id;
END//
DELIMITER ;


DELIMITER //

CREATE PROCEDURE proGetComentariosPsicologo()
BEGIN
    SELECT
        c.com_id,
        c.com_activo,
        u.usu_nombre_usuario,
        e.est_nombre,
        e.est_apellido,
        p.per_foto_ruta,
        c.com_contenido,
        c.com_fecha,
        c.com_respuesta,
        c.com_respuesta_fecha
    FROM tbl_comentarios c
    INNER JOIN tbl_usuarios u
        ON u.usu_id = c.com_usu_id
    INNER JOIN tbl_estudiantes e
        ON e.est_id = u.usu_id
    LEFT JOIN tbl_perfil p
        ON p.usu_id = u.usu_id
    ORDER BY c.com_fecha DESC;
END//

DELIMITER ;

