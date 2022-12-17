using EntitiesLibrary;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataParserLibrary
{
    public class CompanyParser : DataParser<Company>
    {
        public override Company MapData(Dictionary<int, string> indexAndHeaderNames, string[] lineDataArray)
        {
            Company company = new Company();
            company.ContactDetails = new Contact();

            foreach (var indexAndHeader in indexAndHeaderNames)
            {
                if (indexAndHeader.Key > lineDataArray.Length - 1) continue;

                PropertyInfo propertyInfo = typeof(Company).GetProperty(indexAndHeader.Value) ?? typeof(Contact).GetProperty(indexAndHeader.Value);
                if (propertyInfo == null) continue;

                string inputValue = lineDataArray[indexAndHeader.Key].Trim();
                try
                {
                    Type targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    propertyInfo.SetValue(propertyInfo.DeclaringType == typeof(Company) ? company : company.ContactDetails,
                        Convert.ChangeType(inputValue, targetType, null));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Mapped value ** {inputValue} **  has invalid format for property type {propertyInfo.PropertyType.Name}.");
                }
            }

            return company;
        }

    }

}
