
-- Eliminar un diagnóstico técnico
CREATE   PROCEDURE EliminarDiagnosticoTecnico
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DiagnosticosTecnicos WHERE Id = @Id;
    SELECT @Id AS Id;
END