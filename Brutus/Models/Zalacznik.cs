using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Zalacznik
    {
        [Key]
        public int ID_Zalacznika { get; set; }
        public byte[] Dane { get; set; }
        [MaxLength(5)]
        public string Format { get; set; }
        [MaxLength(50)]
        public string Nazwa { get; set; }
    }

}
