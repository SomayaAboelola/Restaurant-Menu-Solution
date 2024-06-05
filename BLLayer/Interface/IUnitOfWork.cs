using DALayer.Entities;

namespace BLLayer.Interface
{
    public interface IUnitOfWork
    {
        
        IMenuRepository MenuRepository { get; set; }
        ICategoryRepository  CategoryRepository { get; set; }   
        Task<int> CompleteAsync();
        void Dispose();
}

  
}
