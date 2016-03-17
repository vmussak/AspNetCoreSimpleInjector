using System.Collections.Generic;
using SimpleInjectorCore.Models;

namespace SimpleInjectorCore.Core
{
    public interface IPessoaService
    {
        IEnumerable<Pessoa> Listar();
    }
}