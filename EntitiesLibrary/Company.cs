using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLibrary
{
    public class Company
    {
        public string CompanyName { get; set; }
        public int? YearFounded { get; set; }
        public int? YearsInBusiness { get; set; }
        public Contact ContactDetails { get; set; }

        public override bool Equals(object? obj)
        {
            // Check if the object is null or is not of the same type
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // Cast the object to the derived type
            Company other = (Company)obj;

            // Compare the properties of the two objects
            return CompanyName == other.CompanyName;
        }
    }
}
