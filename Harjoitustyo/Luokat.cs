using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Harjoitustyo
{
    class Luokat
    {
        public class PuhelinluetteloContext : DbContext
        {
            public DbSet<Henkilo> Henkilot { get; set; }
            public DbSet<PostiToimipaikka> Postitoimipaikat { get; set; }
            public DbSet<Operaattori> Operaattorit { get; set; }
        }

        public class PuhelinluetteloDbInitializer : DropCreateDatabaseAlways<PuhelinluetteloContext>
        {
            protected override void Seed(PuhelinluetteloContext context)
            {
                var Operaattorit = new List<Operaattori>
                {
                    new Operaattori { Id = 1, OperaattoriNimi = "DNA" },
                    new Operaattori { Id = 2, OperaattoriNimi = "Saunalahti" },
                    new Operaattori { Id = 3, OperaattoriNimi = "Sonera" }
                };
                Operaattorit.ForEach(x => context.Operaattorit.Add(x));

                var Postitoimipaikat = new List<PostiToimipaikka>
                {
                    new PostiToimipaikka { Id = 1, PostiNumero = "70500", PostiToimipaikanNimi = "Kuopio" },
                    new PostiToimipaikka { Id = 2, PostiNumero = "70100", PostiToimipaikanNimi = "Kuopio" },
                    new PostiToimipaikka { Id = 3, PostiNumero = "72100", PostiToimipaikanNimi = "Karttula" },
                    new PostiToimipaikka { Id = 4, PostiNumero = "72210", PostiToimipaikanNimi = "Tervo" }
                };
                Postitoimipaikat.ForEach(x => context.Postitoimipaikat.Add(x));

                var Yhteystiedot = new List<Henkilo>
                {
                    new Henkilo { HenkiloId = 1, Nimi = "Keke", LahiOsoite = "Kirkkokatu 6", PuhNo = "1231231234",  OperaattoriId = 1, PostiToimipaikkaId = 2 },
                    new Henkilo { HenkiloId = 2, Nimi = "Veijo", LahiOsoite = "Kalevalankatu 21", PuhNo = "2343246546",  OperaattoriId = 2, PostiToimipaikkaId = 1 },
                    new Henkilo { HenkiloId = 3, Nimi = "Simo", LahiOsoite = "Pohjolankatu 2", PuhNo = "768673435",  OperaattoriId = 1, PostiToimipaikkaId = 1 },
                    new Henkilo { HenkiloId = 4, Nimi = "Sirkka", LahiOsoite = "Kissakuusentie 44", PuhNo = "65765222",  OperaattoriId = 3, PostiToimipaikkaId = 3 },
                    new Henkilo { HenkiloId = 5, Nimi = "Tapio", LahiOsoite = "Suokatu 7", PuhNo = "456675675",  OperaattoriId = 1, PostiToimipaikkaId = 2 },
                    new Henkilo { HenkiloId = 6, Nimi = "Pirkko", LahiOsoite = "Marjantie 1", PuhNo = "5454323",  OperaattoriId = 1, PostiToimipaikkaId = 4 },
                    new Henkilo { HenkiloId = 7, Nimi = "Mahmud", LahiOsoite = "Kauppakatu 2", PuhNo = "89789879",  OperaattoriId = 2, PostiToimipaikkaId = 2 }
                };
                Yhteystiedot.ForEach(x => context.Henkilot.Add(x));

                context.SaveChanges();
            }
        }

        public class Henkilo
        {
            public int HenkiloId { get; set; }
            public string Nimi { get; set; }
            public string PuhNo { get; set; }
            public string LahiOsoite { get; set; }
            public int PostiToimipaikkaId { get; set; }
            public int OperaattoriId { get; set; }

            public virtual PostiToimipaikka PostiToimipaikka { get; set; }
            public virtual Operaattori Operaattori { get; set; }
        }

        public class PostiToimipaikka
        {
            public int Id { get; set; }
            public string PostiNumero { get; set; }
            public string PostiToimipaikanNimi { get; set; }

            public virtual ICollection<Henkilo> Henkilot { get; set; }
        }

        public class Operaattori
        {
            public int Id { get; set; }
            public string OperaattoriNimi { get; set; }

            public virtual ICollection<Henkilo> Henkilot { get; set; }
        }
    }
}
