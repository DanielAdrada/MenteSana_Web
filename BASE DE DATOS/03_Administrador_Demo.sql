
USE mente_sana;

-- Administrador de demostración para Mente Sana.
-- Credenciales de acceso:
-- Usuario: admin
-- Contraseña: Admin123
--
-- Este script evita duplicar el administrador
-- si ya existe en la base de datos.

INSERT INTO tbl_usuarios (
    usu_id,
    usu_nombre_usuario,
    usu_correo,
    usu_contrasena,
    usu_salt,
    usu_rol
)
SELECT
    '900000001',
    'admin',
    NULL,
    'cf3f52a449e2f91d107d4f9831ddeb0edff17900ee22adeea02cf9e03f53c27a',
    'AbC123Xy',
    'ADMINISTRADOR'
WHERE NOT EXISTS (
    SELECT 1
    FROM tbl_usuarios
    WHERE usu_id = '900000001'
       OR usu_nombre_usuario = 'admin'
);
