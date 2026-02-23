
-- Agregar una nueva dirección
CREATE   PROCEDURE AgregarDireccion
    @Provincia NVARCHAR(100),
    @Canton NVARCHAR(100),
    @Distrito NVARCHAR(100),
    @CodigoPostal NVARCHAR(20),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Direcciones (Provincia, Canton, Distrito, CodigoPostal, Descripcion)
    VALUES (@Provincia, @Canton, @Distrito, @CodigoPostal, @Descripcion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END