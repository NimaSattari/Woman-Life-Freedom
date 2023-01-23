using UnityEngine;

namespace ExamineSystem
{
    [System.Serializable]
    public class TextCustomisationClass
    {
        #region Text Customisation Variables
        [Header("Item UI Type")]
        public ExamineItemController.UIType _UIType = ExamineItemController.UIType.None;

        [Header("Item Name")]
        public string itemName;
        public bool showHelpUI = false;

        [Header("Item Name Settings")]
        public int textSize = 40;
        public Font fontType = null;
        public FontStyle fontStyle = FontStyle.Normal;
        public Color fontColor = Color.white;

        [Space(5)] [TextArea] public string itemDescription;

        [Header("Item Descriptor Settings")]
        public int textSizeDesc = 32;
        public Font fontTypeDesc = null;
        public FontStyle fontStyleDesc = FontStyle.Normal;
        public Color fontColorDesc = Color.white;
        #endregion
    }

    public class ExamineItemController : MonoBehaviour
    {
        #region Main Examine Variables
        [Header("Parent Settings")]
        [Tooltip("Select this option if the object that has this script attached has no mesh renderer and it's an empty parent which holds children")]
        [SerializeField] private bool isEmptyParent = false;

        [Header("Children Settings")]
        [Tooltip("Select this option if the object you're examining has multiple children - Add the child objects to the array")]
        [SerializeField] private bool hasChildren = false;
        [SerializeField] private GameObject[] childObjects = null;     

        [Header("Initial Offsets for objects")]
        [SerializeField] private Vector3 initialRotationOffset = new Vector3(0, 0, 0);
        [Tooltip("Horizontal and Vertical offsets control how far vertically and horizentally the object is positioned when examined. Keep this a low value (-0.X) is left and (+0.X) is right")]
        [Range(-1, 1)][SerializeField] private float horizontalOffset = 0;
        [Range(-1, 1)][SerializeField] private float verticalOffset = 0;

        [Header("Zoom Settings")]
        [SerializeField] private float initialZoom = 1f;
        [SerializeField] private Vector2 zoomRange = new Vector2(0.5f, 2f);
        [SerializeField] private float zoomSensitivity = 0.1f;

        [Header("Examine Rotation")]
        [SerializeField] private float horizontalSpeed = 5.0F;
        [SerializeField] private float verticalSpeed = 5.0F;

        [Header("Emissive Highlight")]
        [SerializeField] private bool showEmissionHighlight = false;
        [SerializeField] private bool showNameHighlight = false;

        [Header("InterestPoints")]
        [SerializeField] private GameObject[] inspectPoints = null;
        private LayerMask myMask;
        private bool hasInspectPoints = false;
        private float viewDistance = 25;

        [Header("Item Interaction Sound")]
        [SerializeField] private string pickupSound = "YourSound";

        [Header("UI Type / Name / Description Settings")]
        public TextCustomisationClass textCustomisation;

        private Material thisMat;
        Vector3 originalPosition;
        Quaternion originalRotation;
        private Vector3 startPos;
        private bool canRotate;
        private float currentZoom = 1;
        private const string emissive = "_EMISSION";
        private const string mouseX = "Mouse X";
        private const string mouseY = "Mouse Y";
        private const string examineLayer = "ExamineLayer";
        private const string defaultLayer = "Default";

        private Camera mainCamera;
        private Transform examinePoint = null;

        private ExamineRaycast raycastManager;

        public enum UIType { None, BasicLowerUI, RightSideUI }
        #endregion

        void Start()
        {
            //This finds the mask we want and adds it to the variable "myMask"
            myMask = 1 << LayerMask.NameToLayer("InspectPointLayer");

            //Setting some initial positions when we choose to move the objects
            initialZoom = Mathf.Clamp(initialZoom, zoomRange.x, zoomRange.y);
            originalPosition = transform.position; originalRotation = transform.rotation;
            startPos = gameObject.transform.localEulerAngles;

            DisableEmissionOnChildren();
            if (!isEmptyParent)
            {
                thisMat = GetComponent<Renderer>().material;
                thisMat.DisableKeyword(emissive);
            }
            mainCamera = Camera.main; raycastManager = mainCamera.GetComponent<ExamineRaycast>();
            examinePoint = GameObject.FindWithTag("ExaminePoint").GetComponent<Transform>();
        }

