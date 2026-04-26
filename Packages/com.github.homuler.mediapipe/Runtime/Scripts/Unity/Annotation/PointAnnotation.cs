// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

using mplt = Mediapipe.LocationData.Types;
using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class PointAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private Color _color = Color.green;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private int _targetCount = 10;

    private void OnEnable()
    {
      ApplyColor(_color);
      ApplyRadius(_radius);
    }

    private void OnDisable()
    {
      ApplyRadius(0.0f);
    }

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetRadius(float radius)
    {
      _radius = radius;
      ApplyRadius(_radius);
    }

    public void SetTarget(int targetCount)
    {
      _targetCount = targetCount;
      ApplyTargetActive(_targetCount);
    }

    public void Draw(Vector3 position)
    {
      ApplyTargetActive(_targetCount);
      transform.localPosition = position;
    }

    public void Draw(Landmark target, Vector3 scale, bool visualizeZ = true)
    {
      if (ApplyTargetActive(_targetCount))
      {
        var position = GetScreenRect().GetPoint(target, scale, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(NormalizedLandmark target, bool visualizeZ = true)
    {
      if (ApplyTargetActive(_targetCount))
      {
        var position = GetScreenRect().GetPoint(target, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(in mptcc.NormalizedLandmark target, bool visualizeZ = true)
    {
      if (ApplyTargetActive(_targetCount))
      {
        var position = GetScreenRect().GetPoint(in target, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(mplt.RelativeKeypoint target, float threshold = 0.0f)
    {
      if (ApplyTargetActive(_targetCount))
      {
        Draw(GetScreenRect().GetPoint(target, rotationAngle, isMirrored));
        SetColor(GetColor(target.Score, threshold));
      }
    }

    public void Draw(mptcc.NormalizedKeypoint target, float threshold = 0.0f)
    {
      if (ApplyTargetActive(_targetCount))
      {
        Draw(GetScreenRect().GetPoint(target, rotationAngle, isMirrored));
        SetColor(GetColor(target.score ?? 1.0f, threshold));
      }
    }

    private void ApplyColor(Color color)
    {
      GetComponent<Renderer>().material.color = color;
    }

    private void ApplyRadius(float radius)
    {
      transform.localScale = radius * Vector3.one;
    }

    private bool ApplyTargetActive(int targetNum)
    {
      if (this.gameObject.name.EndsWith($"_{targetNum}"))
      {
        this.gameObject.SetActive(true);
        return true;
      }

      this.gameObject.SetActive(false);
      return false;
    }

    private Color GetColor(float score, float threshold)
    {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }
  }
}
