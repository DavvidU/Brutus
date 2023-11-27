using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Admin
    {
        [Key]
        public int ID_Admina { get; set; }

        [ForeignKey("ID_Admina")]
        public virtual Konto? Konto { get; set; }
    }
}
