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

namespace Torn.FactionComparer.Contracts.Fields
{
    /// <summary>
    ///     Company fields that can be queried
    /// </summary>
    public class CompanyField : ApiField
    {
        public static readonly CompanyField Profile = new CompanyField("profile");
        public static readonly CompanyField Detailed = new CompanyField("detailed");
        public static readonly CompanyField Stock = new CompanyField("stock");
        public static readonly CompanyField Employees = new CompanyField("employees");
        public static readonly CompanyField News = new CompanyField("news");
        public static readonly CompanyField NewsFull = new CompanyField("newsfull");
        public static readonly CompanyField Lookup = new CompanyField("lookup");
        public static readonly CompanyField Timestamp = new CompanyField("timestamp");

        protected CompanyField(string fieldName) : base(fieldName)
        {
        }
    }
}