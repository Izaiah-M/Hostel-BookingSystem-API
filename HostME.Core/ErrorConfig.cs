using Newtonsoft.Json;

namespace HostME.Core
{
    public class ErrorConfig
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
