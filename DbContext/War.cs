using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace mdaWar
{
    public class War
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Participant> Participants { get; set; } = new Collection<Participant>();
    }
}
