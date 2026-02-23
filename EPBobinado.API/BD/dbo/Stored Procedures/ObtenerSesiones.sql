
/* =========================================================
   SESIONES
   ========================================================= */

-- Obtener todas las sesiones
CREATE   PROCEDURE ObtenerSesiones
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.Id, s.UsuarioId, s.Token, s.Inicio, s.UltimaActividad, s.Activa,
           u.Nombre AS UsuarioNombre, u.Email AS UsuarioEmail
    FROM Sesiones s
    INNER JOIN Usuarios u ON s.UsuarioId = u.Id
    ORDER BY s.Inicio DESC;
END