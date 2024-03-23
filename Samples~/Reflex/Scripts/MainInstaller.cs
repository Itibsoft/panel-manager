using Itibsoft.PanelManager.External;
using Reflex.Core;
using UnityEngine;

namespace Itibsoft.PanelManager.Sample.Demo
{
    public class MainInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            PanelManagerInstaller.Install(containerBuilder);
        }
    }
}