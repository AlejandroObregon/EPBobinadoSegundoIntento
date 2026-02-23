
-- Agregar un nuevo registro a bitácora
CREATE   PROCEDURE AgregarBitacora
    @UsuarioId INT,
    @Accion NVARCHAR(100),
    @TablaAfectada NVARCHAR(100),
    @RegistroId INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Bitacora (UsuarioId, Accion, TablaAfectada, RegistroId)
    VALUES (@UsuarioId, @Accion, @TablaAfectada, @RegistroId);
    
    SELECT SCOPE_IDENTITY() AS Id;
END