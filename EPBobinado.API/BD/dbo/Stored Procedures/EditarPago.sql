
-- Editar un pago existente
CREATE   PROCEDURE EditarPago
    @Id INT,
    @FacturaId INT,
    @Monto DECIMAL(10,2),
    @MetodoPago NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Pagos
    SET FacturaId = @FacturaId,
        Monto = @Monto,
        MetodoPago = @MetodoPago
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END