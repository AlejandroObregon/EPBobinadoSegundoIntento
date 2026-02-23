
-- Obtener una orden de servicio por ID
CREATE   PROCEDURE ObtenerOrdenServicio
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT os.Id, os.MotorId, os.Estado, os.TecnicoId, os.CreadoEn,
           m.NumeroSerie AS MotorNumeroSerie,
           c.Id AS ClienteId, c.Nombre AS ClienteNombre, c.Telefono AS ClienteTelefono,
           mm.Id AS ModeloId, mm.Nombre AS ModeloNombre, mm.Especificaciones,
           u.Nombre AS TecnicoNombre, u.Email AS TecnicoEmail
    FROM OrdenesServicio os
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Clientes c ON m.ClienteId = c.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    LEFT JOIN Usuarios u ON os.TecnicoId = u.Id
    WHERE os.Id = @Id;
END