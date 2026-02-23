
/* =========================================================
   MOTORES
   ========================================================= */

-- Obtener todos los motores
CREATE   PROCEDURE ObtenerMotores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.Id, m.ClienteId, m.ModeloId, m.NumeroSerie, m.CreadoEn,
           c.Nombre AS ClienteNombre,
           mm.Nombre AS ModeloNombre
    FROM Motores m
    INNER JOIN Clientes c ON m.ClienteId = c.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    ORDER BY m.CreadoEn DESC;
END