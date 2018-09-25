using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Common
{
    public class CacheItemConstant
    {
        public const string UserModelItem = "GetCredential_ByUserID_{0}";

        public const string UserCredentialItem = "GetCredential_ByToken_{0}";

        public const string LoupanModelItem = "GetLoupanModelByLoupanId_{0}";

        public const string LoupanCityListItem = "GetCityList";

        public const string AllCityList = "GetAllCity";

        public const string DynamicImageListItem = "GetDynamicImageList_{0}";

        public const string DynamicSupportItem = "GetDynamicSupport_d{0}_u{1}";

        public const string DynamicSupportListPgItem = "GetDynamicSupportList_d{0}_pi{1}_ps{2}";

        public const string ChatNotepadItem = "ChatNotepadItem_Id{0}";
    }
}
