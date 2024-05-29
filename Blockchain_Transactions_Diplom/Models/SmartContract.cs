using System.ComponentModel.DataAnnotations;

namespace Blockchain_Transactions_Diplom.Models
{
    public class SmartContract
    {
        [Key]
        public string? ContractId { get; set; }
        public string? PublicKeyCreator { get; set; }
        public string? PublicKeyExecutor { get; set; }
        public string? IdExercise { get; set; }
        public ulong? ContractValue { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
