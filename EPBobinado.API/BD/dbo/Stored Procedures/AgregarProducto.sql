-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AgregarProducto
	-- Add the parameters for the stored procedure here
	@Nombre AS NVARCHAR(100)
    ,@Categoria AS NVARCHAR(50)
    ,@Stock AS INT
    ,@StockMinimo AS INT
    ,@Precio AS DECIMAL(10, 2)
    ,@Activo AS BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRANSACTION
	    INSERT INTO [dbo].[Productos]
               ([Nombre]
               ,[Categoria]
               ,[Stock]
               ,[StockMinimo]
               ,[Precio]
               ,[Activo])
         VALUES
               (@Nombre
                ,@Categoria
                ,@Stock
                ,@StockMinimo
                ,@Precio
                ,@Activo)

        SELECT SCOPE_IDENTITY() AS Id
    COMMIT TRANSACTION
END