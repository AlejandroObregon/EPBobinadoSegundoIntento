
-- Eliminar un diagnóstico técnico
CREATE   PROCEDURE [dbo].[EliminarDiagnostico]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Diagnosticos WHERE Id = @Id;
    SELECT @Id AS Id;
END