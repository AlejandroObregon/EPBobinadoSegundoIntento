
-- Obtener una dirección por ID
CREATE   PROCEDURE ObtenerDireccion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Provincia, Canton, Distrito, CodigoPostal, Descripcion, CreadoEn
    FROM Direcciones
    WHERE Id = @Id;
END