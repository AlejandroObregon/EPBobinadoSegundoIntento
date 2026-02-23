
/* =========================================================
   DIAGNÓSTICOS TÉCNICOS
   ========================================================= */

-- Obtener todos los diagnósticos técnicos
CREATE   PROCEDURE ObtenerDiagnosticosTecnicos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dt.Id, dt.OrdenId, dt.Detalle, dt.CreadoEn,
           os.Estado AS OrdenEstado,
           m.NumeroSerie AS MotorNumeroSerie
    FROM DiagnosticosTecnicos dt
    INNER JOIN OrdenesServicio os ON dt.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    ORDER BY dt.CreadoEn DESC;
END