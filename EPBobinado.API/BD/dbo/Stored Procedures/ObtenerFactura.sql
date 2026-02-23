
-- Obtener una factura por ID
CREATE   PROCEDURE ObtenerFactura
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, f.OrdenId, f.Total, f.Impuesto, f.Fecha,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           cl.Id AS ClienteId, cl.Nombre AS ClienteNombre, cl.Telefono AS ClienteTelefono
    FROM Facturas f
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Clientes cl ON m.ClienteId = cl.Id
    WHERE f.Id = @Id;
END