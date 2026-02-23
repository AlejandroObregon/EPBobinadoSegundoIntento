
-- Eliminar un motor
CREATE   PROCEDURE EliminarMotor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Motores WHERE Id = @Id;
    SELECT @Id AS Id;
END