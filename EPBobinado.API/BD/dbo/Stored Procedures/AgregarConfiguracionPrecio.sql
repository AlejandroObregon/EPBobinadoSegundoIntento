
-- Agregar una nueva configuración de precio
CREATE   PROCEDURE AgregarConfiguracionPrecio
    @PrecioHora DECIMAL(10,2),
    @Margen DECIMAL(5,2),
    @ConfiguradoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ConfiguracionPrecios (PrecioHora, Margen, ConfiguradoPor)
    VALUES (@PrecioHora, @Margen, @ConfiguradoPor);
    
    SELECT SCOPE_IDENTITY() AS Id;
END