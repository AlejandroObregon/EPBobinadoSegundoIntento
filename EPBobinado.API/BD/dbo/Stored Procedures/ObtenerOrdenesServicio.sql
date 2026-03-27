CREATE PROCEDURE ObtenerOrdenesServicio
AS
BEGIN
    SET NOCOUNT ON;
SELECT Orden.Id, Motor.Id AS MotorId, Orden.Estado, Orden.CreadoEn,
       Tecnico.Id AS IdTecnico, Tecnico.Nombre AS Tecnico,
       Cliente.Id AS IdCliente, Cliente.Nombre AS Cliente
FROM OrdenesServicio Orden
INNER JOIN Motores Motor ON Orden.MotorId = Motor.Id
INNER JOIN Usuarios Cliente ON Orden.UsuarioId = Cliente.Id
INNER JOIN Usuarios Tecnico ON Orden.TecnicoId = Tecnico.Id
END
GO