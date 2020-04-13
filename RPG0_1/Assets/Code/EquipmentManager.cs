using UnityEngine;

namespace Assets.Code
{
    public class EquipmentManager : MonoBehaviour
    {
        #region Singleton
        public static EquipmentManager instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion

        public Equipment[] defaultItems;
        public SkinnedMeshRenderer targetMesh;
        public Equipment[] currentEquipment;
        SkinnedMeshRenderer[] currentMeshes;
        MeshRenderer[] currentHandMeshes;
        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
        Inventory inventory;
        
        [SerializeField]
        Transform[] hand_grips;


        private void Start()
        {
            int noOfSlots = System.Enum.GetNames(typeof(EquipmentSlotsNames)).Length;
            currentEquipment = new Equipment[noOfSlots];
            inventory = Inventory.instance;
            currentMeshes = new SkinnedMeshRenderer[noOfSlots-2];
            currentHandMeshes = new MeshRenderer[2];

            EquipDefaultItems();
        }
        public void Equip(Equipment newItem)
        {
            int slotIndex = Equiper(newItem);
            if (!newItem.InHand)
            {
                SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.SkinnedMesh);
                newMesh.transform.parent = targetMesh.transform;
                newMesh.bones = targetMesh.bones;
                newMesh.rootBone = targetMesh.rootBone;
                currentMeshes[slotIndex] = newMesh;
                SetEquipmentBlendShapes(newItem, 100);
            }
            else
            {
                if ((int)newItem.EquipSlot == (int)EquipmentSlotsNames.LeftHand_Shield) slotIndex = 0;
                else slotIndex = 1;
                MeshRenderer newMesh = Instantiate<MeshRenderer>(newItem.NormalMesh);
                newMesh.transform.SetParent(hand_grips[slotIndex], false);
                currentHandMeshes[slotIndex] = newMesh;
            }
        }

        private int Equiper(Equipment newItem)
        {
            int slotIndex = (int)newItem.EquipSlot;
            Equipment oldItem = Unequip(slotIndex);
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(newItem, oldItem);
            }

            currentEquipment[slotIndex] = newItem;
            return slotIndex;
        }

        //public void EquipHands(EquipmentHands newItem)
        //{
        //    int slotIndex = Equiper(newItem);

        //    MeshRenderer newMesh = Instantiate<MeshRenderer>(newItem.mesh);
        //    newMesh.transform.parent = hand_grips[slotIndex];
        //    currentHandMeshes[slotIndex] = newMesh;
        //}
        public Equipment Unequip(int slotIndex)
        {
            if (currentEquipment[slotIndex] != null)
            {
                if (!currentEquipment[slotIndex].InHand)
                {
                    if (currentMeshes[slotIndex] != null)
                        Destroy(currentMeshes[slotIndex].gameObject);
                }
                else
                {
                    int tmp = ((int)currentEquipment[slotIndex].EquipSlot == (int)EquipmentSlotsNames.LeftHand_Shield) ? 0 : 1;
                        Destroy(currentHandMeshes[tmp].gameObject);
                }

                Equipment oldItem = currentEquipment[slotIndex];
                SetEquipmentBlendShapes(oldItem, 0);
                inventory.Add(oldItem);

                currentEquipment[slotIndex] = null;
                if (onEquipmentChanged != null)
                {
                    onEquipmentChanged.Invoke(null, oldItem);
                }
                return oldItem;
            }
            return null;
        }
        public void UnequipAll()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
            EquipDefaultItems();
        }

        void SetEquipmentBlendShapes(Equipment item, int weight)
        {
            foreach (EquipmentMeshRegions blendShape in item.coveredMeshRegions)
            {
                targetMesh.SetBlendShapeWeight((int)blendShape, weight);
            }
        }
        void EquipDefaultItems()
        {
            foreach (Equipment item in defaultItems)
            {
                Equip(item);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
                UnequipAll();
        }

    }
}