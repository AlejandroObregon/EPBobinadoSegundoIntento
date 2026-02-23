
-- Agregar un nuevo diagnóstico inicial
CREATE   PROCEDURE AgregarDiagnosticoInicial
    @OrdenId INT,
    @Descripcion NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DiagnosticosIniciales (OrdenId, Descripcion)
    VALUES (@OrdenId, @Descripcion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END