using System;
using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Services.Infrastructure.Annotations
{
    [MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.Itself)]
    public sealed class CreatedImplicitlyAttribute : Attribute { }
}