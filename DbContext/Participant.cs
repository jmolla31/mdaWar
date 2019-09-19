using System;
using System.Collections.Generic;
using System.Text;

namespace mdaWar
{
    public class Participant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int WarId { get; set; }

        public bool Alive { get; set; } = true;

        public virtual War War { get; set; }
    }
}
