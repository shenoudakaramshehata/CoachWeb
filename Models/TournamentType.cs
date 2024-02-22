using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TournamentType
    {
        public TournamentType()
        {
            Tournaments = new HashSet<Tournament>();
        }

        public int TournamentTypeId { get; set; }
        public string TournamentTypeTlAr { get; set; }
        public string TournamentTypeTlEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
