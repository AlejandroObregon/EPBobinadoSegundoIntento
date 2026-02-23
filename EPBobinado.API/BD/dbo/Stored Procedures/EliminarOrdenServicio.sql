
-- Eliminar una orden de servicio
CREATE   PROCEDURE EliminarOrdenServicio
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM OrdenesServicio WHERE Id = @Id;
    SELECT @Id AS Id;
END