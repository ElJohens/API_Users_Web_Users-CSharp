using System;
using System.Collections.Generic;

namespace GestionUsuarios.Models
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string RoleNombre { get; set; }

        public virtual UsuarioRole UsuarioRole { get; set; }
    }
}
