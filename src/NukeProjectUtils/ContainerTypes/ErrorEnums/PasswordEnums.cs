using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NukeProjectUtils.ContainerTypes.ErrorEnums;

public enum PasswordCreateError
{
    DoesNotMeetTheRequirements = 0,
    ThePasswordDoesntMatch = 1
}
