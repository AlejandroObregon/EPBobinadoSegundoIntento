
/* =========================================================
   DIRECCIONES
   ========================================================= */

-- Obtener todas las direcciones
CREATE   PROCEDURE ObtenerDirecciones
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Provincia, Canton, Distrito, CodigoPostal, Descripcion, CreadoEn
    FROM Direcciones
    ORDER BY Provincia, Canton, Distrito;
END