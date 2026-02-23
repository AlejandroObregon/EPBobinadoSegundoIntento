
/* =========================================================
   ÓRDENES DE SERVICIO
   ========================================================= */

-- Obtener todas las órdenes de servicio
CREATE   PROCEDURE ObtenerOrdenesServicio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT os.Id, os.MotorId, os.Estado, os.TecnicoId, os.CreadoEn,
           m.NumeroSerie AS MotorNumeroSerie,
           c.Nombre AS ClienteNombre,
           mm.Nombre AS ModeloNombre,
           u.Nombre AS TecnicoNombre
    FROM OrdenesServicio os
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Clientes c ON m.ClienteId = c.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    LEFT JOIN Usuarios u ON os.TecnicoId = u.Id
    ORDER BY os.CreadoEn DESC;
END