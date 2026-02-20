-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerProductos
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Nombre, Categoria, Stock, StockMinimo, Precio, Activo
	FROM Productos
	WHERE Activo = 1 OR Activo IS NULL  -- Para incluir productos sin valor de Activo
	ORDER BY Nombre
END