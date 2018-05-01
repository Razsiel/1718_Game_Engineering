using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PresentableScriptableObject : ScriptableObject, IPresentable {
        [SerializeField] private List<PresentableConfiguration> _presentableConfigurations;
        [HideInInspector] public List<PresentableConfiguration> PresentableConfigurations => _presentableConfigurations;

        public virtual GameObject Present(Transform parent, bool hideInHierarchy = false, string name = "") {
            return PresentablePresenter.Present(this, parent, hideInHierarchy, name);
        }
    }
}
