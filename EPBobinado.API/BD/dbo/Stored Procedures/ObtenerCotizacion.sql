
-- Obtener una cotización por ID
CREATE   PROCEDURE ObtenerCotizacion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.Id, c.OrdenId, c.Total, c.Aprobada, c.CreadoEn,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           cl.Id AS ClienteId, cl.Nombre AS ClienteNombre, cl.Email AS ClienteEmail
    FROM Cotizaciones c
    INNER JOIN OrdenesServicio os ON c.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Clientes cl ON m.ClienteId = cl.Id
    WHERE c.Id = @Id;
END