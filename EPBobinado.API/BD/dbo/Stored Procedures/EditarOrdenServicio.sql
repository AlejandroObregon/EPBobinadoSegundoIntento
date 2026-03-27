
-- Editar una orden de servicio existente
CREATE   PROCEDURE EditarOrdenServicio
    @Id INT,
    @MotorId INT,
    @Estado NVARCHAR(50),
    @TecnicoId INT,
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE OrdenesServicio
    SET MotorId = @MotorId,
        Estado = @Estado,
        TecnicoId = @TecnicoId,
        UsuarioId = @UsuarioId
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END