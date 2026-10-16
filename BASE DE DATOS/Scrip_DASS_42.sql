CREATE TABLE tbl_tests_dass (
    test_id INT NOT NULL AUTO_INCREMENT,
    test_est_id VARCHAR(20) NOT NULL,
    test_fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    test_nivel_depresion VARCHAR(50) NULL,
    test_nivel_ansiedad VARCHAR(50) NULL,
    test_nivel_estres VARCHAR(50) NULL,

    PRIMARY KEY (test_id),

    CONSTRAINT fk_test_estudiante
        FOREIGN KEY (test_est_id)
        REFERENCES tbl_estudiantes (est_id)
);


CREATE TABLE tbl_respuestas_dass (
    respuesta_id INT NOT NULL AUTO_INCREMENT,
    respuesta_test_id INT NOT NULL,
    respuesta_pregunta INT NOT NULL,
    respuesta_valor TINYINT NOT NULL,

    PRIMARY KEY (respuesta_id),

    CONSTRAINT fk_respuesta_test
        FOREIGN KEY (respuesta_test_id)
        REFERENCES tbl_tests_dass (test_id),

    CONSTRAINT chk_respuesta_valor
        CHECK (respuesta_valor BETWEEN 0 AND 3),

    CONSTRAINT chk_respuesta_pregunta
        CHECK (respuesta_pregunta BETWEEN 1 AND 42)
);


DELIMITER $$
CREATE PROCEDURE proInsertTestDASS(
    IN p_est_id VARCHAR(20),
    IN p_nivel_depresion VARCHAR(50),
    IN p_nivel_ansiedad VARCHAR(50),
    IN p_nivel_estres VARCHAR(50))
BEGIN
    INSERT INTO tbl_tests_dass (
        test_est_id,
        test_nivel_depresion,
        test_nivel_ansiedad,
        test_nivel_estres)
    VALUES (
        p_est_id,
        p_nivel_depresion,
        p_nivel_ansiedad,
        p_nivel_estres);
    SELECT LAST_INSERT_ID() AS test_id;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE proInsertRespuestaDASS(
    IN p_test_id INT,
    IN p_numero_pregunta INT,
    IN p_valor_respuesta TINYINT)
BEGIN
    INSERT INTO tbl_respuestas_dass (
        respuesta_test_id,
        respuesta_pregunta,
        respuesta_valor)
    VALUES (
        p_test_id,
        p_numero_pregunta,
        p_valor_respuesta);
END$$
DELIMITER ;


DELIMITER $$

CREATE PROCEDURE proListTestsDASS()
BEGIN
    SELECT
        t.test_id,
        t.test_est_id,
        CONCAT(e.est_nombre, ' ', e.est_apellido) AS estudiante,
        CONCAT(e.est_grado, '-', e.est_curso) AS grado_curso,
        t.test_nivel_depresion,
        t.test_nivel_ansiedad,
        t.test_nivel_estres,
        t.test_fecha
    FROM tbl_tests_dass t
    INNER JOIN tbl_estudiantes e
        ON t.test_est_id = e.est_id
    ORDER BY t.test_fecha DESC;
END$$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE proGetRespuestasDASS(
    IN p_test_id INT
)
BEGIN
    SELECT
        respuesta_pregunta,
        respuesta_valor
    FROM tbl_respuestas_dass
    WHERE respuesta_test_id = p_test_id
    ORDER BY respuesta_pregunta ASC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE proGetResumenDashboardDASS()
BEGIN
    SELECT
        COUNT(DISTINCT test_est_id) AS total_estudiantes_evaluados,
        COUNT(test_id) AS total_tests_realizados
    FROM tbl_tests_dass;
END$$

DELIMITER ;


DELIMITER $$

CREATE PROCEDURE proGetEstadisticasDashboardDASS()
BEGIN

    SELECT
        'Depresión' AS dimension,
        test_nivel_depresion AS nivel,
        COUNT(*) AS cantidad
    FROM tbl_tests_dass
    WHERE test_nivel_depresion IS NOT NULL
    GROUP BY test_nivel_depresion

    UNION ALL

    SELECT
        'Ansiedad' AS dimension,
        test_nivel_ansiedad AS nivel,
        COUNT(*) AS cantidad
    FROM tbl_tests_dass
    WHERE test_nivel_ansiedad IS NOT NULL
    GROUP BY test_nivel_ansiedad

    UNION ALL

    SELECT
        'Estrés' AS dimension,
        test_nivel_estres AS nivel,
        COUNT(*) AS cantidad
    FROM tbl_tests_dass
    WHERE test_nivel_estres IS NOT NULL
    GROUP BY test_nivel_estres

    ORDER BY dimension, cantidad DESC;

END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE proListUltimosTestsDASS()
BEGIN
    SELECT
        t.test_id,
        t.test_est_id,
        CONCAT(e.est_nombre, ' ', e.est_apellido) AS estudiante,
        CONCAT(e.est_grado, '-', e.est_curso) AS grado_curso,
        t.test_nivel_depresion,
        t.test_nivel_ansiedad,
        t.test_nivel_estres,
        t.test_fecha
    FROM tbl_tests_dass t
    INNER JOIN tbl_estudiantes e
        ON t.test_est_id = e.est_id
    WHERE t.test_id = (
        SELECT t2.test_id
        FROM tbl_tests_dass t2
        WHERE t2.test_est_id = t.test_est_id
        ORDER BY t2.test_fecha DESC, t2.test_id DESC
        LIMIT 1
    )
    ORDER BY t.test_fecha DESC;
END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE proGetTestDASSById(
    IN p_test_id INT
)
BEGIN
    SELECT
        t.test_id,
        t.test_est_id,
        CONCAT(e.est_nombre, ' ', e.est_apellido) AS estudiante,
        CONCAT(e.est_grado, '-', e.est_curso) AS grado_curso,
        t.test_nivel_depresion,
        t.test_nivel_ansiedad,
        t.test_nivel_estres,
        t.test_fecha
    FROM tbl_tests_dass t
    INNER JOIN tbl_estudiantes e
        ON t.test_est_id = e.est_id
    WHERE t.test_id = p_test_id;
END$$

DELIMITER ;

