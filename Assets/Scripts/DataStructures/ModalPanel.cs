using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.DataStructures
{
    public abstract class ModalPanel : MonoBehaviour
    {      
        public GameObject Panel;

        public Button SubmitButton;
        public Button CloseButton;

        public virtual void Submit() { }
        public virtual void Cancel() { }

        public virtual void Show()
        {            
            this.Panel.SetActive(true);
        }

        public virtual void Close()
        {
            this.Panel.SetActive(false);
        }

        public void OnEnable()
        {
            this.gameObject.transform.SetAsLastSibling();

            SubmitButton?.onClick?.RemoveAllListeners();
            CloseButton?.onClick?.RemoveAllListeners();

            SubmitButton?.onClick.AddListener(Submit);
            CloseButton?.onClick.AddListener(Cancel);
        }
    }
}
