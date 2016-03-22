
using DataAccess.Contexts;
using Mine = Repository.Entities.Mine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public class ResourceManager : IDisposable
    {
        protected BlockchainEntities ctx = new BlockchainEntities();

        public Repository<Mine, BlockchainEntities> Mine { get; set; }

        public ResourceManager()
        {
            Mine = new Repository<Mine, BlockchainEntities>(ctx);
        }

        #region IDisposable
        private bool disposedValue = false;

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    if (Mine != null)
                    {
                        Mine.Dispose();
                        Mine = null;
                    }
                    
                    if (ctx != null)
                    {
                        ctx.Dispose();
                        ctx = null;
                    }                   
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        ~ResourceManager()
        {
            Dispose(false);
        }
        #endregion
    }
}
