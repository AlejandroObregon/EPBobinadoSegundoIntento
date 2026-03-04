CREATE PROCEDURE AgregarMotor
    @UsuarioId   INT,
    @ModeloId    INT,
    @NumeroSerie NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Motores (UsuarioId, ModeloId, NumeroSerie, CreadoEn)
    VALUES (@UsuarioId, @ModeloId, @NumeroSerie, GETDATE());

    SELECT SCOPE_IDENTITY();   -- retorna el Id generado
END
GO