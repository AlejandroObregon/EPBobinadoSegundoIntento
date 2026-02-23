
/* =========================================================
   ROLES
   ========================================================= */

-- Obtener todos los roles
CREATE   PROCEDURE ObtenerRoles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion
    FROM Roles
    ORDER BY Nombre;
END