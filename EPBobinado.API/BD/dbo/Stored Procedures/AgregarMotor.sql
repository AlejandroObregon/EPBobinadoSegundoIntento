
-- Agregar un nuevo motor
CREATE   PROCEDURE AgregarMotor
    @ClienteId INT,
    @ModeloId INT,
    @NumeroSerie NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Motores (ClienteId, ModeloId, NumeroSerie)
    VALUES (@ClienteId, @ModeloId, @NumeroSerie);
    
    SELECT SCOPE_IDENTITY() AS Id;
END