// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class MultiHandLandmarkListAnnotation : ListAnnotation<HandLandmarkListAnnotation>
  {
    [SerializeField] private Color _leftLandmarkColor = Color.green;
    [SerializeField] private Color _rightLandmarkColor = Color.green;
    [SerializeField] private float _landmarkRadius = 15.0f;
    [SerializeField] private int _targetCount = 10;


#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLeftLandmarkColor(_leftLandmarkColor);
        ApplyRightLandmarkColor(_rightLandmarkColor);
      }
    }
#endif

    public void SetLeftLandmarkColor(Color leftLandmarkColor)
    {
      _leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(_leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor)
    {
      _rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetHandedness(IReadOnlyList<ClassificationList> handedness)
    {
      var count = handedness == null ? 0 : handedness.Count;
      for (var i = 0; i < Mathf.Min(count, children.Count); i++)
      {
        children[i].SetHandedness(handedness[i]);
      }
      for (var i = count; i < children.Count; i++)
      {
        children[i].SetHandedness((IReadOnlyList<Classification>)null);
      }
    }

    public void SetHandedness(IReadOnlyList<mptcc.Classifications> handedness)
    {
      var count = handedness == null ? 0 : handedness.Count;
      for (var i = 0; i < Mathf.Min(count, children.Count); i++)
      {
        children[i].SetHandedness(handedness[i]);
      }
      for (var i = count; i < children.Count; i++)
      {
        children[i].SetHandedness((IReadOnlyList<mptcc.Category>)null);
      }
    }

    public void Draw(IReadOnlyList<NormalizedLandmarkList> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    public void Draw(IReadOnlyList<mptcc.NormalizedLandmarks> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ);}
        });
      }
    }

    protected override HandLandmarkListAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLeftLandmarkColor(_leftLandmarkColor);
      annotation.SetRightLandmarkColor(_rightLandmarkColor);
      return annotation;
    }

    private void ApplyLeftLandmarkColor(Color leftLandmarkColor)
    {
      foreach (var handLandmarkList in children)
      {
        if (handLandmarkList != null) { handLandmarkList.SetLeftLandmarkColor(leftLandmarkColor); }
      }
    }

    private void ApplyRightLandmarkColor(Color rightLandmarkColor)
    {
      foreach (var handLandmarkList in children)
      {
        if (handLandmarkList != null) { handLandmarkList.SetRightLandmarkColor(rightLandmarkColor); }
      }
    }
  }
}
