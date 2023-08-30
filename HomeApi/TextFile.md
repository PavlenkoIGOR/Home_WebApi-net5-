﻿﻿В нашем API будет две основных сущности — это электронные устройства (чайники, микроволновки, и т.д.) и комнаты, где эти устройства будут подключаться (ванная, кухня, гостиная и т.д. )
﻿﻿# создание проекта:
---
Добавлен    JSON-файл "HomeOptions.json" (настройки дома).

Добавлена   папка для моделей конфигурации "Configuration".

//////////Использование конфигурации
Допустим нам нужно показывать пользователю информацию о его доме на соответствующей информационной странице (как в обычном MVC-приложении):

Добавлен   класс "HomeControllers.cs" в папку "Controllers". Сделаем страницу доступной по определенному URL, который будет обслуживать новый контроллер HomeController.cs.

//////////Внедрение конфигурации
Чтобы получить возможность использовать объект конфигурации здесь, нам пришлось подключить сервис IOptions в Startup.cs:

`private IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("HomeOptions.json").Build();`
в Main:
```C#
builder.Services.Configure<HomeOptions>(Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
{
    Title = "SmartHome_ASPNetCore_WebApi_6.0",
    Version = "v1"
}));
            //надо обязательно добавлять:
app.UseRouting();
            //это тоже обязательно добавить
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
```


Доавлена	папка  Маршрут для контроллера задан атрибутами:
```C#
[ApiController]
   [Route("[controller]")] //
```
Добавлена   папка "Contracts". В ней будут классы для моделей запросов/ответов. И для этого применим Mapper (Nuget: AutoMapper.Extensions.Microsoft.Dependency.Ejections).
            Добавлен класс MappingProfile.cs. Для получения объекта маппера в контроллерах и других сервисах нашего приложения, сначала зарегистрируем его в Startup.cs:
```C#
// Подключаем автомаппинг
var assembly = Assembly.GetAssembly(typeof(MappingProfile));
services.AddAutoMapper(assembly);
```
           Передадим и используем сервис маппинга в нашем контроллер HomeController.cs. Свойство должно совпадать по имени, иначе понадобится дополнительная конфигурация. К примеру, если бы у нас имена всех свойств, нуждающихся в маппинге, в обоих классах бы совпадали, то класс, содержащий конфигурацию маппера у нас содержал бы следующую строчку:
```C#
CreateMap<HomeOptions, InfoResponse>();
```
Но при несовпадении, например, если бы у нас свойства назывались по разному (Address — в HomeOptions, и AddressInfo — в InfoResponse), то понадобилось бы сделать дополнительно такую настройку:
```C#
CreateMap<HomeOptions, InfoResponse>()
   .ForMember(m => m.AddressInfo,
       opt => opt.MapFrom(src => src.Address));
```

Добавлен    контроллер "DevicesController.cs", в котором и будут располагаться методы управления устройствами.

Добавлен    класс "AddDeviceRequest.cs" - модель запроса на добавление нового устройства в папке "Contracts/devices".

**Валидация**
Добавлен    параметр [Required] в класс "AddDeviceRequest.cs".

Добавлен    для примера объект "ModelState" в метод "Add()" в DevicesController.cs. (ручная валидация какого-то параметра Device).

Добавлен    новый солюшн "SmartHome.Contracts". Создать новый проект по шаблону Class Library внутри нашего солюшена, назвав его 
            SmartHome.Contracts. Таким образом мы уберали модели запросов и ответов в отдельную сборку, 
            чтобы не перегружать проект. Позже в эту сборку мы также добавим и классы-валидаторы.

Добавлен    класс "AddDeviceRequestValidator.cs" с nuget "FluentValidation.DependencyEjection.Extensions". Dся основная логика валидации происходит в специальном
            классе-валидаторе.

Добавлено	в Startup.cs подключение валидации ```services.AddFluentValidation( fv =>  fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>() );``` и поэтому подчищен класс DeviceController.

