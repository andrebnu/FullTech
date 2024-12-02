namespace BancoChu.API.Models
{
    public class Transferencia
    {

        public int Id { get; set; }
        public int ContaOrigemId { get; set; }
        public int ContaDestinoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransferencia { get; set; }
    }
}
