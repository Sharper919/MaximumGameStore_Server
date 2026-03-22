//using MaximumGameStore.Data;
//using MaximumGameStore.Models;
//using MaximumGameStore.Services.Interfaces;

//namespace MaximumGameStore.Services.GameDetailsServices
//{
//    public class PublisherService : IGameFeatureService
//    {
//        private readonly MaximumGameStoreContext _context;

//        public PublisherService(MaximumGameStoreContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> Create(string name)
//        {
//            var entity = new Publisher
//            {
//                Name = name
//            };

//            _context.Publishers.Add(entity);
//            await _context.SaveChangesAsync();

//            return entity.Id;
//        }

//        public async Task<bool> Delete(int Id)
//        {
//            var entity = await _context.Publishers.FindAsync(Id);

//            if (entity == null) return false;

//            _context.Publishers.Remove(entity);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<bool> Update(int Id, string name)
//        {
//            var entity = await _context.Publishers.FindAsync(Id);

//            if (entity == null) return false;

//            entity.Name = name;
//            await _context.SaveChangesAsync();

//            return true;
//        }
//    }
//}
