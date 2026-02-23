
-- Editar un diagnóstico inicial existente
CREATE   PROCEDURE EditarDiagnosticoInicial
    @Id INT,
    @OrdenId INT,
    @Descripcion NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DiagnosticosIniciales
    SET OrdenId = @OrdenId,
        Descripcion = @Descripcion
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END