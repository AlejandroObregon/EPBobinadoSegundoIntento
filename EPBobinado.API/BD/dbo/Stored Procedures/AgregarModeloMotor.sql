
-- Agregar un nuevo modelo de motor
CREATE   PROCEDURE AgregarModeloMotor
    @Nombre NVARCHAR(100),
    @Especificaciones NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ModelosMotor (Nombre, Especificaciones)
    VALUES (@Nombre, @Especificaciones);
    
    SELECT SCOPE_IDENTITY() AS Id;
END