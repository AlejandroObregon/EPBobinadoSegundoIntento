
-- Editar un cliente existente
CREATE   PROCEDURE EditarCliente
    @Id INT,
    @Nombre NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(255),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Clientes
    SET Nombre = @Nombre,
        Telefono = @Telefono,
        Email = @Email,
        Direccion = @Direccion,
        Activo = @Activo
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END