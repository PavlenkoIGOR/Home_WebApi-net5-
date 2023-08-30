using System;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Room в базе 
    /// </summary>
    public interface IRoomRepository
    {
        Task UpdateRoom(Room room, UpdateRoomQuery roomQuery);
        Task<Room> GetRoomById(Guid id);
        Task<Room> GetRoomByName(string name);
        Task AddRoom(Room room);
    }
}