using EntitiesLibrary;
using FluentAssertions;
using InMemoryStorageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsLibrary
{
    public class InMemoryStorageTests
    {
        private readonly InMemoryStorage _storage;
        private readonly Company _company;
        private readonly Company _companyCompare;

        public InMemoryStorageTests()
        {
            _storage = new InMemoryStorage();
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
        public void Add_CanAddCompanyToStorage()
        {
            // Act
            _storage.Add(_company);

            // Assert
            _storage.GetAll<Company>().Should().Contain(_company);
            _storage.GetAll<Company>().Should().NotContain(_companyCompare);
        }

        [Fact]
        public void Remove_CanRemoveCompanyFromStorage()
        {
            // Arrange
            _storage.Add(_company);
            _storage.Add(_companyCompare);
            // Act
            _storage.Remove(_company);

            // Assert
            _storage.GetAll<Company>().Should().NotContain(_company);
            _storage.GetAll<Company>().Should().Contain(_companyCompare);
        }

        [Fact]
        public void GetAll_CanGetAllCompaniesFromStorage()
        {
            // Arrange
            _storage.Add(_company);
            _storage.Add(_companyCompare);

            var expected = new[] { _company, _companyCompare };

            // Act
            var result = _storage.GetAll<Company>();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
