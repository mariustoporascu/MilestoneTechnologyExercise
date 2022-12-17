using DataParserLibrary;
using EntitiesLibrary;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsLibrary
{
	public class CompanyTests
	{
        private readonly Company _company;
        private readonly Company _companyCompare;
        public CompanyTests()
        {
            _company = new Company
            {
                CompanyName = "Creative Name Corp.",
                YearFounded = 2000,
                ContactDetails = new Contact
                {
                    Email = "bstone@rapidadvance.info",
                    FirstName = "Bob",
                    LastName = "Stone",
                    PhoneNumber = "+13016674477"
                }
            };
            _companyCompare = new Company
            {
                CompanyName = "Company Y Corporation",
                YearsInBusiness = 19,
                ContactDetails = new Contact
                {
                    Email = "jjones@rapidadvance.info",
                    FullName = "Jane Jones",
                    PhoneNumber = "(301) 222-1234"
                }
            };
        }
        [Fact]
        public void Equals_ShouldNotBeEqual_WhenCompanyNameIsTheSame()
        {

            // Act
            bool result = _company.Equals(_companyCompare);

            // Assert
            result.Should().NotBe(true);
        }
    }
}
