
-- Editar un diagnóstico técnico existente
CREATE   PROCEDURE EditarDiagnosticoTecnico
    @Id INT,
    @OrdenId INT,
    @Detalle NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DiagnosticosTecnicos
    SET OrdenId = @OrdenId,
        Detalle = @Detalle
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END