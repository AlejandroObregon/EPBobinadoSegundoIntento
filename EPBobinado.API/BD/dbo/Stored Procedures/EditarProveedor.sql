
-- Editar un proveedor existente
CREATE   PROCEDURE EditarProveedor
    @Id INT,
    @Nombre NVARCHAR(100),
    @Contacto NVARCHAR(100),
    @CreadoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Proveedores
    SET Nombre = @Nombre,
        Contacto = @Contacto,
        CreadoPor = @CreadoPor
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END