using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace mdaWar
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var war = modelBuilder.Entity<War>();

            war.HasKey(x => x.Id);
            war.HasMany(x => x.Participants).WithOne(x => x.War).HasForeignKey(x => x.WarId);

            var participant = modelBuilder.Entity<Participant>();

            participant.HasKey(x => x.Id);
            participant.HasOne(x => x.War).WithMany(x => x.Participants).HasForeignKey(x => x.WarId);
        }
    }
}
