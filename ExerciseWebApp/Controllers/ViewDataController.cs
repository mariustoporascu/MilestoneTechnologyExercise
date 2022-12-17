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

        // For more dynamic sorting i would use a nuget package called System.Linq.Dynamic.Core
        // But for the purpose of this exercise i will give the posibility to sort by 2 columns only
        [HttpPost("SortData")]
        public IActionResult SortData(string[] sortCriteria)
        {
            if (sortCriteria.Length == 0) return RedirectToAction("Index");

            IEnumerable<CompanyViewModel> companies = _memoryStorage.GetAll<Company>().Select(company => new CompanyViewModel(company));
            
            string[] parts1st = sortCriteria[0].Split(' ');
            string sortColumn1st = parts1st[0];
            string sortDirection1st = parts1st[1];
            PropertyInfo propertyInfo1st = typeof(Company).GetProperty(sortColumn1st) ?? typeof(Contact).GetProperty(sortColumn1st);
            if (sortCriteria.Length == 1)
            {
                companies = sortDirection1st == "asc" ? 
                    companies.OrderBy( c => propertyInfo1st.DeclaringType == typeof(Company) ? 
                        propertyInfo1st.GetValue(c) : propertyInfo1st.GetValue(c.ContactDetails)) : 
                    companies.OrderByDescending( c => propertyInfo1st.DeclaringType == typeof(Company) ? 
                        propertyInfo1st.GetValue(c) : propertyInfo1st.GetValue(c.ContactDetails));
            }
            if (sortCriteria.Length == 2)
            {
                string[] parts2nd = sortCriteria[1].Split(' ');
                string sortColumn2nd = parts2nd[0];
                string sortDirection2nd = parts2nd[1];
                if(sortDirection1st == "asc" && sortDirection2nd == "asc")
                    companies = companies.OrderBy(c => c.GetType().GetProperty(sortColumn1st)?.GetValue(c, null) ??
                            c.ContactDetails.GetType().GetProperty(sortColumn1st)?.GetValue(c, null))
                        .ThenBy(c => c.GetType().GetProperty(sortColumn2nd)?.GetValue(c, null));
                else if (sortDirection1st == "desc" && sortDirection2nd == "asc")
                    companies = companies.OrderByDescending(c => c.GetType().GetProperty(sortColumn1st)?.GetValue(c, null) ??
                            c.ContactDetails.GetType().GetProperty(sortColumn1st)?.GetValue(c, null))
                        .ThenBy(c => c.GetType().GetProperty(sortColumn2nd)?.GetValue(c, null));
                else if (sortDirection1st == "asc" && sortDirection2nd == "desc")
                    companies = companies.OrderBy(c => c.GetType().GetProperty(sortColumn1st)?.GetValue(c, null) ??
                            c.ContactDetails.GetType().GetProperty(sortColumn1st)?.GetValue(c, null))
                        .ThenByDescending(c => c.GetType().GetProperty(sortColumn2nd)?.GetValue(c, null));
                else
                    companies = companies.OrderByDescending(c => c.GetType().GetProperty(sortColumn1st)?.GetValue(c, null) ??
                            c.ContactDetails.GetType().GetProperty(sortColumn1st)?.GetValue(c, null))
                        .ThenByDescending(c => c.GetType().GetProperty(sortColumn2nd)?.GetValue(c, null));
            }

            return Json(companies);
        }

        [HttpGet("ExportData")]
        public FileStreamResult ExportData()
        {
            List<Company> companies = _memoryStorage.GetAll<Company>().ToList();

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
    }
}
