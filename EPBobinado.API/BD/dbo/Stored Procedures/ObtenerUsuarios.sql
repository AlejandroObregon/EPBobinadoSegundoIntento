
/* =========================================================
   USUARIOS
   ========================================================= */

-- Obtener todos los usuarios
CREATE   PROCEDURE ObtenerUsuarios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.Id, u.Cedula, u.Nombre, u.Email, u.PasswordHash, u.RolId, u.DireccionId, 
           u.Telefono, u.Activo, u.CreadoEn,
           r.Nombre AS RolNombre,
           d.Provincia, d.Canton, d.Distrito
    FROM Usuarios u
    LEFT JOIN Roles r ON u.RolId = r.Id
    LEFT JOIN Direcciones d ON u.DireccionId = d.Id
    ORDER BY u.Nombre;
END