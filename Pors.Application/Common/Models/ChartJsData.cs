using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pors.Application.Common.Models
{
    public class ChartJsData
    {
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }

        [JsonPropertyName("datasets")]
        public List<ChartJsDataDataset> DataSets { get; set; }
    }

    public class ChartJsDataDataset
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("stack")]
        public string Stack { get; set; }

        [JsonPropertyName("data")]
        public List<int> Data { get; set; } = new();
    }
}