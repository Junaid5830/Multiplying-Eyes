using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Models;
using ME.DAL;
namespace ME
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;
        MultiplyingEyesEntities context = new MultiplyingEyesEntities();

        public AppDAL _appDal;
        public HomeDAL _homeDal;

        #region App section
        public AppDAL AppDAL
        {
            get
            {
                return this._appDal ?? new AppDAL(context);
            }
        }
        #endregion

        #region Home section
        public HomeDAL HomeDAL
        {
            get
            {
                return this._homeDal ?? new HomeDAL(context);
            }
        }
        #endregion


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}