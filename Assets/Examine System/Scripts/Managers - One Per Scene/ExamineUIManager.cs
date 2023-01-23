using UnityEngine;
using UnityEngine.UI;

namespace ExamineSystem
{
    public class ExamineUIManager : MonoBehaviour
    {
        [Header("No UI Close Button")]
        public GameObject noUICloseButton = null;

        [Header("Basic Example UI References")]
        public Text basicItemNameUI = null;
        public Text basicItemDescUI = null;
        public GameObject basicExamineUI = null;

        [Header("Right Side Example UI References")]
        public Text rightItemNameUI = null;
        public Text rightItemDescUI = null;
        public GameObject rightExamineUI = null;

        [Header("Interaction Name UI")]
        public Text interactionItemNameUI = null;
        public GameObject interactionNameMainUI = null;

        [Header("Interest Point UI's")]
        public Text interestPointText = null;
        public GameObject interestPointParentUI = null;

        [HideInInspector] public ExamineItemController examineController;

        [Header("Help Panel Visibility")]
        [SerializeField] private GameObject examineHelpUI = null;

        public static ExamineUIManager instance;

        private void Awake()
        {
            if (instance == null) { instance = this; }
        }

        public void CloseButton()
        {
            examineController.DropObject();
        }

        public void ShowHelpPrompt(bool showHelp)
        {
            if (showHelp)
            {
                examineHelpUI.SetActive(true);
            }
            else
            {
                examineHelpUI.SetActive(false);
            }
        }
    }
}
