
-- Eliminar una cotización
CREATE   PROCEDURE EliminarCotizacion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Cotizaciones WHERE Id = @Id;
    SELECT @Id AS Id;
END