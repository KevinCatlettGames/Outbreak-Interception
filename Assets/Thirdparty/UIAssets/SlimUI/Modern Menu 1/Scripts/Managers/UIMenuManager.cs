using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	public class UIMenuManager : MonoBehaviour {
		private Animator CameraObject;

		// campaign button sub menu
        [Header("MENUS")]
        [Tooltip("The Menu for when the MAIN menu buttons")]
        public GameObject mainMenu;
        [Tooltip("The first list of buttons")]
        public GameObject firstMenu;       
        [Tooltip("The credits menu")]
        public GameObject creditsMenu;
		[Tooltip("The difficulty menu")]
		public GameObject difficultyMenu;

        [Header("PANELS")]
        [Tooltip("The UI Panel parenting all sub menus")]
        public GameObject mainCanvas;
        [Tooltip("The UI Panel that holds the AUDIO window tab")]
        public GameObject PanelAudio;
        [Tooltip("The UI Panel that holds the VIDEO window tab")]
        public GameObject PanelVideo;            

        // highlights in settings screen
        [Header("SETTINGS SCREEN")]
        [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
        public GameObject lineGame;
        [Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
        public GameObject lineVideo;
        [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
        public GameObject lineControls;

		void OnEnable(){
			CameraObject = transform.GetComponent<Animator>();
		
			if(creditsMenu) creditsMenu.SetActive(false);
			firstMenu.SetActive(true);
			mainMenu.SetActive(true);
		}

        private void Update()
        {
            if(Time.timeScale == 1 && difficultyMenu.activeSelf && SceneManager.GetActiveScene().buildIndex != 0)
			{
				difficultyMenu.SetActive(false);
			}
        }

        public void ReturnMenu(){		
			if(creditsMenu) creditsMenu.SetActive(false);	
			mainMenu.SetActive(true);
		}

		public void Position2(){
			if(mainCanvas.activeSelf && CameraObject && Time.timeScale == 0)
				CameraObject.SetFloat("Animate",1);
			else if (SceneManager.GetActiveScene().buildIndex == 0 && mainCanvas.activeSelf && CameraObject)
			{
                CameraObject.SetFloat("Animate", 1);
            }

			if(creditsMenu)
			{
				creditsMenu.SetActive(false);
			}
			if(difficultyMenu)
			{
				difficultyMenu.SetActive(false);
			}
		}

		public void Position1(){
            if (mainCanvas.activeSelf && CameraObject && Time.timeScale == 0)
                CameraObject.SetFloat("Animate",0);
            else if (SceneManager.GetActiveScene().buildIndex == 0 && mainCanvas.activeSelf && CameraObject)
            {
                CameraObject.SetFloat("Animate", 0);
            }
        }

		void DisablePanels(){
			PanelVideo.SetActive(false);
			PanelVideo.SetActive(false);
			PanelAudio.SetActive(false);

			lineGame.SetActive(false);
			lineControls.SetActive(false);
			lineVideo.SetActive(false);
		}

		public void GamePanel(){
			DisablePanels();
			PanelAudio.SetActive(true);
			lineGame.SetActive(true);
		}

		public void VideoPanel(){
			DisablePanels();
			PanelVideo.SetActive(true);
			lineVideo.SetActive(true);
		}

		public void ControlsPanel(){
			DisablePanels();
			PanelVideo.SetActive(true);
			lineControls.SetActive(true);
		}

		public void CreditsMenu(){
			if (creditsMenu)
			{
				if (creditsMenu.activeSelf)
				{
					creditsMenu.SetActive(false);
				}
				else
				{
					difficultyMenu.SetActive(false);
					creditsMenu.SetActive(true);
				}
			}
		}

		public void DifficultyMenu()
		{
			if (difficultyMenu)
			{
				if (difficultyMenu.activeSelf)
				{
					difficultyMenu.SetActive(false);
				}
				else
				{
					if (creditsMenu)
					{
						creditsMenu.SetActive(false);
					}
					difficultyMenu.SetActive(true);
				}
			}
		}
	}
}