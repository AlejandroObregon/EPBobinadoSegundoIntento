-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE EditarProducto
	-- Add the parameters for the stored procedure here
	@Id AS INT
    ,@Nombre AS NVARCHAR(100)
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
        UPDATE [dbo].[Productos]
           SET [Nombre] = @Nombre
              ,[Categoria] = @Categoria
              ,[Stock] = @Stock
              ,[StockMinimo] = @StockMinimo
              ,[Precio] = @Precio
              ,[Activo] = @Activo
         WHERE Id = @Id

         SELECT @Id
    COMMIT TRANSACTION
END