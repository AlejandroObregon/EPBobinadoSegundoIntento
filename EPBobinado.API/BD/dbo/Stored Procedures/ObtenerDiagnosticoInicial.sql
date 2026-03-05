
-- =============================================
-- Obtener un diagnóstico inicial por ID
-- =============================================
CREATE PROCEDURE ObtenerDiagnosticoInicial
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT di.Id, di.OrdenId, di.Descripcion, di.CreadoEn,
           os.Estado AS OrdenEstado,
           m.Id AS MotorId, m.NumeroSerie AS MotorNumeroSerie,
           u.Nombre AS UsuarioNombre
    FROM DiagnosticosIniciales di
    INNER JOIN OrdenesServicio os ON di.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    WHERE di.Id = @Id;
END
GO