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
            public PostiToimipaikka()
            {
                Henkilot = new List<Henkilo>();
            }

            public int Id { get; set; }
            public string PostiNumero { get; set; }
            public string PostiToimipaikanNimi { get; set; }

            public virtual ICollection<Henkilo> Henkilot { get; set; }
        }

        public class Operaattori
        {
            public Operaattori()
            {
                Henkilot = new List<Henkilo>();
            }

            public int Id { get; set; }
            public string OperaattoriNimi { get; set; }

            public virtual ICollection<Henkilo> Henkilot { get; set; }
        }
    }
}
