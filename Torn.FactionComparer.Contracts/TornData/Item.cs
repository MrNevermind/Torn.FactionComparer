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

using System;
using Newtonsoft.Json;
using Torn.FactionComparer.Contracts.CommonData;

namespace Torn.FactionComparer.Contracts.TornData
{
    public class Item : IntApiListItem
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("effect")] public string Effect { get; set; }

        [JsonProperty("requirement")] public string Requirement { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("weapon_type")] public string WeaponType { get; set; }

        [JsonProperty("buy_price")] public long BuyPrice { get; set; }

        [JsonProperty("sell_price")] public long SellPrice { get; set; }

        [JsonProperty("market_value")] public long MarketValue { get; set; }

        [JsonProperty("circulation")] public int Circulation { get; set; }

        [JsonProperty("image")] public Uri ImageUrl { get; set; }
    }
}