---
# **Основные моменты, на которые стоит обратить внимание:**
```C#
1. AddControllers() — подключает только контроллеры без представлений
```
```C#
2. AddSwaggerGen() — позволяет использовать Swagger — очень удобный инструмент для тестирования и документирования Web API
```
```C#
3. Поскольку Web API — приложение без пользовательского интерфейса, то тестирование его при разработке сопряжено с рядом трудностей.
```
        Придётся либо писать автотесты, либо использовать специальные программы-клиенты (например, Postman, который мы рассмотрим позже).

        Но есть и третий способ — сразу «встроить» в ваше разрабатываемое API тестовый графический клиент, написанный на JavaScript. Примером такого клиента является Swagger, и он позволяет не только тестировать Web API прямо из браузера, но и отображать разработчикам документацию к каждому методу и модели.

        Swagger тоже обязательно рассмотрим далее
  ```C# endpoints.MapControllers()``` — используется для автоматического сопоставления маршрутов с контроллерами. Теперь нам нет необходимости вручную определять маршруты.
  ```C#
IConfiguration Configuration — зависит от конфигурации прилложения (строка ["ASPNETCORE_ENVIRONMENT": "Development"] в launchSettings.json).
```
```C#
IOptions — для передачи прилоэению конфигурации через IOptions. Использование оправдоно, когда:
-много первоначальных настроек для запуска приложения
```
При создании JSON-файла с конфигурациями его надо подключать в Program или Sturtup (в зависимости от версии .Net) через  строку : 
```C#
private static IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("JSON/HomeOptions.json").Build();
```
затем добавить в builder ```builder.Services.Configure<HomeOptions>(Configuration);```(для .Net6) или  
```C#
public void ConfigureServices(IServiceCollection services) 
{
    service.Configure<[класс - конфигуратор(в нем поля, которые нужны из JSON)]>(Configuration);
``` 
для .Net5


### **Как подключается IOption'ы:**

-сначала в Startup / Program (в зависимости от версии .Net) записываются все парамтре из JSON-файлов [```private static IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("JSON/HomeOptions.json").Build();```];
-затем добавить в builder ```builder.Services.Configure<HomeOptions>(Configuration);```(для .Net6) или  
```C#
public void ConfigureServices(IServiceCollection services) 
{
    service.Configure<[класс - конфигуратор(в нем поля, которые нужны из JSON)]>(Configuration); (для .Net5)
```
-и уже в контроллерах сделать инжект ```IOptions``` (т.е. через конструктор передать в наш контроллер);

Также можно при необходимости переопределять те или иные настройки, полученные из JSON-файла, следующим образом (для .Net6, для .Net5 в методе ConfigureService()):
```C#
// Добавляем новый сервис
builder.services.Configure<HomeOptions>(Configuration);
builder.services.Configure<HomeOptions>(opt => 
{
  opt.Area = 120;
});
```
Есть возможность загружать из JSON-файла не весь объект целиком, а лишь отдельные секции. Например, чтобы загрузить только адрес, можем сделать так:
```C#
// Загружаем только "адрес" (вложенный Json-объект) 
builder.services.Configure<Address>(Configuration.GetSection("Address")); чето не работает
```

про валидацию:
если пользователь будет явно ошибаться в формате, запрос не пройдет, и мы увидим некоторое сообщение об 
ошибке. 
-Можно сделать проверку в контроллере, и возвращать клиенту информативный код ошибки:
```C#
if (request.CurrentVolts < 120){
	      return StatusCode(403, $"Устройства с напряжением меньше 120 вольт не поддерживаются!");
}
```
-Но лучше использовать встроенный механизм валидации на атрибутах (добавить "[Required]"-обязательный параметр для заполнения или для проверки 
условия "[Range(120, 220, ErrorMessage = "Поддерживаются устройства с напряжением от {1} до {2} вольт")]" совместно с [Required] над нужным полем в классе).

Кстати, если уже есть валидация на атрибутах, но для какого-либо из свойств дополнительно хотите добавить ручную валидацию — правильным будет возвращать текст с ошибками
в том же формате, в каком он возвращается при валидации на атрибутах. Сделать это можно, используя объект ModelState.