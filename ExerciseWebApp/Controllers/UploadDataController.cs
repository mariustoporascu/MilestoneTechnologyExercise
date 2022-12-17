using DataParserLibrary;
using EntitiesLibrary;
using InMemoryStorageLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ExerciseWebApp.Controllers
{
	public class UploadDataController : Controller
	{
        public UploadDataController()
        {
        }

        public IActionResult Index() => View();

        [HttpPost("PostFilesAndConfiguration")]
        public async Task<IActionResult> PostFilesAndConfiguration([FromServices] IMemoryStorage memoryStorage)
        {
            if(Request.Form.Files.Count > 0)
            {
                for(int i = 0; i < Request.Form.Files.Count; i++)
                {
                    // Check if file extension is accepted
                    IFormFile file = Request.Form.Files[i];
                    var extension = Path.GetExtension(file.FileName);
                    if (!extension.ToLower().Equals(".txt")) continue;

                    // Read the file contents
                    List<string> fileContents = new List<string>();
                    using(var streamReader = new StreamReader(file.OpenReadStream()))
                    {
                        string line;
                        while((line = await streamReader.ReadLineAsync()) != null)
                            fileContents.Add(line);
                    }
                    if (fileContents.Count == 0) continue;

                    // Check the delimiter
                    bool formHasDelimiter = Request.Form.TryGetValue($"delimiter-{i}", out StringValues delimiterFormValue);
                    if (!formHasDelimiter) continue;
                    
                    char delimiter = delimiterFormValue[0].ToCharArray()[0];
                    CompanyParser companyParser = new CompanyParser();
                    if(!companyParser.IsDelimiterValid(delimiter)) continue;

                    // Get the headers names and indexes
                    bool formHasHeaderName = Request.Form.TryGetValue($"header-name-{i}", out StringValues headersNameFormValue);
                    bool formHasHeaderOrder = Request.Form.TryGetValue($"header-order-{i}", out StringValues headersOrderFormValue);
                    if(!formHasHeaderName || !formHasHeaderOrder) continue;
                    string[] headerNamesArray = headersNameFormValue.ToArray();
                    string[] headersOrderArray = headersOrderFormValue.ToArray();

                    Dictionary<int, string> indexAndHeaderNames = new Dictionary<int, string>();
                    for(int j = 0; j < headerNamesArray.Length; j++)
                    {
                        bool hasOrder = int.TryParse(headersOrderArray[j], out int headerIndex);
                        if(!hasOrder) continue;
                        indexAndHeaderNames[headerIndex - 1] = headerNamesArray[j];
                    }

                    // Parse and save data in memory
                    ParseAndSaveDataInMemory(memoryStorage, indexAndHeaderNames, delimiter, fileContents.ToArray(), companyParser);
                }
                
            }
            return RedirectToAction("Index");
        }
        [NonAction]
        private void ParseAndSaveDataInMemory(IMemoryStorage memoryStorage, Dictionary<int,string> indexAndHeaderNames, 
            char delimiter, string[] fileLines, CompanyParser companyParser)
        {
            foreach(var line in fileLines)
            {
                if (!companyParser.IsLineDataValid(line)) continue;
                Company company = companyParser.MapData(indexAndHeaderNames, companyParser.SplitLineData(delimiter,line));
                memoryStorage.Add(company);
            }
        }
    }
}
