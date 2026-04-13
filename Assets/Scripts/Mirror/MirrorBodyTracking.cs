using System.Collections.Generic;
using UnityEngine;
using nuitrack;

/// <summary>
/// MirrorBodyTracking
///
/// Reads skeleton data from Nuitrack and drives a humanoid Animator
/// with mirror-corrected orientations (right hand -> right side of screen).
///
/// SETUP in Unity:
///   1. Assign the Animator of your humanoid avatar to 'Avatar Animator'.
///   2. Keep 'Mirror Skeleton' enabled.
///   3. Make sure NuitrackManager is present in the scene.
/// </summary>
public class MirrorBodyTracking : MonoBehaviour
{
    [Header("Avatar")]
    [SerializeField] private Animator avatarAnimator;

    [Header("Mirror")]
    [SerializeField] private bool mirrorSkeleton = true;

    [Header("Filtering")]
    [Range(0f, 1f)]
    [SerializeField] private float minConfidence = 0.5f;

    [Range(0f, 0.99f)]
    [SerializeField] private float smoothing = 0.3f;

    private static readonly Dictionary<JointType, HumanBodyBones> JointMap =
        new Dictionary<JointType, HumanBodyBones>
    {
        { JointType.Head,         HumanBodyBones.Head          },
        { JointType.Neck,         HumanBodyBones.Neck          },
        { JointType.LeftCollar,   HumanBodyBones.LeftShoulder  },
        { JointType.RightCollar,  HumanBodyBones.RightShoulder },
        { JointType.LeftElbow,    HumanBodyBones.LeftUpperArm  },
        { JointType.RightElbow,   HumanBodyBones.RightUpperArm },
        { JointType.LeftWrist,    HumanBodyBones.LeftLowerArm  },
        { JointType.RightWrist,   HumanBodyBones.RightLowerArm },
        { JointType.LeftHand,     HumanBodyBones.LeftHand      },
        { JointType.RightHand,    HumanBodyBones.RightHand     },
        { JointType.Torso,        HumanBodyBones.Spine         },
        { JointType.Waist,        HumanBodyBones.Hips          },
        { JointType.LeftHip,      HumanBodyBones.LeftUpperLeg  },
        { JointType.RightHip,     HumanBodyBones.RightUpperLeg },
        { JointType.LeftKnee,     HumanBodyBones.LeftLowerLeg  },
        { JointType.RightKnee,    HumanBodyBones.RightLowerLeg },
        { JointType.LeftAnkle,    HumanBodyBones.LeftFoot      },
        { JointType.RightAnkle,   HumanBodyBones.RightFoot     },
    };

    private Dictionary<JointType, Transform>   boneCache;
    private Dictionary<JointType, Quaternion>  lastRotation;

    void Start()    { CacheBones(); }
    void OnEnable() { NuitrackManager.onSkeletonTrackerUpdate += OnSkeletonUpdate; }
    void OnDisable(){ NuitrackManager.onSkeletonTrackerUpdate -= OnSkeletonUpdate; }

    private void CacheBones()
    {
        boneCache    = new Dictionary<JointType, Transform>();
        lastRotation = new Dictionary<JointType, Quaternion>();

        if (avatarAnimator == null) return;

        foreach (var pair in JointMap)
        {
            Transform t = avatarAnimator.GetBoneTransform(pair.Value);
            if (t != null)
            {
                boneCache[pair.Key]    = t;
                lastRotation[pair.Key] = t.rotation;
            }
        }
    }

    private void OnSkeletonUpdate(SkeletonData skeletonData)
    {
        if (skeletonData == null || skeletonData.Skeletons.Length == 0) return;

        Skeleton skeleton = skeletonData.Skeletons[0];

        foreach (var pair in JointMap)
        {
            if (!boneCache.TryGetValue(pair.Key, out Transform bone)) continue;

            Joint joint = skeleton.GetJoint(pair.Key);
            if (joint.Confidence < minConfidence) continue;

            Quaternion targetRot = OrientationToQuaternion(joint.Orient.Matrix);

            if (mirrorSkeleton)
                targetRot = MirrorQuaternion(targetRot);

            Quaternion smoothed = Quaternion.Slerp(
                lastRotation.ContainsKey(pair.Key) ? lastRotation[pair.Key] : targetRot,
                targetRot,
                1f - smoothing
            );

            bone.rotation          = smoothed;
            lastRotation[pair.Key] = smoothed;
        }
    }

    // Nuitrack (repère main droite) -> Unity (repère main gauche)
    private static Quaternion OrientationToQuaternion(float[] m)
    {
        Matrix4x4 mat = Matrix4x4.identity;
        mat.m00 =  m[0]; mat.m01 =  m[1]; mat.m02 = -m[2];
        mat.m10 =  m[3]; mat.m11 =  m[4]; mat.m12 = -m[5];
        mat.m20 = -m[6]; mat.m21 = -m[7]; mat.m22 =  m[8];
        return mat.rotation;
    }

    // Reflète autour du plan YZ (effet miroir gauche/droite)
    private static Quaternion MirrorQuaternion(Quaternion q)
        => new Quaternion(-q.x, q.y, q.z, -q.w);

    // Coordonnées projetées Nuitrack -> position écran miroir
    public static Vector2 ProjectedToMirrorScreen(float projX, float projY,
                                                   float screenW, float screenH)
        => new Vector2((1f - projX) * screenW, projY * screenH);

    public void ToggleMirror(bool enable)   { mirrorSkeleton = enable; }
    public void SetSmoothing(float value)   { smoothing = Mathf.Clamp01(value); }
}

