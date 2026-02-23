
-- Eliminar un pago
CREATE   PROCEDURE EliminarPago
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Pagos WHERE Id = @Id;
    SELECT @Id AS Id;
END