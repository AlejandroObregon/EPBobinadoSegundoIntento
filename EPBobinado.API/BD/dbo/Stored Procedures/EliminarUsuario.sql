
-- Eliminar un usuario
CREATE   PROCEDURE EliminarUsuario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Usuarios WHERE Id = @Id;
    SELECT @Id AS Id;
END