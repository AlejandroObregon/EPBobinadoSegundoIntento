
-- Editar un motor existente
CREATE   PROCEDURE EditarMotor
    @Id INT,
    @ClienteId INT,
    @ModeloId INT,
    @NumeroSerie NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Motores
    SET ClienteId = @ClienteId,
        ModeloId = @ModeloId,
        NumeroSerie = @NumeroSerie
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END