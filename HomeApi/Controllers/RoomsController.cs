using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using HomeApi.Data.Queries;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _rooms;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _rooms = repository;
            _mapper = mapper;
        }

        

        /// <summary>
        /// Обновление данных комнаты
        /// </summary>
        [HttpPut]
        [Route("{guid}")]
        public async Task<IActionResult> UpdateRooms([FromRoute] Guid guid,

            [FromBody] EditRoomRequest request)
        {
            var roomToEdit = await _rooms.GetRoomById(guid);
            if (roomToEdit == null)
                return StatusCode(400, $"Ошибка: Комната {guid} не существует.");

            UpdateRoomQuery query = new UpdateRoomQuery(
                request.Name,
                request.Area,
                request.GasConnected,
                request.Voltage
                );
            await _rooms.UpdateRoom(roomToEdit, query);
            return StatusCode(201, $"Комната {guid} изменена!");
        }

        
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _rooms.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _rooms.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
    }
}