
-- Agregar un nuevo cliente
CREATE   PROCEDURE AgregarCliente
    @Nombre NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(255),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Clientes (Nombre, Telefono, Email, Direccion, Activo)
    VALUES (@Nombre, @Telefono, @Email, @Direccion, @Activo);
    
    SELECT SCOPE_IDENTITY() AS Id;
END