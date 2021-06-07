using Microsoft.EntityFrameworkCore.Metadata;
using GroceryStoreAPI.Data.DataProviders;

namespace GroceryStoreAPI.Data
{
    public static class GroceryStoreAPIDBContextExtensions
    {
        public static void HasData<T>(this GroceryStoreAPIDBContext context, params T[] data)
                 where T : class
        {

            var dbSet = context.Set<T>();
            var keyName = context.GetPrimaryKeyName(typeof(T).FullName);

            foreach (var item in data)
            {
                var keyValue = item.GetType().GetProperty(keyName).GetValue(item, null);
                var entity = dbSet.Find(keyValue);
                if (entity != null)
                {
                    context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }

                if (entity == null)
                {
                    context.Add(item);
                }
                else
                {
                    context.Update(item);
                }
            }
            context.SaveChanges();
        }

        public static string GetPrimaryKeyName(this GroceryStoreAPIDBContext context, string type)
        {
            IKey es = context.Model.FindEntityType(type).FindPrimaryKey();
            return es.Properties[0].Name;
        }
    }
}
