using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class UczenOcena
    {
        public int Id { get; set; }
        [ForeignKey("ID_Ucznia")]
        public virtual Uczen? Uczen { get; set; }

        [ForeignKey("ID_Oceny")]
        public virtual Ocena? Ocena { get; set; }
    }
}