        void Update()
        {
            if (canRotate)
            {
                float h = horizontalSpeed * Input.GetAxis(mouseX);
                float v = verticalSpeed * Input.GetAxis(mouseY);

                if (hasInspectPoints)
                {
                    FindInspectPoints();
                }
                if (Input.GetKey(ExamineInputManager.instance.rotateKey))
                {
                    gameObject.transform.Rotate(v, h, 0);
                }
                else if (Input.GetKeyDown(ExamineInputManager.instance.dropKey))
                {
                    DropObject();
                }
                //Handle zooming
                bool zoomAdjusted = false;
                float scrollDelta = Input.mouseScrollDelta.y;
                if (scrollDelta > 0)
                {
                    currentZoom += zoomSensitivity;
                    zoomAdjusted = true;
                }
                else if (scrollDelta < 0)
                {
                    currentZoom -= zoomSensitivity;
                    zoomAdjusted = true;
                }

                if (zoomAdjusted)
                {
                    currentZoom = Mathf.Clamp(currentZoom, zoomRange.x, zoomRange.y);
                    MoveZoom(currentZoom);
                }
            }
        }

        public void ExamineObject()
        {
            ExamineUIManager.instance.examineController = gameObject.GetComponent<ExamineItemController>();
            ExamineAudioManager.instance.Play(pickupSound);

            if (inspectPoints.Length >= 1)
            {
                hasInspectPoints = true;

                foreach (GameObject pointToEnable in inspectPoints)
                {
                    pointToEnable.SetActive(true);
                }
            }

            currentZoom = initialZoom; MoveZoom(initialZoom);
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            transform.Rotate(initialRotationOffset);

            ExamineDisableManager.instance.DisablePlayer(true);
            ExamineUIManager.instance.interactionNameMainUI.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer(examineLayer);

            if (textCustomisation.showHelpUI)
            {
                ExamineUIManager.instance.ShowHelpPrompt(true);
            }
            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    gameobjectToLayer.layer = LayerMask.NameToLayer(examineLayer);
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }
            if (!isEmptyParent)
            {
                thisMat.DisableKeyword(emissive);
            }
            canRotate = true;

            switch (textCustomisation._UIType)
            {
                case UIType.None:
                    ExamineUIManager.instance.noUICloseButton.SetActive(true);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.text = textCustomisation.itemName;
                    ExamineUIManager.instance.basicItemDescUI.text = textCustomisation.itemDescription;
                    TextCustomisation();
                    ExamineUIManager.instance.basicExamineUI.SetActive(true);
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.text = textCustomisation.itemName;
                    ExamineUIManager.instance.rightItemDescUI.text = textCustomisation.itemDescription;
                    TextCustomisation();
                    ExamineUIManager.instance.rightExamineUI.SetActive(true);
                    break;
            }
        }
    
        public void DropObject()
        {
            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    gameobjectToLayer.layer = LayerMask.NameToLayer(defaultLayer);
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }
            gameObject.layer = LayerMask.NameToLayer(defaultLayer);
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            ExamineDisableManager.instance.DisablePlayer(false);
            canRotate = false;
            hasInspectPoints = false;

