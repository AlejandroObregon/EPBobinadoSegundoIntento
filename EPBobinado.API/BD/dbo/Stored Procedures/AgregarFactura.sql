
-- Agregar una nueva factura
CREATE   PROCEDURE AgregarFactura
    @OrdenId INT,
    @Total DECIMAL(10,2),
    @Impuesto DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Facturas (OrdenId, Total, Impuesto)
    VALUES (@OrdenId, @Total, @Impuesto);
    
    SELECT SCOPE_IDENTITY() AS Id;
END