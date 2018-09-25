using System.Runtime.Serialization;

namespace ZJB.Opportal.Models
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
