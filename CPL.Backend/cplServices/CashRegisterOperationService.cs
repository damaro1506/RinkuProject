using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.BL;

namespace Cover.Backend.BL
{
    public class CashRegisterOperationService
    {
        #region "Properties"

        private CashRegisterOperationRepository _cashRegisterOperationRepository;
        private CashRegisterOperationRepository cashRegisterOperationRepository
        {
            get
            {
                if (_cashRegisterOperationRepository == null)
                    _cashRegisterOperationRepository = new CashRegisterOperationRepository();
                return _cashRegisterOperationRepository;
            }
            set
            {
                _cashRegisterOperationRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreateCashRegisterOperation(Decimal cashFund, DateTime operationDate)
        {
            var cashRegisterOperation = new CashRegisterOperation();
            cashRegisterOperation.CashFund = cashFund;
            cashRegisterOperation.OperationDate = operationDate;
            cashRegisterOperation.TerminalId = Convert.ToInt32(Context.Terminal.Id);
            cashRegisterOperation.UserId = Context.User.Id;
            cashRegisterOperation.Status = (int)CashRegisterOperationStatus.Open;
            cashRegisterOperationRepository.CreateCashRegisterOperation(cashRegisterOperation);
        }

        #endregion

        #region "Get_Events"

        public CashRegisterOperation GetLastOpenCashRegisterOperationByTerminal()
        {
            return cashRegisterOperationRepository.GetLastOpenCashRegisterOperationByTerminal();
        }

        public CashRegisterOperation GetOpenCashRegisterOperationByStatus()
        {
            return cashRegisterOperationRepository.GetOpenCashRegisterOperationByStatus();
        }

        public List<CashRegisterOperation> GetCashRegisterOperations()
        {
            return cashRegisterOperationRepository.GetCashRegisterOperations();
        }

        #endregion

        #region "Update_Events"

        public void UpdateCashOutRegisterOperation(CashRegisterOperation cashRegisterOperation)
        {
            cashRegisterOperationRepository.UpdateCashOutRegisterOperation(cashRegisterOperation);
        }

        #endregion
    }
}
