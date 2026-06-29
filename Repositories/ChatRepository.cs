using Entities;

namespace Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly dbSHOPContext _db;

        public ChatRepository(dbSHOPContext db)
        {
            _db = db;
        }
    }
}
