using System;
using System.Collections.Generic;
#nullable disable
namespace GestionUsuarios.Models
{
    public partial class Usuario
    {
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellidos { get; set; }
        public string UsuarioIdentificacion { get; set; }
        public string UsuarioCorreo { get; set; }
        public string UsuarioTelefono { get; set; }
        public string UsuarioAlias { get; set; }

        public virtual UsuarioRole UsuarioRole { get; set; }

        //Traerme las claves que le pertenecen al usuario
        //public virtual ICollection<Password> Pass { get; set; }

        //Conocer los datos de password y jugar con ellos
        public virtual Password PasswordNavigation { get; set; }

       
    }
}
