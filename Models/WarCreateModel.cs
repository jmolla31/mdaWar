using System;
using System.Collections.Generic;
using System.Text;

namespace mdaWar.Models
{
    public class WarCreateModel
    {
        public string Name { get; set; }

        public IEnumerable<ParticipantModel> Participants { get; set; }
    }
}
