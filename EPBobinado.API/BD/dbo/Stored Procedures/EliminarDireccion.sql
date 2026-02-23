
-- Eliminar una dirección
CREATE   PROCEDURE EliminarDireccion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Direcciones WHERE Id = @Id;
    SELECT @Id AS Id;
END