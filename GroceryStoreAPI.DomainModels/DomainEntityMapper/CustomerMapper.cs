namespace GroceryStoreAPI.DomainModels.DomainEntityMapper
{
    public static class CustomerMapper
    {
        public static DomainModels.Customer ToDomainModel(this Entities.Customer value)
        {
            return new DomainModels.Customer
            {
                ID = value.ID,
                Name = value.Name
            };
        }
    }
}
