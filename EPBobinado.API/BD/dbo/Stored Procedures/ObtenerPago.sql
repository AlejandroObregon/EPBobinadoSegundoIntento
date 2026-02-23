
-- Obtener un pago por ID
CREATE   PROCEDURE ObtenerPago
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.FacturaId, p.Monto, p.MetodoPago, p.Fecha,
           f.Total AS FacturaTotal, f.Impuesto AS FacturaImpuesto,
           os.Id AS OrdenId, os.Estado AS OrdenEstado
    FROM Pagos p
    INNER JOIN Facturas f ON p.FacturaId = f.Id
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    WHERE p.Id = @Id;
END