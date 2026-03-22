//using MaximumGameStore.Data;
//using MaximumGameStore.Models;
//using MaximumGameStore.Services.Interfaces;

//namespace MaximumGameStore.Services
//{
//    public class DeveloperService : IGameFeatureService
//    {
//        private readonly MaximumGameStoreContext _context;

//        public DeveloperService(MaximumGameStoreContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> Create(string name)
//        {
//            //if (_context.Developers.Any(d => d.Name == name)) return null;

//            var entity = new Developer
//            {
//                Name = name
//            };

//            _context.Developers.Add(entity);
//            await _context.SaveChangesAsync();

//            return entity.Id;
//        }

//        public async Task<bool> Delete(int Id)
//        {
//            var entity = await _context.Developers.FindAsync(Id);

//            if (entity == null) return false;

//            _context.Developers.Remove(entity);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<bool> Update(int Id, string name)
//        {
//            var entity = await _context.Developers.FindAsync(Id);

//            if (entity == null) return false;

//            entity.Name = name;
//            await _context.SaveChangesAsync();

//            return true;
//        }
//    }
//}
