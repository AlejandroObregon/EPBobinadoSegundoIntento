
-- Editar una orden de servicio existente
CREATE   PROCEDURE EditarOrdenServicio
    @Id INT,
    @MotorId INT,
    @Estado NVARCHAR(50),
    @TecnicoId INT,
    @UsuarioId INT,
    @Costo DECIMAL(10, 2),
    @Descripcion NVARCHAR(MAX),
    @FechaCita DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE OrdenesServicio
    SET MotorId = @MotorId,
        Estado = @Estado,
        TecnicoId = @TecnicoId,
        UsuarioId = @UsuarioId,
        Costo = @Costo,
        Descripcion = @Descripcion,
        FechaCita = @FechaCita
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END