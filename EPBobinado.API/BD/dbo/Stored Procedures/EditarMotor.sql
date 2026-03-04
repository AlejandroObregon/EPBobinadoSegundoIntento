
-- Editar un motor existente
CREATE PROCEDURE EditarMotor
    @Id          INT,
    @UsuarioId   INT,
    @ModeloId    INT,
    @NumeroSerie NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Motores
    SET
        UsuarioId   = @UsuarioId,
        ModeloId    = @ModeloId,
        NumeroSerie = @NumeroSerie
    WHERE Id = @Id;

    SELECT @Id AS Id;
END
GO