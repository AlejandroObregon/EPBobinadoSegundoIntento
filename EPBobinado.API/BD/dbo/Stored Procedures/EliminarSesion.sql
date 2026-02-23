
-- Eliminar una sesión
CREATE   PROCEDURE EliminarSesion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Sesiones WHERE Id = @Id;
    SELECT @Id AS Id;
END