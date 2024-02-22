using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class TournamentImage
    {
        public int TournamentImageId { get; set; }
        public int TournamentId { get; set; }
        public string Pic { get; set; }

        public virtual Tournament Tournament { get; set; }
    }
}
