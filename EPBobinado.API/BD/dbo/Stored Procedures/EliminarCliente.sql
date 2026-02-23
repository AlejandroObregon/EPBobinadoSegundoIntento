
-- Eliminar un cliente
CREATE   PROCEDURE EliminarCliente
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Clientes WHERE Id = @Id;
    SELECT @Id AS Id;
END