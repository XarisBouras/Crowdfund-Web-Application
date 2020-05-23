using Crowdfund.Core.Data;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbCtx = new DataContext();
            
            dbCtx.Dispose();
        }
    }
}