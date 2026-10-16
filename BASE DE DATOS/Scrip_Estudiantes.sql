-- DLL Estudiantes
-- CREATE – Registrar estudiante
DELIMITER //

CREATE PROCEDURE proInsertEstudiante(IN v_id VARCHAR(20), IN v_nombre VARCHAR(50), IN v_apellido VARCHAR(50), IN v_grado VARCHAR(10), IN v_curso VARCHAR(5), IN v_fecha_nacimiento DATE)
BEGIN
    INSERT INTO tbl_estudiantes (est_id, est_nombre, est_apellido, est_grado, est_curso, est_fecha_nacimiento)
    VALUES (
        v_id,
        v_nombre,
        v_apellido,
        v_grado,
        v_curso,
        v_fecha_nacimiento
    );
END//

DELIMITER ;

-- READ – Obtener estudiante por ID
DELIMITER //

CREATE PROCEDURE proGetEstudianteById(IN v_id VARCHAR(20))
BEGIN
    SELECT
        e.est_id,
        e.est_nombre,
        e.est_apellido,
        e.est_grado,
        e.est_curso,
        e.est_fecha_nacimiento,
        e.est_estado,
        u.usu_nombre_usuario
    FROM tbl_estudiantes e
    INNER JOIN tbl_usuarios u
        ON e.est_id = u.usu_id
    WHERE e.est_id = v_id;
END//

DELIMITER ;

-- READ – Listar estudiantes
DELIMITER //

CREATE PROCEDURE proListEstudiantes()
BEGIN
    SELECT
        e.est_id,
        e.est_nombre,
        e.est_apellido,
        e.est_grado,
        e.est_curso,
        e.est_fecha_nacimiento,
        e.est_estado,
        u.usu_nombre_usuario
    FROM tbl_estudiantes e
    INNER JOIN tbl_usuarios u
        ON e.est_id = u.usu_id;
END//

DELIMITER ;

-- UPDATE – Actualizar estudiante
DELIMITER //

CREATE PROCEDURE proUpdateEstudiante(IN v_id VARCHAR(20), IN v_nombre VARCHAR(50), IN v_apellido VARCHAR(50), IN v_grado VARCHAR(10), IN v_curso VARCHAR(5), IN v_fecha_nacimiento DATE)
BEGIN
    UPDATE tbl_estudiantes
    SET
        est_nombre = v_nombre,
        est_apellido = v_apellido,
        est_grado = v_grado,
        est_curso = v_curso,
        est_fecha_nacimiento = v_fecha_nacimiento
    WHERE est_id = v_id;
END//

DELIMITER ;

-- DELETE – Eliminar estudiante
DELIMITER //
CREATE PROCEDURE proDeleteEstudiante(IN v_id VARCHAR(20))
BEGIN
    DELETE FROM tbl_estudiantes
    WHERE est_id = v_id;
END//
DELIMITER ;

