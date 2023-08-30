using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    /// <summary>
    /// Запрос на изменение свойств комнаты
    /// </summary>
    public class UpdateRoomQuery
    {
        public string UpdateName { get; }
        public int? UpdateArea { get; set; }
        public bool? UpdateGasConnected { get; set; }
        public int? UpdateVoltage { get; set; }


        public UpdateRoomQuery(string? newName, int? newArea, bool? newGasConnected, int? newVoltage)
        {
            UpdateName = newName;
            UpdateArea = newArea;
            UpdateGasConnected = newGasConnected;
            UpdateVoltage = newVoltage;
        }
    }
}
