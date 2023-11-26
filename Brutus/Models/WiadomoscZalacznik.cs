using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class WiadomoscZalacznik
    {
        public int Id { get; set; }
        [ForeignKey("ID_Wiadomosci")]
        public virtual Wiadomosc? Wiadomosc { get; set; }

        [ForeignKey("ID_Zalacznika")]
        public virtual Zalacznik? Zalacznik { get; set; }
    }


}
