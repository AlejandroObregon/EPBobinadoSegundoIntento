
-- Eliminar un proveedor
CREATE   PROCEDURE EliminarProveedor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Proveedores WHERE Id = @Id;
    SELECT @Id AS Id;
END