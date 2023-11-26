using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class UczenTest
    {
        public int Id { get; set; }
        [ForeignKey("ID_Ucznia")]
        public virtual Uczen? Uczen { get; set; }

        [ForeignKey("ID_Testu")]
        public virtual Test? Test { get; set; }
    }
}
