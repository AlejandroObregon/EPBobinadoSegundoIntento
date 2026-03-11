CREATE   PROCEDURE UsarTokenReset
    @Token NVARCHAR(100)
AS BEGIN
    SET NOCOUNT ON;
    UPDATE PasswordResetTokens SET Usado = 1 WHERE Token = @Token;
END;