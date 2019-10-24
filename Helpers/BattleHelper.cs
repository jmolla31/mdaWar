using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mdaWar.Helpers
{
    public class BattleHelper
    {
        private readonly Random random;
        private readonly WeaponsHelper weapons;
        public BattleHelper(Random random, WeaponsHelper weapons)
        {
            this.random = random;
            this.weapons = weapons;
        }

        public string SimulateEngagement(IList<string> names)
        {
            var lives = random.Next(names.Count());

            var dies = 0;
            do
            {
                dies = random.Next(names.Count());
            } while (lives == dies);

            var weapon = weapons.GetRandomWeapon();

            names.RemoveAt(dies);

            return ($"{names[lives]} kills {names[dies]} with a {weapon}");
        }

        public string SimulateEngagement(IList<Participant> participants)
        {
            var lives = random.Next(participants.Count);

            var dies = 0;
            do
            {
                dies = random.Next(participants.Count);
            } while (lives == dies);

            var weapon = weapons.GetRandomWeapon();

            participants[dies].Alive = false;

            return $"{participants[lives]} kills {participants[dies]} with a {weapon}";
        }
    }
}
