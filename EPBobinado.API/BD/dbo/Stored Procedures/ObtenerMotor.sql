
-- Obtener un motor por ID
CREATE   PROCEDURE ObtenerMotor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.Id, m.ClienteId, m.ModeloId, m.NumeroSerie, m.CreadoEn,
           c.Nombre AS ClienteNombre, c.Telefono AS ClienteTelefono, c.Email AS ClienteEmail,
           mm.Nombre AS ModeloNombre, mm.Especificaciones AS ModeloEspecificaciones
    FROM Motores m
    INNER JOIN Clientes c ON m.ClienteId = c.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    WHERE m.Id = @Id;
END