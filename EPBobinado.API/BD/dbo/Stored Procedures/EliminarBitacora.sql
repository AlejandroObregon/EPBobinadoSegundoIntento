
-- Eliminar un registro de bitácora
CREATE   PROCEDURE EliminarBitacora
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Bitacora WHERE Id = @Id;
    SELECT @Id AS Id;
END