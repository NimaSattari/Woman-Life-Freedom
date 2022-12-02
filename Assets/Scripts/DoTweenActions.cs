using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DoTweenActions : MonoBehaviour
{
    Vector3 initalLocation = Vector3.zero;
    Vector3 initalSize = Vector3.zero;
    Quaternion initalRotation;

    public Vector3 targetLocation = Vector3.zero;
    public Vector3 targetSize = Vector3.zero;
    public Vector3 targetRotation;
    public float animationDuration = 1f;
    public Ease animationEase = Ease.Linear;
    [SerializeField] AnimationType animationType = AnimationType.Move;
    public bool doOnStart = false;
    public bool rotatethis;
    public bool oneLoop;
    public bool loop;
    enum AnimationType
    {
        Move,
        Rotate,
        Scale,
        MoveAndScale,
        MoveAndRotate
    }
    Tweener tr;
    private void Start()
    {
        if (rotatethis)
        {
            tr = transform.DOBlendableLocalRotateBy(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360);
            tr.SetAutoKill(false);
            tr.Pause();
        }
    }
    private void OnEnable()
    {
        initalLocation = transform.localPosition;
        initalSize = transform.localScale;
        initalRotation = transform.localRotation;
        if (doOnStart)
        {
            DoAnimation();
        }
    }
    private void OnDisable()
    {
        transform.localPosition = initalLocation;
        transform.localScale = initalSize;
        transform.localRotation = initalRotation;
    }
    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    public void DoAnimation()
    {
        if (animationType == AnimationType.Move)
        {
            transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.Rotate)
        {
            transform.DORotate(targetRotation, animationDuration).SetEase(animationEase);
            //transform.DORotateQuaternion(targetRotation, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.Scale)
        {
            transform.DOScale(targetSize, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.MoveAndScale)
        {
            DOTween.Sequence().SetAutoKill(false)
                .Append(transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase))
                .Join(transform.DOScale(targetSize, animationDuration).SetEase(animationEase));
        }
        /*        else if (animationType == AnimationType.MoveAndRotate)
                {
                    DOTween.Sequence().SetAutoKill(false)
                        .Append(transform.DOLocalMove(targetLocation, animationDuration).SetEase(animationEase))
                        .Join(transform.DORotateQuaternion(targetRotation, animationDuration).SetEase(animationEase));
                }*/
/*        if (loop || oneLoop)
        {
            StartCoroutine(DoAnimationBackward());
        }*/
    }
    public void DoAnimationBackward()
    {
        //yield return new WaitForSeconds(animationDuration);
        if (animationType == AnimationType.Move)
        {
            transform.DOLocalMove(initalLocation, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.Rotate)
        {
            transform.DORotateQuaternion(initalRotation, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.Scale)
        {
            transform.DOScale(initalSize, animationDuration).SetEase(animationEase);
        }
        else if (animationType == AnimationType.MoveAndScale)
        {
            DOTween.Sequence().SetAutoKill(false)
                .Append(transform.DOLocalMove(initalLocation, animationDuration).SetEase(animationEase))
                .Join(transform.DOScale(initalSize, animationDuration).SetEase(animationEase));
        }
        else if (animationType == AnimationType.MoveAndRotate)
        {
            DOTween.Sequence().SetAutoKill(false)
                .Append(transform.DOLocalMove(initalLocation, animationDuration).SetEase(animationEase))
                .Join(transform.DORotateQuaternion(initalRotation, animationDuration).SetEase(animationEase));
        }
/*        if (loop)
        {
            yield return new WaitForSeconds(animationDuration);
            DoAnimation();
        }*/
    }
}