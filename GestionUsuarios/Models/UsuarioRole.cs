using System;
using System.Collections.Generic;

namespace GestionUsuarios.Models
{
    public partial class UsuarioRole
    {
        public int UsuarioRoleId { get; set; }
        public int UsuarioId { get; set; }
        public int RoleId { get; set; }

        public virtual Usuario UsuarioRole1 { get; set; }
        public virtual Role UsuarioRoleNavigation { get; set; }
    }
}
