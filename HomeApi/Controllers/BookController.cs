using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Validations.Rules;
using System.IO;
using System.Linq;

//класс для примера загрузки файла пользователем из нета
namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private IHostEnvironment _environment;
        public BookController(IHostEnvironment hostEnvironment)
        {
            _environment = hostEnvironment;
        }

        [HttpGet] //в swaggere будут раздел [get]: /Book/{manufacturer111}
        [HttpHead] //в swaggere будут раздел [head]: /Book/{manufacturer111}
        [Route("{manufacturer111}")] //в {}-скобках это то, что эквивалентно placeholder в HTML. Если отличаются имена в [Route("{моёИмя}")] и в IActionResult Get([FromRoute] string моёИмя) то будет два поля для ввода
        public IActionResult GetBook([FromRoute] string manufacturer111)
        {
            //далее логика для загрузки файла из ресурса (для этого передается в текущий класс IHostEnvironment)

            //получаем корневую директорию через ContentRootPath:
            string pathWWWROOT = Path.Combine(_environment.ContentRootPath, "wwwroot", "filesForDownload");
            string filePath = Directory.GetFiles(pathWWWROOT)
                .FirstOrDefault(f => f.Split("\\")
                .Last()
                .Split('.')[0] == manufacturer111); //-как в js вывод первого элемента массива

            if (string.IsNullOrEmpty(filePath))
            {
                return StatusCode(404, $"Файл {manufacturer111} не найден!");
            }

            //хорошая практика - показ свойств объекта в ответах, чтообы клиент мог их прочитать.
            string fileType = "aplication/pdf"; //т.к. передаем pdf, что бы клиентское приложение могло адекватно отображать файл
            string fileName = $"{manufacturer111}.pdf"; //имя файла с которым автоматически сохранится файл.

            //чтобы вернуть именно файл - return PhysicalFile
            return PhysicalFile(filePath, fileType, fileName);
        }
    }
}
