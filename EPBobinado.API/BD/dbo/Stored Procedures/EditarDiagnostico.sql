
-- Editar un diagnóstico técnico existente
CREATE   PROCEDURE [dbo].[EditarDiagnostico]
    @Id INT,
    @OrdenId INT,
    @Detalle NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Diagnosticos
    SET OrdenId = @OrdenId,
        Detalle = @Detalle
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END