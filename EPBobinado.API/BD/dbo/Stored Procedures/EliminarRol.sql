
-- Eliminar un rol
CREATE   PROCEDURE EliminarRol
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Roles WHERE Id = @Id;
    SELECT @Id AS Id;
END