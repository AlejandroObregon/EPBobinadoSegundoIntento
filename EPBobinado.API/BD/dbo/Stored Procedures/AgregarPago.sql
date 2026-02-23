
-- Agregar un nuevo pago
CREATE   PROCEDURE AgregarPago
    @FacturaId INT,
    @Monto DECIMAL(10,2),
    @MetodoPago NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Pagos (FacturaId, Monto, MetodoPago)
    VALUES (@FacturaId, @Monto, @MetodoPago);
    
    SELECT SCOPE_IDENTITY() AS Id;
END