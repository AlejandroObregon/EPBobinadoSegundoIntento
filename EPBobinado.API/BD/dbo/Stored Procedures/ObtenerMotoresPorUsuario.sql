CREATE PROCEDURE ObtenerMotoresPorUsuario
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        m.Id,
        m.UsuarioId,
        m.ModeloId,
        m.NumeroSerie,
        m.CreadoEn,
        u.Id          AS UsuId,
        u.Nombre      AS UsuNombre,
        u.Email       AS UsuEmail,
        u.Cedula      AS UsuCedula,
        u.Telefono    AS UsuTelefono,
        u.RolId       AS UsuRolId,
        u.Activo      AS UsuActivo,
        mm.Id         AS ModId,
        mm.Nombre     AS ModNombre,
        mm.Especificaciones AS ModEspecificaciones
    FROM Motores m
    INNER JOIN Usuarios     u  ON m.UsuarioId = u.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId  = mm.Id
    WHERE m.UsuarioId = @UsuarioId
    ORDER BY m.CreadoEn DESC;
END
GO