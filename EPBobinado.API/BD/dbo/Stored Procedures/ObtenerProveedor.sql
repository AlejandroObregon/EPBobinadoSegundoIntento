
-- Obtener un proveedor por ID
CREATE   PROCEDURE ObtenerProveedor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.Nombre, p.Contacto, p.CreadoPor, p.FechaCreacion,
           u.Nombre AS UsuarioCreador, u.Email AS UsuarioEmail
    FROM Proveedores p
    LEFT JOIN Usuarios u ON p.CreadoPor = u.Id
    WHERE p.Id = @Id;
END