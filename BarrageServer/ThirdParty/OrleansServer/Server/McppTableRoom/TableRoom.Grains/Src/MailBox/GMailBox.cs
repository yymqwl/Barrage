using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GMailBox : Grain , IMailBox
    {
        protected IMailBoxObserver m_IMailBoxObserver;

        public Task SendMessage(byte[] message)
        {
            if(m_IMailBoxObserver!=null)
            {
                m_IMailBoxObserver.Receive(message);
            }
            return Task.CompletedTask;
        }

        public Task SetObserver(IMailBoxObserver mbobser)
        {
            this.m_IMailBoxObserver = mbobser;
            return Task.CompletedTask;
        }

    }
}
