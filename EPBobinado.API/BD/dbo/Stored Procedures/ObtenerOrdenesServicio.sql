CREATE PROCEDURE ObtenerOrdenesServicio
AS
BEGIN
    SET NOCOUNT ON;
SELECT Orden.Id, Orden.Estado, Orden.CreadoEn, Orden.Costo, Orden.Descripcion, Orden.FechaCita,
       Motor.Id AS MotorId, Motor.NumeroSerie AS NumeroSerie, ModelosMotor.Nombre AS Modelo,
       Tecnico.Id AS IdTecnico, Tecnico.Nombre AS Tecnico,
       Cliente.Id AS IdCliente, Cliente.Nombre AS Cliente
FROM OrdenesServicio Orden
LEFT JOIN Motores Motor ON Orden.MotorId = Motor.Id
LEFT JOIN ModelosMotor ON Motor.ModeloId = ModelosMotor.Id
LEFT JOIN Usuarios Cliente ON Orden.UsuarioId = Cliente.Id
LEFT JOIN Usuarios Tecnico ON Orden.TecnicoId = Tecnico.Id
END
GO