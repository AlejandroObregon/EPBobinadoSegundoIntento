
/* =========================================================
   CLIENTES
   ========================================================= */

-- Obtener todos los clientes
CREATE   PROCEDURE ObtenerClientes
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Telefono, Email, Direccion, Activo, CreadoEn
    FROM Clientes
    ORDER BY Nombre;
END