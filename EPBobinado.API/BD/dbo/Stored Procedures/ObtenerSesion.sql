
-- Obtener una sesión por ID
CREATE   PROCEDURE ObtenerSesion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.Id, s.UsuarioId, s.Token, s.Inicio, s.UltimaActividad, s.Activa,
           u.Nombre AS UsuarioNombre, u.Email AS UsuarioEmail
    FROM Sesiones s
    INNER JOIN Usuarios u ON s.UsuarioId = u.Id
    WHERE s.Id = @Id;
END