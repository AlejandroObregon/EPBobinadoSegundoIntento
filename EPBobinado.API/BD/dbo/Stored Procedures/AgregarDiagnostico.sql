
-- Agregar un nuevo diagnóstico técnico
CREATE   PROCEDURE [dbo].[AgregarDiagnostico]
    @OrdenId INT,
    @Detalle NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Diagnosticos (OrdenId, Detalle)
    VALUES (@OrdenId, @Detalle);
    
    SELECT SCOPE_IDENTITY() AS Id;
END