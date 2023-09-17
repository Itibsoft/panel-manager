using System;
using System.Reflection;
using Assets.Project.Scripts.UI.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Project.Scripts.UI.Factories
{
    public class DefaultPanelControllerFactory : IPanelControllerFactory
    {
        public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);
            
            var panelPrefab = Resources.Load<PanelBase>(meta.AssetId);

            if (panelPrefab == default)
            {
                throw new Exception($"Not found asset in Resources for path: {meta.AssetId}");
            }

            var panel = Object.Instantiate(panelPrefab);
            
            var extraArguments = new object[]
            {
                panel
            };

            var controller = Activator.CreateInstance(type, extraArguments);

            return (TPanelController)controller;
        }
    }
}