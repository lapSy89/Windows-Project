using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptiLight.Command
{
    public interface IUndoRedo
    {
        void Execute();

        void UnExecute();
    }
}
