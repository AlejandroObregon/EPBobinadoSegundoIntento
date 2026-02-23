
-- Editar una dirección existente
CREATE   PROCEDURE EditarDireccion
    @Id INT,
    @Provincia NVARCHAR(100),
    @Canton NVARCHAR(100),
    @Distrito NVARCHAR(100),
    @CodigoPostal NVARCHAR(20),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Direcciones
    SET Provincia = @Provincia,
        Canton = @Canton,
        Distrito = @Distrito,
        CodigoPostal = @CodigoPostal,
        Descripcion = @Descripcion
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END