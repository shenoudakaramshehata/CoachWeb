using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TournamentTarget
    {
        public TournamentTarget()
        {
            Tournaments = new HashSet<Tournament>();
        }

        public int TournamentTargetId { get; set; }
        public string TournamentTargetTlAr { get; set; }
        public string TournamentTargetTlEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
