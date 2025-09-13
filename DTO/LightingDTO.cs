using System.Collections.Generic;
using System.IO.Ports;

namespace ASTI_Vision.DTO
{
    public class ChannelDTO
    {
        public int Channel { get; set; }
        public int Intensity { get; set; }
        public bool IsOn { get; set; }
    }

    public class ExportDTO
    {
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }

        public List<ChannelDTO> Channels { get; set; } = new List<ChannelDTO>();
    }
}
