namespace BSXWashing.Models.DBClass
{
    public class DbInitializer
    {
        public static void Initialize(WashingContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
