
-- Editar una configuración de impuesto existente
CREATE   PROCEDURE EditarConfiguracionImpuesto
    @Id INT,
    @Porcentaje DECIMAL(5,2),
    @ConfiguradoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ConfiguracionImpuestos
    SET Porcentaje = @Porcentaje,
        ConfiguradoPor = @ConfiguradoPor
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END