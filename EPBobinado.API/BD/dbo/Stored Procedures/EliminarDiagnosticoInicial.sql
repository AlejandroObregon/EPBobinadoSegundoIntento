
-- Eliminar un diagnóstico inicial
CREATE   PROCEDURE EliminarDiagnosticoInicial
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DiagnosticosIniciales WHERE Id = @Id;
    SELECT @Id AS Id;
END