using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mdaWar.Helpers
{
    public class WeaponsHelper
    {
        private readonly Random random;

        private readonly string[] weapons =
        {
                "watergun",
                "butter knife",
                "rusted candlestick",
                "toy rifle",
                "posioned potato",
                "broken Duff bottle",
                "BelleDelphine bathwater jar",
                "Avril Lavigne marble sculpture",
                "area 51 stolen raygun"
        };

        public WeaponsHelper(Random random)
        {
            this.random = random;
        }

        public string GetRandomWeapon()
        {
            return this.weapons[this.random.Next()];
        }

        public string GetWeapon(int index)
        {
            return this.weapons[index];
        }

        public IEnumerable<string> GetAllWeapons()
        {
            // Copy so this.weapons can be modified outside
            return this.weapons.ToList();
        }
    }
}
