
/* =========================================================
   PROVEEDORES
   ========================================================= */

-- Obtener todos los proveedores
CREATE   PROCEDURE ObtenerProveedores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nombre, p.Contacto, p.CreadoPor, p.FechaCreacion,
           u.Nombre AS UsuarioCreador
    FROM Proveedores p
    LEFT JOIN Usuarios u ON p.CreadoPor = u.Id
    ORDER BY p.Nombre;
END