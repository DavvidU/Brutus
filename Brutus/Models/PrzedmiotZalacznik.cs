using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class PrzedmiotZalacznik
    {
        public int Id { get; set; }
        [ForeignKey("ID_Przedmiotu")]
        public virtual Przedmiot? Przedmiot { get; set; }

        [ForeignKey("ID_Zalacznika")]
        public virtual Zalacznik? Zalacznik { get; set; }
    }
}
