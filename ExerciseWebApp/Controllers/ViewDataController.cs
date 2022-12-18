using EntitiesLibrary;
using ExerciseWebApp.Models;
using InMemoryStorageLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ExerciseWebApp.Controllers
{
    public class ViewDataController : Controller
    {
        private readonly IMemoryStorage _memoryStorage;
        public ViewDataController(IMemoryStorage memoryStorage)
        {
            _memoryStorage = memoryStorage;
        }
        public IActionResult Index() => View(_memoryStorage.GetAll<Company>().Select(company => new CompanyViewModel(company)));

        [HttpPost("SortData")]
        public IActionResult SortData(string[] sortCriteria)
        {
            IEnumerable<CompanyViewModel> companies = _memoryStorage.GetAll<Company>().Select(company => new CompanyViewModel(company));

            if (sortCriteria.Length > 0)
                companies = SortFunction(companies, sortCriteria);

            return Json(companies);
        }

        [HttpGet("ExportData")]
        public FileStreamResult ExportData([FromQuery] string sortCriteria)
        {
            IEnumerable<CompanyViewModel> companies = _memoryStorage.GetAll<Company>().Select(company => new CompanyViewModel(company));
            if (!string.IsNullOrWhiteSpace(sortCriteria))
            {
                string[] sortCriteriaArray = sortCriteria.Split(',');
                companies = SortFunction(companies, sortCriteriaArray);
            }

            // Create the memory stream and stream writer
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("Company Name, Years in Business, Contact Name, Contact Phone Number, Contact Email");

            foreach (var company in companies)
                writer.WriteLine($"{company.CompanyName}, {company.YearsInBusiness}, {company.ContactDetails.FullName}, {company.ContactDetails.PhoneNumber}, {company.ContactDetails.Email}");
            writer.Flush();
            // Reset the stream position to the beginning
            stream.Position = 0;

            FileStreamResult result = new FileStreamResult(stream, "text/csv")
            {
                FileDownloadName = $"Export_{DateTime.UtcNow.Ticks}.csv"
            };

            // Return the result
            return result;
        }

        // For more dynamic sorting i would use a nuget package called System.Linq.Dynamic.Core
        // But for the purpose of this exercise i will give the posibility to sort by 2 columns only
        [NonAction]
        private IEnumerable<CompanyViewModel> SortFunction(IEnumerable<CompanyViewModel> companies, string[] sortCriteria)
        {
            string[] parts1st = sortCriteria[0].Split(' ');
            string sortColumn1st = parts1st[0];
            string sortDirection1st = parts1st[1];
            if (sortCriteria.Length == 1)
            {
                companies = sortDirection1st == "asc" ?
                    companies.OrderBy(c => GetPropertyValue(c, sortColumn1st)) :
                    companies.OrderByDescending(c => GetPropertyValue(c, sortColumn1st));
            }
            if (sortCriteria.Length == 2)
            {
                string[] parts2nd = sortCriteria[1].Split(' ');
                string sortColumn2nd = parts2nd[0];
                string sortDirection2nd = parts2nd[1];

                if (sortDirection1st == "asc" && sortDirection2nd == "asc")
                    companies = companies.OrderBy(c => GetPropertyValue(c, sortColumn1st))
                        .ThenBy(c => GetPropertyValue(c, sortColumn2nd));

                else if (sortDirection1st == "desc" && sortDirection2nd == "asc")
                    companies = companies.OrderByDescending(c => GetPropertyValue(c, sortColumn1st))
                        .ThenBy(c => GetPropertyValue(c, sortColumn2nd));

                else if (sortDirection1st == "asc" && sortDirection2nd == "desc")
                    companies = companies.OrderBy(c => GetPropertyValue(c, sortColumn1st))
                        .ThenByDescending(c => GetPropertyValue(c, sortColumn2nd));

                else
                    companies = companies.OrderByDescending(c => GetPropertyValue(c, sortColumn1st))
                        .ThenByDescending(c => GetPropertyValue(c, sortColumn2nd));
            }
            return companies;
        }
        [NonAction]
        private object? GetPropertyValue(CompanyViewModel obj, string propertyName)
        {
            PropertyInfo propertyInfo = typeof(Company).GetProperty(propertyName) ?? typeof(Contact).GetProperty(propertyName);
            if (propertyInfo == null) return null;

            return propertyInfo.DeclaringType == typeof(Company) ? propertyInfo.GetValue(obj) : propertyInfo.GetValue(obj.ContactDetails);
        }
    }
}
