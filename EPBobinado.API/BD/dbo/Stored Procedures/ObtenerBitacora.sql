
/* =========================================================
   BITÁCORA
   ========================================================= */

-- Obtener todos los registros de bitácora
CREATE   PROCEDURE ObtenerBitacora
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.Id, b.UsuarioId, b.Accion, b.TablaAfectada, b.RegistroId, b.Fecha,
           u.Nombre AS UsuarioNombre
    FROM Bitacora b
    LEFT JOIN Usuarios u ON b.UsuarioId = u.Id
    ORDER BY b.Fecha DESC;
END