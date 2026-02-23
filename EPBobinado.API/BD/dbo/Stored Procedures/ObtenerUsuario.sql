
-- Obtener un usuario por ID
CREATE   PROCEDURE ObtenerUsuario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.Id, u.Cedula, u.Nombre, u.Email, u.PasswordHash, u.RolId, u.DireccionId, 
           u.Telefono, u.Activo, u.CreadoEn,
           r.Nombre AS RolNombre,
           d.Provincia, d.Canton, d.Distrito, d.CodigoPostal, d.Descripcion AS DireccionDescripcion
    FROM Usuarios u
    LEFT JOIN Roles r ON u.RolId = r.Id
    LEFT JOIN Direcciones d ON u.DireccionId = d.Id
    WHERE u.Id = @Id;
END