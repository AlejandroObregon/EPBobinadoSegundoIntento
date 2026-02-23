
-- Agregar una nueva cotización
CREATE   PROCEDURE AgregarCotizacion
    @OrdenId INT,
    @Total DECIMAL(10,2),
    @Aprobada BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Cotizaciones (OrdenId, Total, Aprobada)
    VALUES (@OrdenId, @Total, @Aprobada);
    
    SELECT SCOPE_IDENTITY() AS Id;
END