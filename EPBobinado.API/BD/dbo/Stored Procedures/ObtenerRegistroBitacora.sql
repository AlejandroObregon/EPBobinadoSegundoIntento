
-- Obtener un registro de bitácora por ID
CREATE   PROCEDURE ObtenerRegistroBitacora
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.Id, b.UsuarioId, b.Accion, b.TablaAfectada, b.RegistroId, b.Fecha,
           u.Nombre AS UsuarioNombre, u.Email AS UsuarioEmail
    FROM Bitacora b
    LEFT JOIN Usuarios u ON b.UsuarioId = u.Id
    WHERE b.Id = @Id;
END