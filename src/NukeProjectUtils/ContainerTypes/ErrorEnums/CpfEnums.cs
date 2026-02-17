using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NukeProjectUtils.ContainerTypes.ErrorEnums;

public enum CpfCreateError
{
    NullOrEmpty = 0,
    InvalidFormat = 1,
    NotInTheRange = 2,
    KnowInvalidCpf = 4,
    MathematicallyInvalid = 5,
}