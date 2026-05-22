using System;
using UnityEngine;

namespace Core
{
    public class ParentUtil : MonoBehaviour
    {
        [SerializeField] private Transform _newParentAfterStart;
        
        public void Awake()
        {
            this.transform.SetParent(_newParentAfterStart);
        }
    }
}
