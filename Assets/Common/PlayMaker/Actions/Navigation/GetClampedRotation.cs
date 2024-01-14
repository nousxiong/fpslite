using System;
using System.Collections.Generic;
using Common.Utility;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

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
        [Tooltip("The fsm name of paths")]
        public FsmString pathsFsmName;

        [RequiredField]
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
        readonly List<GameObject> prevPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前新加入的Paths
        /// </summary>
        readonly List<GameObject> addedPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前移除的Paths
        /// </summary>
        readonly List<GameObject> removedPaths = new List<GameObject>();
        /// <summary>
        /// 上一次到当前保留的Paths
        /// </summary>
        readonly List<GameObject> remainPaths = new List<GameObject>();
        readonly List<GameObject> currentPaths = new List<GameObject>();
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

        // public override void Init(FsmState state)
        // {
        //     base.Init(state);
        // }

        void ClearTempPaths()
        {
            addedPaths.Clear();
            removedPaths.Clear();
            remainPaths.Clear();
        }

		// Code that runs on entering the state.
		public override void OnEnter()
        {
            prevPaths.Clear();
            currentPaths.Clear();
            ClearTempPaths();
            currentPath = null;
            
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            pathsFsm = ActionHelpers.GetGameObjectFsm(go, pathsFsmName.Value)!;
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
                var id = Array.IndexOf(paths.Values, prevPath);
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
            currentPaths.Clear();
            for (var i = 0; i < newCount; i++)
            {
                var path = paths.Get(i) as GameObject;
                prevPaths.Add(path);
                currentPaths.Add(path);
            }

            if (addedCount == 0 && removedCount == 0 && remainCount == prevCount && currentPath != null)
            {
                // no change
                return;
            }
            
            // 决定当前的Path
            Vector3 dir = go.transform.forward;
            dir.ProjectionXZ(out dirXZ);
            GameObject newPath = GetNearestParallelPath(dirXZ, remainPaths);

            if (newPath == null)
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
            if (currentPaths.Count <= 1)
            {
                var offsetAngleY = currentPath.transform.localEulerAngles.y;
                minAngleY.Value = offsetAngleY;
                maxAngleY.Value = offsetAngleY;
            }
            else
            {
                // > 1，给所有Paths排序，将currentPath的前后2个path的Y旋转角度取出，作为min和max
                currentPaths.Sort((a, b) =>
                {
                    var aY = a.transform.localEulerAngles.y;
                    var bY = b.transform.localEulerAngles.y;
                    return aY.CompareTo(bY);
                });
                
                var index = currentPaths.IndexOf(currentPath);
                if (currentPaths.Count == 2)
                {
                    // 需要在两paths的小于180度的位插入一个currentPath
                    if (Mathf.Abs(currentPaths[0].transform.localEulerAngles.y - currentPaths[1].transform.localEulerAngles.y) < 180)
                    {
                        // 插入到1
                        currentPaths.Insert(1, currentPath);
                        if (index != 0)
                        {
                            index = 2;
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            // 插入到0
                            currentPaths.Insert(0, currentPath);
                            index = 1;
                        }
                        else
                        {
                            currentPaths.Add(currentPath);
                        }
                    }
                }
                
                // 选择prev和next
                GameObject prev;
                GameObject next;
                var invertNextY = false;
                if (index == 0)
                {
                    prev = currentPaths[^1];
                    next = currentPaths[1];
                }
                else if (index == currentPaths.Count - 1)
                {
                    prev = currentPaths[index - 1];
                    next = currentPaths[0];
                }
                else
                {
                    // 我们想要得是从next 旋转到 prev这个范围
                    // 当currentPath角度处于next和prev之间时，需要将next表示的旋转方向取反
                    prev = currentPaths[index - 1];
                    next = currentPaths[index + 1];
                    invertNextY = true;
                }
                
                var prevY = prev.transform.localEulerAngles.y;
                var nextY = next.transform.localEulerAngles.y;
                if (invertNextY)
                {
                    // 取反
                    nextY = -(360 - nextY);
                }
                minAngleY.Value = nextY;
                maxAngleY.Value = prevY;
            }
        }

        GameObject GetNearestParallelPath(Vector3 dir, List<GameObject> pathList)
        {
            GameObject path = null;
            var angle = -1f;
            foreach (GameObject remainPath in pathList)
            {
                remainPath.transform.forward.ProjectionXZ(out tmpDirXZ);
                var acuteAngle = Vector3Utility.AcuteAngle(tmpDirXZ, dir);
                if (angle < 0f || acuteAngle < angle)
                {
                    angle = acuteAngle;
                    path = remainPath;
                }
            }
            return path;
        }

	}

}
