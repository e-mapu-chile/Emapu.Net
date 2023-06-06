using SSoft.DataAccess;
using SSoft.Dominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSoft.Repositorio.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        public IEnumerable<Usuario> ObtenerUsuarios()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Roles> ObtenerRoles()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Sistema> ObtenerSistemas(int idUsuario)
        {
            masterEntities _db = new masterEntities();
            var data = _db.UsuarioSistema.Where(r => r.UsuarioId == idUsuario);
            var sistemas = new List<Sistema>();
            foreach (var d in data)
            {
                var r = new Sistema();
                Sistema firstOrDefault;
                if (d.SistemaId != null)
                {
                    r.Id = d.SistemaId.Value;
                    firstOrDefault = _db.Sistema.FirstOrDefault(rs => rs.Id == d.SistemaId);
                    if (firstOrDefault != null)
                    {
                        r.Nombre = firstOrDefault.Nombre;
                        r.Descripcion = firstOrDefault.Descripcion;
                        r.Color = firstOrDefault.Color;
                        r.ColorLetra = firstOrDefault.ColorLetra;
                    }
                }
                sistemas.Add(r);
            }
            return sistemas;
        }

        public IEnumerable<Roles> ObtenerRolesAsignados(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool ContieneRol(int idRol, int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Usuario ObtenerUsuario(int id)
        {
            masterEntities _db = new masterEntities();

            return _db.Usuario.FirstOrDefault(u => u.Id == id);
        }

        public Usuario ObtenerUsuario(string email)
        {
            throw new NotImplementedException();
        }

        public Usuario ObtenerUsuario(string cuenta, string clave)
        {
            masterEntities _db = new masterEntities();

            return _db.Usuario.FirstOrDefault(u => u.UserName == cuenta && u.Password == clave);
        }

        public Usuario ObtenerUsuarioNick(string cuenta)
        {
            throw new NotImplementedException();
        }

        public Usuario ObtenerUsuarioClave(string clave)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Recurso> ObtenerRecursos(int idUsuario, int idSistema)
        {
            masterEntities _db = new masterEntities();
            var data = _db.UsuarioRecurso.Where(r => r.UsuarioId == idUsuario);
            var recursos = new List<Recurso>();
            foreach (var d in data)
            {
                var r = new Recurso();
                Recurso firstOrDefault;
                if (d.RecursoId != null)
                {
                    r.Id = d.RecursoId.Value;
                    firstOrDefault = _db.Recurso.FirstOrDefault(rs => rs.Id == d.RecursoId);
                    if (firstOrDefault != null)
                    {
                        r.Nombre = firstOrDefault.Nombre;
                        r.Modulo = firstOrDefault.Modulo;
                        r.Descripcion = firstOrDefault.Descripcion;
                        r.Url = firstOrDefault.Url;
                        r.Color = firstOrDefault.Color;
                        r.ColorLetra = firstOrDefault.ColorLetra;

                        if (firstOrDefault.SistemaId == idSistema)
                            recursos.Add(r);
                    }
                }
            }
            return recursos;
        }

        public IEnumerable<Roles> ObtenerRoles(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public void EliminarUsuarioRol(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public void AsignarRol(int idUsuario, int idRol)
        {
            throw new NotImplementedException();
        }

        public void Crear(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Actualizar(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }


        public Sistema ObtenerSistema(int idSistema)
        {
            masterEntities _db = new masterEntities();

            return _db.Sistema.FirstOrDefault(s => s.Id == idSistema);
        }


        public IEnumerable<UsuarioEstablecimiento> ObtenerEstablecimientosUsuario(int idUsuario)
        {
            masterEntities _db = new masterEntities();

            return _db.UsuarioEstablecimiento.Where(e => e.UsuarioId == idUsuario);
        }


        public Establecimiento ObtenerEstablecimiento(int id)
        {
            masterEntities _db = new masterEntities();

            return _db.Establecimiento.FirstOrDefault(e => e.Id == id);
        }
    }
}
