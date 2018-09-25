using System.Runtime.Serialization;

namespace ZJB.WX.Models
{
    [DataContract]
    public class CellResponse
    {
        public CellResponse(dynamic response)
        {

            cells = response;
          
        }
        public CellResponse()
        {
        }

        [DataMember(Name = "cells")]
        public dynamic cells { get; set; }

    }
}
