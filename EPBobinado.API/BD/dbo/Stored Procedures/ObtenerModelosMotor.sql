
/* =========================================================
   MODELOS DE MOTOR
   ========================================================= */

-- Obtener todos los modelos de motor
CREATE   PROCEDURE ObtenerModelosMotor
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Especificaciones
    FROM ModelosMotor
    ORDER BY Nombre;
END