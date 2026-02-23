
-- Obtener un rol por ID
CREATE   PROCEDURE ObtenerRol
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion
    FROM Roles
    WHERE Id = @Id;
END