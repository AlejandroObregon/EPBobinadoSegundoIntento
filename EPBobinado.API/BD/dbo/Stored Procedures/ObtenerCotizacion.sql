
-- =============================================
-- Obtener una cotización por ID
-- =============================================
CREATE PROCEDURE ObtenerCotizacion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.Id, c.OrdenId, c.Total, c.Aprobada, c.CreadoEn,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           u.Id AS UsuarioId, u.Nombre AS UsuarioNombre, u.Email AS UsuarioEmail
    FROM Cotizaciones c
    INNER JOIN OrdenesServicio os ON c.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    WHERE c.Id = @Id;
END
GO