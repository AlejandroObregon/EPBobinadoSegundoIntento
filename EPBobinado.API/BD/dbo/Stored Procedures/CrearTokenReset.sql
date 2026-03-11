CREATE   PROCEDURE CrearTokenReset
    @UsuarioId INT,
    @Token     NVARCHAR(100),
    @Expiracion DATETIME
AS BEGIN
    SET NOCOUNT ON;
    -- Invalidar tokens anteriores del mismo usuario
    UPDATE PasswordResetTokens SET Usado = 1
    WHERE UsuarioId = @UsuarioId AND Usado = 0;
    -- Insertar nuevo
    INSERT INTO PasswordResetTokens (UsuarioId, Token, Expiracion)
    VALUES (@UsuarioId, @Token, @Expiracion);
END;