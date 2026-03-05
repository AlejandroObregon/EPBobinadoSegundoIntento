-- =============================================
-- Obtener un diagnóstico técnico por ID
-- =============================================
CREATE PROCEDURE ObtenerDiagnosticoTecnico
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dt.Id, dt.OrdenId, dt.Detalle, dt.CreadoEn,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           u.Nombre AS UsuarioNombre
    FROM DiagnosticosTecnicos dt
    INNER JOIN OrdenesServicio os ON dt.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    WHERE dt.Id = @Id;
END
GO