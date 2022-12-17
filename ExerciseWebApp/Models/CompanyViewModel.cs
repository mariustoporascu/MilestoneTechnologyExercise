using EntitiesLibrary;

namespace ExerciseWebApp.Models
{
    public class CompanyViewModel : Company
    {
        public CompanyViewModel(Company company)
        {
            CompanyName = company.CompanyName;
            YearsInBusiness = company.YearsInBusiness;
            ContactDetails = new Contact();
            ContactDetails.FullName = company.ContactDetails.FullName;
            ContactDetails.PhoneNumber = company.ContactDetails.PhoneNumber;
            ContactDetails.Email = company.ContactDetails.Email;
        }
    }
}
