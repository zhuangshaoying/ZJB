using System;

namespace ZJB.Web.SiteGuard
{
    public class SiteGuardException:Exception
    {
        public SiteGuardException()
            : base()
        {
        }
        public SiteGuardException(string message)
            : base(message)
        {
        }
    }
}
