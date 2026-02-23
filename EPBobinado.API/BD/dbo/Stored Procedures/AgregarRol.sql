
-- Agregar un nuevo rol
CREATE   PROCEDURE AgregarRol
    @Nombre NVARCHAR(50),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Roles (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END