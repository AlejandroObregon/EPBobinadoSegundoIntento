
-- Agregar una nueva configuración de impuesto
CREATE   PROCEDURE AgregarConfiguracionImpuesto
    @Porcentaje DECIMAL(5,2),
    @ConfiguradoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ConfiguracionImpuestos (Porcentaje, ConfiguradoPor)
    VALUES (@Porcentaje, @ConfiguradoPor);
    
    SELECT SCOPE_IDENTITY() AS Id;
END