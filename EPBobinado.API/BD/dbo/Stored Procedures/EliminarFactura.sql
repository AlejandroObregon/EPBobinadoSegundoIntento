
-- Eliminar una factura
CREATE   PROCEDURE EliminarFactura
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Facturas WHERE Id = @Id;
    SELECT @Id AS Id;
END