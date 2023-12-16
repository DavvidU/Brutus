using Brutus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.DTOs
{
    public class TestDetails
    {
        public Test Test { get; set; }
        public List<Pytanie> Pytania { get; set; }
    }
}
