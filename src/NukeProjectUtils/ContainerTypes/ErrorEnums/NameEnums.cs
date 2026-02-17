using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NukeProjectUtils.ContainerTypes.ErrorEnums;

public enum NameCreationError
{
    NullOrEmpty = 0,
    NameExedesLength = 1,
    NameTooShort = 2,
    InvalidCharacters = 3,
}