            if (textCustomisation.showHelpUI)
            {
                ExamineUIManager.instance.ShowHelpPrompt(false);
            }
            foreach (GameObject pointToEnable in inspectPoints)
            {
                pointToEnable.SetActive(false);
            }
            switch (textCustomisation._UIType)
            {
                case UIType.None:
                    ExamineUIManager.instance.noUICloseButton.SetActive(false);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.text = null;
                    ExamineUIManager.instance.basicExamineUI.SetActive(false);
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.text = null;
                    ExamineUIManager.instance.rightExamineUI.SetActive(false);
                    break;
            }
        }

        public void MainHighlight(bool isHighlighted)
        {
            if (showNameHighlight)
            {
                if (isHighlighted)
                {
                    ExamineUIManager.instance.interactionItemNameUI.text = textCustomisation.itemName;
                    ExamineUIManager.instance.interactionNameMainUI.SetActive(true);
                }
                else
                {
                    ExamineUIManager.instance.interactionItemNameUI.text = textCustomisation.itemName;
                    ExamineUIManager.instance.interactionNameMainUI.SetActive(false);
                }
            }

            if (showEmissionHighlight)
            {
                if (isHighlighted)
                {
                    if (!isEmptyParent)
                    {
                        thisMat.EnableKeyword(emissive);
                    }
                    if (hasChildren)
                    {
                        foreach (GameObject gameobjectToLayer in childObjects)
                        {
                            Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                            thisMat.EnableKeyword(emissive);
                        }
                    }
                }
                else
                {
                    if (!isEmptyParent)
                    {
                        thisMat.DisableKeyword(emissive);
                    }
                    DisableEmissionOnChildren();
                }
            }
        }

        //value = The distance from the camera to position the object</param>
        //MoveSelf = Whether to move the actual object. If set to false the object may not move, but only the represented point.</param>
        private void MoveZoom(float value, bool moveSelf = true)
        {
            examinePoint.transform.localPosition = new Vector3(horizontalOffset, verticalOffset, value);

            if (moveSelf)
            {
                transform.position = examinePoint.transform.position;
            }
        }

        private void TextCustomisation()
        {
            switch (textCustomisation._UIType)
            {
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.fontSize = textCustomisation.textSize;
                    ExamineUIManager.instance.basicItemNameUI.fontStyle = textCustomisation.fontStyle;
                    ExamineUIManager.instance.basicItemNameUI.font = textCustomisation.fontType;
                    ExamineUIManager.instance.basicItemNameUI.color = textCustomisation.fontColor;
                    ExamineUIManager.instance.basicItemDescUI.fontSize = textCustomisation.textSizeDesc;
                    ExamineUIManager.instance.basicItemDescUI.fontStyle = textCustomisation.fontStyleDesc;
                    ExamineUIManager.instance.basicItemDescUI.font = textCustomisation.fontTypeDesc;
                    ExamineUIManager.instance.basicItemDescUI.color = textCustomisation.fontColorDesc;
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.fontSize = textCustomisation.textSize;
                    ExamineUIManager.instance.rightItemNameUI.fontStyle = textCustomisation.fontStyle;
                    ExamineUIManager.instance.rightItemNameUI.font = textCustomisation.fontType;
                    ExamineUIManager.instance.rightItemNameUI.color = textCustomisation.fontColor;
                    ExamineUIManager.instance.rightItemDescUI.fontSize = textCustomisation.textSizeDesc;
                    ExamineUIManager.instance.rightItemDescUI.fontStyle = textCustomisation.fontStyleDesc;
                    ExamineUIManager.instance.rightItemDescUI.font = textCustomisation.fontTypeDesc;
                    ExamineUIManager.instance.rightItemDescUI.color = textCustomisation.fontColorDesc;
                    break;
            }         
        }

        void FindInspectPoints()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, viewDistance, myMask))
            {
                if (hit.transform.CompareTag("InspectPoint"))
                {
                    InspectPointUI(hit.transform.gameObject, mainCamera, true); //Enable inspect point UI
                    if (Input.GetKeyDown(ExamineInputManager.instance.interactKey))
                    {
                        hit.transform.gameObject.GetComponent<ExamineInspectPoint>().InspectPointInteract();
                    }
                }
                else
                {
                    InspectPointUI(null, null, false); //Disable inspect point UI
                }
            }
            else
            {
                InspectPointUI(null, null, false); //Disable inspect point UI
            }
        }

        void InspectPointUI(GameObject item, Camera camera, bool detected) // Enable/disable inspect point UI
        { 
            if (detected)
            {
                ExamineUIManager.instance.interestPointParentUI.SetActive(true);
                ExamineUIManager.instance.interestPointParentUI.transform.position = camera.WorldToScreenPoint(item.transform.position);
                ExamineUIManager.instance.interestPointText.text = item.GetComponent<ExamineInspectPoint>().InspectInformation();
            }
            else
            {
                ExamineUIManager.instance.interestPointParentUI.SetActive(false); //Disable inspect UI
            }
        }

        void DisableEmissionOnChildren()
        {
            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }
        }

        private void OnDestroy()
        {
            Destroy(thisMat);
        }
    }
}