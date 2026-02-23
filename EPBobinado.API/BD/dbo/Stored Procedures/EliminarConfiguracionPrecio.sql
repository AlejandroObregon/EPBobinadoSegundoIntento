
-- Eliminar una configuración de precio
CREATE   PROCEDURE EliminarConfiguracionPrecio
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ConfiguracionPrecios WHERE Id = @Id;
    SELECT @Id AS Id;
END