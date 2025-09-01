using BlogApp.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
    }
}
