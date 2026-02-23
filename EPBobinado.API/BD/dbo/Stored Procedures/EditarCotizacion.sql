
-- Editar una cotización existente
CREATE   PROCEDURE EditarCotizacion
    @Id INT,
    @OrdenId INT,
    @Total DECIMAL(10,2),
    @Aprobada BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Cotizaciones
    SET OrdenId = @OrdenId,
        Total = @Total,
        Aprobada = @Aprobada
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END