CREATE   PROCEDURE ValidarTokenReset
    @Token NVARCHAR(100)
AS BEGIN
    SET NOCOUNT ON;
    SELECT t.Id, t.UsuarioId, t.Expiracion, t.Usado, u.Email
    FROM PasswordResetTokens t
    INNER JOIN Usuarios u ON u.Id = t.UsuarioId
    WHERE t.Token = @Token AND t.Usado = 0 AND t.Expiracion > GETDATE();
END;