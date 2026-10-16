CREATE TABLE tbl_psicologos (
    psi_id VARCHAR(20) NOT NULL,
    psi_nombre VARCHAR(50) NOT NULL,
    psi_apellido VARCHAR(50) NOT NULL,
    psi_correo VARCHAR(100) NOT NULL,
    psi_formacion VARCHAR(150) NOT NULL,
    psi_horario VARCHAR(100) NOT NULL,
    psi_estado ENUM('ACTIVO','INACTIVO') NOT NULL DEFAULT 'ACTIVO',

    PRIMARY KEY (psi_id),

    CONSTRAINT fk_psi_usuario
        FOREIGN KEY (psi_id)
        REFERENCES tbl_usuarios(usu_id)
        ON DELETE CASCADE
);

-- Registrar psicólogo
DELIMITER //

CREATE PROCEDURE proInsertPsicologo(IN v_id VARCHAR(20), IN v_nombre VARCHAR(50), IN v_apellido VARCHAR(50), IN v_correo VARCHAR(100), IN v_telefono VARCHAR(20),
    IN v_formacion VARCHAR(150), IN v_horario VARCHAR(100))
BEGIN
    INSERT INTO tbl_psicologos
    (psi_id,
    psi_nombre,
    psi_apellido, 
    psi_correo,
    psi_telefono,
    psi_formacion,
    psi_horario)
    VALUES
    (v_id, v_nombre, v_apellido, v_correo, v_telefono, v_formacion, v_horario);
END//

DELIMITER ;

-- Obtener psicólogo por ID
DELIMITER //

CREATE PROCEDURE proGetPsicologoById(
    IN v_id VARCHAR(20)
)
BEGIN
    SELECT
        psi_id,
        psi_nombre,
        psi_apellido,
        psi_correo,
        psi_telefono,
        psi_formacion,
        psi_horario,
        psi_estado
    FROM tbl_psicologos
    WHERE psi_id = v_id;
END//

DELIMITER ;

-- Listar psicólogos
DELIMITER //

CREATE PROCEDURE proListPsicologos()
BEGIN
    SELECT
        psi_id,
        psi_nombre,
        psi_apellido,
        psi_correo,
        psi_telefono,
        psi_formacion,
        psi_horario,
        psi_estado
    FROM tbl_psicologos;
END//

DELIMITER ;

-- Actualizar psicólogo
DELIMITER //

CREATE PROCEDURE proUpdatePsicologo(
    IN v_id VARCHAR(20),
    IN v_nombre VARCHAR(50),
    IN v_apellido VARCHAR(50),
    IN v_correo VARCHAR(100),
    IN v_telefono VARCHAR(20),
    IN v_formacion VARCHAR(150),
    IN v_horario VARCHAR(100)
)
BEGIN
    UPDATE tbl_psicologos
    SET
        psi_nombre = v_nombre,
        psi_apellido = v_apellido,
        psi_correo = v_correo,
        psi_telefono = v_telefono,
        psi_formacion = v_formacion,
        psi_horario = v_horario
    WHERE psi_id = v_id;
END//

DELIMITER ;

-- Activar / Desactivar psicólogo
DELIMITER //

CREATE PROCEDURE proUpdateEstadoPsicologo(
    IN v_id VARCHAR(20),
    IN v_estado VARCHAR(10)
)
BEGIN
    UPDATE tbl_psicologos
    SET psi_estado = v_estado
    WHERE psi_id = v_id;
END//

DELIMITER ;


