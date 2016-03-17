using System.Collections.Generic;
using SimpleInjectorCore.Models;

namespace SimpleInjectorCore.Core
{
    public class PessoaService : IPessoaService
    {
        public IEnumerable<Pessoa> Listar()
        {
            for (int i = 0; i < 10; i++)
                yield return new Pessoa(i, $"Pessoa - {i}");
        }
    }
}