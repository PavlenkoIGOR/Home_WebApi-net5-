using System;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace HomeApi.Data
{
    public sealed class HomeApiContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        
        public HomeApiContext(DbContextOptions<HomeApiContext> options)  : base(options)
        {
            Database.EnsureCreated(); //Проверка состояния на Detached необходима для того, чтобы избежать повторного добавления объекта в контекст базы данных.
                                      //Если объект уже отслеживается контекстом, то повторное добавление может привести к ошибкам или непредвиденному поведению.
                                      //Поэтому перед добавлением объекта в контекст проверяется его состояние. Если объект не отслеживается, то он добавляется в контекст.
                                      //В противном случае ничего не происходит.
        }

        //Этот метод вызывается, если модель для производного контекста была инициализирована, прежде чем модель была заблокирована и использована для инициализации контекста.
        //Реализация этого метода по умолчанию не делает ничего, но его можно переопределить в производном классе и выполнить в нем дальнейшую настройку модели перед ее блокировкой.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().ToTable("Rooms");
            builder.Entity<Device>().ToTable("Devices");
        }
    }
}