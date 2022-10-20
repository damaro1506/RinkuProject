using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.ExceptionManagement;

namespace Cover.Backend.BL
{
    public class TerminalService
    {
        #region "Properties"

        private TerminalRepository _terminalRepository;
        private TerminalRepository terminalRepository
        {
            get
            {
                if (_terminalRepository == null)
                    _terminalRepository = new TerminalRepository();
                return _terminalRepository;
            }
            set
            {
                _terminalRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public Terminal GetTerminal(String name)
        {
            return terminalRepository.GetTerminal(name);
        }

        public List<Terminal> GetTerminals()
        {
            return terminalRepository.GetTerminals();
        }

        #endregion
    }
}
