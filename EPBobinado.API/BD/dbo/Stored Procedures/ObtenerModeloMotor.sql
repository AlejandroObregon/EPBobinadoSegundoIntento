
-- Obtener un modelo de motor por ID
CREATE   PROCEDURE ObtenerModeloMotor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Especificaciones
    FROM ModelosMotor
    WHERE Id = @Id;
END