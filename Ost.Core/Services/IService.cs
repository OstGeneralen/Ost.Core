using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ost.Core.Services
{
    public interface IService
    {
        string ReadableName { get; }
        Type ServiceType { get; }
    }

    public interface IService<T> : IService where T: IService<T>
    {
        string IService.ReadableName { get => ServiceType.Name; }
        Type IService.ServiceType { get => typeof(T); }
    }
}
