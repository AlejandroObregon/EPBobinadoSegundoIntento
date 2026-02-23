using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using DA;
using DA.Repositorios;
using Flujo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// INYECCIÓN DE DEPENDENCIAS - REPOSITORIOS (DA)
builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();

// Seguridad y Usuarios
builder.Services.AddScoped<IRolDA, RolDA>();
builder.Services.AddScoped<IDireccionDA, DireccionDA>();
builder.Services.AddScoped<IUsuarioDA, UsuarioDA>();
builder.Services.AddScoped<ISesionDA, SesionDA>();

// Clientes y Motores
builder.Services.AddScoped<IClienteDA, ClienteDA>();
builder.Services.AddScoped<IModeloMotorDA, ModeloMotorDA>();
builder.Services.AddScoped<IMotorDA, MotorDA>();

// Órdenes de Servicio
builder.Services.AddScoped<IOrdenServicioDA, OrdenServicioDA>();
builder.Services.AddScoped<IDiagnosticoInicialDA, DiagnosticoInicialDA>();
builder.Services.AddScoped<IDiagnosticoTecnicoDA, DiagnosticoTecnicoDA>();

// Inventario
builder.Services.AddScoped<IProductoDA, ProductoDA>();
builder.Services.AddScoped<IProveedorDA, ProveedorDA>();
builder.Services.AddScoped<IMovimientoInventarioDA, MovimientoInventarioDA>();

// Cotización y Facturación
builder.Services.AddScoped<ICotizacionDA, CotizacionDA>();
builder.Services.AddScoped<IFacturaDA, FacturaDA>();
builder.Services.AddScoped<IPagoDA, PagoDA>();

// Configuración
builder.Services.AddScoped<IConfiguracionPrecioDA, ConfiguracionPrecioDA>();
builder.Services.AddScoped<IConfiguracionImpuestoDA, ConfiguracionImpuestoDA>();

// Auditoría
builder.Services.AddScoped<IBitacoraDA, BitacoraDA>();

// INYECCIÓN DE DEPENDENCIAS - FLUJOS (Lógica de Negocio)

// Seguridad y Usuarios
builder.Services.AddScoped<IRolFlujo, RolFlujo>();
builder.Services.AddScoped<IDireccionFlujo, DireccionFlujo>();
builder.Services.AddScoped<IUsuarioFlujo, UsuarioFlujo>();
builder.Services.AddScoped<ISesionFlujo, SesionFlujo>();

// Clientes y Motores
builder.Services.AddScoped<IClienteFlujo, ClienteFlujo>();
builder.Services.AddScoped<IModeloMotorFlujo, ModeloMotorFlujo>();
builder.Services.AddScoped<IMotorFlujo, MotorFlujo>();

// Órdenes de Servicio
builder.Services.AddScoped<IOrdenServicioFlujo, OrdenServicioFlujo>();
builder.Services.AddScoped<IDiagnosticoInicialFlujo, DiagnosticoInicialFlujo>();
builder.Services.AddScoped<IDiagnosticoTecnicoFlujo, DiagnosticoTecnicoFlujo>();

// Inventario
builder.Services.AddScoped<IProductoFlujo, ProductoFlujo>();
builder.Services.AddScoped<IProveedorFlujo, ProveedorFlujo>();
builder.Services.AddScoped<IMovimientoInventarioFlujo, MovimientoInventarioFlujo>();

// Cotización y Facturación
builder.Services.AddScoped<ICotizacionFlujo, CotizacionFlujo>();
builder.Services.AddScoped<IFacturaFlujo, FacturaFlujo>();
builder.Services.AddScoped<IPagoFlujo, PagoFlujo>();

// Configuración
builder.Services.AddScoped<IConfiguracionPrecioFlujo, ConfiguracionPrecioFlujo>();
builder.Services.AddScoped<IConfiguracionImpuestoFlujo, ConfiguracionImpuestoFlujo>();

// Auditoría
builder.Services.AddScoped<IBitacoraFlujo, BitacoraFlujo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
