-- =============================================
-- Obtener una factura por ID
-- =============================================
CREATE PROCEDURE ObtenerFactura
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, f.OrdenId, f.Total, f.Impuesto, f.Fecha,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           u.Id AS UsuarioId, u.Nombre AS UsuarioNombre, u.Telefono AS UsuarioTelefono
    FROM Facturas f
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    WHERE f.Id = @Id;
END
GO