using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;
using TestProject;

namespace TestProject1
{
    class UserRepositoryIntegrationTest
    {
        public class UserRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
        {
            private readonly dbSHOPContext _dbContext;
            private readonly UserRepository _userRepository;
            public UserRepositoryIntegrationTests(DatabaseFixture databaseFixture)
            {
                _dbContext = databaseFixture.Context;
                _userRepository = new UserRepository(_dbContext);
            }

        }
    }
}
