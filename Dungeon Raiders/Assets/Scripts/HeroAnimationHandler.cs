﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationHandler : MonoBehaviour
{
    public Unit unit;

    public Animator animator;
    public TrailRenderer weaponTrail;

    public Transform chestPoint;
    public Transform headPoint;
    public Transform handRightPoint;
    public Transform handLeftPoint;
    public Transform overheadPoint;

    public AnimationClipGroup jumpClips;

    public List<AudioClip> deathSounds;

    public List<AudioClip> fallSounds;

    [Tooltip("Части тела персонажа, которые отвалятся, например, при анимации смерти")]
    public List<Rigidbody> bodyparts;

    public bool animEventStart = false;
    public bool animEventEnd = false;

    //public bool animEventLeap = false;

    public bool animEventJumpStart = false;
    public bool animEventJumpEnd = false;

    public bool animEventCastStart = false;
    public bool animEventCast= false;
    public bool animEventCastEnd = false;

    public void EventStart()
    {
        animEventStart = true;
    }
    public void EventEnd()
    {
        animEventEnd = true;
    }

    public void EventJumpStart()
    {
        //Debug.Log("Прыжок начался");
        animEventJumpStart = true;
    }
    public void EventJumpEnd()
    {
        //Debug.Log("Прыжок окончен");
        animEventJumpEnd = true;
    }

    //public void EventLeap()
    //{
    //    animEventLeap = true;
    //}

    public void EventCastStart()
    {
        //Debug.Log("НАЧАЛАСЬ анимация применения способности");
        animEventCastStart = true;
    }
    public void EventCast()
    {
        //Debug.Log("Анимация применения способности СРАБОТАЛА");
        animEventCast = true;
    }
    public void EventCastEnd()
    {
        //Debug.Log("Анимация применения способности ЗАКОНЧИЛАСЬ");
        animEventCastEnd = true;
    }

    public void ClearFalgs()
    {
        animEventStart = false;
        animEventEnd = false;
        animEventJumpStart = false;
        animEventJumpEnd = false;
        //animEventLeap = false;
        animEventCastStart = false;
        animEventCast = false;
        animEventCastEnd = false;
    }


    public void PlayAnimation(string tag)
    {
        AnimationClipGroup clips;

        switch (tag)    
        {
            case "jump":
                clips = jumpClips;
                break;
            default:
                clips = null;
                break;
        }

        if (clips != null)
        {
            int index;
            if (clips.allowRepeat)
            {
                index = Random.Range(0, clips.count);
            }
            else
            {
                var indexes = new List<int>();
                for (int i = 0; i < clips.count; i++)
                    if (i != clips.excluded)
                        indexes.Add(i);
                index = indexes[Random.Range(0, indexes.Count)];
                clips.excluded = index;
            }
            animator.SetInteger("index", index);
        }

        animator.SetTrigger(tag);
    }

    public void SetMoveFlag(bool flag)
    {
        animator.SetBool("isMoving", flag);
    }

    public void ReleaseBodyparts()
    {
        foreach (var part in bodyparts)
        {
            part.transform.SetParent(null);
            part.isKinematic = false;
        }
    }

    public void EnableWeaponTrail()
    {
        weaponTrail.emitting = true;
    }

    public void DisableWeaponTrail()
    {
        weaponTrail.emitting = false;
    }

    public void PlayFallSound()
    {
        unit.PlayRandomSound(fallSounds);
    }

    public void PlayDeathSound()
    {
        unit.PlayRandomSound(deathSounds);
    }
}

public enum UnitBodyPoints { origin, chest, head, handRight, handLeft, overhead}

public enum AnimationEvents { start, end, leap, jumpStart, jumpEnd, castStart, cast, castEnd }

[System.Serializable]
public class AnimationClipGroup : object
{
    [Range(1,5)][Tooltip("Количество анимационных клипов в группе")]
    public int count;

    [Tooltip("Повторы одинаковых анимаций разрешены?")]
    public bool allowRepeat;

    [HideInInspector] // Индекс последней проигранной анимации из этой группы
    public int excluded = -1;
}