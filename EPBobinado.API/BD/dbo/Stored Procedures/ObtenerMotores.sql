
/* =========================================================
   MOTORES
   ========================================================= */

-- Obtener todos los motores
CREATE OR ALTER PROCEDURE ObtenerMotores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.Id,
        m.UsuarioId,
        m.ModeloId,
        m.NumeroSerie,
        m.CreadoEn,
        -- datos del usuario
        u.Id        AS UsuarioId,
        u.Nombre    AS Nombre,
        u.Email     AS Email,
        u.Cedula    AS Cedula,
        u.Telefono  AS Telefono,
        -- datos del modelo
        mm.Id       AS ModeloId,
        mm.Nombre   AS ModeloNombre
    FROM Motores m
    INNER JOIN Usuarios      u  ON m.UsuarioId = u.Id
    INNER JOIN ModelosMotor  mm ON m.ModeloId  = mm.Id;
END
GO