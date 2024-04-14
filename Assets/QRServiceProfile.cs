
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace WalkLens_Lab
{
    [CreateAssetMenu(menuName = "QRServiceProfile", fileName = "QRServiceProfile", order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class QRServiceProfile : BaseServiceProfile<IServiceModule>
    { }
}
