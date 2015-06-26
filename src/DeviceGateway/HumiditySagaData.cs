using System;
using System.Collections.Generic;
using NServiceBus.Saga;

namespace DeviceGateway
{
    public class HumiditySagaData : IContainSagaData
    {
        private List<int> _data = new List<int>();
 
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
         
        public string DeviceId { get; set; }

        public List<int> DataPoints
        {
            get { return _data; }
            set { _data = value; }
        } 
    }
}