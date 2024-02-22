using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class Configuration
    {
        public int ConfigurationId { get; set; }
        public string Facebook { get; set; }
        public string WhatsApp { get; set; }
        public string LinkedIn { get; set; }
        public string Instgram { get; set; }
        public string Twitter { get; set; }
    }
}
