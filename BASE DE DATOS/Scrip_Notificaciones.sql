CREATE TABLE tbl_notificaciones (
    notificacion_id INT AUTO_INCREMENT PRIMARY KEY,

    notificacion_test_id INT NOT NULL,

    notificacion_est_id VARCHAR(20) NOT NULL,

    notificacion_tipo VARCHAR(50) NOT NULL,

    notificacion_mensaje VARCHAR(255) NOT NULL,

    notificacion_fecha DATETIME DEFAULT CURRENT_TIMESTAMP,

    notificacion_leida TINYINT(1) DEFAULT 0,

    notificacion_activa TINYINT(1) DEFAULT 1,

    -- Estado de gestión de la notificación:
    -- Pendiente = requiere revisión del psicólogo
    -- Atendida = ya fue revisada
    notificacion_estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente',

    CONSTRAINT fk_notificacion_test
        FOREIGN KEY (notificacion_test_id)
        REFERENCES tbl_tests_dass(test_id),

    CONSTRAINT uq_notificacion_test
        UNIQUE (notificacion_test_id)
);




DELIMITER $$
CREATE PROCEDURE proInsertNotificacionDASS(
    IN p_test_id INT,
    IN p_est_id VARCHAR(20),
    IN p_nivel_depresion VARCHAR(50),
    IN p_nivel_ansiedad VARCHAR(50),
    IN p_nivel_estres VARCHAR(50))
    
BEGIN
    DECLARE v_mensaje VARCHAR(255);
    SET v_mensaje = 'Nueva evaluación DASS-42 requiere revisión profesional.';
    IF (
        p_nivel_depresion IN ('Severo', 'Extremadamente_Severo')
        OR
        p_nivel_ansiedad IN ('Severo', 'Extremadamente_Severo')
        OR
        p_nivel_estres IN ('Severo', 'Extremadamente_Severo')) 
        THEN
        INSERT IGNORE INTO tbl_notificaciones (
            notificacion_test_id,
            notificacion_est_id,
            notificacion_tipo,
            notificacion_mensaje)
            
        VALUES (
            p_test_id,
            p_est_id,
            'ALERTA_DASS',
            v_mensaje);
    END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE proMarcarNotificacionAtendida(
    IN p_notificacion_id INT)
BEGIN

    UPDATE tbl_notificaciones
    SET notificacion_estado = 'Atendida'
    WHERE notificacion_id = p_notificacion_id
      AND notificacion_estado = 'Pendiente';

END$$
DELIMITER ;


























