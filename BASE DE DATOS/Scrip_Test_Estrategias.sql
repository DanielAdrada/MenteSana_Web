CREATE TABLE tbl_test_estrategias (
    test_estrategia_id INT AUTO_INCREMENT PRIMARY KEY,
    test_id INT NOT NULL,
    estrategia_id INT NOT NULL,

    fecha_asignacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_test_estrategia_test
        FOREIGN KEY (test_id)
        REFERENCES tbl_tests_dass(test_id),

    CONSTRAINT fk_test_estrategia_estrategia
        FOREIGN KEY (estrategia_id)
        REFERENCES tbl_estrategias(estrategia_id)
);



DELIMITER $$
CREATE PROCEDURE proInsertTestEstrategia(
    IN p_test_id INT,
    IN p_estrategia_id INT)
BEGIN
    INSERT INTO tbl_test_estrategias (
        test_id,
        estrategia_id)
    VALUES (
        p_test_id,
        p_estrategia_id);
END$$
DELIMITER ;



