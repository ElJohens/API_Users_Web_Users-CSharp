using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionUsuarios.Models
{
    public class Password
    {
        //Con esto le decimos cuando es model first que es una primary key auto incrementable
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PasswordId { get; set; }
        public string PasswordText { get; set; }
        public int UserdId { get; set; }

        //Acá agregamos la llave foranea
        //El campo que la une
        [ForeignKey("UserdId")]
        public virtual Usuario Usuario { get; set; }
    }
}
