using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace BarrageSilo
{
    public class UserIdBv : ABehaviour
    {
        protected long m_Id;

        public long Id
        {
            get
            {
                return m_Id;
            }
        }
        public UserIdBv(long id)
        {
            m_Id = id;
        }
    }
}
