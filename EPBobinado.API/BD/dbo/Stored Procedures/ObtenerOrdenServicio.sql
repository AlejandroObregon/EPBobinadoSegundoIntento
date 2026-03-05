-- Obtener una orden de servicio por ID
CREATE PROCEDURE ObtenerOrdenServicio
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT os.Id, os.MotorId, os.Estado, os.TecnicoId, os.CreadoEn,
           m.NumeroSerie AS MotorNumeroSerie,
           u.Id AS UsuarioId, u.Nombre AS UsuarioNombre, u.Telefono AS UsuarioTelefono,
           mm.Id AS ModeloId, mm.Nombre AS ModeloNombre, mm.Especificaciones,
           t.Nombre AS TecnicoNombre, t.Email AS TecnicoEmail
    FROM OrdenesServicio os
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    INNER JOIN ModelosMotor mm ON m.ModeloId = mm.Id
    LEFT JOIN Usuarios t ON os.TecnicoId = t.Id
    WHERE os.Id = @Id;
END
GO