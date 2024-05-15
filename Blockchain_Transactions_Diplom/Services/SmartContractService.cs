using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;

namespace Blockchain_Transactions_Diplom.Services
{
    public class SmartContractService : ISmartContractService
    {
        private readonly ISmartContractRepository _smartContractRepository;
        public SmartContractService(ISmartContractRepository smartContractRepository)
        {
            _smartContractRepository = smartContractRepository;
        }
    }
}
