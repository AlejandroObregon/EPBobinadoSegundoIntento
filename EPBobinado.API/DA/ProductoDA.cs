using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Nombre = producto.Nombre,
                Categoria = producto.Categoria,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                Precio = producto.Precio,
                Activo = producto.Activo
            });

            return resultadoConsulta;
        }

        public async Task<int> Editar(int Id, ProductoRequest producto)
        {
            await verificarProductoExiste(Id);
            string query = @"EditarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id,
                Nombre = producto.Nombre,
                Categoria = producto.Categoria,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                Precio = producto.Precio,
                Activo = producto.Activo
            });
            return resultadoConsulta;
        }

        public async Task<int> Eliminar(int Id)
        {
            await verificarProductoExiste(Id);
            string query = @"EliminarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id = Id
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"ObtenerProductos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProductoResponse> Obtener(int Id)
        {
            string query = @"ObtenerProducto";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query,
                new { Id = Id });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarProductoExiste(int Id)
        {
            ProductoResponse? resultadoConsultaProducto = await Obtener(Id);
            if (resultadoConsultaProducto == null)
                throw new Exception("No se encontró el producto");
        }
    }
}