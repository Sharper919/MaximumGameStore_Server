//using MaximumGameStore.Data;
//using MaximumGameStore.Models;
//using MaximumGameStore.Services.Interfaces;

//namespace MaximumGameStore.Services
//{
//    public class ModeService : IGameFeatureService
//    {
//        private readonly MaximumGameStoreContext _context;

//        public ModeService(MaximumGameStoreContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> Create(string name)
//        {
//            var entity = new Mode
//            {
//                Name = name
//            };

//            _context.Modes.Add(entity);
//            await _context.SaveChangesAsync();

//            return entity.Id;
//        }

//        public async Task<bool> Delete(int Id)
//        {
//            var entity = await _context.Modes.FindAsync(Id);

//            if (entity == null) return false;

//            _context.Modes.Remove(entity);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<bool> Update(int Id, string name)
//        {
//            var entity = await _context.Modes.FindAsync(Id);

//            if (entity == null) return false;

//            entity.Name = name;
//            await _context.SaveChangesAsync();

//            return true;
//        }
//    }
//}
