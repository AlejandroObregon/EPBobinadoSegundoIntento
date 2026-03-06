CREATE PROCEDURE ObtenerOrdenesServicio
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        -- OrdenServicio
        os.Id,
        os.MotorId,
        os.Estado,
        os.TecnicoId,
        os.CreadoEn,

        -- Motor
        m.Id           AS MotId,
        m.NumeroSerie  AS MotNumeroSerie,
        m.UsuarioId    AS MotUsuarioId,
        m.ModeloId     AS MotModeloId,
        m.CreadoEn     AS MotCreadoEn,

        -- Usuario dueño del motor
        due.Id         AS DueId,
        due.Nombre     AS DueNombre,
        due.Email      AS DueEmail,
        due.Cedula     AS DueCedula,
        due.Telefono   AS DueTelefono,
        due.RolId      AS DueRolId,
        due.Activo     AS DueActivo,

        -- Modelo del motor
        mm.Id              AS ModId,
        mm.Nombre          AS ModNombre,
        mm.Especificaciones AS ModEspecificaciones,

        -- Técnico (puede ser NULL)
        tec.Id         AS TecId,
        tec.Nombre     AS TecNombre,
        tec.Email      AS TecEmail,
        tec.Cedula     AS TecCedula,
        tec.Telefono   AS TecTelefono,
        tec.RolId      AS TecRolId,
        tec.Activo     AS TecActivo

    FROM OrdenesServicio os
    INNER JOIN Motores        m   ON os.MotorId   = m.Id
    INNER JOIN Usuarios       due ON m.UsuarioId  = due.Id
    INNER JOIN ModelosMotor   mm  ON m.ModeloId   = mm.Id
    LEFT  JOIN Usuarios       tec ON os.TecnicoId = tec.Id

    ORDER BY os.CreadoEn DESC;
END
GO