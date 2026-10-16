CREATE TABLE tbl_estrategias (

    estrategia_id INT AUTO_INCREMENT PRIMARY KEY,
    estrategia_dimension VARCHAR(30) NOT NULL,
    estrategia_area VARCHAR(50) NOT NULL,
    estrategia_nivel VARCHAR(30) NOT NULL,
    estrategia_titulo VARCHAR(150) NOT NULL,
    estrategia_descripcion TEXT NOT NULL,
    estrategia_activa TINYINT(1) NOT NULL DEFAULT 1,
    estrategia_usu_id VARCHAR(20) NOT NULL,
    estrategia_fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estrategia_fecha_actualizacion TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT fk_estrategia_usuario
        FOREIGN KEY (estrategia_usu_id)
        REFERENCES tbl_usuarios(usu_id)
);


DELIMITER $$
CREATE PROCEDURE proInsertEstrategia(
    IN p_dimension VARCHAR(30),
    IN p_area VARCHAR(50),
    IN p_nivel VARCHAR(30),
    IN p_titulo VARCHAR(150),
    IN p_descripcion TEXT,
    IN p_usu_id VARCHAR(20))
BEGIN
    INSERT INTO tbl_estrategias (
        estrategia_dimension,
        estrategia_area,
        estrategia_nivel,
        estrategia_titulo,
        estrategia_descripcion,
        estrategia_activa,
        estrategia_usu_id)
    VALUES (
        p_dimension,
        p_area,
        p_nivel,
        p_titulo,
        p_descripcion,
        1,
        p_usu_id);

    SELECT LAST_INSERT_ID();
END$$
DELIMITER ;



DELIMITER $$
CREATE PROCEDURE proGetEstrategias()
BEGIN
    SELECT
        estrategia_id,
        estrategia_dimension,
        estrategia_area,
        estrategia_nivel,
        estrategia_titulo,
        estrategia_descripcion,
        estrategia_activa,
        estrategia_usu_id,
        estrategia_fecha_creacion,
        estrategia_fecha_actualizacion
    FROM tbl_estrategias
    ORDER BY estrategia_fecha_creacion DESC;
END$$
DELIMITER ;




DELIMITER $$
CREATE PROCEDURE proUpdateEstrategia(
    IN p_estrategia_id INT,
    IN p_dimension VARCHAR(30),
    IN p_area VARCHAR(50),
    IN p_nivel VARCHAR(30),
    IN p_titulo VARCHAR(150),
    IN p_descripcion TEXT)
BEGIN
    UPDATE tbl_estrategias
    SET
        estrategia_dimension = p_dimension,
        estrategia_area = p_area,
        estrategia_nivel = p_nivel,
        estrategia_titulo = p_titulo,
        estrategia_descripcion = p_descripcion
    WHERE estrategia_id = p_estrategia_id;
END$$
DELIMITER ;



DELIMITER $$
CREATE PROCEDURE proEstadoEstrategia(
    IN p_estrategia_id INT,
    IN p_activa TINYINT(1))
BEGIN
    UPDATE tbl_estrategias
    SET estrategia_activa = p_activa
    WHERE estrategia_id = p_estrategia_id;
END$$
DELIMITER ;



DELIMITER $$
CREATE PROCEDURE proGetEstrategiasRecomendadas(
    IN p_dimension VARCHAR(30),
    IN p_nivel VARCHAR(30))
BEGIN
    SELECT
        estrategia_id,
        estrategia_dimension,
        estrategia_nivel,
        estrategia_titulo,
        estrategia_descripcion
    FROM tbl_estrategias
    WHERE UPPER(TRIM(estrategia_dimension)) = UPPER(TRIM(p_dimension))
      AND UPPER(TRIM(estrategia_nivel)) = UPPER(TRIM(p_nivel))
      AND estrategia_activa = 1
    ORDER BY RAND();
END$$
DELIMITER ;










