
-- Eliminar un modelo de motor
CREATE   PROCEDURE EliminarModeloMotor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ModelosMotor WHERE Id = @Id;
    SELECT @Id AS Id;
END