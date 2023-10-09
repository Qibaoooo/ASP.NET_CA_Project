namespace ASP.NET_CA_Project.Middleware
{
    public class PurchasedOrderCheck
    {
        private RequestDelegate next;

        public PurchasedOrderCheck(RequestDelegate next){
            this.next = next;
        }

        public async Task Invoke(HttpContext context) {
            ISession session = context.Session;
            await next(context);
        }
    }
}
