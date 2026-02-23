
-- Editar una factura existente
CREATE   PROCEDURE EditarFactura
    @Id INT,
    @OrdenId INT,
    @Total DECIMAL(10,2),
    @Impuesto DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Facturas
    SET OrdenId = @OrdenId,
        Total = @Total,
        Impuesto = @Impuesto
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END