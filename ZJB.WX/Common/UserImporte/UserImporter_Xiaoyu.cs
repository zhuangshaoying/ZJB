
using ZJB.WX.Common.UserImporte.Models;

namespace ZJB.WX.Common.UserImporte
{
    class UserImporter_Xiaoyu : User_Importer
    {
        protected override string version()
        {
            return "1.1";
        }

        public override ImportedUser Run(string uname, string pwd)
        {
            throw new System.NotImplementedException();
        }
    }
}
