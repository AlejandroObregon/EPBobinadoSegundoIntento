
-- Eliminar una configuración de impuesto
CREATE   PROCEDURE EliminarConfiguracionImpuesto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ConfiguracionImpuestos WHERE Id = @Id;
    SELECT @Id AS Id;
END