/* =========================================================
   ÓRDENES DE SERVICIO
   ========================================================= */
-- Obtener todas las órdenes de servicio
CREATE PROCEDURE ObtenerOrdenesServicio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT os.Id, os.MotorId, os.Estado, os.TecnicoId, os.CreadoEn,
           m.NumeroSerie AS MotorNumeroSerie,
           u.Nombre AS UsuarioNombre,
           mm.Nombre AS ModeloNombre,
           t.Nombre AS TecnicoNombre
    FROM OrdenesServicio os
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    LEFT JOIN Usuarios t ON os.TecnicoId = t.Id
    ORDER BY os.CreadoEn DESC;
END
GO