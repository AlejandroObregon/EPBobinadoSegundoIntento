
-- Agregar una nueva orden de servicio
CREATE   PROCEDURE AgregarOrdenServicio
    @MotorId INT,
    @Estado NVARCHAR(50),
    @TecnicoId INT,
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO OrdenesServicio (MotorId, Estado, TecnicoId, UsuarioId)
    VALUES (@MotorId, @Estado, @TecnicoId, @UsuarioId);
    
    SELECT SCOPE_IDENTITY() AS Id;
END