
/* =========================================================
   DIAGNÓSTICOS INICIALES
   ========================================================= */

-- Obtener todos los diagnósticos iniciales
CREATE   PROCEDURE ObtenerDiagnosticosIniciales
AS
BEGIN
    SET NOCOUNT ON;
    SELECT di.Id, di.OrdenId, di.Descripcion, di.CreadoEn,
           os.Estado AS OrdenEstado,
           m.NumeroSerie AS MotorNumeroSerie
    FROM DiagnosticosIniciales di
    INNER JOIN OrdenesServicio os ON di.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    ORDER BY di.CreadoEn DESC;
END