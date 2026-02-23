
-- Editar un rol existente
CREATE   PROCEDURE EditarRol
    @Id INT,
    @Nombre NVARCHAR(50),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Roles
    SET Nombre = @Nombre,
        Descripcion = @Descripcion
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END