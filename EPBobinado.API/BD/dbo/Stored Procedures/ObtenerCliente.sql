
-- Obtener un cliente por ID
CREATE   PROCEDURE ObtenerCliente
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Telefono, Email, Direccion, Activo, CreadoEn
    FROM Clientes
    WHERE Id = @Id;
END