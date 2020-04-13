namespace Assets.Code
{
    public class PlayerAnimator : CharacterAnimator
    {
        protected override void Start()
        {
            base.Start();
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        }

        void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if(newItem!= null && newItem.EquipSlot == EquipmentSlotsNames.RightHand_Weapon)
            {
                animator.SetLayerWeight(2, 1);
            }
            else if(newItem == null && oldItem!=null && oldItem.EquipSlot == EquipmentSlotsNames.RightHand_Weapon)
            {
                animator.SetLayerWeight(2, 0);
            }
            if (newItem != null && newItem.EquipSlot == EquipmentSlotsNames.LeftHand_Shield)
            {
                animator.SetLayerWeight(1, 1);
            }
            else if (newItem == null && oldItem != null && oldItem.EquipSlot == EquipmentSlotsNames.LeftHand_Shield)
            {
                animator.SetLayerWeight(1, 0);
            }
        }

    }
}