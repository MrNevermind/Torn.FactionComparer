/***********************************************************************
  This project provides a C# interface to the Torn.com API.
  Copyright (C) 2020  TornCityPro
  
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
  
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
************************************************************************/

using Newtonsoft.Json;

namespace Torn.FactionComparer.Contracts.UserData
{
    public class Refills
    {
        [JsonProperty("energy_refill_used")] public bool EnergyRefillUsed { get; set; }

        [JsonProperty("nerve_refill_used")] public bool NerveRefillUsed { get; set; }

        [JsonProperty("token_refill_used")] public bool TokenRefillUsed { get; set; }

        [JsonProperty("special_refills_available")]
        public int SpecialRefillsAvailable { get; set; }
    }
}