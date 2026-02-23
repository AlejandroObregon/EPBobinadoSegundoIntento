
-- Editar una configuración de precio existente
CREATE   PROCEDURE EditarConfiguracionPrecio
    @Id INT,
    @PrecioHora DECIMAL(10,2),
    @Margen DECIMAL(5,2),
    @ConfiguradoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ConfiguracionPrecios
    SET PrecioHora = @PrecioHora,
        Margen = @Margen,
        ConfiguradoPor = @ConfiguradoPor
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END