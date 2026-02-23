
-- Agregar un nuevo diagnóstico técnico
CREATE   PROCEDURE AgregarDiagnosticoTecnico
    @OrdenId INT,
    @Detalle NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DiagnosticosTecnicos (OrdenId, Detalle)
    VALUES (@OrdenId, @Detalle);
    
    SELECT SCOPE_IDENTITY() AS Id;
END