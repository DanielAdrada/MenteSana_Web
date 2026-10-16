CREATE TABLE tbl_recuperacion_contrasena (
    rec_id INT AUTO_INCREMENT PRIMARY KEY,
    rec_usu_id VARCHAR(20) NOT NULL,
    rec_token VARCHAR(255) NOT NULL,
    rec_fecha_expiracion DATETIME NOT NULL,
    rec_usado TINYINT(1) NOT NULL DEFAULT 0,

    CONSTRAINT fk_recuperacion_usuario
        FOREIGN KEY (rec_usu_id)
        REFERENCES tbl_usuarios(usu_id));

-- Guardar una solicitud de recuperación
DELIMITER //

CREATE PROCEDURE proInsertRecuperacion(IN v_usu_id VARCHAR(20) ,IN v_token VARCHAR(255), IN v_fecha_expiracion DATETIME)
BEGIN
    INSERT INTO tbl_recuperacion_contrasena
    (
        rec_usu_id,
        rec_token,
        rec_fecha_expiracion,
        rec_usado
    )
    VALUES
    (
        v_usu_id,
        v_token,
        v_fecha_expiracion,
        0
    );
END//

DELIMITER ;

-- Consultar token válido
DELIMITER //

CREATE PROCEDURE proGetRecuperacionValida(IN v_token VARCHAR(255))
BEGIN
    SELECT
        rec_id,
        rec_usu_id,
        rec_token,
        rec_fecha_expiracion,
        rec_usado
    FROM tbl_recuperacion_contrasena
    WHERE rec_token = v_token
      AND rec_usado = 0
      AND rec_fecha_expiracion > NOW()
    LIMIT 1;
END//

DELIMITER ;


-- Marcar el token como utilizado
DELIMITER //

CREATE PROCEDURE proMarcarRecuperacionUsada(
    IN v_rec_id INT
)
BEGIN
    UPDATE tbl_recuperacion_contrasena
    SET rec_usado = 1
    WHERE rec_id = v_rec_id;
END//

DELIMITER ;

-- Obtener usuario por correo
DELIMITER //

CREATE PROCEDURE proGetUsuarioByCorreo(
    IN v_correo VARCHAR(100)
)
BEGIN
    SELECT
        usu_id,
        usu_nombre_usuario,
        usu_correo,
        usu_rol
    FROM tbl_usuarios
    WHERE usu_correo = v_correo
    LIMIT 1;
END//

DELIMITER ;

