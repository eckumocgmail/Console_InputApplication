using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
internal class MyOperationModel<In> 
    : Task<MethodResult<In>> where In : class
{
    public MyOperationModel(
        Func<object, MethodResult<In>> fx, 
        object state) : base(fx, state)
    {
    }

    public string Name { get; set; }
}
 
