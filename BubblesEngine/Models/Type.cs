using System.Collections.Generic;
using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class Type
    {
        public List<string> NodeIds { get; set; }
        public string TypeName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}