
-- Agregar una nueva orden de servicio
CREATE   PROCEDURE AgregarOrdenServicio
    @MotorId INT,
    @Estado NVARCHAR(50),
    @TecnicoId INT,
    @UsuarioId INT,
    @Costo DECIMAL(10, 2),
    @Descripcion NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO OrdenesServicio (MotorId, Estado, TecnicoId, UsuarioId, Costo, Descripcion)
    VALUES (@MotorId, @Estado, @TecnicoId, @UsuarioId, @Costo, @Descripcion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END