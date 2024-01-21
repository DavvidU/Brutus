﻿using Brutus.Models;

namespace Brutus.Services.ZestawienieUcznia
{
    public class KomentarzeOcen : DodatkowaInformacja
    {
        public KomentarzeOcen(Zestawienie zestawienie) : base(zestawienie) { }
        public new string GetDaneUcznia() { return base.GetDaneUcznia(); }
        public new string GetDanePrzedmiotu() { return base.GetDanePrzedmiotu(); }
        public new List<string> GetSformatowanaListaOcen() 
        { 
            List<string> sformatowanaListaOcen = base.GetSformatowanaListaOcen();

            List<Ocena> listaOcen = GetOceny();

            for (int  i = 0; i < sformatowanaListaOcen.Count; ++i)
                sformatowanaListaOcen[i] = sformatowanaListaOcen[i] + " " + listaOcen[i].Komentarz;

            return sformatowanaListaOcen; 
        }
        public new List<string> GetDodatkoweInformacje() { return base.GetDodatkoweInformacje(); }
        public new List<Ocena> GetOceny() { return base.GetOceny(); }
    }
}
