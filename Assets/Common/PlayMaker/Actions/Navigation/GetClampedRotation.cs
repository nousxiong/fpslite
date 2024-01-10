using System;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace HutongGames.PlayMaker.Actions
{
    /// <summary>
    /// 根据Paths信息，计算GameObject的转向范围
    /// </summary>
	[ActionCategory("Navigation")]
	public class GetClampedRotation : FsmStateAction
	{
        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;
        
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The current paths")]
        public FsmArray paths;

        /// <summary>
        /// 上一次的paths
        /// </summary>
        List<GameObject> prevPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前新加入的Paths
        /// </summary>
        List<GameObject> addedPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前移除的Paths
        /// </summary>
        List<GameObject> removedPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前保留的Paths
        /// </summary>
        List<GameObject> remainPaths = new List<GameObject>();
        /// <summary>
        /// 当前的Path，用于旋转基点计算
        /// </summary>
        GameObject currentPath;

        public override void Reset()
        {
            paths = null;
            prevPaths.Clear();
            addedPaths.Clear();
            removedPaths.Clear();
            remainPaths.Clear();
            currentPath = null;
        }

        public override void Init(FsmState state)
        {
            base.Init(state);
            prevPaths.Clear();
            addedPaths.Clear();
            removedPaths.Clear();
            remainPaths.Clear();
            currentPath = null;
        }

		// Code that runs on entering the state.
		public override void OnEnter()
        {
		}

		// Code that runs every frame.
		public override void OnUpdate()
        {
            DoGetClampedRotation();
        }

        void DoGetClampedRotation()
        {
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }
            
            var prevCount = prevPaths.Count;
            var newCount = paths.Length;
            var prevEmpty = prevCount == 0;
            var newEmpty = newCount == 0;

            remainPaths.Clear();
            addedPaths.Clear();
            removedPaths.Clear();
            
            // 找出和prevPaths不同的地方
            for (var i = 0; i < newCount; i++)
            {
                var path = paths.Get(i) as GameObject;
                if (prevPaths.Contains(path))
                {
                    // remain
                    remainPaths.Add(path);
                }
                else
                {
                    // added
                    addedPaths.Add(path);
                }
            }

            foreach (GameObject prevPath in prevPaths)
            {
                var id = -1;
                id = Array.IndexOf(paths.Values, prevPath);
                var contained = id != -1;
                if (!contained)
                {
                    // removed
                    removedPaths.Add(prevPath);
                }
            }
            
            var addedCount = addedPaths.Count;
            var removedCount = removedPaths.Count;
            var remainCount = remainPaths.Count;
            
            // save new paths
            prevPaths.Clear();
            for (var i = 0; i < newCount; i++)
            {
                var path = paths.Get(i) as GameObject;
                prevPaths.Add(path);
            }

            if (addedCount == 0 && removedCount == 0 && remainCount == prevCount && currentPath != null)
            {
                // no change
                return;
            }
            
            // 决定当前的Path
            var selfPath = currentPath == go;
            if (remainCount > 0)
            {
            }
        }


	}

}
