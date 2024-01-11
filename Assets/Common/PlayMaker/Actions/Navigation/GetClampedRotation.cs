using System;
using System.Collections.Generic;
using Common.Utility;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnassignedField.Global

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
        [Tooltip("The fsm name of paths")]
        public FsmString pathsFsmName;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The var name of paths")]
        public FsmString pathsVarName;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the min angle of Y")]
        public FsmFloat minAngleY;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the max angle of Y")]
        public FsmFloat maxAngleY;

        // [UIHint(UIHint.Variable)]
        // [Tooltip("Store the offset angle of Y")]
        // public FsmFloat offsetAngleY;

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
        
        // temp, for reduce gc
        Vector3 dirXZ = Vector3.zero;
        Vector3 tmpDirXZ = Vector3.zero;

        // cache
        GameObject go;
        PlayMakerFSM pathsFsm;

        public override void Reset()
        {
            pathsFsmName = null;
            pathsVarName = null;
            prevPaths.Clear();
            ClearTempPaths();
            currentPath = null;
            go = null;
            pathsFsm = null;
        }

        public override void Init(FsmState state)
        {
            base.Init(state);
            prevPaths.Clear();
            ClearTempPaths();
            currentPath = null;
            
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            pathsFsm = ActionHelpers.GetGameObjectFsm(go, pathsVarName.Value)!;
        }

        void ClearTempPaths()
        {
            addedPaths.Clear();
            removedPaths.Clear();
            remainPaths.Clear();
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
            FsmArray paths = pathsFsm.FsmVariables.GetFsmArray(pathsVarName.Value);
            
            var prevCount = prevPaths.Count;
            var newCount = paths.Length;

            ClearTempPaths();
            
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
                ClearTempPaths();
                return;
            }
            
            // 决定当前的Path
            Vector3 dir = go.transform.forward;
            dir.ProjectionXZ(out dirXZ);
            GameObject newPath = null;
            if (remainCount > 0)
            {
                newPath = GetNearestParallelPath(dirXZ, remainPaths);
            }

            if (newPath == null && addedCount > 0)
            {
                newPath = GetNearestParallelPath(dirXZ, addedPaths);
            }

            if (newPath == null)
            {
                // 如果不存在path、旧path也不存在，选择自己
                newPath = currentPath == null ? go : currentPath;
            }
            currentPath = newPath;

            // 当前Path作为offsetAngleY
            // offsetAngleY.Value = currentPath.transform.localEulerAngles.y;
            
            // 找出最大夹角范围的2个Paths作为min和max
            if (prevPaths.Count <= 1)
            {
                var offsetAngleY = currentPath.transform.localEulerAngles.y;
                minAngleY.Value = offsetAngleY;
                maxAngleY.Value = offsetAngleY;
            }
            else
            {
                // > 1，给所有Paths排序，将currentPath的前后2个path的Y旋转角度取出，作为min和max
                prevPaths.Sort((a, b) =>
                {
                    var aY = a.transform.localEulerAngles.y;
                    var bY = b.transform.localEulerAngles.y;
                    return aY.CompareTo(bY);
                });
                var index = prevPaths.IndexOf(currentPath);
                GameObject prev = null;
                GameObject next = null;
                if (index == 0)
                {
                    prev = prevPaths[^1];
                    next = prevPaths[1];
                }
                else if (index == prevPaths.Count - 1)
                {
                    prev = prevPaths[index - 1];
                    next = prevPaths[0];
                }
                else
                {
                    prev = prevPaths[index - 1];
                    next = prevPaths[index + 1];
                }
            }
            
            
            ClearTempPaths();
        }

        GameObject GetNearestParallelPath(Vector3 dir, List<GameObject> pathList)
        {
            GameObject path = null;
            var angle = 0f;
            foreach (GameObject remainPath in pathList)
            {
                remainPath.transform.forward.ProjectionXZ(out tmpDirXZ);
                var acuteAngle = Vector3Utility.AcuteAngle(tmpDirXZ, dir);
                if (acuteAngle < angle)
                {
                    angle = acuteAngle;
                    path = remainPath;
                }
            }
            return path;
        }

	}

}
