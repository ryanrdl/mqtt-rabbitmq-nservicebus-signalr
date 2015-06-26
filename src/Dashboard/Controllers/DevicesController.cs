using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Messages;
using Messages.DeviceGateway;

namespace Dashboard.Controllers
{
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        [Route("temperature"), HttpPost]
        public async Task<string> AddTemperature()
        {
            AddTemperatureDevice command = new AddTemperatureDevice
            {
                DeviceId = Guid.NewGuid().ToString()
            };

            int result = await _.Bus.Send(command).Register();

            if (result == ResultCode.OK) return command.DeviceId;

            throw new HttpException((int) HttpStatusCode.BadRequest, "Unable to add device");
        }

        [Route("temperature"), HttpDelete]
        public void RemoveTemperature([FromUri] string deviceId)
        {
            _.Bus.Send(new RemoveTemperatureDevice {DeviceId = deviceId});
        }

        [Route("humidity"), HttpPost]
        public async Task<string> AddHumidity()
        {
            AddHumidityDevice command = new AddHumidityDevice
            {
                DeviceId = Guid.NewGuid().ToString()
            };

            int result = await _.Bus.Send(command).Register();

            if (result == ResultCode.OK) return command.DeviceId;

            throw new HttpException((int) HttpStatusCode.BadRequest, "Unable to add device");
        }

        [Route("humidity"), HttpDelete]
        public void RemoveHumidity([FromUri] string deviceId)
        {
            _.Bus.Send(new RemoveTemperatureDevice {DeviceId = deviceId});
        }
    }
}
