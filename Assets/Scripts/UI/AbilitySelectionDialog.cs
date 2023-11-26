using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class AbilitySelectionDialog : DialogBox
    {
        [SerializeField] private Transform abilityCardsParent;
        [SerializeField] private GameObject abilityCardPrefab;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private GameObject particles;
        [SerializeField] private ChestBlueprint failsafeChestBlueprint;
        [SerializeField] private float cardPopupDelay = 0.1f;
        private AbilityManager abilityManager;
        private EntityManager entityManager;
        private Character playerCharacter;
        private List<AbilityCard> abilityCards;
        private List<Ability> displayedAbilities;
        private bool menuOpen = false;
        public bool MenuOpen { get => menuOpen; }

        public void Init(AbilityManager abilityManager, EntityManager entityManager, Character playerCharacter)
        {
            this.abilityManager = abilityManager;
            this.entityManager = entityManager;
            this.playerCharacter = playerCharacter;
        }

        public void Open(bool failsafe = true)
        {
            base.Open();
            menuOpen = true;
            Time.timeScale = 0;
            pauseMenu.TimeIsFrozen = true;
            particles.SetActive(true);

            // Select abilities/upgrades to display
            displayedAbilities = abilityManager.SelectAbilities();
            if (displayedAbilities.Count > 0)
            {
                Populate(displayedAbilities);
            }
            // Close menu and spawn a chest instead as a failsafe there are no abilities left
            else
            {
                if (failsafe)
                    entityManager.SpawnChest(failsafeChestBlueprint, (Vector2)playerCharacter.transform.position + Vector2.up);
                Close();
            }
        }

        private void Populate(List<Ability> abilities)
        {
            if (abilityCards == null)
                abilityCards = new List<AbilityCard>();

            int i = 0;
            for (; i < abilities.Count; i++)
            {
                if (i >= abilityCards.Count)
                    abilityCards.Add(Instantiate(abilityCardPrefab, abilityCardsParent).GetComponent<AbilityCard>());
                abilityCards[i].Init(this, abilities[i], cardPopupDelay*i);
                abilityCards[i].gameObject.SetActive(true);
            }
            for (; i < abilityCards.Count; i++)
            {
                abilityCards[i].gameObject.SetActive(false);
            }
        }

        public override void Close()
        {
            abilityManager.ReturnAbilities(displayedAbilities);
            menuOpen = false;
            Time.timeScale = 1;
            pauseMenu.TimeIsFrozen = false;
            particles.SetActive(false);
            base.Close();
        }

        public bool HasAvailableAbilities()
        {
            return abilityManager.HasAvailableAbilities();
        }
    }
}
