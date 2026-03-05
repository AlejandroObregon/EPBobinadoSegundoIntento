using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using System.Security.Cryptography;
using System.Text;

namespace Flujo
{
    public class UsuarioFlujo : IUsuarioFlujo
    {
        private IUsuarioDA _usuarioDA;

        public UsuarioFlujo(IUsuarioDA usuarioDA)
        {
            _usuarioDA = usuarioDA;
        }

        public Task<int> Agregar(UsuarioRequest usuario)
        {
            // Hashear contraseña antes de guardar
            usuario.PasswordHash = HashearPassword(usuario.PasswordHash);
            return _usuarioDA.Agregar(usuario);
        }

        public Task<int> Editar(int Id, UsuarioRequest usuario)
        {
            // Solo hashear si viene una contraseña nueva
            if (!string.IsNullOrWhiteSpace(usuario.PasswordHash))
                usuario.PasswordHash = HashearPassword(usuario.PasswordHash);
            return _usuarioDA.Editar(Id, usuario);
        }

        public Task<int> Eliminar(int Id)
        {
            return _usuarioDA.Eliminar(Id);
        }

        public Task<IEnumerable<UsuarioResponse>> Obtener()
        {
            return _usuarioDA.Obtener();
        }

        public Task<UsuarioResponse> Obtener(int Id)
        {
            return _usuarioDA.Obtener(Id);
        }

        public Task<UsuarioResponse?> Login(string email, string password)
        {
            var hash = HashearPassword(password);
            return _usuarioDA.Login(email, hash);
        }

        // ── SHA256 ──────────────────────────────────────────────────────
        private static string HashearPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}