
-- Editar un modelo de motor existente
CREATE   PROCEDURE EditarModeloMotor
    @Id INT,
    @Nombre NVARCHAR(100),
    @Especificaciones NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ModelosMotor
    SET Nombre = @Nombre,
        Especificaciones = @Especificaciones
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END