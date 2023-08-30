using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        ///<summary>
        ///обновление даных комнаты
        ///<summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery roomQuery)
        {
            // Если в запрос переданы параметры для обновления - проверяем их на null
            // И если нужно - обновляем устройство
            //if (!string.IsNullOrEmpty(query.NewName))
            room.Name = string.IsNullOrEmpty(roomQuery.UpdateName) ? room.Name : roomQuery.UpdateName;
            room.Area = roomQuery.UpdateArea ?? room.Area;
            room.GasConnected = roomQuery.UpdateGasConnected ?? room.GasConnected;
            room.Voltage = roomQuery.UpdateVoltage ?? room.Voltage;

            // Добавляем в базу 
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Найти комнату по id
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }
    }
}