using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private bool appearInstantly = false;
        [SerializeField] private float animationSpeed;
        [SerializeField] private DialogBox previousDialog, nextDialog;

        public virtual void Open()
        {
            gameObject.SetActive(true);
            if (appearInstantly)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(OpenAnimation());
            }
        }

        public virtual void Close()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            // StopAllCoroutines();
            // StartCoroutine(CloseAnimation());
        }

        public void Return()
        {
            previousDialog?.Open();
            Close();
        }

        public void Continue()
        {
            nextDialog?.Open();
            Close();
        }

        private IEnumerator OpenAnimation()
        {
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, EasingUtils.EaseOutBack(t));
                t += Time.unscaledDeltaTime * animationSpeed;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        private IEnumerator CloseAnimation()
        {
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, EasingUtils.EaseOutQuart(t));
                t += Time.deltaTime * animationSpeed;
                yield return null;
            }
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
