using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class KontoWiadomosc
    {
        public int Id { get; set; }
        [ForeignKey("ID_Odbiorcy")]
        public virtual Konto? Odbiorca { get; set; }

        [ForeignKey("ID_Wiadomosci")]
        public virtual Wiadomosc? Wiadomosc { get; set; }
    }
}
