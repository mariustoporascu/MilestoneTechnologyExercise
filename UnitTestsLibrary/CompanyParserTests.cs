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
    public class CompanyParserTests
    {
        [Fact]
        public void MapData_ShouldMapLineDataToCompanyObject_HyphenDelimiter()
        {
            // Arrange
            string lineData = "Creative Name Corp.-2000-+13016674477-bstone@rapidadvance.info-Bob-Stone";
            char delimiter = '-';
            Dictionary<int, string> indexAndHeaderNames = new Dictionary<int, string>
            {
                { 0, "CompanyName" },
                { 1, "YearFounded" },
                { 2, "PhoneNumber" },
                { 3, "Email" },
                { 4, "FirstName" },
                { 5, "LastName" }
            };
            CompanyParser parser = new CompanyParser();

            // Act
            Company result = parser.MapData(indexAndHeaderNames, parser.SplitLineData(delimiter, lineData));

            // Assert
            result.Should().BeEquivalentTo(new Company
            {
                CompanyName = "Creative Name Corp.",
                YearFounded = 2000,
                YearsInBusiness = null,
                ContactDetails = new Contact
                {
                    Email = "bstone@rapidadvance.info",
                    FirstName = "Bob",
                    LastName = "Stone",
                    PhoneNumber = "+13016674477"
                }
            });
        }
        [Fact]
        public void MapData_ShouldMapLineDataToCompanyObject_HashDelimiter()
        {
            // Arrange
            string lineData = "Company 12 AB, LLC #1978#Oliver Washington#+1 (301)667-1244";
            char delimiter = '#';
            Dictionary<int, string> indexAndHeaderNames = new Dictionary<int, string>
            {
                { 0, "CompanyName" },
                { 1, "YearFounded" },
                { 2, "FullName" },
                { 3, "PhoneNumber" },
                { 4, "LastName" }
            };
            CompanyParser parser = new CompanyParser();

            // Act
            Company result = parser.MapData(indexAndHeaderNames, parser.SplitLineData(delimiter, lineData));

            // Assert
            result.Should().BeEquivalentTo(new Company
            {
                CompanyName = "Company 12 AB, LLC",
                YearFounded = 1978,
                YearsInBusiness = null,
                ContactDetails = new Contact
                {
                    FullName = "Oliver Washington",
                    PhoneNumber = "+1 (301)667-1244"
                }
            });
        }
        [Fact]
        public void MapData_ShouldMapLineDataToCompanyObject_CommaDelimiter()
        {
            // Arrange
            string lineData = "Company Y Corporation, Jane Jones, (301) 222-1234, 19, jjones@rapidadvance.info";
            char delimiter = ',';
            Dictionary<int, string> indexAndHeaderNames = new Dictionary<int, string>
            {
                { 0, "CompanyName" },
                { 1, "FullName" },
                { 2, "PhoneNumber" },
                { 3, "YearsInBusiness" },
                { 4, "Email" }
            };
            CompanyParser parser = new CompanyParser();

            // Act
            Company result = parser.MapData(indexAndHeaderNames, parser.SplitLineData(delimiter, lineData));

            // Assert
            result.Should().BeEquivalentTo(new Company
            {
                CompanyName = "Company Y Corporation",
                YearFounded = null,
                YearsInBusiness = 19,
                ContactDetails = new Contact
                {
                    Email = "jjones@rapidadvance.info",
                    FullName = "Jane Jones",
                    PhoneNumber = "(301) 222-1234"
                }
            });
        }
    }
}
