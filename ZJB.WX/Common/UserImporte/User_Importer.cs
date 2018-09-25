using ZJB.WX.Common.UserImporte.Models;

namespace ZJB.WX.Common.UserImporte
{
    public abstract class User_Importer
    {
        protected abstract string version();
        public abstract ImportedUser Run(string uname, string pwd);
    }
}