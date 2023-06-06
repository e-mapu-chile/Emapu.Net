using SSoft.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Dominio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        IEnumerable<Usuario> ObtenerUsuarios();
        IEnumerable<Roles> ObtenerRoles();
        IEnumerable<Roles> ObtenerRolesAsignados(int usuarioId);
        bool ContieneRol(int idRol, int idUsuario);
        Usuario ObtenerUsuario(int id);
        Usuario ObtenerUsuario(string email);
        Usuario ObtenerUsuario(string cuenta, string clave);
        Usuario ObtenerUsuarioNick(string cuenta);
        Usuario ObtenerUsuarioClave(string clave);
        IEnumerable<Recurso> ObtenerRecursos(int idUsuario, int idSistema);
        IEnumerable<Roles> ObtenerRoles(int idUsuario);
        IEnumerable<Sistema> ObtenerSistemas(int idUsuario);
        Sistema ObtenerSistema(int idSistema);
        IEnumerable<UsuarioEstablecimiento> ObtenerEstablecimientosUsuario(int idUsuario);
        Establecimiento ObtenerEstablecimiento(int id);

        void EliminarUsuarioRol(int idUsuario);


        void AsignarRol(int idUsuario, int idRol);



        void Crear(Usuario usuario);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);


    }
}
