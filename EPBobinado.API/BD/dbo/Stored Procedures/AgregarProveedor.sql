
-- Agregar un nuevo proveedor
CREATE   PROCEDURE AgregarProveedor
    @Nombre NVARCHAR(100),
    @Contacto NVARCHAR(100),
    @CreadoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Proveedores (Nombre, Contacto, CreadoPor)
    VALUES (@Nombre, @Contacto, @CreadoPor);
    
    SELECT SCOPE_IDENTITY() AS Id;
END