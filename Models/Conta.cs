using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BancoChu.API.Models
{
    public class Conta
    {
        [Key] 
        [JsonIgnore]
        public int Id { get; set; }
        public string Titular { get; set; }

        public string NumeroConta { get; set; }
        public decimal Saldo { get; set; }
    }
}
